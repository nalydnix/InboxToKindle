create extension if not exists "pgcrypto";

create table if not exists public.profiles (
  id uuid primary key references auth.users(id) on delete cascade,
  kindle_email text,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create table if not exists public.subscriptions (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references public.profiles(id) on delete cascade,
  newsletter_name text not null,
  subscription_email text not null unique,
  is_active boolean not null default true,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create table if not exists public.incoming_emails (
  id uuid primary key default gen_random_uuid(),
  subscription_id uuid not null references public.subscriptions(id) on delete cascade,
  source_from_email text not null,
  message_id text,
  subject text not null,
  raw_html text,
  raw_text text,
  received_at timestamptz not null default now(),
  created_at timestamptz not null default now()
);

create table if not exists public.processed_newsletters (
  id uuid primary key default gen_random_uuid(),
  incoming_email_id uuid references public.incoming_emails(id) on delete set null,
  subscription_id uuid not null references public.subscriptions(id) on delete cascade,
  subject text not null,
  processing_status text not null default 'received',
  sanitized_html text,
  epub_storage_path text,
  error_log text,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create table if not exists public.delivery_logs (
  id uuid primary key default gen_random_uuid(),
  processed_newsletter_id uuid not null references public.processed_newsletters(id) on delete cascade,
  destination_email text not null,
  provider_message_id text,
  status text not null,
  error_log text,
  delivered_at timestamptz,
  created_at timestamptz not null default now()
);

alter table public.profiles enable row level security;
alter table public.subscriptions enable row level security;
alter table public.incoming_emails enable row level security;
alter table public.processed_newsletters enable row level security;
alter table public.delivery_logs enable row level security;

create policy "profiles_select_own"
on public.profiles
for select
using (auth.uid() = id);

create policy "profiles_update_own"
on public.profiles
for update
using (auth.uid() = id);

create policy "subscriptions_select_own"
on public.subscriptions
for select
using (auth.uid() = user_id);

create policy "processed_newsletters_select_own"
on public.processed_newsletters
for select
using (
  exists (
    select 1
    from public.subscriptions s
    where s.id = processed_newsletters.subscription_id
      and s.user_id = auth.uid()
  )
);

create policy "delivery_logs_select_own"
on public.delivery_logs
for select
using (
  exists (
    select 1
    from public.processed_newsletters pn
    join public.subscriptions s on s.id = pn.subscription_id
    where pn.id = delivery_logs.processed_newsletter_id
      and s.user_id = auth.uid()
  )
);
