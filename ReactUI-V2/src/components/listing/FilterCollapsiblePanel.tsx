import { useState, type ReactNode } from 'react'
import { ChevronIcon } from '@/components/ui/ChevronIcon'

interface FilterCollapsiblePanelProps {
  title: string
  icon?: ReactNode
  activeCount?: number
  defaultExpanded?: boolean
  children: ReactNode
}

export function FilterCollapsiblePanel({
  title,
  icon,
  activeCount = 0,
  defaultExpanded = false,
  children,
}: FilterCollapsiblePanelProps) {
  const [expanded, setExpanded] = useState(defaultExpanded)

  return (
    <section className="min-w-0 overflow-hidden rounded-xl border border-border/70 bg-surface shadow-sm ring-1 ring-black/[0.02]">
      <button
        type="button"
        className="flex w-full items-center gap-2.5 px-4 py-3.5 text-start transition-colors hover:bg-surface-muted/35"
        aria-expanded={expanded}
        onClick={() => setExpanded((value) => !value)}
      >
        {icon && (
          <span className="flex size-8 shrink-0 items-center justify-center rounded-lg bg-warm-soft/70 text-warm ring-1 ring-warm/10">
            {icon}
          </span>
        )}
        <span className="min-w-0 flex-1 text-sm font-semibold leading-snug text-text">
          {title}
        </span>
        {activeCount > 0 && (
          <span className="shrink-0 rounded-full bg-warm px-2 py-0.5 text-[10px] font-bold tabular-nums text-warm-text">
            {activeCount}
          </span>
        )}
        <ChevronIcon expanded={expanded} className="text-text-muted" />
      </button>

      {expanded && (
        <div className="border-t border-border/50 px-4 pb-4 pt-3">{children}</div>
      )}
    </section>
  )
}
