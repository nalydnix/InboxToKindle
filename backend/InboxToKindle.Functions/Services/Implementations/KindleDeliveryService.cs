using InboxToKindle.Functions.Models;
using InboxToKindle.Functions.Options;
using InboxToKindle.Functions.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InboxToKindle.Functions.Services.Implementations;

public sealed class KindleDeliveryService(
    IOptions<SendGridOptions> sendGridOptions,
    IOptions<KindleDeliveryOptions> kindleOptions,
    ILogger<KindleDeliveryService> logger) : IKindleDeliveryService
{
    public Task<DeliveryResult> SendAsync(string kindleEmail, string subject, byte[] epubBytes, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Preparing Kindle delivery to {KindleEmail} from {FromEmail} with subject prefix {SubjectPrefix}",
            kindleEmail,
            sendGridOptions.Value.FromEmail,
            kindleOptions.Value.DefaultSubjectPrefix);

        return Task.FromResult(new DeliveryResult
        {
            Success = true,
            ProviderMessageId = Guid.NewGuid().ToString("N")
        });
    }
}
