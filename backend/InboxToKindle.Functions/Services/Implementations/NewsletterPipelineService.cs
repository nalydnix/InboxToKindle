using InboxToKindle.Functions.Models;
using InboxToKindle.Functions.Repositories;
using InboxToKindle.Functions.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace InboxToKindle.Functions.Services.Implementations;

public sealed class NewsletterPipelineService(
    ISupabaseRepository repository,
    IHtmlProcessingService htmlProcessingService,
    IEpubGenerationService epubGenerationService,
    IKindleDeliveryService kindleDeliveryService,
    ILogger<NewsletterPipelineService> logger) : INewsletterPipelineService
{
    public async Task<PipelineResult> ProcessAsync(InboundEmailPayload payload, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(payload.To) || string.IsNullOrWhiteSpace(payload.Html))
        {
            return new PipelineResult
            {
                Success = false,
                Status = "invalid_payload",
                Error = "Inbound email payload is missing required fields."
            };
        }

        var subscription = await repository.GetSubscriptionByEmailAsync(payload.To, cancellationToken);
        if (subscription is null || !subscription.IsActive)
        {
            return new PipelineResult
            {
                Success = false,
                Status = "subscription_not_found",
                Error = $"No active subscription found for {payload.To}."
            };
        }

        if (!string.IsNullOrWhiteSpace(payload.MessageId) &&
            await repository.HasProcessedMessageAsync(payload.MessageId, cancellationToken))
        {
            return new PipelineResult
            {
                Success = true,
                Status = "duplicate_skipped"
            };
        }

        await repository.CreateIncomingEmailRecordAsync(payload, subscription, cancellationToken);

        var cleanHtml = await htmlProcessingService.SanitizeAsync(payload.Html, cancellationToken);
        var epubBytes = await epubGenerationService.GenerateAsync(payload.Subject, cleanHtml, cancellationToken);

        var processed = new ProcessedNewsletter
        {
            SubscriptionId = subscription.Id,
            Subject = payload.Subject,
            SourceFromEmail = payload.From,
            MessageId = payload.MessageId ?? string.Empty,
            SanitizedHtml = cleanHtml,
            EpubBytes = epubBytes,
            ProcessingStatus = "processed"
        };

        var processedId = await repository.CreateProcessedNewsletterAsync(processed, cancellationToken);

        try
        {
            var delivery = await kindleDeliveryService.SendAsync(subscription.KindleEmail, payload.Subject, epubBytes, cancellationToken);
            await repository.CreateDeliveryLogAsync(processedId, delivery, cancellationToken);
            await repository.UpdateProcessedNewsletterStatusAsync(
                processedId,
                delivery.Success ? "sent" : "failed",
                delivery.Error,
                cancellationToken);

            return new PipelineResult
            {
                Success = delivery.Success,
                Status = delivery.Success ? "sent" : "failed",
                Error = delivery.Error,
                ProcessedNewsletterId = processedId
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Newsletter pipeline failed for subscription {SubscriptionId}", subscription.Id);
            await repository.UpdateProcessedNewsletterStatusAsync(processedId, "failed", ex.Message, cancellationToken);

            return new PipelineResult
            {
                Success = false,
                Status = "failed",
                Error = ex.Message,
                ProcessedNewsletterId = processedId
            };
        }
    }
}
