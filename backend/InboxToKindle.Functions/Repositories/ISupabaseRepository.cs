using InboxToKindle.Functions.Models;

namespace InboxToKindle.Functions.Repositories;

public interface ISupabaseRepository
{
    Task<Subscription?> GetSubscriptionByEmailAsync(string subscriptionEmail, CancellationToken cancellationToken);
    Task<bool> HasProcessedMessageAsync(string messageId, CancellationToken cancellationToken);
    Task<Guid> CreateIncomingEmailRecordAsync(InboundEmailPayload payload, Subscription subscription, CancellationToken cancellationToken);
    Task<Guid> CreateProcessedNewsletterAsync(ProcessedNewsletter newsletter, CancellationToken cancellationToken);
    Task UpdateProcessedNewsletterStatusAsync(Guid newsletterId, string status, string? errorMessage, CancellationToken cancellationToken);
    Task CreateDeliveryLogAsync(Guid processedNewsletterId, DeliveryResult result, CancellationToken cancellationToken);
}
