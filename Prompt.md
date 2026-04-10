# Kindle Newsletter Automation Platform — Codex Prompt

## 🚀 System Overview

You are a senior full-stack engineer. Build a production-ready system that converts email newsletters into Kindle-readable ebooks and delivers them automatically.

Users subscribe to tech newsletters using a platform-provided email address. When emails arrive, the system:

1. Receives the email
2. Parses and cleans the HTML content
3. Converts it into Kindle-friendly ebook format (EPUB preferred)
4. Sends it automatically to the user’s Kindle device email
5. Stores metadata and history for user access via a web dashboard

Target device: Amazon Kindle

---

## 🏗️ Tech Stack Requirements

### Frontend
- React (modern functional components)
- Simple dashboard UI:
  - user signup/login
  - list of newsletters
  - assigned subscription email address
  - delivery history

---

### Backend
- .NET 8 Azure Functions
- Email ingestion trigger (SMTP / SendGrid inbound parse / webhook)
- HTML parsing + transformation service
- EPUB generation service
- Kindle delivery service (email-based Send-to-Kindle)

---

### Database + Auth
- Supabase
  - Authentication (email/password)
  - PostgreSQL database
  - Row-level security enabled

---

## 📩 Core System Flow

Newsletter email arrives  
→ Azure Function triggered  
→ Extract raw HTML content  
→ Clean + sanitize HTML  
→ Extract main article content  
→ Convert to EPUB  
→ Send to Kindle email  
→ Store record in Supabase  

---

## 📦 Functional Requirements

### 1. Email Ingestion Service (Azure Function)
- Accept incoming email payload (HTML + metadata)
- Identify user via subscription email mapping
- Store raw email (optional)
- Trigger processing pipeline

---

### 2. HTML Processing Pipeline
- Remove scripts/styles
- Remove tracking pixels
- Strip email-client-specific markup (Outlook/Gmail junk)
- Extract main readable content

Output:
- clean structured HTML

---

### 3. EPUB Generator
- Convert cleaned HTML into EPUB
- Ensure Kindle compatibility:
  - simple HTML structure
  - no JavaScript
  - minimal CSS

Target device: Amazon Kindle

---

### 4. Kindle Delivery Service
- Send EPUB or HTML via email to user Kindle address
- Ensure stable sender identity (verified domain)

---

### 5. Supabase Schema

Design tables:

- users
- subscriptions
- incoming_emails
- processed_newsletters
- delivery_logs

Include:
- timestamps
- processing status
- error logs

---

### 6. Frontend (React)

Pages:
- Login / signup (Supabase auth)
- Dashboard:
  - Kindle email settings
  - subscription email address display
  - processed newsletters list
  - delivery status (processed / failed / sent)

---

## ⚙️ Non-functional Requirements

- Cloud-ready (Azure deployment)
- Stateless Azure Functions
- Retry-safe processing pipeline
- Idempotent (no duplicate processing)
- Full logging for every pipeline step

---

## 🧠 Suggested .NET Libraries

- HtmlAgilityPack (HTML parsing)
- AngleSharp (alternative parser)
- Ganss.XSS (HTML sanitization)
- VersOne.Epub OR Pandoc (EPUB generation)
- SendGrid (email delivery)

---

## 🔐 Security Requirements

- Validate inbound emails (prevent spoofing)
- Only process known subscription addresses
- Supabase RLS enabled
- Secure Kindle email storage

---

## 📊 Output Expected from Codex

Generate:

1. Full .NET 8 Azure Functions project
2. Email ingestion trigger
3. HTML processing service
4. EPUB generation module
5. Email sending service
6. Supabase schema + integration layer
7. React frontend (minimal but functional)
8. Deployment instructions

---

## 💡 Optional Enhancements

- AI summarization of newsletters before EPUB conversion
- Deduplication of repeated articles
- Tagging system (AI / fintech / dev / etc.)
- Digest mode (multiple emails → single EPUB)
