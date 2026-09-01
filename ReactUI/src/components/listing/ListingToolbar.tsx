import { useTranslation } from 'react-i18next'
import type { SortOption } from '@/models/catalog/listing.model'

interface ListingToolbarProps {
  totalCount: number
  sortOptions: SortOption[]
  currentSort: string
  onSortChange: (sort: string) => void
  onOpenFilters?: () => void
  activeFilterCount?: number
}

function SlidersIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 6h16M7 12h10M10 18h4"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
    </svg>
  )
}

export function ListingToolbar({
  totalCount,
  sortOptions,
  currentSort,
  onSortChange,
  onOpenFilters,
  activeFilterCount = 0,
}: ListingToolbarProps) {
  const { t } = useTranslation()

  return (
    <div className="mb-5 flex flex-wrap items-center justify-between gap-3 rounded-xl border border-border/70 bg-surface px-4 py-3 shadow-sm ring-1 ring-black/[0.02] sm:px-5">
      <div>
        <p className="text-sm font-semibold text-text">
          {t('listing.resultCount', { count: totalCount })}
        </p>
        {activeFilterCount > 0 && (
          <p className="mt-0.5 text-xs text-text-muted">
            {t('listing.activeFilters', { count: activeFilterCount })}
          </p>
        )}
      </div>

      <div className="flex items-center gap-2 sm:gap-3">
        {onOpenFilters && (
          <button
            type="button"
            onClick={onOpenFilters}
            className="inline-flex items-center gap-2 rounded-lg border border-border/80 bg-surface px-3 py-2 text-sm font-medium text-text transition-colors hover:border-warm/40 hover:bg-warm-soft/40 hover:text-warm lg:hidden"
          >
            <SlidersIcon />
            {t('listing.filters')}
            {activeFilterCount > 0 && (
              <span className="rounded-full bg-warm px-1.5 py-0.5 text-[10px] font-bold text-warm-text">
                {activeFilterCount}
              </span>
            )}
          </button>
        )}

        <label className="flex items-center gap-2 text-sm text-text-muted">
          <span className="hidden sm:inline">{t('listing.sortBy')}</span>
          <select
            value={currentSort}
            onChange={(e) => onSortChange(e.target.value)}
            className="cursor-pointer rounded-lg border border-border/80 bg-surface-muted/50 px-3 py-2 text-sm font-medium text-text outline-none transition-colors hover:border-warm/30 focus:border-warm focus:ring-2 focus:ring-warm/15"
          >
            {sortOptions.map((option) => (
              <option key={option.id} value={option.id}>
                {option.label}
              </option>
            ))}
          </select>
        </label>
      </div>
    </div>
  )
}
