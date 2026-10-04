import { useTranslation } from 'react-i18next'
import type { FilterFacet } from '@/models/catalog/listing.model'
import { FilterCollapsiblePanel } from '@/components/listing/FilterCollapsiblePanel'
import { FilterFacetIcon } from '@/components/listing/FilterFacetIcon'
import { FilterGroup } from '@/components/listing/FilterGroup'
import { getPriceFacetActiveCount, PriceRangeGroup } from '@/components/listing/PriceRangeGroup'

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

function getFacetActiveCount(
  facet: FilterFacet,
  activeFilters: Record<string, string[]>,
): number {
  if (facet.type === 'range') {
    return getPriceFacetActiveCount(facet, activeFilters.price ?? [])
  }

  return (activeFilters[facet.id] ?? []).length
}

function shouldExpandFacetByDefault(
  facet: FilterFacet,
  activeFilters: Record<string, string[]>,
  index: number,
): boolean {
  if (getFacetActiveCount(facet, activeFilters) > 0) return true
  if (facet.type === 'range') return true
  return index === 0
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
    <div className="shrink-0 overflow-hidden rounded-xl border border-border/70 bg-surface px-4 py-4 shadow-sm ring-1 ring-black/[0.02]">
      <div className="flex items-center justify-between gap-3">
        <div className="flex items-center gap-2.5">
          <span className="flex size-9 items-center justify-center rounded-lg bg-warm-soft text-warm">
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
    </div>
  )

  const filterBody = (
    <div className="flex min-w-0 flex-col gap-3">
      {facets.map((facet, index) => {
        const activeFacetCount = getFacetActiveCount(facet, activeFilters)
        const defaultExpanded = shouldExpandFacetByDefault(facet, activeFilters, index)

        return (
          <FilterCollapsiblePanel
            key={facet.id}
            title={facet.label}
            icon={<FilterFacetIcon facetId={facet.id} facetType={facet.type} />}
            activeCount={activeFacetCount}
            defaultExpanded={defaultExpanded}
          >
            {facet.type === 'range' ? (
              <PriceRangeGroup
                facet={facet}
                activeBucketValues={activeFilters.price ?? []}
                onToggleBucket={onToggle}
                onApplyRange={onApplyPriceRange}
              />
            ) : (
              <FilterGroup
                facet={facet}
                activeValues={activeFilters[facet.id] ?? []}
                onToggle={onToggle}
              />
            )}
          </FilterCollapsiblePanel>
        )
      })}
    </div>
  )

  if (embedded) {
    return (
      <div className={`flex min-h-0 flex-col gap-3 ${className}`}>
        {filterHeader}
        <div className="min-h-0 flex-1 overflow-x-hidden overflow-y-auto overscroll-contain">
          {filterBody}
        </div>
      </div>
    )
  }

  return (
    <aside className={className}>
      <div className="sticky top-24 flex max-h-[calc(100vh-6rem)] min-w-0 flex-col gap-3 overflow-hidden">
        {filterHeader}
        <div className="min-h-0 min-w-0 flex-1 overflow-x-hidden overflow-y-auto overscroll-contain pe-0.5 [scrollbar-gutter:stable]">
          {filterBody}
        </div>
      </div>
    </aside>
  )
}
