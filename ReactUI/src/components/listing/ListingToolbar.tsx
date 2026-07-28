import { useTranslation } from 'react-i18next'
import type { SortOption } from '@/models/catalog/listing.model'

interface ListingToolbarProps {
  totalCount: number
  sortOptions: SortOption[]
  currentSort: string
  onSortChange: (sort: string) => void
  onOpenFilters?: () => void
}

export function ListingToolbar({
  totalCount,
  sortOptions,
  currentSort,
  onSortChange,
  onOpenFilters,
}: ListingToolbarProps) {
  const { t } = useTranslation()

  return (
    <div className="mb-6 flex flex-wrap items-center justify-between gap-4 border-b border-border pb-4">
      <p className="text-sm text-text-muted">
        {t('listing.resultCount', { count: totalCount })}
      </p>

      <div className="flex items-center gap-3">
        {onOpenFilters && (
          <button
            type="button"
            onClick={onOpenFilters}
            className="rounded-sm border border-border px-3 py-2 text-sm font-medium text-text transition-colors hover:border-warm hover:text-warm lg:hidden"
          >
            {t('listing.filters')}
          </button>
        )}

        <label className="flex items-center gap-2 text-sm text-text-muted">
          <span className="hidden sm:inline">{t('listing.sortBy')}</span>
          <select
            value={currentSort}
            onChange={(e) => onSortChange(e.target.value)}
            className="rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text outline-none focus:border-warm"
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
