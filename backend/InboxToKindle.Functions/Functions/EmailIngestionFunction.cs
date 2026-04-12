using System.Net;
using System.Text.Json;
using InboxToKindle.Functions.Models;
using InboxToKindle.Functions.Options;
using InboxToKindle.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InboxToKindle.Functions.Functions;

public sealed class EmailIngestionFunction(
    INewsletterPipelineService pipelineService,
    IOptions<SendGridOptions> sendGridOptions,
    ILogger<EmailIngestionFunction> logger)
{
    [Function("EmailIngestion")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "ingest/email")] HttpRequestData request,
        CancellationToken cancellationToken)
    {

        var payload = await JsonSerializer.DeserializeAsync<InboundEmailPayload>(
            request.Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            cancellationToken);

        if (payload == null)
        {
            logger.LogWarning("Failed to deserialize inbound email payload.");
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }

        if (payload.Type == "webhook.test")
        {
            logger.LogInformation("Received webhook test request from MailerSend. Responding with 200 OK.");
            var ok = request.CreateResponse(HttpStatusCode.OK);
            await ok.WriteStringAsync("OK", cancellationToken);
            return ok;
        }

        logger.LogInformation("Received inbound email for {To}", payload.To);
        var result = await pipelineService.ProcessAsync(payload, cancellationToken);

        var response = request.CreateResponse(result.Success ? HttpStatusCode.Accepted : HttpStatusCode.BadRequest);
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}
