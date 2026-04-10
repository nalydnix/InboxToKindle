using InboxToKindle.Functions.Models;

namespace InboxToKindle.Functions.Services.Interfaces;

public interface INewsletterPipelineService
{
    Task<PipelineResult> ProcessAsync(InboundEmailPayload payload, CancellationToken cancellationToken);
}
