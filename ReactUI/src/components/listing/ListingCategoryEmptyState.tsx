import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'

interface ListingCategoryEmptyStateProps {
  categoryTitle: string
}

export function ListingCategoryEmptyState({ categoryTitle }: ListingCategoryEmptyStateProps) {
  const { t } = useTranslation()

  return (
    <section
      className="relative overflow-hidden rounded-2xl border border-border/80 bg-gradient-to-br from-surface via-surface-muted/30 to-warm-soft/25 px-6 py-14 text-center shadow-sm md:px-12 md:py-20"
      aria-labelledby="listing-category-empty-title"
    >
      <div
        aria-hidden
        className="pointer-events-none absolute -end-16 -top-16 size-48 rounded-full bg-warm/10 blur-3xl"
      />
      <div
        aria-hidden
        className="pointer-events-none absolute -start-12 bottom-0 size-40 rounded-full bg-accent/10 blur-3xl"
      />

      <div className="relative mx-auto max-w-lg">
        <span className="mx-auto flex size-16 items-center justify-center rounded-full bg-warm-soft text-warm ring-4 ring-warm/10 md:size-20">
          <EmptyShelfIcon className="size-8 md:size-9" />
        </span>

        <p className="mt-6 text-xs font-semibold uppercase tracking-[0.16em] text-warm">
          {t('listing.categoryEmptyEyebrow')}
        </p>
        <h2
          id="listing-category-empty-title"
          className="mt-3 text-xl font-semibold tracking-tight text-text md:text-2xl"
        >
          {t('listing.categoryEmptyTitle', { category: categoryTitle })}
        </h2>
        <p className="mt-3 text-sm leading-relaxed text-text-muted md:text-base">
          {t('listing.categoryEmptyMessage')}
        </p>

        <div className="mt-8 flex flex-col items-center justify-center gap-3 sm:flex-row">
          <Link to="/">
            <Button variant="warm">{t('listing.categoryEmptyBrowseHome')}</Button>
          </Link>
          <Link to="/contact">
            <Button variant="secondary">{t('listing.categoryEmptyContact')}</Button>
          </Link>
        </div>
      </div>
    </section>
  )
}

function EmptyShelfIcon({ className }: { className?: string }) {
  return (
    <svg className={className} viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 7h16M4 12h16M4 17h10"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <rect x="3" y="4" width="18" height="16" rx="2" stroke="currentColor" strokeWidth="1.5" />
    </svg>
  )
}
