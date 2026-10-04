import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ReviewStatusBadge } from '@/components/dashboard/StatusBadge'
import { ReviewsIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useMyReviews } from '@/hooks/useMyReviews'

function StarRating({ rating }: { rating: number }) {
  return (
    <div className="flex gap-0.5" aria-label={`${rating} stars`}>
      {Array.from({ length: 5 }, (_, i) => (
        <svg
          key={i}
          width="14"
          height="14"
          viewBox="0 0 14 14"
          aria-hidden
          className={i < rating ? 'text-warm' : 'text-border'}
        >
          <path
            d="M7 1.5 8.6 5.1l3.9.4-3 2.6.9 3.8L7 10.1 3.6 11.9l.9-3.8-3-2.6 3.9-.4L7 1.5Z"
            fill="currentColor"
          />
        </svg>
      ))}
    </div>
  )
}

export function ReviewsPanel() {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const { reviews, loading, error, reload } = useMyReviews()

  if (loading) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.reviews.title')}
          description={t('dashboard.reviews.description')}
          icon={<ReviewsIcon size={22} />}
        />
        <InlineLoading label={t('common.loading')} />
      </div>
    )
  }

  if (error) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.reviews.title')}
          description={t('dashboard.reviews.description')}
          icon={<ReviewsIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<ReviewsIcon size={28} />}
          title={t('dashboard.reviews.loadFailedTitle')}
          message={t('dashboard.reviews.loadFailedMessage')}
          action={
            <Button variant="warm" onClick={() => void reload()}>
              {t('common.retry')}
            </Button>
          }
        />
      </div>
    )
  }

  if (reviews.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.reviews.title')}
          description={t('dashboard.reviews.description')}
          icon={<ReviewsIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<ReviewsIcon size={28} />}
          title={t('dashboard.reviews.emptyTitle')}
          message={t('dashboard.reviews.emptyMessage')}
        />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.reviews.title')}
        description={t('dashboard.reviews.description')}
        icon={<ReviewsIcon size={22} />}
      />

      <div className="space-y-4">
        {reviews.map((review) => (
          <article
            key={review.id}
            className="relative overflow-hidden rounded-sm border border-border bg-surface p-5 shadow-sm transition-shadow duration-200 hover:shadow-md sm:p-6"
          >
            <div
              aria-hidden
              className="absolute inset-y-0 start-0 w-1 bg-gradient-to-b from-warm to-warm-hover"
            />
            <div className="flex flex-wrap items-start justify-between gap-3 ps-2">
              <div>
                {review.productSlug ? (
                  <Link
                    to={`/product/${review.productSlug}`}
                    className="text-sm font-semibold text-text transition-colors hover:text-warm"
                  >
                    {review.productTitle}
                  </Link>
                ) : (
                  <p className="text-sm font-semibold text-text">{review.productTitle}</p>
                )}
                <div className="mt-2 flex flex-wrap items-center gap-3">
                  <StarRating rating={review.rating} />
                  {review.date && (
                    <span className="text-xs text-text-muted">
                      {formatBlogDate(review.date, locale)}
                    </span>
                  )}
                </div>
              </div>
              <ReviewStatusBadge status={review.status} />
            </div>
            {review.comment && (
              <p className="mt-4 border-s-2 border-warm-soft ps-4 text-sm leading-relaxed text-text-muted">
                {review.comment}
              </p>
            )}
          </article>
        ))}
      </div>
    </div>
  )
}
