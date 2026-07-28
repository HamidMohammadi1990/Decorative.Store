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
}

export function ListingSidebar({
  facets,
  activeFilters,
  onToggle,
  onApplyPriceRange,
  onClear,
  className = '',
}: ListingSidebarProps) {
  const { t } = useTranslation()
  const hasActive = Object.values(activeFilters).some((values) => values.length > 0)

  return (
    <aside className={className}>
      <div className="flex items-center justify-between border-b border-border pb-4">
        <h2 className="text-sm font-semibold uppercase tracking-wider text-text">
          {t('listing.filters')}
        </h2>
        {hasActive && (
          <button
            type="button"
            onClick={onClear}
            className="text-xs font-medium text-warm transition-colors hover:underline"
          >
            {t('listing.clearFilters')}
          </button>
        )}
      </div>

      <div>
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
    </aside>
  )
}
