import { useTranslation } from 'react-i18next'
import type { FilterFacet } from '@/models/catalog/listing.model'
import { FilterGroup } from '@/components/listing/FilterGroup'
import { PriceRangeGroup } from '@/components/listing/PriceRangeGroup'

interface ListingSidebarProps {
  facets: FilterFacet[]
  activeFilters: Record<string, string[]>
  onToggle: (facetId: string, value: string) => void
  onApplyPriceRange: (min?: number, max?: number) => void
  onClear: () => void
  className?: string
  embedded?: boolean
}

function FilterIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 6h16M7 12h10M10 18h4"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
    </svg>
  )
}

export function ListingSidebar({
  facets,
  activeFilters,
  onToggle,
  onApplyPriceRange,
  onClear,
  className = '',
  embedded = false,
}: ListingSidebarProps) {
  const { t } = useTranslation()
  const activeCount = Object.values(activeFilters).reduce((sum, values) => sum + values.length, 0)
  const hasActive = activeCount > 0

  const filterHeader = (
    <div className="flex shrink-0 items-center justify-between gap-3 border-b border-border/60 px-4 py-4">
      <div className="flex items-center gap-2.5">
        <span className="flex size-8 items-center justify-center rounded-lg bg-warm-soft text-warm">
          <FilterIcon />
        </span>
        <div>
          <h2 className="text-sm font-semibold text-text">{t('listing.filters')}</h2>
          {hasActive && (
            <p className="text-[11px] text-text-muted">
              {t('listing.activeFilters', { count: activeCount })}
            </p>
          )}
        </div>
      </div>
      {hasActive && (
        <button
          type="button"
          onClick={onClear}
          className="shrink-0 rounded-lg border border-border/80 px-2.5 py-1.5 text-[11px] font-semibold text-warm transition-colors hover:border-warm/40 hover:bg-warm-soft/50"
        >
          {t('listing.clearFilters')}
        </button>
      )}
    </div>
  )

  const filterBody = (
    <div className="divide-y divide-border/40">
      {facets.map((facet) => {
        if (facet.type === 'range') {
          return (
            <PriceRangeGroup
              key={facet.id}
              facet={facet}
              activeBucketValues={activeFilters.price ?? []}
              onToggleBucket={onToggle}
              onApplyRange={onApplyPriceRange}
            />
          )
        }

        return (
          <FilterGroup
            key={facet.id}
            facet={facet}
            activeValues={activeFilters[facet.id] ?? []}
            onToggle={onToggle}
          />
        )
      })}
    </div>
  )

  if (embedded) {
    return (
      <div className={`flex min-h-0 flex-col ${className}`}>
        {filterHeader}
        <div className="min-h-0 flex-1 overflow-y-auto overscroll-contain">{filterBody}</div>
      </div>
    )
  }

  return (
    <aside className={className}>
      <div className="sticky top-24 flex max-h-[calc(100vh-6rem)] flex-col overflow-hidden rounded-xl border border-border/80 bg-surface shadow-sm ring-1 ring-black/[0.03]">
        {filterHeader}
        <div className="min-h-0 flex-1 overflow-y-auto overscroll-contain [scrollbar-gutter:stable]">
          {filterBody}
        </div>
      </div>
    </aside>
  )
}
