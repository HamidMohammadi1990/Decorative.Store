import type { ReactNode } from 'react'
import type { FilterFacet } from '@/models/catalog/listing.model'
import { getColorSwatch } from '@/extensions/colorSwatches'

interface FilterGroupProps {
  facet: FilterFacet
  activeValues: string[]
  onToggle: (facetId: string, value: string) => void
}

function isActiveValue(activeValues: string[], value: string) {
  const normalized = value.toLowerCase()
  return activeValues.some((active) => active.toLowerCase() === normalized)
}

function FilterOptionButton({
  checked,
  onClick,
  children,
  className = '',
}: {
  checked: boolean
  onClick: () => void
  children: ReactNode
  className?: string
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      aria-pressed={checked}
      className={`flex w-full items-center justify-between gap-2 rounded-lg border px-3 py-2.5 text-start text-sm transition-all duration-200 ${
        checked
          ? 'border-warm/50 bg-warm-soft/90 text-text shadow-sm ring-1 ring-warm/15'
          : 'border-border/70 bg-surface hover:border-warm/25 hover:bg-surface-muted/60'
      } ${className}`}
    >
      {children}
    </button>
  )
}

function CountBadge({ count }: { count: number }) {
  return (
    <span className="shrink-0 rounded-full bg-surface-muted px-2 py-0.5 text-[10px] font-medium tabular-nums text-text-muted">
      {count}
    </span>
  )
}

export function FilterGroup({ facet, activeValues, onToggle }: FilterGroupProps) {
  const isColor = facet.type === 'color'

  return (
    <div className="border-b border-border/60 px-4 py-4 last:border-b-0">
      <h3 className="mb-3 text-xs font-semibold uppercase tracking-[0.14em] text-text-muted">
        {facet.label}
      </h3>

      {isColor ? (
        <ul className="grid grid-cols-2 gap-2">
          {facet.options.map((option) => {
            const checked = isActiveValue(activeValues, option.value)
            const swatch = option.swatch ?? getColorSwatch(option.value)

            return (
              <li key={option.value}>
                <FilterOptionButton
                  checked={checked}
                  onClick={() => onToggle(facet.id, option.value)}
                  className="!px-2.5 !py-2"
                >
                  <span className="flex min-w-0 flex-1 items-center gap-2">
                    <span
                      aria-hidden
                      className={`size-6 shrink-0 rounded-full border-2 shadow-inner transition-transform ${
                        checked ? 'border-warm scale-105' : 'border-border-strong'
                      }`}
                      style={{ backgroundColor: swatch }}
                    />
                    <span className="truncate text-xs font-medium">{option.label}</span>
                  </span>
                  <CountBadge count={option.count} />
                </FilterOptionButton>
              </li>
            )
          })}
        </ul>
      ) : (
        <ul className="space-y-2">
          {facet.options.map((option) => {
            const checked = isActiveValue(activeValues, option.value)

            return (
              <li key={option.value}>
                <FilterOptionButton
                  checked={checked}
                  onClick={() => onToggle(facet.id, option.value)}
                >
                  <span className="flex items-center gap-2.5">
                    <span
                      aria-hidden
                      className={`flex size-4 shrink-0 items-center justify-center rounded border transition-colors ${
                        checked
                          ? 'border-warm bg-warm text-[10px] text-warm-text'
                          : 'border-border bg-surface'
                      }`}
                    >
                      {checked ? '✓' : null}
                    </span>
                    <span className="font-medium">{option.label}</span>
                  </span>
                  <CountBadge count={option.count} />
                </FilterOptionButton>
              </li>
            )
          })}
        </ul>
      )}
    </div>
  )
}
