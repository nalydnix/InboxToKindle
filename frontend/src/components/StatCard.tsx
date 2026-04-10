type StatCardProps = {
  label: string;
  value: string;
};

export function StatCard({ label, value }: StatCardProps) {
  return (
    <div className="card stat-card">
      <span className="eyebrow">{label}</span>
      <strong>{value}</strong>
    </div>
  );
}
