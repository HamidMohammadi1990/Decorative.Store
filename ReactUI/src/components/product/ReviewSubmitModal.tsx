import { useEffect, useId, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ReviewPostAsModal, type ReviewPostAsMode } from '@/components/product/ReviewPostAsModal'
import { LocalImage } from '@/components/ui/LocalImage'
import { Portal } from '@/components/ui/Portal'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { useCommentTopics } from '@/hooks/useCommentTopics'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

interface ReviewSubmitModalProps {
  product: ProductDetail
  isOpen: boolean
  onClose: () => void
  onSubmit: (comment: string, commentTopicId: string) => Promise<void>
  submitting?: boolean
  submitError?: string | null
}

export function ReviewSubmitModal({
  product,
  isOpen,
  onClose,
  onSubmit,
  submitting = false,
  submitError = null,
}: ReviewSubmitModalProps) {
  const { t } = useTranslation()
  const labelId = useId()
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const { topics, loading: topicsLoading, error: topicsError } = useCommentTopics(isOpen)

  const [comment, setComment] = useState('')
  const [commentTopicId, setCommentTopicId] = useState('')
  const [postAs, setPostAs] = useState<ReviewPostAsMode>('named')
  const [postAsModalOpen, setPostAsModalOpen] = useState(false)

  useEffect(() => {
    if (!isOpen) {
      setComment('')
      setCommentTopicId('')
      setPostAs('named')
      setPostAsModalOpen(false)
      return
    }

    const timer = window.setTimeout(() => textareaRef.current?.focus(), 120)
    return () => window.clearTimeout(timer)
  }, [isOpen])

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
    comment.trim().length > 0 && commentTopicId.length > 0 && !submitting && !topicsLoading

  const handleSubmit = async () => {
    if (!canSubmit) return
    await onSubmit(comment.trim(), commentTopicId)
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

            {submitError && (
              <p className="mt-3 text-center text-xs text-red-600">{submitError}</p>
            )}

            <p className="mt-4 text-center text-xs leading-relaxed text-text-muted">
              {t('product.reviewDisclaimer')}{' '}
              <button type="button" className="text-accent hover:underline">
                {t('product.reviewRulesLink')}
              </button>
            </p>
          </div>
        </div>
      </div>
    </Portal>
  )
}
