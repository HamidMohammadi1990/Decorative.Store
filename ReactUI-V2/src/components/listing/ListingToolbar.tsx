import { useTranslation } from 'react-i18next'
import type { SortOption } from '@/models/catalog/listing.model'
import { ChevronIcon } from '@/components/ui/ChevronIcon'

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
    <svg width="15" height="15" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 6h16M7 12h10M10 18h4"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
    </svg>
  )
}

function SortIcon() {
  return (
    <svg width="15" height="15" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 7h12M4 12h8M4 17h4"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
      <path
        d="M18 7v10M18 17l2.5-2.5M18 17l-2.5-2.5"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
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
  const currentSortLabel =
    sortOptions.find((option) => option.id === currentSort)?.label ?? sortOptions[0]?.label

  return (
    <div className="mb-4 space-y-3">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div className="flex min-w-0 flex-wrap items-center gap-2.5">
          <div className="inline-flex items-center gap-2 rounded-full bg-surface-muted/45 px-3 py-1.5 ring-1 ring-border/40">
            <span className="text-base font-bold tabular-nums leading-none text-warm">
              {totalCount.toLocaleString()}
            </span>
            <span className="text-xs font-medium text-text-muted">{t('listing.productsLabel')}</span>
          </div>

          {activeFilterCount > 0 && (
            <span className="inline-flex items-center gap-1.5 rounded-full bg-warm-soft/60 px-2.5 py-1 text-[11px] font-semibold text-warm ring-1 ring-warm/15">
              <span className="size-1.5 rounded-full bg-warm" aria-hidden />
              {t('listing.activeFilters', { count: activeFilterCount })}
            </span>
          )}
        </div>

        <div className="flex shrink-0 items-center gap-2">
          {onOpenFilters && (
            <button
              type="button"
              onClick={onOpenFilters}
              className="inline-flex items-center gap-1.5 rounded-full border border-border/70 bg-surface px-3 py-2 text-xs font-semibold text-text transition-colors hover:border-warm/35 hover:bg-warm-soft/35 hover:text-warm lg:hidden"
            >
              <SlidersIcon />
              {t('listing.filters')}
              {activeFilterCount > 0 && (
                <span className="rounded-full bg-warm px-1.5 py-0.5 text-[10px] font-bold leading-none text-warm-text">
                  {activeFilterCount}
                </span>
              )}
            </button>
          )}

          <div className="relative">
            <label className="sr-only" htmlFor="listing-sort">
              {t('listing.sortBy')}
            </label>
            <span className="pointer-events-none absolute start-3 top-1/2 -translate-y-1/2 text-text-muted">
              <SortIcon />
            </span>
            <select
              id="listing-sort"
              value={currentSort}
              onChange={(event) => onSortChange(event.target.value)}
              className="max-w-[11.5rem] cursor-pointer appearance-none rounded-full border border-border/70 bg-surface py-2 pe-9 ps-9 text-xs font-semibold text-text outline-none transition-colors hover:border-warm/30 focus:border-warm focus:ring-2 focus:ring-warm/15 sm:max-w-none sm:min-w-[10.5rem]"
              aria-label={`${t('listing.sortBy')}: ${currentSortLabel}`}
            >
              {sortOptions.map((option) => (
                <option key={option.id} value={option.id}>
                  {option.label}
                </option>
              ))}
            </select>
            <ChevronIcon
              expanded={false}
              className="pointer-events-none absolute end-2.5 top-1/2 -translate-y-1/2 text-text-muted"
            />
          </div>
        </div>
      </div>

      <div className="h-px bg-gradient-to-r from-transparent via-border/70 to-transparent" aria-hidden />
    </div>
  )
}
