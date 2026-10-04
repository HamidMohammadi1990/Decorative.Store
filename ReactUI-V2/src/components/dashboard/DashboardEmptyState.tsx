import type { ReactNode } from 'react'

interface DashboardEmptyStateProps {
  icon: ReactNode
  title: string
  message: string
  action?: ReactNode
}

export function DashboardEmptyState({ icon, title, message, action }: DashboardEmptyStateProps) {
  return (
    <div className="relative overflow-hidden rounded-sm border border-dashed border-border bg-gradient-to-b from-surface-muted/50 to-surface px-6 py-16 text-center sm:px-10">
      <div
        aria-hidden
        className="pointer-events-none absolute inset-x-0 top-0 h-24 bg-gradient-to-b from-warm-soft/40 to-transparent"
      />
      <div className="relative mx-auto flex size-16 items-center justify-center rounded-full bg-warm-soft text-warm shadow-sm ring-8 ring-warm-soft/50">
        {icon}
      </div>
      <h2 className="relative mt-6 text-lg font-semibold text-text">{title}</h2>
      <p className="relative mx-auto mt-2 max-w-sm text-sm leading-relaxed text-text-muted">
        {message}
      </p>
      {action && <div className="relative mt-7">{action}</div>}
    </div>
  )
}
