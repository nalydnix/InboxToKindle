# InboxToKindle

InboxToKindle is a scaffold for a Kindle newsletter automation platform. The repository currently contains:

- a .NET 8 isolated Azure Functions backend for email ingestion and processing
- a React + Vite frontend shell
- a Supabase schema for subscriptions, newsletters, and delivery logs

## Repository layout

```text
backend/   Azure Functions app and processing services
frontend/  React dashboard
infra/     Supabase SQL schema
docs/      deployment notes
```

## Current status

The project structure is in place and the Azure Function can be built, run, and invoked. Some integrations are still scaffolded:

- `SupabaseRepository` is a placeholder implementation
- `KindleDeliveryService` is a placeholder implementation
- `EpubGenerationService` is a placeholder implementation

That means local and Azure testing currently verifies the function path and request handling, not full production integrations.

## Prerequisites

Install these tools before building or running the project:

- .NET SDK 9.x or later installed locally
- Azure Functions Core Tools v4
- Node.js 18+
- npm 8+

Optional but recommended for local development:

- Azurite, if you want `AzureWebJobsStorage=UseDevelopmentStorage=true` to be healthy locally

## Configuration

### Backend

The Azure Function uses:

- [local.settings.json](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/local.settings.json) for local development
- Azure Function App application settings in Azure for deployed environments

Fill in these values locally before testing:

```json
{
  "SendGrid": {
    "ApiKey": "<sendgrid-api-key>",
    "FromEmail": "<verified-sender@example.com>",
    "InboundWebhookSecret": "<shared-secret>"
  },
  "Supabase": {
    "Url": "<https://your-project.supabase.co>",
    "AnonKey": "<anon-key>",
    "ServiceRoleKey": "<service-role-key>"
  },
  "KindleDelivery": {
    "DefaultSubjectPrefix": "InboxToKindle"
  }
}
```

Important:

- `local.settings.json` is for local development only
- do not deploy `local.settings.json`
- in Azure, set the same values as app settings using `Section__Property` names such as `SendGrid__ApiKey`

### Frontend

Create a frontend env file from [frontend/.env.example](D:/repos/InboxToKindle/frontend/.env.example):

```bash
cp frontend/.env.example frontend/.env
```

Then set:

- `VITE_SUPABASE_URL`
- `VITE_SUPABASE_ANON_KEY`

## Build

### Build the Azure Functions backend

From the repo root:

```powershell
dotnet build backend\InboxToKindle.Functions\InboxToKindle.Functions.csproj
```

### Build the frontend

```powershell
Set-Location frontend
npm install
npm run build
```

## Run locally

### Run the Azure Function locally

From the repo root:

```powershell
func start --script-root backend\InboxToKindle.Functions --port 7071
```

When the host starts successfully, the HTTP trigger is available at:

```text
POST http://localhost:7071/api/ingest/email
```

Notes:

- If `AzureWebJobsStorage=UseDevelopmentStorage=true` and Azurite is not running, the function host may report storage health warnings.
- For the current HTTP trigger scaffold, the host can still start and accept requests.

### Run the frontend locally

```powershell
Set-Location frontend
npm install
npm run dev
```

Default local frontend URL:

```text
http://localhost:5173
```

## Test the Azure Function locally

The function in [EmailIngestionFunction.cs](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/Functions/EmailIngestionFunction.cs) expects:

- HTTP `POST`
- JSON body
- `x-inbound-secret` header

### Example PowerShell test

```powershell
$headers = @{
  "x-inbound-secret" = "<your-inbound-secret>"
}

$body = @{
  to = "reader+test@yourdomain.com"
  from = "newsletter@example.com"
  subject = "Manual test"
  html = "<html><head><title>Manual test</title></head><body><h1>Hello Kindle</h1><p>Sample body.</p></body></html>"
  text = "Hello Kindle. Sample body."
  messageId = "local-test-1"
  receivedAtUtc = "2026-04-12T00:00:00Z"
  headers = @{
    "X-Test" = "true"
  }
} | ConvertTo-Json -Depth 5

Invoke-RestMethod `
  -Method Post `
  -Uri "http://localhost:7071/api/ingest/email" `
  -Headers $headers `
  -ContentType "application/json" `
  -Body $body
```

### Expected local results

- `202 Accepted` when the request is valid and processing completes
- `401 Unauthorized` if `x-inbound-secret` is missing or wrong
- `400 Bad Request` if the payload is invalid

## Test the Azure Function in Azure

Once deployed, call the function URL:

```text
https://<your-function-app>.azurewebsites.net/api/ingest/email?code=<function-key>
```

You still need:

- the same JSON request body
- the `x-inbound-secret` header
- the Azure function key because the trigger uses `AuthorizationLevel.Function`

### Example `curl`

```bash
curl -X POST "https://<your-function-app>.azurewebsites.net/api/ingest/email?code=<function-key>" \
  -H "Content-Type: application/json" \
  -H "x-inbound-secret: <your-inbound-secret>" \
  -d '{
    "to": "reader+test@yourdomain.com",
    "from": "newsletter@example.com",
    "subject": "Azure test",
    "html": "<html><body><h1>Hello Kindle</h1><p>Sample body.</p></body></html>",
    "messageId": "azure-test-1"
  }'
```

## Deploy the Azure Function

If the Function App already exists, publish it with Azure Functions Core Tools:

```powershell
func azure functionapp publish <your-function-app-name> --dotnet-isolated
```

Set these Azure app settings in the Function App configuration:

- `SendGrid__ApiKey`
- `SendGrid__FromEmail`
- `SendGrid__InboundWebhookSecret`
- `Supabase__Url`
- `Supabase__AnonKey`
- `Supabase__ServiceRoleKey`
- `KindleDelivery__DefaultSubjectPrefix`

## Supabase schema

Apply the schema in [schema.sql](D:/repos/InboxToKindle/infra/supabase/schema.sql) to your Supabase project.

That creates:

- `profiles`
- `subscriptions`
- `incoming_emails`
- `processed_newsletters`
- `delivery_logs`

## Useful files

- [EmailIngestionFunction.cs](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/Functions/EmailIngestionFunction.cs)
- [NewsletterPipelineService.cs](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/Services/Implementations/NewsletterPipelineService.cs)
- [local.settings.json](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/local.settings.json)
- [InboxToKindle.Functions.csproj](D:/repos/InboxToKindle/backend/InboxToKindle.Functions/InboxToKindle.Functions.csproj)
- [schema.sql](D:/repos/InboxToKindle/infra/supabase/schema.sql)

## Next implementation steps

1. Replace the placeholder Supabase repository with real database calls.
2. Replace the placeholder Kindle delivery service with real SendGrid sending.
3. Replace the placeholder EPUB generation with a real EPUB pipeline.
4. Connect the frontend dashboard to live Supabase data.
