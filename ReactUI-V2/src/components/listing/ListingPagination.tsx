import { useCallback, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'

interface ListingPaginationProps {
  page: number
  pageSize: number
  totalCount: number
  onPageChange: (page: number) => void
}

function ChevronStartIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden className="rtl:rotate-180">
      <path
        d="M14 6l-6 6 6 6"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function ChevronEndIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden className="rtl:rotate-180">
      <path
        d="M10 6l6 6-6 6"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

export function ListingPagination({
  page,
  pageSize,
  totalCount,
  onPageChange,
}: ListingPaginationProps) {
  const { t } = useTranslation()
  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize))

  const goToPage = useCallback(
    (nextPage: number) => {
      onPageChange(nextPage)
      window.scrollTo({ top: 0, behavior: 'smooth' })
    },
    [onPageChange],
  )

  if (totalPages <= 1) return null

  const from = (page - 1) * pageSize + 1
  const to = Math.min(page * pageSize, totalCount)
  const pages = buildPageNumbers(page, totalPages)

  return (
    <section aria-label={t('listing.paginationLabel')} className="mt-10 border-t border-border/50 pt-8">
      <p className="mb-5 text-center text-xs leading-relaxed text-text-muted sm:text-sm">
        <span className="font-semibold tabular-nums text-text">
          {t('listing.paginationRange', { from, to })}
        </span>
        <span className="mx-1.5 text-border-strong" aria-hidden>
          ·
        </span>
        <span>{t('listing.paginationTotal', { total: totalCount })}</span>
        <span className="mx-1.5 hidden text-border-strong sm:inline" aria-hidden>
          ·
        </span>
        <span className="hidden sm:inline">{t('listing.paginationPageOf', { page, totalPages })}</span>
      </p>

      <nav className="flex items-center justify-center gap-2 sm:gap-3">
        <NavIconButton
          label={t('listing.paginationPrevious')}
          disabled={page <= 1}
          onClick={() => goToPage(page - 1)}
        >
          <ChevronStartIcon />
        </NavIconButton>

        <ol className="inline-flex max-w-full items-center gap-0.5 overflow-x-auto rounded-full bg-surface-muted/35 p-1 ring-1 ring-border/45 [scrollbar-width:none] sm:gap-1 sm:p-1.5 [&::-webkit-scrollbar]:hidden">
          {pages.map((item, index) =>
            item === '…' ? (
              <li
                key={`ellipsis-${index}`}
                className="flex size-8 shrink-0 items-center justify-center text-sm text-text-muted sm:size-9"
                aria-hidden
              >
                …
              </li>
            ) : (
              <li key={item} className="shrink-0">
                <button
                  type="button"
                  onClick={() => goToPage(item)}
                  aria-current={item === page ? 'page' : undefined}
                  aria-label={t('listing.paginationGoToPage', { page: item })}
                  className={`flex size-8 items-center justify-center rounded-full text-xs font-semibold tabular-nums transition-all sm:size-9 sm:text-sm ${
                    item === page
                      ? 'bg-surface text-text shadow-sm ring-1 ring-border/60'
                      : 'text-text-muted hover:bg-surface/70 hover:text-text'
                  }`}
                >
                  {item}
                </button>
              </li>
            ),
          )}
        </ol>

        <NavIconButton
          label={t('listing.paginationNext')}
          disabled={page >= totalPages}
          onClick={() => goToPage(page + 1)}
        >
          <ChevronEndIcon />
        </NavIconButton>
      </nav>

      <p className="mt-3 text-center text-[11px] text-text-muted sm:hidden">
        {t('listing.paginationPageOf', { page, totalPages })}
      </p>

      {page < totalPages && (
        <div className="mt-5 flex justify-center">
          <button
            type="button"
            onClick={() => goToPage(page + 1)}
            className="group inline-flex items-center gap-1.5 rounded-full px-4 py-2 text-xs font-semibold text-warm transition-colors hover:bg-warm-soft/50 sm:text-sm"
          >
            {t('listing.paginationContinue')}
            <ChevronEndIcon />
          </button>
        </div>
      )}
    </section>
  )
}

function NavIconButton({
  children,
  label,
  disabled,
  onClick,
}: {
  children: ReactNode
  label: string
  disabled: boolean
  onClick: () => void
}) {
  return (
    <button
      type="button"
      aria-label={label}
      disabled={disabled}
      onClick={onClick}
      className="flex size-9 shrink-0 items-center justify-center rounded-full border border-border/60 bg-surface text-text-muted transition-all hover:border-warm/30 hover:bg-warm-soft/35 hover:text-warm disabled:cursor-not-allowed disabled:opacity-35 disabled:hover:border-border/60 disabled:hover:bg-surface disabled:hover:text-text-muted sm:size-10"
    >
      {children}
    </button>
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
