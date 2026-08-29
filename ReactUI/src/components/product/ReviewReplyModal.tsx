import { useEffect, useId, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Portal } from '@/components/ui/Portal'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import type { ProductReviewItem } from '@/extensions/productReviews'

interface ReviewReplyModalProps {
  review: ProductReviewItem | null
  isOpen: boolean
  onClose: () => void
  onSubmit: (description: string, review: ProductReviewItem) => Promise<void>
  submitting?: boolean
  submitError?: string | null
  submitSuccess?: boolean
}

export function ReviewReplyModal({
  review,
  isOpen,
  onClose,
  onSubmit,
  submitting = false,
  submitError = null,
  submitSuccess = false,
}: ReviewReplyModalProps) {
  const { t } = useTranslation()
  const labelId = useId()
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const [reply, setReply] = useState('')

  useEffect(() => {
    if (!isOpen) {
      setReply('')
      return
    }

    if (submitSuccess) return

    const timer = window.setTimeout(() => textareaRef.current?.focus(), 120)
    return () => window.clearTimeout(timer)
  }, [isOpen, submitSuccess])

  useEffect(() => {
    if (!isOpen) return

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => window.removeEventListener('keydown', onKeyDown)
  }, [isOpen, onClose])

  if (!isOpen || !review) return null

  const canSubmit = reply.trim().length > 0 && !submitting

  return (
    <Portal>
      <div className="fixed inset-0 z-[110] flex items-center justify-center p-4 sm:p-6">
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
          className="relative z-10 flex max-h-[min(88vh,32rem)] w-full max-w-lg flex-col overflow-hidden rounded-lg bg-surface shadow-2xl"
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
              {t('product.reviewReplyTitle')}
            </h2>
          </header>

          <div className="overflow-y-auto px-4 py-4">
            {submitSuccess ? (
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
                  <p className="text-base font-semibold text-text">
                    {t('product.reviewReplySubmitSuccessTitle')}
                  </p>
                  <p className="mt-1.5 text-sm leading-relaxed text-text-muted">
                    {t('product.reviewReplySubmitSuccessMessage')}
                  </p>
                  <button
                    type="button"
                    onClick={onClose}
                    className="mt-4 rounded-lg bg-warm px-4 py-2 text-sm font-semibold text-warm-text transition-colors hover:bg-warm-hover"
                  >
                    {t('product.reviewSubmitSuccessClose')}
                  </button>
                </div>
              </div>
            ) : (
              <>
                <p className="text-xs text-text-muted">
                  {t('product.reviewReplyTo', { author: review.author })}
                </p>
                <p className="mt-2 rounded-md bg-surface-muted/60 px-3 py-2 text-sm text-text-muted">
                  {review.text}
                </p>

                <label htmlFor="review-reply" className="mt-4 block text-sm font-medium text-text">
                  {t('product.reviewReplyLabel')}
                </label>
                <textarea
                  ref={textareaRef}
                  id="review-reply"
                  value={reply}
                  onChange={(e) => setReply(e.target.value)}
                  placeholder={t('product.reviewReplyPlaceholder')}
                  rows={5}
                  className="mt-2 block w-full resize-none rounded-lg border border-border bg-surface px-3 py-3 text-sm leading-relaxed text-text outline-none focus:border-warm"
                />

                <button
                  type="button"
                  disabled={!canSubmit}
                  onClick={() => void onSubmit(reply.trim(), review)}
                  className={`mt-4 w-full rounded-lg py-3 text-sm font-semibold transition-colors ${
                    canSubmit
                      ? 'bg-warm text-warm-text hover:bg-warm-hover'
                      : 'cursor-not-allowed bg-border text-text-muted'
                  }`}
                >
                  {submitting ? t('common.loading') : t('product.reviewReplySubmit')}
                </button>

                {submitError && (
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
                    <p className="min-w-0 flex-1 text-sm leading-relaxed text-sale">{submitError}</p>
                  </div>
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </Portal>
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
