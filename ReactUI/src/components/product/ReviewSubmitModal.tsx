import { useEffect, useId, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ReviewPostAsModal, type ReviewPostAsMode } from '@/components/product/ReviewPostAsModal'
import { LocalImage } from '@/components/ui/LocalImage'
import { Portal } from '@/components/ui/Portal'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { useCommentTopics } from '@/hooks/useCommentTopics'
import { StarRatingInput } from '@/components/product/StarRatingInput'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

interface ReviewSubmitModalProps {
  product: ProductDetail
  isOpen: boolean
  onClose: () => void
  onSubmit: (payload: {
    description: string
    commentTopicId: string
    commentRate: number
    qualityRating: number
    affordableRating: number
  }) => Promise<void>
  submitting?: boolean
  submitError?: string | null
  submitSuccess?: boolean
  onWriteAnother?: () => void
}

export function ReviewSubmitModal({
  product,
  isOpen,
  onClose,
  onSubmit,
  submitting = false,
  submitError = null,
  submitSuccess = false,
  onWriteAnother,
}: ReviewSubmitModalProps) {
  const { t } = useTranslation()
  const labelId = useId()
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const { topics, loading: topicsLoading, error: topicsError } = useCommentTopics(isOpen)

  const [comment, setComment] = useState('')
  const [commentTopicId, setCommentTopicId] = useState('')
  const [commentRate, setCommentRate] = useState(5)
  const [qualityRating, setQualityRating] = useState(5)
  const [affordableRating, setAffordableRating] = useState(5)
  const [postAs, setPostAs] = useState<ReviewPostAsMode>('named')
  const [postAsModalOpen, setPostAsModalOpen] = useState(false)

  useEffect(() => {
    if (!isOpen) {
      setComment('')
      setCommentTopicId('')
      setCommentRate(5)
      setQualityRating(5)
      setAffordableRating(5)
      setPostAs('named')
      setPostAsModalOpen(false)
      return
    }

    if (submitSuccess) return

    const timer = window.setTimeout(() => textareaRef.current?.focus(), 120)
    return () => window.clearTimeout(timer)
  }, [isOpen, submitSuccess])

  useEffect(() => {
    if (!isOpen || topics.length === 0) return

    setCommentTopicId((current) => {
      if (current && topics.some((topic) => topic.id === current)) {
        return current
      }
      return topics[0]?.id ?? ''
    })
  }, [isOpen, topics])

  useEffect(() => {
    if (!isOpen) return

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key !== 'Escape') return
      if (postAsModalOpen) {
        setPostAsModalOpen(false)
        return
      }
      onClose()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => window.removeEventListener('keydown', onKeyDown)
  }, [isOpen, onClose, postAsModalOpen])

  if (!isOpen) return null

  const canSubmit =
    comment.trim().length > 0 &&
    commentTopicId.length > 0 &&
    commentRate > 0 &&
    qualityRating > 0 &&
    affordableRating > 0 &&
    !submitting &&
    !topicsLoading

  const handleSubmit = async () => {
    if (!canSubmit) return
    await onSubmit({
      description: comment.trim(),
      commentTopicId,
      commentRate,
      qualityRating,
      affordableRating,
    })
  }

  const displayName =
    postAs === 'named' ? t('product.reviewAuthorName') : t('product.reviewAnonymousName')

  return (
    <Portal>
      <ReviewPostAsModal
        isOpen={postAsModalOpen}
        value={postAs}
        onChange={setPostAs}
        onClose={() => setPostAsModalOpen(false)}
      />

      <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={onClose}
        />

        <div
          role="dialog"
          aria-modal="true"
          aria-labelledby={labelId}
          className="relative z-10 flex max-h-[min(88vh,40rem)] w-full max-w-lg flex-col overflow-hidden rounded-lg bg-surface shadow-2xl"
        >
          <header className="flex items-center gap-3 border-b border-border px-4 py-4">
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-9 shrink-0 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
            >
              <ChevronIcon expanded={false} className="rotate-90 rtl:-rotate-90" />
            </button>
            <h2 id={labelId} className="text-base font-bold text-text">
              {t('product.reviewModalTitle')}
            </h2>
          </header>

          <div className="overflow-y-auto px-4 py-4">
            {submitSuccess ? (
              <ReviewSubmitSuccess
                onClose={onClose}
                onWriteAnother={() => {
                  onWriteAnother?.()
                  setComment('')
                }}
              />
            ) : (
              <>
                <div className="flex items-start gap-3 border-b border-border pb-4">
                  <div className="size-16 shrink-0 overflow-hidden rounded-md border border-border bg-surface-muted">
                    <LocalImage image={product.image} className="size-full object-cover" />
                  </div>
                  <p className="min-w-0 flex-1 text-sm leading-relaxed text-text">{product.title}</p>
                </div>

                <div className="mt-5">
                  <label htmlFor="review-topic" className="text-sm font-medium text-text">
                    {t('product.reviewTopicLabel')}
                    <span className="text-warm">*</span>
                  </label>

                  {topicsLoading ? (
                    <p className="mt-2 text-sm text-text-muted">{t('common.loading')}</p>
                  ) : topicsError ? (
                    <p className="mt-2 text-sm text-red-600">{t('product.reviewTopicsLoadFailed')}</p>
                  ) : topics.length === 0 ? (
                    <p className="mt-2 text-sm text-text-muted">{t('product.reviewTopicsEmpty')}</p>
                  ) : (
                    <select
                      id="review-topic"
                      value={commentTopicId}
                      onChange={(e) => setCommentTopicId(e.target.value)}
                      className="mt-2 w-full rounded-lg border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none focus:border-warm"
                    >
                      {topics.map((topic) => (
                        <option key={topic.id} value={topic.id}>
                          {topic.title}
                        </option>
                      ))}
                    </select>
                  )}
                </div>

                <div className="mt-5 grid gap-4 sm:grid-cols-3">
                  <StarRatingInput
                    label={t('product.reviewOverallRating')}
                    value={commentRate}
                    onChange={setCommentRate}
                    disabled={submitting}
                  />
                  <StarRatingInput
                    label={t('product.reviewQualityRating')}
                    value={qualityRating}
                    onChange={setQualityRating}
                    disabled={submitting}
                  />
                  <StarRatingInput
                    label={t('product.reviewAffordableRating')}
                    value={affordableRating}
                    onChange={setAffordableRating}
                    disabled={submitting}
                  />
                </div>

                <div className="mt-5">
                  <label htmlFor="review-comment" className="text-sm font-medium text-text">
                    {t('product.reviewCommentLabel')}
                    <span className="text-warm">*</span>
                  </label>

                  <div className="mt-2 overflow-hidden rounded-lg border border-border focus-within:border-warm">
                    <textarea
                      ref={textareaRef}
                      id="review-comment"
                      value={comment}
                      onChange={(e) => setComment(e.target.value)}
                      placeholder={t('product.reviewCommentPlaceholder')}
                      rows={6}
                      className="block w-full resize-none border-0 bg-transparent px-3 py-3 text-sm leading-relaxed text-text outline-none placeholder:text-text-muted"
                    />

                    <div className="flex items-center justify-between gap-3 border-t border-border bg-surface-muted/40 px-3 py-2.5">
                      <span className="truncate text-xs text-text-muted">{displayName}</span>

                      <button
                        type="button"
                        onClick={() => setPostAsModalOpen(true)}
                        className="inline-flex shrink-0 items-center gap-1 rounded-md border border-border bg-surface px-2.5 py-1 text-xs text-text-muted transition-colors hover:border-border-strong hover:text-text"
                      >
                        {postAs === 'named'
                          ? t('product.reviewPostAsNamed')
                          : t('product.reviewPostAnonymous')}
                        <ChevronIcon expanded={false} className="size-3" />
                      </button>
                    </div>
                  </div>
                </div>

                <button
                  type="button"
                  disabled={!canSubmit}
                  onClick={() => void handleSubmit()}
                  className={`mt-5 w-full rounded-lg py-3 text-sm font-semibold transition-colors ${
                    canSubmit
                      ? 'bg-warm text-warm-text hover:bg-warm-hover'
                      : 'cursor-not-allowed bg-border text-text-muted'
                  }`}
                >
                  {submitting ? t('common.loading') : t('product.reviewModalTitle')}
                </button>

                {submitError && <ReviewSubmitError message={submitError} />}

                <p className="mt-4 text-center text-xs leading-relaxed text-text-muted">
                  {t('product.reviewDisclaimer')}{' '}
                  <button type="button" className="text-accent hover:underline">
                    {t('product.reviewRulesLink')}
                  </button>
                </p>
              </>
            )}
          </div>
        </div>
      </div>
    </Portal>
  )
}

