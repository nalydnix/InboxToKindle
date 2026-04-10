namespace InboxToKindle.Functions.Services.Interfaces;

public interface IHtmlProcessingService
{
    Task<string> SanitizeAsync(string rawHtml, CancellationToken cancellationToken);
}
