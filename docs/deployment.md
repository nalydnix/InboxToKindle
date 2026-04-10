# Deployment Notes

## Azure Functions

- Deploy `backend/InboxToKindle.Functions` to an Azure Functions app running .NET 8 isolated worker.
- Configure app settings for SendGrid, Supabase, and the inbound webhook secret.
- Point your SendGrid inbound parse webhook to `/api/ingest/email`.

## Supabase

- Run `infra/supabase/schema.sql`.
- Enable email/password auth.
- Store Kindle email per user in `public.profiles`.

## Frontend

- Build and host the `frontend` app on Azure Static Web Apps, Vercel, or Netlify.
- Configure `VITE_SUPABASE_URL` and `VITE_SUPABASE_ANON_KEY`.
