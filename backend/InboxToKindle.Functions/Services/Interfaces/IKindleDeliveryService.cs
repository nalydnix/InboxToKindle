using InboxToKindle.Functions.Models;

namespace InboxToKindle.Functions.Services.Interfaces;

public interface IKindleDeliveryService
{
    Task<DeliveryResult> SendAsync(string kindleEmail, string subject, byte[] epubBytes, CancellationToken cancellationToken);
}
