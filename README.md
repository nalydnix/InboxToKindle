# 📚 Newsletter to Kindle — Development Plan

## 🚀 Phase 0 — Define MVP

### 🎯 MVP Goal

Forward a newsletter → it shows up on Kindle

### ✅ MVP Scope

* 1 user (you)
* 1 email ingestion pipeline
* basic HTML → Kindle-friendly conversion
* send to Kindle email

---

## 🧱 Phase 1 — Email → Kindle Pipeline

### Step 1: Set up Kindle

* Find your Kindle email
* Whitelist your sender email
* Test sending a file manually

### Step 2: Receive Emails

Use SendGrid Inbound Parse

Flow:
newsletter → custom email → SendGrid → webhook → Azure Function

### Step 3: Azure Function (.NET)

Create HTTP trigger function to receive email payload

### Step 4: Clean HTML

* Use HtmlAgilityPack
* Remove scripts/styles
* Sanitize content

### Step 5: Send to Kindle

* Attach cleaned HTML
* Send via SMTP or SendGrid

---

## 📘 Phase 2 — Reliability & EPUB

### Step 6: Convert HTML → EPUB

* Use Pandoc (recommended)

### Step 7: Improve Parsing

* Remove Outlook junk
* Extract main content

### Step 8: Logging

* Log emails
* Track failures

---

## 👤 Phase 3 — Multi-user (Supabase)

### Step 9: Database Schema

Tables:

* users
* subscriptions
* incoming_emails
* processed_newsletters

### Step 10: Unique Emails

Example:
[user123@yourdomain.com](mailto:user123@yourdomain.com)

### Step 11: Update Pipeline

incoming email → identify user → process → send to Kindle

---

## 🌐 Phase 4 — Frontend (React)

### Step 12: Dashboard

* Login
* Show subscription email
* List newsletters

### Step 13: Settings

* Kindle email
* Format (HTML/EPUB)

---

## 🤖 Phase 5 — AI Enhancements

### Step 14: Summarization

* Extract key points

### Step 15: Smart Formatting

* Clean structure

### Step 16: Digest Mode

* Combine multiple emails

---

## ⚙️ Phase 6 — Production Hardening

### Step 17: Queue Architecture

Email → Queue → Processor

### Step 18: Retry Logic

* Retry failures

### Step 19: Deduplication

* Hash content

---

## 🧠 Final Architecture

SendGrid → Azure Function → Queue → Processor → Kindle → Supabase → React

---

## ⏱️ Timeline

### Week 1

* MVP pipeline

### Week 2

* EPUB + parsing

### Week 3

* Multi-user + UI

### Week 4+

* AI + polish

---

## 🧠 Advice

Start simple:

Can I forward one newsletter and read it on Kindle?

Everything else comes later.
