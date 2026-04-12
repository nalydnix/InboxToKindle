using System.Text.Json.Serialization;

namespace InboxToKindle.Functions.Models;

public sealed class InboundEmailPayload
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("inbound_id")]
    public string? InboundId { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAtUtc { get; set; }

    [JsonPropertyName("data")]
    public MailerSendInboundData? Data { get; set; }

    [JsonIgnore]
    public string To => Data?.Recipients?.To?.Raw ?? string.Empty;

    [JsonIgnore]
    public string From => Data?.From?.Email ?? string.Empty;

    [JsonIgnore]
    public string Subject => Data?.Subject ?? string.Empty;

    [JsonIgnore]
    public string Html => Data?.Html ?? string.Empty;

    [JsonIgnore]
    public string? Text => Data?.Text;

    [JsonIgnore]
    public string? MessageId =>
        Data?.Headers is not null && Data.Headers.TryGetValue("Message-ID", out var messageId)
            ? messageId
            : null;

    [JsonIgnore]
    public DateTimeOffset? ReceivedAtUtc => Data?.CreatedAtUtc ?? CreatedAtUtc;

    [JsonIgnore]
    public Dictionary<string, string> Headers => Data?.Headers ?? new(StringComparer.OrdinalIgnoreCase);
}

public sealed class MailerSendInboundData
{
    [JsonPropertyName("object")]
    public string? Object { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("recipients")]
    public MailerSendRecipients? Recipients { get; set; }

    [JsonPropertyName("from")]
    public MailerSendAddress? From { get; set; }

    [JsonPropertyName("sender")]
    public MailerSendSender? Sender { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("html")]
    public string? Html { get; set; }

    [JsonPropertyName("raw")]
    public string? Raw { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAtUtc { get; set; }
}

public sealed class MailerSendRecipients
{
    [JsonPropertyName("to")]
    public MailerSendRecipientGroup? To { get; set; }
}

public sealed class MailerSendRecipientGroup
{
    [JsonPropertyName("raw")]
    public string? Raw { get; set; }

    [JsonPropertyName("data")]
    public List<MailerSendAddress> Data { get; set; } = [];
}

public sealed class MailerSendAddress
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

public sealed class MailerSendSender
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
