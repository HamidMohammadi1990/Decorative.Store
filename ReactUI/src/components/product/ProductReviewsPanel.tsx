import { useCallback, useMemo, useState } from 'react'
import type { TFunction } from 'i18next'
import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { ReviewSubmitModal } from '@/components/product/ReviewSubmitModal'
import type { ProductReviewItem } from '@/extensions/productReviews'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import { useProductReviews } from '@/hooks/useProductReviews'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { submitProductReview } from '@/services/reviewSubmitService'
import { openLoginModal } from '@/stores/authModalStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken } from '@/stores/userStore'

interface ProductReviewsPanelProps {
  product: ProductDetail
}

type SortId = 'newest' | 'buyers' | 'useful'

const INITIAL_VISIBLE = 5
const MOBILE_TEXT_LIMIT = 110

function computeReviewStats(reviews: ProductReviewItem[]) {
  if (reviews.length === 0) {
    return { ratingDisplay: '—', ratingValue: 0, total: 0 }
  }

  const ratingValue = reviews.reduce((sum, review) => sum + review.rating, 0) / reviews.length

  return {
    ratingDisplay: ratingValue >= 4.95 ? '5' : ratingValue.toFixed(1),
    ratingValue,
    total: reviews.length,
  }
}

export function ProductReviewsPanel({ product }: ProductReviewsPanelProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useAccessToken()
  const mobileDrag = useHorizontalDragScroll<HTMLDivElement>()
  const { reviews: allReviews, loading, reload } = useProductReviews(product.id)
  const stats = computeReviewStats(allReviews)
  const buyerCount = allReviews.filter((review) => review.isBuyer).length

  const [sort, setSort] = useState<SortId>('useful')
  const [expanded, setExpanded] = useState(false)
  const [mobileShowAll, setMobileShowAll] = useState(false)
  const [reviewModalOpen, setReviewModalOpen] = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)

  const sortedReviews = useMemo(() => {
    const list = [...allReviews]
    if (sort === 'buyers') return list.filter((review) => review.isBuyer)
    if (sort === 'useful') return list.sort((a, b) => b.helpful - a.helpful)
    return list
  }, [allReviews, sort])

  const visibleReviews = expanded ? sortedReviews : sortedReviews.slice(0, INITIAL_VISIBLE)
  const hiddenCount = Math.max(0, sortedReviews.length - INITIAL_VISIBLE)

  const sortOptions: { id: SortId; label: string }[] = [
    { id: 'newest', label: t('product.sortNewest') },
    { id: 'buyers', label: t('product.sortBuyers') },
    { id: 'useful', label: t('product.sortUseful') },
  ]

  const openReviewModal = () => {
    if (!accessToken) {
      openLoginModal({
        onSuccess: () => {
          setSubmitError(null)
          setReviewModalOpen(true)
        },
      })
      return
    }
    setSubmitError(null)
    setReviewModalOpen(true)
  }

  const handleSubmitReview = useCallback(
    async (description: string, commentTopicId: string) => {
      setSubmitting(true)
      setSubmitError(null)

      try {
        await submitProductReview({
          product,
          locale,
          description,
          commentTopicId,
        })

        setReviewModalOpen(false)
        await reload()
      } catch {
        setSubmitError(t('product.reviewSubmitFailed'))
      } finally {
        setSubmitting(false)
      }
    },
    [locale, product, reload, t],
  )

  return (
    <div>
      <ReviewSubmitModal
        product={product}
        isOpen={reviewModalOpen}
        onClose={() => setReviewModalOpen(false)}
        onSubmit={handleSubmitReview}
        submitting={submitting}
        submitError={submitError}
      />

      {loading && (
        <p className="py-6 text-center text-sm text-text-muted">{t('common.loading')}</p>
      )}

      {!loading && sortedReviews.length === 0 && (
        <p className="py-6 text-center text-sm text-text-muted">{t('product.noReviewsYet')}</p>
      )}

      {!loading && sortedReviews.length > 0 && (
        <>
          {/* ── Mobile ── */}
          <div className="lg:hidden">
            <div className="flex items-start justify-between gap-3">
              <div>
                <h3 className="text-base font-bold text-text">{t('product.reviewsMobileTitle')}</h3>
                <p className="mt-1.5 flex flex-wrap items-center gap-1 text-sm text-text-muted">
                  <span className="text-base font-bold text-text">{stats.ratingDisplay}</span>
                  <Star filled size={14} />
                  <span>{t('product.basedOnBuyers', { count: buyerCount })}</span>
                </p>
              </div>
              <button
                type="button"
                onClick={() => setMobileShowAll((value) => !value)}
                className="inline-flex shrink-0 items-center gap-0.5 text-sm font-medium text-warm"
              >
                {t('product.viewAllReviews', { count: sortedReviews.length })}
                <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
              </button>
            </div>

            <button
              type="button"
              onClick={openReviewModal}
              className="mt-4 w-full rounded-md border border-warm bg-surface py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft"
            >
              {t('product.writeReview')}
            </button>

            {!mobileShowAll ? (
              <div
                ref={mobileDrag.ref}
                className={`-mx-4 mt-4 flex gap-3 overflow-x-auto px-4 pb-1 touch-pan-y select-none ${
                  mobileDrag.isGrabbing ? 'cursor-grabbing snap-none' : 'cursor-grab snap-x snap-mandatory'
                }`}
                onPointerDown={mobileDrag.onPointerDown}
                onPointerMove={mobileDrag.onPointerMove}
                onPointerUp={mobileDrag.onPointerUp}
                onLostPointerCapture={mobileDrag.onLostPointerCapture}
                onClickCapture={mobileDrag.onClickCapture}
                onDragStart={(event) => event.preventDefault()}
              >
                {sortedReviews.map((review) => (
                  <MobileReviewCard key={review.id} review={review} t={t} />
                ))}
              </div>
            ) : (
              <ul className="mt-4 divide-y divide-border">
                {sortedReviews.map((review) => (
                  <ReviewCard key={review.id} review={review} t={t} compact />
                ))}
              </ul>
            )}
          </div>

          {/* ── Desktop ── */}
          <div className="hidden lg:block">
            <h3 className="text-base font-bold text-text">
              {t('product.reviewsSectionTitle')}
              <span className="mt-2 block h-0.5 w-10 bg-warm" aria-hidden />
            </h3>

            <div className="mt-6 grid grid-cols-[15rem_minmax(0,1fr)] gap-10">
              <aside className="flex flex-col items-center border-s border-border ps-8 text-center">
                <p className="text-sm font-medium text-text">
                  <span className="text-3xl font-bold">{stats.ratingDisplay}</span>{' '}
                  {t('product.outOfFive')}
                </p>

                <div className="mt-2 flex justify-center gap-0.5" aria-hidden>
                  {Array.from({ length: 5 }).map((_, index) => (
                    <Star key={index} filled={index < Math.round(stats.ratingValue)} />
                  ))}
                </div>

                <p className="mt-2 text-xs text-text-muted">
                  {t('product.fromTotalRatings', { count: stats.total })}
                </p>

                <p className="mt-8 text-sm leading-relaxed text-text-muted">
                  {t('product.reviewInvite')}
                </p>

                <button
                  type="button"
                  onClick={openReviewModal}
                  className="mt-4 w-full rounded-md border border-warm bg-surface px-4 py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft"
                >
                  {t('product.writeReview')}
                </button>

                <p className="mt-4 flex items-start gap-1.5 text-start text-[11px] leading-relaxed text-text-muted">
                  <InfoIcon />
                  {t('product.reviewPointsHint')}
                </p>
              </aside>

              <div className="min-w-0">
                <div className="flex flex-wrap items-center justify-between gap-3 border-b border-border pb-3">
                  <div className="flex flex-wrap items-center gap-1 text-sm">
                    <span className="text-text-muted">{t('product.sortBy')}:</span>
                    {sortOptions.map((option, index) => (
                      <span key={option.id} className="inline-flex items-center">
                        {index > 0 && (
                          <span className="mx-1.5 text-border-strong" aria-hidden>
                            |
                          </span>
                        )}
                        <button
                          type="button"
                          onClick={() => setSort(option.id)}
                          className={
                            sort === option.id
                              ? 'font-semibold text-warm'
                              : 'text-text-muted transition-colors hover:text-text'
                          }
                        >
                          {option.label}
                        </button>
                      </span>
                    ))}
                  </div>
                  <span className="text-sm text-text-muted">
                    {t('product.reviewsTotal', { count: sortedReviews.length })}
                  </span>
                </div>

                <ul>
                  {visibleReviews.map((review) => (
                    <ReviewCard key={review.id} review={review} t={t} />
                  ))}
                </ul>

                {!expanded && hiddenCount > 0 && (
                  <button
                    type="button"
                    onClick={() => setExpanded(true)}
                    className="mt-2 flex w-full items-center justify-center gap-2 border-t border-border pt-4 text-sm font-medium text-warm hover:underline"
                  >
                    {t('product.moreReviews', { count: hiddenCount })}
                    <ChevronIcon expanded={false} className="text-warm" />
                  </button>
                )}
              </div>
            </div>
          </div>
        </>
      )}

      {!loading && sortedReviews.length === 0 && (
        <button
          type="button"
          onClick={openReviewModal}
          className="mt-4 w-full rounded-md border border-warm bg-surface py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft lg:max-w-xs"
        >
          {t('product.writeReview')}
        </button>
      )}
    </div>
  )
}

