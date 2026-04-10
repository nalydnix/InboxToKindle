namespace InboxToKindle.Functions.Models;

public sealed class PipelineResult
{
    public bool Success { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Error { get; init; }
    public Guid? ProcessedNewsletterId { get; init; }
}
