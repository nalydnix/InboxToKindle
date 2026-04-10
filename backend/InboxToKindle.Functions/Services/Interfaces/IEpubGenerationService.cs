namespace InboxToKindle.Functions.Services.Interfaces;

public interface IEpubGenerationService
{
    Task<byte[]> GenerateAsync(string title, string html, CancellationToken cancellationToken);
}
