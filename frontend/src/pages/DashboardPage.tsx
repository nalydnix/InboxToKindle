import { StatCard } from "../components/StatCard";

const newsletters = [
  { title: "Platformer Weekly", status: "sent", deliveredAt: "2026-04-10 07:15 UTC" },
  { title: "Pragmatic Engineer", status: "processed", deliveredAt: "2026-04-09 08:30 UTC" },
  { title: "ByteByteGo", status: "failed", deliveredAt: "2026-04-08 06:42 UTC" }
];

export function DashboardPage() {
  return (
    <main className="page-shell dashboard-shell">
      <section className="dashboard-header">
        <div>
          <p className="eyebrow">Dashboard</p>
          <h1>Your newsletter delivery pipeline</h1>
          <p className="muted">
            Surface Kindle settings, assigned inbound email addresses, and delivery history from Supabase.
          </p>
        </div>
        <div className="address-chip">reader+u_123@inboxtokindle.dev</div>
      </section>

      <section className="stats-grid">
        <StatCard label="Kindle Address" value="my-kindle@kindle.com" />
        <StatCard label="Active Subscriptions" value="3" />
        <StatCard label="Last Delivery" value="Sent 34m ago" />
      </section>

      <section className="card table-card">
        <div className="section-heading">
          <h2>Processed newsletters</h2>
          <span className="muted">Status synced from `processed_newsletters`</span>
        </div>

        <div className="newsletter-list">
          {newsletters.map((newsletter) => (
            <article className="newsletter-row" key={`${newsletter.title}-${newsletter.deliveredAt}`}>
              <div>
                <strong>{newsletter.title}</strong>
                <p className="muted">{newsletter.deliveredAt}</p>
              </div>
              <span className={`status-pill status-${newsletter.status}`}>{newsletter.status}</span>
            </article>
          ))}
        </div>
      </section>
    </main>
  );
}
