using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;
using Ganss.Xss;
using InboxToKindle.Functions.Services.Interfaces;

namespace InboxToKindle.Functions.Services.Implementations;

public sealed class HtmlProcessingService : IHtmlProcessingService
{
    private static readonly Regex TrackingPixelsRegex = new("<img[^>]+(width=[\"']?1[\"']?|height=[\"']?1[\"']?).*?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public async Task<string> SanitizeAsync(string rawHtml, CancellationToken cancellationToken)
    {
        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(rawHtml, cancellationToken);

        foreach (var node in document.QuerySelectorAll("script, style, noscript, svg, iframe"))
        {
            node.Remove();
        }

        var bodyHtml = document.Body?.InnerHtml ?? rawHtml;
        bodyHtml = TrackingPixelsRegex.Replace(bodyHtml, string.Empty);

        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedSchemes.Add("data");

        var sanitized = sanitizer.Sanitize(bodyHtml);
        return $"<article><h1>{document.Title}</h1>{sanitized}</article>";
    }
}
