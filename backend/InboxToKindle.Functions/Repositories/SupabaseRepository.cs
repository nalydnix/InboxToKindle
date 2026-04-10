using InboxToKindle.Functions.Models;
using Microsoft.Extensions.Logging;

namespace InboxToKindle.Functions.Repositories;

public sealed class SupabaseRepository(ILogger<SupabaseRepository> logger) : ISupabaseRepository
{
    public Task<Subscription?> GetSubscriptionByEmailAsync(string subscriptionEmail, CancellationToken cancellationToken)
    {
        logger.LogInformation("Looking up subscription for {SubscriptionEmail}", subscriptionEmail);

        return Task.FromResult<Subscription?>(new Subscription
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            NewsletterName = "Sample Newsletter",
            SubscriptionEmail = subscriptionEmail,
            KindleEmail = "your-kindle@example.com",
            IsActive = true
        });
    }

    public Task<bool> HasProcessedMessageAsync(string messageId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking idempotency for message {MessageId}", messageId);
        return Task.FromResult(false);
    }

    public Task<Guid> CreateIncomingEmailRecordAsync(InboundEmailPayload payload, Subscription subscription, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        logger.LogInformation("Creating incoming email record {IncomingEmailId} for subscription {SubscriptionId}", id, subscription.Id);
        return Task.FromResult(id);
    }

    public Task<Guid> CreateProcessedNewsletterAsync(ProcessedNewsletter newsletter, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating processed newsletter record {ProcessedNewsletterId}", newsletter.Id);
        return Task.FromResult(newsletter.Id);
    }

    public Task UpdateProcessedNewsletterStatusAsync(Guid newsletterId, string status, string? errorMessage, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating newsletter {ProcessedNewsletterId} to status {Status}", newsletterId, status);
        return Task.CompletedTask;
    }

    public Task CreateDeliveryLogAsync(Guid processedNewsletterId, DeliveryResult result, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating delivery log for newsletter {ProcessedNewsletterId} with success {Success}", processedNewsletterId, result.Success);
        return Task.CompletedTask;
    }
}
