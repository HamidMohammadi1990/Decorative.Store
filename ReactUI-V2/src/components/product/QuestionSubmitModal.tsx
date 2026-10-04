import { useEffect, useId, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { LocalImage } from '@/components/ui/LocalImage'
import { Portal } from '@/components/ui/Portal'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

const MAX_LENGTH = 100

interface QuestionSubmitModalProps {
  product: ProductDetail
  isOpen: boolean
  onClose: () => void
  onSubmit: (question: string) => Promise<void>
  submitting?: boolean
  submitError?: string | null
  submitSuccess?: boolean
  onWriteAnother?: () => void
}

export function QuestionSubmitModal({
  product,
  isOpen,
  onClose,
  onSubmit,
  submitting = false,
  submitError = null,
  submitSuccess = false,
  onWriteAnother,
}: QuestionSubmitModalProps) {
  const { t, i18n } = useTranslation()
  const labelId = useId()
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const [question, setQuestion] = useState('')

  const numberLocale = i18n.language === 'fa' ? 'fa-IR' : 'en-US'
  const formatCount = (n: number) => n.toLocaleString(numberLocale)

  useEffect(() => {
    if (!isOpen) {
      setQuestion('')
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

  if (!isOpen) return null

  const trimmed = question.trim()
  const canSubmit = trimmed.length > 0 && !submitting

  const handleSubmit = async () => {
    if (!canSubmit) return
    await onSubmit(trimmed)
  }

  return (
    <Portal>
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
          className="relative z-10 flex max-h-[min(88vh,32rem)] w-full max-w-lg flex-col overflow-hidden rounded-lg bg-surface shadow-2xl"
        >
          <header className="flex items-center justify-between gap-4 border-b border-border px-4 py-4">
            <h2 id={labelId} className="text-base font-bold text-text">
              {t('product.questionModalDetailsTitle')}
            </h2>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-9 shrink-0 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
            >
              <CloseIcon />
            </button>
          </header>

          {submitSuccess ? (
            <div className="flex-1 overflow-y-auto px-4 py-5">
              <QuestionSubmitSuccess
                onClose={onClose}
                onWriteAnother={() => {
                  onWriteAnother?.()
                  setQuestion('')
                }}
              />
            </div>
          ) : (
            <>
              <div className="flex-1 overflow-y-auto px-4 py-5">
                <div className="flex items-start gap-3 border-b border-border pb-4">
                  <div className="size-14 shrink-0 overflow-hidden rounded-md border border-border bg-surface-muted">
                    <LocalImage image={product.image} className="size-full object-cover" />
                  </div>
                  <p className="min-w-0 flex-1 text-sm leading-relaxed text-text">{product.title}</p>
                </div>

                <p className="mt-4 text-sm text-text">{t('product.questionPrompt')}</p>

                <div className="mt-3 overflow-hidden rounded-lg border border-border focus-within:border-warm">
                  <textarea
                    ref={textareaRef}
                    value={question}
                    onChange={(e) => setQuestion(e.target.value.slice(0, MAX_LENGTH))}
                    placeholder={t('product.questionTextPlaceholder')}
                    rows={6}
                    className="block w-full resize-none border-0 bg-transparent px-3 py-3 text-sm leading-relaxed text-text outline-none placeholder:text-text-muted"
                  />
                  <div className="px-3 pb-2.5 text-end text-xs text-text-muted" dir="ltr">
                    {formatCount(question.length)}/{formatCount(MAX_LENGTH)}
                  </div>
                </div>
              </div>

              <footer className="border-t border-border px-4 py-4">
                <button
                  type="button"
                  disabled={!canSubmit}
                  onClick={() => void handleSubmit()}
                  className={`w-full rounded-lg py-3.5 text-sm font-semibold transition-colors ${
                    canSubmit
                      ? 'bg-warm text-warm-text hover:bg-warm-hover'
                      : 'cursor-not-allowed bg-border text-text-muted'
                  }`}
                >
                  {submitting ? t('common.loading') : t('product.questionModalTitle')}
                </button>

                {submitError && <QuestionSubmitError message={submitError} />}

                <p className="mt-3 text-center text-xs leading-relaxed text-text-muted">
                  {t('product.questionDisclaimer')}{' '}
                  <button type="button" className="text-accent hover:underline">
                    {t('product.questionRulesLink')}
                  </button>
                </p>
              </footer>
            </>
          )}
        </div>
      </div>
    </Portal>
  )
}

function QuestionSubmitSuccess({
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
        <p className="text-base font-semibold text-text">{t('product.questionSubmitSuccessTitle')}</p>
        <p className="mt-1.5 text-sm leading-relaxed text-text-muted">
          {t('product.questionSubmitSuccessMessage')}
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
            {t('product.questionSubmitAnother')}
          </button>
        </div>
      </div>
    </div>
  )
}

function QuestionSubmitError({ message }: { message: string }) {
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

function CloseIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <path
        d="M4.5 4.5l9 9M13.5 4.5l-9 9"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </svg>
  )
}
