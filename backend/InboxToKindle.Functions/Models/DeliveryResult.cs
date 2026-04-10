namespace InboxToKindle.Functions.Models;

public sealed class DeliveryResult
{
    public bool Success { get; init; }
    public string ProviderMessageId { get; init; } = string.Empty;
    public string? Error { get; init; }
}
