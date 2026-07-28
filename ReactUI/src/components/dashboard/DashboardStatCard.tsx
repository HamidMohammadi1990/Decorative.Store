import type { ReactNode } from 'react'

interface DashboardStatCardProps {
  label: string
  value: ReactNode
  hint?: string
  icon?: ReactNode
  accent?: boolean
}

export function DashboardStatCard({ label, value, hint, icon, accent = false }: DashboardStatCardProps) {
  return (
    <div
      className={`group relative overflow-hidden rounded-sm border p-5 shadow-sm transition-shadow duration-200 hover:shadow-md ${
        accent
          ? 'border-warm/25 bg-gradient-to-br from-warm via-warm to-warm-hover text-warm-text'
          : 'border-border bg-surface hover:border-border-strong'
      }`}
    >
      {accent && (
        <div
          aria-hidden
          className="pointer-events-none absolute -end-6 -top-6 size-28 rounded-full bg-warm-text/10"
        />
      )}
      <div className="relative flex items-start justify-between gap-3">
        <div className="min-w-0">
          <p
            className={`text-xs font-semibold uppercase tracking-[0.12em] ${
              accent ? 'text-warm-text/75' : 'text-text-muted'
            }`}
          >
            {label}
          </p>
          <p className={`mt-2 truncate text-2xl font-semibold tracking-tight ${accent ? 'text-warm-text' : 'text-text'}`}>
            {value}
          </p>
          {hint && (
            <p className={`mt-1.5 text-xs ${accent ? 'text-warm-text/70' : 'text-text-muted'}`}>
              {hint}
            </p>
          )}
        </div>
        {icon && (
          <span
            className={`flex size-11 shrink-0 items-center justify-center rounded-sm transition-transform duration-200 group-hover:scale-105 ${
              accent ? 'bg-warm-text/15 text-warm-text' : 'bg-warm-soft text-warm ring-1 ring-warm/10'
            }`}
          >
            {icon}
          </span>
        )}
      </div>
    </div>
  )
}
