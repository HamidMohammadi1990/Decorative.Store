import { useEffect, useSyncExternalStore } from 'react'
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'
import { Portal } from '@/components/ui/Portal'
import {
  getConfirmState,
  resolveConfirm,
  subscribeConfirm,
} from '@/stores/confirmStore'

export function ConfirmDialog() {
  const { t } = useTranslation()
  const state = useSyncExternalStore(subscribeConfirm, getConfirmState, getConfirmState)

  useEffect(() => {
    if (!state.open) return

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') resolveConfirm(false)
    }

    document.addEventListener('keydown', handleKeyDown)
    return () => document.removeEventListener('keydown', handleKeyDown)
  }, [state.open])

  if (!state.open || !state.options) return null

  const {
    title = t('common.confirmDeleteTitle'),
    message,
    confirmLabel = t('common.confirmDelete'),
    cancelLabel = t('common.cancel'),
    tone = 'danger',
  } = state.options

  return (
    <Portal>
      <div className="fixed inset-0 z-[200] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/45"
          onClick={() => resolveConfirm(false)}
        />
        <div
          role="alertdialog"
          aria-modal="true"
          aria-labelledby="confirm-dialog-title"
          aria-describedby="confirm-dialog-message"
          className="relative z-10 w-full max-w-md overflow-hidden rounded-xl border border-border bg-surface shadow-2xl"
        >
          <div className="border-b border-border px-5 py-4">
            <h2 id="confirm-dialog-title" className="text-base font-semibold text-text">
              {title}
            </h2>
          </div>
          <div className="px-5 py-4">
            <p id="confirm-dialog-message" className="text-sm leading-relaxed text-text-muted">
              {message}
            </p>
          </div>
          <div className="flex flex-wrap justify-end gap-2 border-t border-border bg-surface-muted/30 px-5 py-4">
            <Button variant="secondary" onClick={() => resolveConfirm(false)}>
              {cancelLabel}
            </Button>
            <Button
              variant={tone === 'danger' ? 'warm' : 'primary'}
              className={tone === 'danger' ? 'bg-sale text-text-inverse hover:bg-sale/90' : ''}
              onClick={() => resolveConfirm(true)}
            >
              {confirmLabel}
            </Button>
          </div>
        </div>
      </div>
    </Portal>
  )
}
