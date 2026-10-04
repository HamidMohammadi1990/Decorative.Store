import type { ReactNode } from 'react'

interface DashboardSectionProps {
  title: string
  icon?: ReactNode
  action?: ReactNode
  children: ReactNode
  className?: string
}

export function DashboardSection({
  title,
  icon,
  action,
  children,
  className = '',
}: DashboardSectionProps) {
  return (
    <section
      className={`overflow-hidden rounded-sm border border-border bg-surface shadow-sm ${className}`}
    >
      <header className="flex items-center justify-between gap-4 border-b border-border bg-surface-muted/40 px-5 py-4 sm:px-6">
        <div className="flex items-center gap-3">
          {icon && (
            <span className="flex size-9 shrink-0 items-center justify-center rounded-sm bg-warm-soft text-warm ring-1 ring-warm/10">
              {icon}
            </span>
          )}
          <h2 className="text-base font-semibold text-text">{title}</h2>
        </div>
        {action}
      </header>
      {children}
    </section>
  )
}
