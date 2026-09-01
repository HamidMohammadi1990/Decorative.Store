import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'

interface ListingPaginationProps {
  page: number
  pageSize: number
  totalCount: number
  onPageChange: (page: number) => void
}

export function ListingPagination({
  page,
  pageSize,
  totalCount,
  onPageChange,
}: ListingPaginationProps) {
  const { t } = useTranslation()
  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize))

  if (totalPages <= 1) return null

  const pages = buildPageNumbers(page, totalPages)

  return (
    <nav
      aria-label={t('listing.paginationLabel')}
      className="mt-10 flex flex-wrap items-center justify-center gap-2 rounded-xl border border-border/60 bg-surface px-4 py-4 shadow-sm"
    >
      <Button
        variant="secondary"
        className="rounded-lg px-3 py-2 text-xs"
        disabled={page <= 1}
        onClick={() => onPageChange(page - 1)}
      >
        {t('listing.paginationPrevious')}
      </Button>

      {pages.map((item, index) =>
        item === '…' ? (
          <span key={`ellipsis-${index}`} className="px-2 text-sm text-text-muted">
            …
          </span>
        ) : (
          <button
            key={item}
            type="button"
            onClick={() => onPageChange(item)}
            aria-current={item === page ? 'page' : undefined}
            className={`min-w-9 rounded-lg px-3 py-2 text-xs font-semibold transition-all ${
              item === page
                ? 'bg-warm text-warm-text shadow-sm'
                : 'border border-border/70 bg-surface text-text hover:border-warm/35 hover:text-warm'
            }`}
          >
            {item}
          </button>
        ),
      )}

      <Button
        variant="secondary"
        className="rounded-lg px-3 py-2 text-xs"
        disabled={page >= totalPages}
        onClick={() => onPageChange(page + 1)}
      >
        {t('listing.paginationNext')}
      </Button>
    </nav>
  )
}

function buildPageNumbers(current: number, total: number): Array<number | '…'> {
  if (total <= 7) {
    return Array.from({ length: total }, (_, index) => index + 1)
  }

  const pages: Array<number | '…'> = [1]

  if (current > 3) pages.push('…')

  const start = Math.max(2, current - 1)
  const end = Math.min(total - 1, current + 1)

  for (let pageNum = start; pageNum <= end; pageNum += 1) {
    pages.push(pageNum)
  }

  if (current < total - 2) pages.push('…')
  pages.push(total)

  return pages
}
