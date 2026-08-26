import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'

export interface AdminPaginationProps {
  pageNumber: number
  pageSize: number
  totalCount: number
  totalPages: number
  onPageChange: (page: number) => void
  onPageSizeChange?: (pageSize: number) => void
  pageSizeOptions?: number[]
  disabled?: boolean
}

const DEFAULT_PAGE_SIZES = [10, 20, 50, 100]

export function AdminPagination({
  pageNumber,
  pageSize,
  totalCount,
  totalPages,
  onPageChange,
  onPageSizeChange,
  pageSizeOptions = DEFAULT_PAGE_SIZES,
  disabled = false,
}: AdminPaginationProps) {
  const { t } = useTranslation()

  const canPrev = pageNumber > 1
  const canNext = pageNumber < totalPages
  const from = totalCount === 0 ? 0 : (pageNumber - 1) * pageSize + 1
  const to = Math.min(pageNumber * pageSize, totalCount)

  return (
    <div className="flex flex-col gap-3 border-t border-border bg-surface-muted/30 px-4 py-3 sm:flex-row sm:items-center sm:justify-between sm:px-5">
      <p className="text-xs text-text-muted">
        {totalCount === 0
          ? t('common.pagination.empty')
          : t('common.pagination.range', { from, to, total: totalCount })}
      </p>

      <div className="flex flex-wrap items-center gap-2">
        {onPageSizeChange && (
          <label className="flex items-center gap-2 text-xs text-text-muted">
            <span>{t('common.pagination.perPage')}</span>
            <select
              value={pageSize}
              disabled={disabled}
              onChange={(e) => onPageSizeChange(Number(e.target.value))}
              className="rounded-sm border border-border bg-surface px-2 py-1.5 text-xs text-text outline-none focus:border-warm"
            >
              {pageSizeOptions.map((size) => (
                <option key={size} value={size}>{size}</option>
              ))}
            </select>
          </label>
        )}

        <div className="flex items-center gap-1">
          <button
            type="button"
            disabled={disabled || !canPrev}
            onClick={() => onPageChange(pageNumber - 1)}
            aria-label={t('common.pagination.previous')}
            className="flex size-8 items-center justify-center rounded-sm border border-border bg-surface text-text-muted transition-colors enabled:hover:border-warm/40 enabled:hover:bg-warm-soft enabled:hover:text-warm disabled:opacity-40"
          >
            <ChevronIcon expanded={false} className="ltr:-rotate-180 rtl:rotate-180" />
          </button>

          <span className="min-w-[5.5rem] px-2 text-center text-xs font-medium text-text">
            {t('common.pagination.pageOf', { page: pageNumber, total: totalPages })}
          </span>

          <button
            type="button"
            disabled={disabled || !canNext}
            onClick={() => onPageChange(pageNumber + 1)}
            aria-label={t('common.pagination.next')}
            className="flex size-8 items-center justify-center rounded-sm border border-border bg-surface text-text-muted transition-colors enabled:hover:border-warm/40 enabled:hover:bg-warm-soft enabled:hover:text-warm disabled:opacity-40"
          >
            <ChevronIcon expanded={false} />
          </button>
        </div>
      </div>
    </div>
  )
}
