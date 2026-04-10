# InboxToKindle

InboxToKindle is a scaffold for a Kindle newsletter automation platform. It includes:

- a .NET 8 isolated Azure Functions backend for inbound email ingestion and processing
- a React + Vite dashboard with Supabase auth wiring
- a Supabase schema with row-level security for the core entities

## Repository layout

```text
backend/   Azure Functions app and processing services
frontend/  React dashboard
infra/     Supabase SQL schema
docs/      Project notes and deployment material
```

## Backend

The backend is organized around a webhook-style ingestion pipeline:

1. `POST /api/ingest/email`
2. validate inbound secret
3. resolve the destination subscription email
4. sanitize HTML
5. generate Kindle-safe content
6. send to the user's Kindle email
7. store processing and delivery metadata

Important files:

- `backend/InboxToKindle.Functions/Functions/EmailIngestionFunction.cs`
- `backend/InboxToKindle.Functions/Services/Implementations/NewsletterPipelineService.cs`
- `backend/InboxToKindle.Functions/Repositories/SupabaseRepository.cs`

## Frontend

The frontend is a small React shell with:

- login page
- dashboard shell
- Supabase client setup
- placeholder delivery history UI

## Configuration

Copy values from `.env.example`, `frontend/.env.example`, and `backend/InboxToKindle.Functions/local.settings.json` into your real environment configuration.

## Next steps

1. Replace scaffold repository methods with real Supabase CRUD calls.
2. Replace the placeholder EPUB generator with a real EPUB library or Pandoc-based flow.
3. Implement SendGrid inbound parse and Kindle delivery email sending.
4. Hook the React dashboard up to live Supabase queries and auth state.