function MobileReviewCard({ review, t }: { review: ProductReviewItem; t: TFunction }) {
  const [showFull, setShowFull] = useState(false)
  const isLong = review.text.length > MOBILE_TEXT_LIMIT
  const displayText =
    showFull || !isLong ? review.text : `${review.text.slice(0, MOBILE_TEXT_LIMIT)}…`

  return (
    <article className="w-[calc(50%-0.375rem)] min-w-[calc(50%-0.375rem)] shrink-0 snap-start rounded-lg border border-border bg-surface p-3.5">
      <div className="flex items-start gap-2.5">
        <UserAvatar name={review.author} />
        <div className="min-w-0 flex-1">
          <div className="flex flex-wrap items-center gap-1.5">
            <span className="truncate text-xs font-semibold text-text">{review.author}</span>
            {review.isBuyer && (
              <span className="shrink-0 rounded bg-accent/10 px-1.5 py-0.5 text-[10px] font-medium text-accent">
                {t('product.buyerBadge')}
              </span>
            )}
          </div>
          <div className="mt-1.5 flex gap-0.5">
            {Array.from({ length: 5 }).map((_, index) => (
              <Star key={index} filled={index < review.rating} size={12} />
            ))}
          </div>
        </div>
      </div>

      <p className="mt-3 text-xs leading-relaxed text-text">{displayText}</p>

      {isLong && !showFull && (
        <button
          type="button"
          onClick={() => setShowFull(true)}
          className="mt-1 text-xs text-warm underline"
        >
          {t('product.seeMoreReview')}
        </button>
      )}

      <div className="mt-3 flex items-center justify-between gap-2 text-[11px] text-text-muted">
        <span>{review.date}</span>
        <div className="flex items-center gap-3">
          <span className="inline-flex items-center gap-1">
            <ThumbUpIcon />
            {review.helpful}
          </span>
          <span className="inline-flex items-center gap-1">
            <ThumbDownIcon />
            {review.notHelpful}
          </span>
        </div>
      </div>
    </article>
  )
}

