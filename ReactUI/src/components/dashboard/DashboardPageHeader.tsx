import type { ReactNode } from 'react'

interface DashboardPageHeaderProps {
  title: string
  description?: string
  icon?: ReactNode
  action?: ReactNode
}

export function DashboardPageHeader({
  title,
  description,
  icon,
  action,
}: DashboardPageHeaderProps) {
  return (
    <header className="mb-6 border-b border-border/70 pb-6 md:mb-8 md:pb-7">
      <div className="flex flex-wrap items-start justify-between gap-4">
        <div className="flex min-w-0 items-start gap-4">
          {icon && (
            <span className="flex size-12 shrink-0 items-center justify-center rounded-sm bg-gradient-to-br from-warm-soft to-surface text-warm shadow-sm ring-1 ring-warm/15">
              {icon}
            </span>
          )}
          <div className="min-w-0">
            <h1 className="text-xl font-semibold tracking-tight text-text sm:text-2xl md:text-[1.75rem]">
              {title}
            </h1>
            {description && (
              <p className="mt-2 max-w-2xl text-sm leading-relaxed text-text-muted">
                {description}
              </p>
            )}
          </div>
        </div>
        {action}
      </div>
    </header>
  )
}
