using System.Text;
using InboxToKindle.Functions.Services.Interfaces;

namespace InboxToKindle.Functions.Services.Implementations;

public sealed class EpubGenerationService : IEpubGenerationService
{
    public Task<byte[]> GenerateAsync(string title, string html, CancellationToken cancellationToken)
    {
        var epubPlaceholder = $"""
            Title: {title}
            Content-Type: text/html

            {html}
            """;

        return Task.FromResult(Encoding.UTF8.GetBytes(epubPlaceholder));
    }
}
