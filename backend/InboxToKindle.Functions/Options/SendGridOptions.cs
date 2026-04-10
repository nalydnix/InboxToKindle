namespace InboxToKindle.Functions.Options;

public sealed class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string InboundWebhookSecret { get; set; } = string.Empty;
}
