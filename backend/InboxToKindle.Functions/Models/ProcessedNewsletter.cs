namespace InboxToKindle.Functions.Models;

public sealed class ProcessedNewsletter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SubscriptionId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string SourceFromEmail { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public string SanitizedHtml { get; set; } = string.Empty;
    public byte[] EpubBytes { get; set; } = [];
    public string ProcessingStatus { get; set; } = "received";
    public string? ErrorMessage { get; set; }
}
