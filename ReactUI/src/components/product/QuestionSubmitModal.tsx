import { useEffect, useId, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Portal } from '@/components/ui/Portal'

const MAX_LENGTH = 100

interface QuestionSubmitModalProps {
  isOpen: boolean
  onClose: () => void
}

export function QuestionSubmitModal({ isOpen, onClose }: QuestionSubmitModalProps) {
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

    const timer = window.setTimeout(() => textareaRef.current?.focus(), 120)
    return () => window.clearTimeout(timer)
  }, [isOpen])

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
  const canSubmit = trimmed.length > 0

  const handleSubmit = () => {
    if (!canSubmit) return
    onClose()
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

          <div className="flex-1 overflow-y-auto px-4 py-5">
            <p className="text-sm text-text">{t('product.questionPrompt')}</p>

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
              onClick={handleSubmit}
              className={`w-full rounded-lg py-3.5 text-sm font-semibold transition-colors ${
                canSubmit
                  ? 'bg-warm text-warm-text hover:bg-warm-hover'
                  : 'cursor-not-allowed bg-border text-text-muted'
              }`}
            >
              {t('product.questionModalTitle')}
            </button>

            <p className="mt-3 text-center text-xs leading-relaxed text-text-muted">
              {t('product.questionDisclaimer')}{' '}
              <button type="button" className="text-accent hover:underline">
                {t('product.questionRulesLink')}
              </button>
            </p>
          </footer>
        </div>
      </div>
    </Portal>
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