function ReviewCard({
  review,
  t,
  compact = false,
}: {
  review: ProductReviewItem
  t: TFunction
  compact?: boolean
}) {
  return (
    <li className={`border-b border-border ${compact ? 'py-4' : 'py-5'} last:border-b-0`}>
      <div className="flex items-start justify-between gap-3">
        <div className="flex flex-wrap items-center gap-2">
          {!compact && <UserAvatar name={review.author} size="sm" />}
          <span className="text-sm font-semibold text-text">{review.author}</span>
          {review.isBuyer && (
            <span className="rounded-md bg-accent/10 px-2 py-0.5 text-[11px] font-medium text-accent">
              {t('product.buyerBadge')}
            </span>
          )}
        </div>
        <div className="flex shrink-0 items-center gap-2">
          <span className="text-xs text-text-muted">{review.date}</span>
          {!compact && (
            <button
              type="button"
              aria-label={t('product.reviewMenu')}
              className="rounded p-1 text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
            >
              <MoreIcon />
            </button>
          )}
        </div>
      </div>

      <div
        className="mt-2 flex gap-0.5"
        aria-label={t('product.ratingStars', { count: review.rating })}
      >
        {Array.from({ length: 5 }).map((_, index) => (
          <Star key={index} filled={index < review.rating} />
        ))}
      </div>

      <p className="mt-3 text-sm leading-[1.85] text-text">{review.text}</p>

      <div className="mt-4 flex flex-wrap items-center justify-between gap-3">
        <div className="flex flex-wrap items-center gap-4 text-xs text-text-muted">
          {!compact && (
            <button
              type="button"
              className="inline-flex items-center gap-1 transition-colors hover:text-text"
            >
              <QuoteIcon />
              {t('product.quote')}
            </button>
          )}
          {review.variant && (
            <span className="inline-flex items-center gap-1 rounded bg-surface-muted px-2 py-0.5 text-text-muted">
              <span className="size-2 rounded-full bg-border-strong" aria-hidden />
              {review.variant}
            </span>
          )}
        </div>

        <div className="flex items-center gap-4 text-xs text-text-muted">
          <button
            type="button"
            className="inline-flex items-center gap-1.5 transition-colors hover:text-text"
          >
            <ThumbUpIcon />
            {review.helpful}
          </button>
          <button
            type="button"
            className="inline-flex items-center gap-1.5 transition-colors hover:text-text"
          >
            <ThumbDownIcon />
            {review.notHelpful}
          </button>
        </div>
      </div>
    </li>
  )
}

