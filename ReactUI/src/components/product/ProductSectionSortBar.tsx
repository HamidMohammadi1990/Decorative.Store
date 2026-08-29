import type { ReactNode } from 'react'

export interface ProductSectionSortOption<T extends string> {
  id: T
  label: string
  icon?: ReactNode
}

interface ProductSectionSortBarProps<T extends string> {
  label: string
  options: ProductSectionSortOption<T>[]
  value: T
  onChange: (id: T) => void
  totalLabel?: string
  className?: string
}

export function ProductSectionSortBar<T extends string>({
  label,
  options,
  value,
  onChange,
  totalLabel,
  className = '',
}: ProductSectionSortBarProps<T>) {
  return (
    <div
      className={`flex flex-col gap-3 border-b border-border pb-4 sm:flex-row sm:items-center sm:justify-between ${className}`}
    >
      <div className="flex min-w-0 flex-col gap-2.5 sm:flex-row sm:items-center sm:gap-3">
        <span className="shrink-0 text-xs font-medium text-text-muted">{label}</span>
        <div
          role="group"
          aria-label={label}
          className="-mx-1 flex gap-1 overflow-x-auto px-1 pb-0.5 sm:mx-0 sm:flex-wrap sm:overflow-visible sm:pb-0"
        >
          <div className="inline-flex shrink-0 gap-1 rounded-xl bg-surface-muted/90 p-1 shadow-inner ring-1 ring-border/70">
            {options.map((option) => {
              const active = value === option.id

              return (
                <button
                  key={option.id}
                  type="button"
                  aria-pressed={active}
                  onClick={() => onChange(option.id)}
                  className={`inline-flex shrink-0 items-center gap-1.5 rounded-lg px-3 py-2 text-sm font-medium transition-all duration-200 ${
                    active
                      ? 'bg-surface text-warm shadow-sm ring-1 ring-warm/30'
                      : 'text-text-muted hover:bg-surface/80 hover:text-text'
                  }`}
                >
                  {option.icon && (
                    <span className={active ? 'text-warm' : 'text-text-muted/80'} aria-hidden>
                      {option.icon}
                    </span>
                  )}
                  <span>{option.label}</span>
                </button>
              )
            })}
          </div>
        </div>
      </div>

      {totalLabel && (
        <span className="inline-flex shrink-0 items-center self-start rounded-full bg-surface-muted px-3 py-1 text-xs font-medium text-text-muted ring-1 ring-border/70 sm:self-center">
          {totalLabel}
        </span>
      )}
    </div>
  )
}
