import { useTranslation } from 'react-i18next'
import type { OrderSortOption, OrderStatusOption } from '@/models/dashboard/dashboard.model'

interface OrderFiltersBarProps {
  statuses: OrderStatusOption[]
  selectedStatusId: number | null
  query: string
  sort: OrderSortOption
  resultCount: number
  onStatusChange: (statusId: number) => void
  onQueryChange: (query: string) => void
  onSortChange: (sort: OrderSortOption) => void
}

const SORT_OPTIONS: OrderSortOption[] = ['newest', 'oldest', 'amountHigh', 'amountLow']

export function OrderFiltersBar({
  statuses,
  selectedStatusId,
  query,
  sort,
  resultCount,
  onStatusChange,
  onQueryChange,
  onSortChange,
}: OrderFiltersBarProps) {
  const { t } = useTranslation()

  return (
    <div className="mb-6 space-y-4">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <p className="text-sm text-text-muted">
          {t('dashboard.orders.resultCount', { count: resultCount })}
        </p>
        <label className="flex items-center gap-2 text-sm text-text-muted">
          <span className="hidden sm:inline">{t('dashboard.orders.sortBy')}</span>
          <select
            value={sort}
            onChange={(e) => onSortChange(e.target.value as OrderSortOption)}
            className="rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text outline-none focus:border-warm"
          >
            {SORT_OPTIONS.map((option) => (
              <option key={option} value={option}>
                {t(`dashboard.orders.sort.${option}`)}
              </option>
            ))}
          </select>
        </label>
      </div>

      <input
        type="search"
        value={query}
        onChange={(e) => onQueryChange(e.target.value)}
        placeholder={t('dashboard.orders.searchPlaceholder')}
        className="w-full rounded-sm border border-border bg-surface px-4 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm"
      />

      <div
        role="tablist"
        aria-label={t('dashboard.orders.filterByStatus')}
        className="scrollbar-none -mx-1 flex gap-2 overflow-x-auto px-1 pb-1"
      >
        {statuses.map((status) => {
          const isActive = selectedStatusId === status.id

          return (
            <button
              key={status.id}
              type="button"
              role="tab"
              aria-selected={isActive}
              onClick={() => onStatusChange(status.id)}
              className={`inline-flex shrink-0 items-center gap-2 rounded-sm border px-3.5 py-2 text-xs font-semibold transition-all duration-150 ${
                isActive
                  ? 'border-warm bg-warm text-warm-text shadow-sm'
                  : 'border-border bg-surface text-text-muted hover:border-warm/40 hover:bg-warm-soft hover:text-warm'
              }`}
            >
              {status.title}
            </button>
          )
        })}
      </div>
    </div>
  )
}
