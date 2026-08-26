import type { ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'

export function AdminLargeModal({
  open,
  title,
  description,
  onClose,
  children,
}: {
  open: boolean
  title: string
  description?: string
  onClose: () => void
  children: ReactNode
}) {
  const { t } = useTranslation()

  if (!open) return null

  return (
    <Portal>
      <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/45"
          onClick={onClose}
        />
        <div
          role="dialog"
          aria-modal="true"
          aria-label={title}
          className="relative z-10 flex max-h-[min(92vh,52rem)] w-full max-w-4xl flex-col overflow-hidden rounded-sm bg-surface shadow-2xl"
        >
          <header className="flex shrink-0 items-start justify-between gap-4 border-b border-border px-5 py-4 sm:px-6">
            <div className="min-w-0">
              <h2 className="text-lg font-semibold text-text">{title}</h2>
              {description && (
                <p className="mt-0.5 text-sm text-text-muted">{description}</p>
              )}
            </div>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-9 shrink-0 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
            >
              <CloseIcon />
            </button>
          </header>
          <div className="min-h-0 flex-1 overflow-y-auto px-5 py-4 sm:px-6 sm:py-5">
            {children}
          </div>
        </div>
      </div>
    </Portal>
  )
}
