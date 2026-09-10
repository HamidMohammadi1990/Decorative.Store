import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react'
import type { TFunction } from 'i18next'
import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { UserAvatar } from '@/components/ui/UserAvatar'
import { ReviewReplyModal } from '@/components/product/ReviewReplyModal'
import { ReviewSubmitModal } from '@/components/product/ReviewSubmitModal'
import {
  ProductSectionSortBar,
  type ProductSectionSortOption,
} from '@/components/product/ProductSectionSortBar'
import type { ProductReviewItem } from '@/extensions/productReviews'
import {
  applyVoteToReviewTree,
  buildReviewTree,
  computeReviewStatsFromItems,
  isMainReview,
  sortReviewRoots,
} from '@/extensions/productReviews'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { productCommentService } from '@/services/productCommentService'
import { submitProductReview } from '@/services/reviewSubmitService'
import { openLoginModal } from '@/stores/authModalStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken } from '@/stores/userStore'

interface ProductReviewsPanelProps {
  product: ProductDetail
  reviews: ProductReviewItem[]
  loading: boolean
  reload: () => Promise<void>
}

type SortId = 'newest' | 'buyers' | 'useful'

const INITIAL_VISIBLE = 5
const MOBILE_TEXT_LIMIT = 110

export function ProductReviewsPanel({
  product,
  reviews: flatReviews,
  loading,
  reload,
}: ProductReviewsPanelProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useAccessToken()
  const mobileDrag = useHorizontalDragScroll<HTMLDivElement>()

  const reviewTree = useMemo(() => buildReviewTree(flatReviews), [flatReviews])
  const [displayTree, setDisplayTree] = useState<ProductReviewItem[]>([])
  const stats = computeReviewStatsFromItems(flatReviews)
  const buyerCount = flatReviews.filter((review) => review.isBuyer && !review.parentId).length

  useEffect(() => {
    setDisplayTree(reviewTree)
  }, [reviewTree])

  const [sort, setSort] = useState<SortId>('newest')
  const [expanded, setExpanded] = useState(false)
  const [mobileShowAll, setMobileShowAll] = useState(false)
  const [reviewModalOpen, setReviewModalOpen] = useState(false)
  const [replyTarget, setReplyTarget] = useState<ProductReviewItem | null>(null)
  const [submitting, setSubmitting] = useState(false)
  const [replySubmitting, setReplySubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [replyError, setReplyError] = useState<string | null>(null)
  const [submitSuccess, setSubmitSuccess] = useState(false)
  const [replySubmitSuccess, setReplySubmitSuccess] = useState(false)

  const closeReviewModal = () => {
    setReviewModalOpen(false)
    setSubmitError(null)
    setSubmitSuccess(false)
  }

  const closeReplyModal = () => {
    setReplyTarget(null)
    setReplyError(null)
    setReplySubmitSuccess(false)
  }

  const sortedRoots = useMemo(() => sortReviewRoots(displayTree, sort), [displayTree, sort])
  const mainReviewCount = useMemo(
    () => displayTree.filter(isMainReview).length,
    [displayTree],
  )
  const visibleReviews = expanded ? sortedRoots : sortedRoots.slice(0, INITIAL_VISIBLE)
  const hiddenCount = Math.max(0, sortedRoots.length - INITIAL_VISIBLE)
  const hasReviews = flatReviews.length > 0
  const hasVisibleReviews = sortedRoots.some(isMainReview) || sortedRoots.some((r) => r.isStandaloneReply)

  const sortOptions: ProductSectionSortOption<SortId>[] = [
    { id: 'newest', label: t('product.sortNewest'), icon: <SortNewestIcon /> },
    { id: 'buyers', label: t('product.sortBuyers'), icon: <SortBuyersIcon /> },
    { id: 'useful', label: t('product.sortUseful'), icon: <SortUsefulIcon /> },
  ]

  const openReviewModal = () => {
    if (!accessToken) {
      openLoginModal({
        onSuccess: () => {
          setSubmitError(null)
          setSubmitSuccess(false)
          setReviewModalOpen(true)
        },
      })
      return
    }
    setSubmitError(null)
    setSubmitSuccess(false)
    setReviewModalOpen(true)
  }

  const handleSubmitReview = useCallback(
    async (payload: {
      description: string
      commentTopicId: string
      commentRate: number
      qualityRating: number
      affordableRating: number
    }) => {
      setSubmitting(true)
      setSubmitError(null)

      try {
        await submitProductReview({
          product,
          locale,
          ...payload,
        })

        setSubmitSuccess(true)
        setSubmitError(null)
        await reload()
      } catch {
        setSubmitSuccess(false)
        setSubmitError(t('product.reviewSubmitFailed'))
      } finally {
        setSubmitting(false)
      }
    },
    [locale, product, reload, t],
  )

  const handleReplySubmit = useCallback(
    async (description: string, target: ProductReviewItem) => {
      if (!target.id) return

      setReplySubmitting(true)
      setReplyError(null)

      try {
        await submitProductReview({
          product,
          locale,
          description,
          commentTopicId: target.commentTopicId,
          commentRate: 5,
          qualityRating: 5,
          affordableRating: 5,
          parentId: target.id,
        })

        setReplySubmitSuccess(true)
        setReplyError(null)
        await reload()
      } catch {
        setReplySubmitSuccess(false)
        setReplyError(t('product.reviewReplyFailed'))
      } finally {
        setReplySubmitting(false)
      }
    },
    [locale, product, reload, t],
  )

  const handleVote = useCallback(
    async (reviewId: string, isHelpful: boolean) => {
      if (!accessToken) {
        openLoginModal({
          onSuccess: () => void handleVote(reviewId, isHelpful),
        })
        return
      }

      try {
        const result = await productCommentService.vote(reviewId, isHelpful, locale, accessToken)
        setDisplayTree((prev) =>
          applyVoteToReviewTree(prev, reviewId, {
            helpfulCount: result.helpfulCount,
            notHelpfulCount: result.notHelpfulCount,
            userVoteHelpful: result.userVoteHelpful,
          }),
        )
      } catch {
        // ignore vote errors silently
      }
    },
    [accessToken, locale],
  )

  const openReply = (review: ProductReviewItem) => {
    if (!accessToken) {
      openLoginModal({
        onSuccess: () => {
          setReplyError(null)
          setReplySubmitSuccess(false)
          setReplyTarget(review)
        },
      })
      return
    }
    setReplyError(null)
    setReplySubmitSuccess(false)
    setReplyTarget(review)
  }

  return (
    <div>
      <ReviewSubmitModal
        product={product}
        isOpen={reviewModalOpen}
        onClose={closeReviewModal}
        onSubmit={handleSubmitReview}
        submitting={submitting}
        submitError={submitError}
        submitSuccess={submitSuccess}
        onWriteAnother={() => setSubmitSuccess(false)}
      />

      <ReviewReplyModal
        key={replyTarget?.id ?? 'reply-modal-closed'}
        review={replyTarget}
        isOpen={replyTarget !== null}
        onClose={closeReplyModal}
        onSubmit={handleReplySubmit}
        submitting={replySubmitting}
        submitError={replyError}
        submitSuccess={replySubmitSuccess}
      />

      {loading && (
        <p className="py-6 text-center text-sm text-text-muted">{t('common.loading')}</p>
      )}

      {!loading && !hasReviews && (
        <>
          <p className="py-6 text-center text-sm text-text-muted">{t('product.noReviewsYet')}</p>
          <button
            type="button"
            onClick={openReviewModal}
            className="mt-4 w-full rounded-md border border-warm bg-surface py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft lg:max-w-xs"
          >
            {t('product.writeReview')}
          </button>
        </>
      )}

      {!loading && hasReviews && (
        <>
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
                {t('product.viewAllReviews', { count: mainReviewCount })}
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

            <ProductSectionSortBar
              className="mt-4"
              label={t('product.sortBy')}
              options={sortOptions}
              value={sort}
              onChange={setSort}
              totalLabel={t('product.reviewsTotal', { count: mainReviewCount })}
            />

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
                {sortedRoots.map((review) => (
                  <MobileReviewCard
                    key={review.id}
                    review={review}
                    t={t}
                    onVote={handleVote}
                  />
                ))}
              </div>
            ) : (
              <ul className="mt-4 divide-y divide-border">
                {sortedRoots.map((review) => (
                  <ReviewCard
                    key={review.id}
                    review={review}
                    t={t}
                    compact
                    onVote={handleVote}
                    onReply={openReply}
                  />
                ))}
              </ul>
            )}

            {!hasVisibleReviews && (
              <p className="mt-4 text-center text-sm text-text-muted">
                {sort === 'buyers' ? t('product.noBuyerReviews') : t('product.noReviewsYet')}
              </p>
            )}
          </div>

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
                  {t('product.fromTotalRatings', { count: stats.reviewCount })}
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
                <ProductSectionSortBar
                  label={t('product.sortBy')}
                  options={sortOptions}
                  value={sort}
                  onChange={setSort}
                  totalLabel={t('product.reviewsTotal', { count: mainReviewCount })}
                />

                {!hasVisibleReviews ? (
                  <p className="py-8 text-center text-sm text-text-muted">
                    {sort === 'buyers' ? t('product.noBuyerReviews') : t('product.noReviewsYet')}
                  </p>
                ) : (
                  <ul>
                    {visibleReviews.map((review) => (
                      <ReviewCard
                        key={review.id}
                        review={review}
                        t={t}
                        onVote={handleVote}
                        onReply={openReply}
                      />
                    ))}
                  </ul>
                )}

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
    </div>
  )
}

function ReviewRepliesThread({
  replies,
  parentAuthor,
  t,
  onVote,
  compact = false,
}: {
  replies: ProductReviewItem[]
  parentAuthor: string
  t: TFunction
  onVote: (reviewId: string, isHelpful: boolean) => void
  compact?: boolean
}) {
  if (replies.length === 0) return null

  const elbowWidth = compact ? 'w-3.5' : 'w-4 sm:w-5'
  const elbowOffset = compact ? '-start-4' : '-start-5 sm:-start-6'
  const threadPadding = compact ? 'ps-4' : 'ps-5 sm:ps-6'
  const threadIndent = compact ? 'ms-4 sm:ms-6' : 'ms-6 sm:ms-10 md:ms-12'

  return (
    <div className={`mt-4 ${threadIndent}`}>
      <div className={`relative space-y-3 ${threadPadding}`}>
        <div
          className="absolute start-0 top-3 bottom-3 w-0.5 rounded-full bg-gradient-to-b from-warm/50 via-border-strong/50 to-border/30"
          aria-hidden
        />
        {replies.map((reply) => (
          <div key={reply.id} className="relative">
            <div
              className={`absolute ${elbowOffset} top-5 h-0.5 ${elbowWidth} rounded-full bg-border-strong/55`}
              aria-hidden
            />
            <ReviewReplyCard
              reply={reply}
              parentAuthor={parentAuthor}
              t={t}
              onVote={onVote}
              compact={compact}
            />
          </div>
        ))}
      </div>
    </div>
  )
}

function ReviewReplyIndent({
  children,
  compact = false,
}: {
  children: ReactNode
  compact?: boolean
}) {
  const threadIndent = compact ? 'ms-4 sm:ms-6' : 'ms-6 sm:ms-10 md:ms-12'
  const threadPadding = compact ? 'ps-4' : 'ps-5 sm:ps-6'

  return (
    <div className={`${threadIndent} ${threadPadding} border-s border-border/70`}>
      {children}
    </div>
  )
}

function ReviewReplyCard({
  reply,
  parentAuthor,
  t,
  onVote,
  compact = false,
}: {
  reply: ProductReviewItem
  parentAuthor: string
  t: TFunction
  onVote: (reviewId: string, isHelpful: boolean) => void
  compact?: boolean
}) {
  const replyTo = reply.replyToAuthor ?? parentAuthor

  return (
    <article
      className={`rounded-lg border border-border/80 bg-surface-muted shadow-sm ${compact ? 'p-2.5' : 'px-4 py-3'}`}
    >
      <div className="flex flex-wrap items-center gap-2 text-xs text-warm">
        <ReplyArrowIcon />
        <span className="font-medium">{t('product.reviewReplyBadge')}</span>
        <span className="text-text-muted">
          {t('product.reviewReplyInThread', { author: replyTo })}
        </span>
      </div>

      <div className="mt-2.5 flex items-center gap-2">
        <UserAvatar name={reply.author} imageUrl={reply.authorAvatarUrl} size="sm" />
        <div className="min-w-0">
          <span className="text-sm font-semibold text-text">{reply.author}</span>
          <span className="mt-0.5 block text-xs text-text-muted">{reply.date}</span>
        </div>
      </div>

      <p className={`mt-2 leading-relaxed text-text ${compact ? 'text-xs' : 'text-sm'}`}>
        {reply.text}
      </p>

      <div className="mt-3 flex items-center justify-end gap-4 text-xs text-text-muted">
        <button
          type="button"
          onClick={() => onVote(reply.id, true)}
          className={`inline-flex items-center gap-1.5 transition-colors hover:text-text ${
            reply.userVoteHelpful === true ? 'text-warm' : ''
          }`}
        >
          <ThumbUpIcon />
          {reply.helpful}
        </button>
        <button
          type="button"
          onClick={() => onVote(reply.id, false)}
          className={`inline-flex items-center gap-1.5 transition-colors hover:text-text ${
            reply.userVoteHelpful === false ? 'text-warm' : ''
          }`}
        >
          <ThumbDownIcon />
          {reply.notHelpful}
        </button>
      </div>
    </article>
  )
}

function MobileReviewCard({
  review,
  t,
  onVote,
}: {
  review: ProductReviewItem
  t: TFunction
  onVote: (reviewId: string, isHelpful: boolean) => void
}) {
  const [showFull, setShowFull] = useState(false)
  const isLong = review.text.length > MOBILE_TEXT_LIMIT
  const displayText =
    showFull || !isLong ? review.text : `${review.text.slice(0, MOBILE_TEXT_LIMIT)}…`

  if (review.isStandaloneReply) {
    return (
      <article className="w-[calc(50%-0.375rem)] min-w-[calc(50%-0.375rem)] shrink-0 snap-start">
        <ReviewReplyIndent compact>
          <ReviewReplyCard
            reply={review}
            parentAuthor={t('product.reviewParentUnknown')}
            t={t}
            onVote={onVote}
            compact
          />
        </ReviewReplyIndent>
      </article>
    )
  }

  return (
    <article className="w-[calc(50%-0.375rem)] min-w-[calc(50%-0.375rem)] shrink-0 snap-start rounded-lg border border-border bg-surface p-3.5">
      <div className="flex items-start gap-2.5">
        <UserAvatar name={review.author} imageUrl={review.authorAvatarUrl} />
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
          <button
            type="button"
            onClick={() => onVote(review.id, true)}
            className={`inline-flex items-center gap-1 ${review.userVoteHelpful === true ? 'text-warm' : ''}`}
          >
            <ThumbUpIcon />
            {review.helpful}
          </button>
          <button
            type="button"
            onClick={() => onVote(review.id, false)}
            className={`inline-flex items-center gap-1 ${review.userVoteHelpful === false ? 'text-warm' : ''}`}
          >
            <ThumbDownIcon />
            {review.notHelpful}
          </button>
        </div>
      </div>

      {review.replies.length > 0 && (
        <ReviewRepliesThread
          replies={review.replies}
          parentAuthor={review.author}
          t={t}
          onVote={onVote}
          compact
        />
      )}
    </article>
  )
}

function ReviewCard({
  review,
  t,
  compact = false,
  onVote,
  onReply,
}: {
  review: ProductReviewItem
  t: TFunction
  compact?: boolean
  onVote: (reviewId: string, isHelpful: boolean) => void
  onReply: (review: ProductReviewItem) => void
}) {
  const [menuOpen, setMenuOpen] = useState(false)

  if (review.isStandaloneReply) {
    return (
      <li className={`border-b border-border ${compact ? 'py-4' : 'py-5'} last:border-b-0`}>
        <ReviewReplyIndent compact={compact}>
          <ReviewReplyCard
            reply={review}
            parentAuthor={t('product.reviewParentUnknown')}
            t={t}
            onVote={onVote}
            compact={compact}
          />
        </ReviewReplyIndent>
      </li>
    )
  }

  return (
    <li className={`border-b border-border ${compact ? 'py-4' : 'py-5'} last:border-b-0`}>
      <div className="flex items-start justify-between gap-3">
        <div className="flex flex-wrap items-center gap-2">
          {!compact && (
            <UserAvatar name={review.author} imageUrl={review.authorAvatarUrl} size="sm" />
          )}
          <span className="text-sm font-semibold text-text">{review.author}</span>
          {review.isBuyer && (
            <span className="rounded-md bg-accent/10 px-2 py-0.5 text-[11px] font-medium text-accent">
              {t('product.buyerBadge')}
            </span>
          )}
          {review.replies.length > 0 && (
            <span className="rounded-md bg-warm/10 px-2 py-0.5 text-[11px] font-medium text-warm">
              {t('product.reviewReplyCount', { count: review.replies.length })}
            </span>
          )}
        </div>
        <div className="relative flex shrink-0 items-center gap-2">
          <span className="text-xs text-text-muted">{review.date}</span>
          {!compact && (
            <>
              <button
                type="button"
                aria-label={t('product.reviewMenu')}
                aria-expanded={menuOpen}
                onClick={() => setMenuOpen((open) => !open)}
                className="rounded p-1 text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
              >
                <MoreIcon />
              </button>
              {menuOpen && (
                <div className="absolute end-0 top-full z-10 mt-1 min-w-[10rem] rounded-md border border-border bg-surface py-1 shadow-lg">
                  <button
                    type="button"
                    onClick={() => {
                      setMenuOpen(false)
                      onReply(review)
                    }}
                    className="block w-full px-3 py-2 text-start text-xs text-text hover:bg-surface-muted"
                  >
                    {t('product.reviewReplyAction')}
                  </button>
                </div>
              )}
            </>
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
              onClick={() => onReply(review)}
              className="inline-flex items-center gap-1 transition-colors hover:text-text"
            >
              <QuoteIcon />
              {t('product.reviewReplyAction')}
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
            onClick={() => onVote(review.id, true)}
            className={`inline-flex items-center gap-1.5 transition-colors hover:text-text ${
              review.userVoteHelpful === true ? 'text-warm' : ''
            }`}
          >
            <ThumbUpIcon />
            {review.helpful}
          </button>
          <button
            type="button"
            onClick={() => onVote(review.id, false)}
            className={`inline-flex items-center gap-1.5 transition-colors hover:text-text ${
              review.userVoteHelpful === false ? 'text-warm' : ''
            }`}
          >
            <ThumbDownIcon />
            {review.notHelpful}
          </button>
        </div>
      </div>

      {review.replies.length > 0 && (
        <ReviewRepliesThread
          replies={review.replies}
          parentAuthor={review.author}
          t={t}
          onVote={onVote}
          compact={compact}
        />
      )}
    </li>
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

function ReplyArrowIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden className="shrink-0">
      <path
        d="M9 3H4a2 2 0 00-2 2v4M3 7l2.5 2.5L8 7"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function SortNewestIcon() {
  return (
    <svg width="15" height="15" viewBox="0 0 16 16" fill="none" aria-hidden>
      <circle cx="8" cy="8" r="6" stroke="currentColor" strokeWidth="1.2" />
      <path d="M8 4.5V8l2.5 1.5" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function SortBuyersIcon() {
  return (
    <svg width="15" height="15" viewBox="0 0 16 16" fill="none" aria-hidden>
      <circle cx="8" cy="5.5" r="2.2" stroke="currentColor" strokeWidth="1.2" />
      <path
        d="M3.5 13c0-2.2 2-3.5 4.5-3.5s4.5 1.3 4.5 3.5"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinecap="round"
      />
    </svg>
  )
}

function SortUsefulIcon() {
  return (
    <svg width="15" height="15" viewBox="0 0 16 16" fill="none" aria-hidden>
      <path
        d="M5.5 12V7.5L7 5.5h3l.8 2.8H13V12H5.5zM3 7.5h1.8V12H3V7.5z"
        stroke="currentColor"
        strokeWidth="1.1"
        strokeLinejoin="round"
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