function ReviewSubmitSuccess({
  onClose,
  onWriteAnother,
}: {
  onClose: () => void
  onWriteAnother: () => void
}) {
  const { t } = useTranslation()

  return (
    <div
      className="flex gap-4 rounded-xl border border-accent/20 bg-accent/5 p-4 sm:p-5"
      role="status"
      aria-live="polite"
    >
      <span
        className="flex size-11 shrink-0 items-center justify-center rounded-full bg-accent/15 text-accent"
        aria-hidden
      >
        <CheckCircleIcon />
      </span>
      <div className="min-w-0 flex-1">
        <p className="text-base font-semibold text-text">{t('product.reviewSubmitSuccessTitle')}</p>
        <p className="mt-1.5 text-sm leading-relaxed text-text-muted">
          {t('product.reviewSubmitSuccessMessage')}
        </p>
        <div className="mt-4 flex flex-wrap items-center gap-3">
          <button
            type="button"
            onClick={onClose}
            className="rounded-lg bg-warm px-4 py-2 text-sm font-semibold text-warm-text transition-colors hover:bg-warm-hover"
          >
            {t('product.reviewSubmitSuccessClose')}
          </button>
          <button
            type="button"
            onClick={onWriteAnother}
            className="text-sm font-semibold text-warm transition-colors hover:text-warm-hover hover:underline"
          >
            {t('product.reviewSubmitAnother')}
          </button>
        </div>
      </div>
    </div>
  )
}

function ReviewSubmitError({ message }: { message: string }) {
  return (
    <div
      className="mt-3 flex gap-3 rounded-xl border border-sale/25 bg-sale/5 p-3"
      role="alert"
      aria-live="assertive"
    >
      <span
        className="flex size-9 shrink-0 items-center justify-center rounded-full bg-sale/10 text-sale"
        aria-hidden
      >
        <AlertIcon />
      </span>
      <p className="min-w-0 flex-1 text-sm leading-relaxed text-sale">{message}</p>
    </div>
  )
}

function CheckCircleIcon() {
  return (
    <svg width="22" height="22" viewBox="0 0 22 22" fill="none" aria-hidden>
      <circle cx="11" cy="11" r="10" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M6.5 11.2l3 3 5.5-6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function AlertIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <circle cx="9" cy="9" r="7.5" stroke="currentColor" strokeWidth="1.3" />
      <path d="M9 5.5v4.5" stroke="currentColor" strokeWidth="1.3" strokeLinecap="round" />
      <circle cx="9" cy="12.75" r="0.75" fill="currentColor" />
    </svg>
  )
}
