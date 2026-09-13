import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'

const DISMISS_KEY = 'pwa-install-dismissed'

interface BeforeInstallPromptEvent extends Event {
  prompt: () => Promise<void>
  userChoice: Promise<{ outcome: 'accepted' | 'dismissed' }>
}

function isStandaloneDisplay(): boolean {
  return (
    window.matchMedia('(display-mode: standalone)').matches ||
    (window.navigator as Navigator & { standalone?: boolean }).standalone === true
  )
}

function isIosSafari(): boolean {
  const ua = window.navigator.userAgent
  return /iPad|iPhone|iPod/.test(ua) && !(window as Window & { MSStream?: unknown }).MSStream
}

/** Custom install banner — browsers no longer show a reliable native install popup. */
export function PwaInstallPrompt() {
  const { t } = useTranslation()
  const [deferredPrompt, setDeferredPrompt] = useState<BeforeInstallPromptEvent | null>(null)
  const [showIosHint, setShowIosHint] = useState(false)
  const [dismissed, setDismissed] = useState(false)

  useEffect(() => {
    if (!import.meta.env.PROD || isStandaloneDisplay()) return

    if (sessionStorage.getItem(DISMISS_KEY) === '1') {
      setDismissed(true)
      return
    }

    if (isIosSafari()) {
      setShowIosHint(true)
      return
    }

    const onBeforeInstallPrompt = (event: Event) => {
      event.preventDefault()
      setDeferredPrompt(event as BeforeInstallPromptEvent)
    }

    const onAppInstalled = () => {
      setDeferredPrompt(null)
      setShowIosHint(false)
    }

    window.addEventListener('beforeinstallprompt', onBeforeInstallPrompt)
    window.addEventListener('appinstalled', onAppInstalled)

    return () => {
      window.removeEventListener('beforeinstallprompt', onBeforeInstallPrompt)
      window.removeEventListener('appinstalled', onAppInstalled)
    }
  }, [])

  const dismiss = () => {
    sessionStorage.setItem(DISMISS_KEY, '1')
    setDismissed(true)
    setDeferredPrompt(null)
    setShowIosHint(false)
  }

  const install = async () => {
    if (!deferredPrompt) return
    await deferredPrompt.prompt()
    const { outcome } = await deferredPrompt.userChoice
    setDeferredPrompt(null)
    if (outcome === 'accepted') {
      sessionStorage.setItem(DISMISS_KEY, '1')
    }
  }

  if (dismissed || (!deferredPrompt && !showIosHint)) {
    return null
  }

  return (
    <div
      role="dialog"
      aria-labelledby="pwa-install-title"
      className="fixed inset-x-0 bottom-0 z-[119] px-4 pb-[max(1rem,env(safe-area-inset-bottom))] pt-2"
    >
      <div className="mx-auto flex max-w-lg flex-wrap items-center justify-between gap-3 rounded-sm border border-border bg-surface px-4 py-3 shadow-lg ring-1 ring-black/5">
        <div className="min-w-0 flex-1">
          <p id="pwa-install-title" className="text-sm font-semibold text-text">
            {t('pwa.installTitle')}
          </p>
          <p className="mt-0.5 text-xs text-text-muted">
            {showIosHint ? t('pwa.installIosHint') : t('pwa.installDescription')}
          </p>
        </div>
        <div className="flex shrink-0 items-center gap-2">
          <button
            type="button"
            onClick={dismiss}
            className="rounded-sm px-3 py-1.5 text-xs font-semibold text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
          >
            {t('pwa.installLater')}
          </button>
          {!showIosHint ? (
            <button
              type="button"
              onClick={() => void install()}
              className="rounded-sm bg-warm px-3 py-1.5 text-xs font-semibold text-warm-text transition-colors hover:bg-warm-hover"
            >
              {t('pwa.installNow')}
            </button>
          ) : null}
        </div>
      </div>
    </div>
  )
}