function UserAvatar({ name, size = 'md' }: { name: string; size?: 'sm' | 'md' }) {
  const initial = name.trim().charAt(0) || '?'
  const dim = size === 'sm' ? 'size-7 text-xs' : 'size-9 text-sm'

  return (
    <span
      className={`inline-flex shrink-0 items-center justify-center rounded-full bg-amber-100 font-semibold text-amber-700 ${dim}`}
      aria-hidden
    >
      {initial}
    </span>
  )
}

function Star({ filled, size = 16 }: { filled: boolean; size?: number }) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 12 12"
      className={filled ? 'text-amber-400' : 'text-border-strong'}
      fill="currentColor"
      aria-hidden
    >
      <path d="M6 1.2l1.4 2.9 3.1.5-2.2 2.2.5 3.1L6 8.4 3.2 10l.5-3.1-2.2-2.2 3.1-.5L6 1.2z" />
    </svg>
  )
}

function InfoIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" className="mt-0.5 shrink-0 text-text-muted" aria-hidden>
      <circle cx="6" cy="6" r="5" stroke="currentColor" strokeWidth="1" fill="none" />
      <path d="M6 5v4M6 3.5h.01" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function MoreIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" aria-hidden>
      <circle cx="8" cy="3.5" r="1.2" fill="currentColor" />
      <circle cx="8" cy="8" r="1.2" fill="currentColor" />
      <circle cx="8" cy="12.5" r="1.2" fill="currentColor" />
    </svg>
  )
}

function QuoteIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M3 4h2v4H3V7.5C3 5.8 4 4.5 5.5 4M8 4h2v4H8V7.5C8 5.8 9 4.5 10.5 4"
        stroke="currentColor"
        strokeWidth="1"
        strokeLinecap="round"
      />
    </svg>
  )
}

function ThumbUpIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M4 10V5.5L5.5 3h2.8l.7 2.5H10v3.5H4zM2 5.5h1.5V10H2V5.5z"
        stroke="currentColor"
        strokeWidth="0.9"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function ThumbDownIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M4 2v4.5L5.5 9h2.8l.7-2.5H10V3H4zM2 6.5h1.5V2H2v4.5z"
        stroke="currentColor"
        strokeWidth="0.9"
        strokeLinejoin="round"
      />
    </svg>
  )
}
