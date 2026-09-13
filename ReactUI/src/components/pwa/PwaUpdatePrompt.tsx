import { useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useRegisterSW } from 'virtual:pwa-register/react'
import { showToast } from '@/stores/toastStore'

const UPDATE_CHECK_MS = 60 * 60 * 1000

export function PwaUpdatePrompt() {
  const { t } = useTranslation()
  const [dismissed, setDismissed] = useState(false)
  const offlineToastShown = useRef(false)

  const {
    needRefresh: [needRefresh, setNeedRefresh],
    offlineReady: [offlineReady],
    updateServiceWorker,
  } = useRegisterSW({
    immediate: true,
    onRegisteredSW(_swUrl, registration) {
      if (!registration) return
      window.setInterval(() => {
        void registration.update()
      }, UPDATE_CHECK_MS)
    },
    onRegisterError(error) {
      console.warn('[pwa] service worker registration failed', error)
    },
  })

  const handleUpdate = async () => {
    setDismissed(true)
    setNeedRefresh(false)
    try {
      await updateServiceWorker(true)
    } catch (error) {
      console.warn('[pwa] service worker update failed', error)
    }
    window.setTimeout(() => {
      void navigator.serviceWorker?.ready.then((registration) => {
        if (registration.waiting) {
          registration.waiting.postMessage({ type: 'SKIP_WAITING' })
        }
      })
      window.location.reload()
    }, 300)
  }

  useEffect(() => {
    if (!offlineReady || offlineToastShown.current) return
    offlineToastShown.current = true
    showToast(t('pwa.offlineReady'), 'success')
  }, [offlineReady, t])

  if (!import.meta.env.PROD || !needRefresh || dismissed) {
    return null
  }

  return (
    <div
      role="status"
      aria-live="polite"
      className="fixed inset-x-0 bottom-0 z-[120] px-4 pb-[max(1rem,env(safe-area-inset-bottom))] pt-2"
    >
      <div className="mx-auto flex max-w-lg flex-wrap items-center justify-between gap-3 rounded-sm border border-border bg-surface px-4 py-3 shadow-lg ring-1 ring-black/5">
        <p className="min-w-0 flex-1 text-sm text-text">{t('pwa.updateAvailable')}</p>
        <div className="flex shrink-0 items-center gap-2">
          <button
            type="button"
            onClick={() => setDismissed(true)}
            className="rounded-sm px-3 py-1.5 text-xs font-semibold text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
          >
            {t('pwa.updateLater')}
          </button>
          <button
            type="button"
            onClick={() => void handleUpdate()}
            className="rounded-sm bg-warm px-3 py-1.5 text-xs font-semibold text-warm-text transition-colors hover:bg-warm-hover"
          >
            {t('pwa.updateNow')}
          </button>
        </div>
      </div>
    </div>
  )
}
