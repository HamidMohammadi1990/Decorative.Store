import { useEffect, useId } from 'react'
import { useTranslation } from 'react-i18next'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'

export type ReviewPostAsMode = 'named' | 'anonymous'

interface ReviewPostAsModalProps {
  isOpen: boolean
  value: ReviewPostAsMode
  onChange: (value: ReviewPostAsMode) => void
  onClose: () => void
}

export function ReviewPostAsModal({
  isOpen,
  value,
  onChange,
  onClose,
}: ReviewPostAsModalProps) {
  const { t } = useTranslation()
  const titleId = useId()

  useEffect(() => {
    if (!isOpen) return

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => window.removeEventListener('keydown', onKeyDown)
  }, [isOpen, onClose])

  if (!isOpen) return null

  const options: {
    id: ReviewPostAsMode
    title: string
    description: string
  }[] = [
    {
      id: 'anonymous',
      title: t('product.reviewPostAnonymous'),
      description: t('product.reviewAnonymousDesc'),
    },
    {
      id: 'named',
      title: t('product.reviewPostAsNamed'),
      description: t('product.reviewNamedDesc', { name: t('product.reviewAuthorName') }),
    },
  ]

  return (
    <Portal>
      <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={onClose}
        />

        <div
          role="dialog"
          aria-modal="true"
          aria-labelledby={titleId}
          className="relative z-10 w-full max-w-md overflow-hidden rounded-lg bg-surface shadow-2xl"
        >
          <header className="flex items-center justify-between border-b border-border px-5 py-4">
            <h2 id={titleId} className="text-sm font-bold text-text">
              {t('product.reviewPostAsModalTitle')}
            </h2>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-8 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
            >
              <CloseIcon />
            </button>
          </header>

          <div role="radiogroup" aria-label={t('product.reviewPostAsModalTitle')} className="px-5">
            {options.map((option) => {
              const selected = value === option.id

              return (
                <label
                  key={option.id}
                  className="flex cursor-pointer items-start gap-4 border-b border-border py-5 last:border-b-0"
                >
                  <span className="relative mt-0.5 flex size-5 shrink-0 items-center justify-center">
                    <input
                      type="radio"
                      name="review-post-as"
                      value={option.id}
                      checked={selected}
                      onChange={() => {
                        onChange(option.id)
                        onClose()
                      }}
                      className="peer sr-only"
                    />
                    <span
                      aria-hidden
                      className="size-5 rounded-full border-2 border-border transition-colors peer-checked:border-warm peer-focus-visible:ring-2 peer-focus-visible:ring-warm/30"
                    />
                    <span
                      aria-hidden
                      className={`absolute size-2.5 rounded-full bg-warm transition-opacity ${
                        selected ? 'opacity-100' : 'opacity-0'
                      }`}
                    />
                  </span>

                  <div className="min-w-0 flex-1">
                    <p className="text-sm font-semibold text-text">{option.title}</p>
                    <p className="mt-1.5 text-xs leading-relaxed text-text-muted">
                      {option.description}
                    </p>
                  </div>
                </label>
              )
            })}
          </div>
        </div>
      </div>
    </Portal>
  )
}
