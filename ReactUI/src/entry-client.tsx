import { StrictMode, useEffect } from 'react'
import { createRoot, hydrateRoot } from 'react-dom/client'
import type { HydrationState } from 'react-router-dom'
import { RouterProvider } from 'react-router-dom'
import '@/i18n'
import { syncI18nLocale } from '@/i18n'
import { applyTheme, getStoredTheme } from '@/extensions/applyTheme'
import { writeSettingsCookie } from '@/extensions/settingsCookie'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'
import { initWebVitalsReporting } from '@/extensions/initWebVitalsReporting'
import { AppProviders } from '@/providers/AppProviders'
import { createAppRouter } from '@/routes/createAppRouter'
import { preloadLazyRoutes, preloadLazyRoutesForPath } from '@/routes/preloadLazyRoutes'
import type { Locale } from '@/models/shared/locale.model'
import './index.css'

declare global {
  interface Window {
    __ROUTER_HYDRATION__?: HydrationState
    __SSR_LOCALE__?: Locale
    __SSR_LAZY_ROUTES__?: string[]
  }
}

function ClientShell({ router }: { router: ReturnType<typeof createAppRouter> }) {
  const restoreSession = useUserStore((s) => s.restoreSession)
  const accessToken = useUserStore((s) => s.accessToken)

  useEffect(() => {
    if (!accessToken) return

    const runRestore = () => {
      void restoreSession()
    }

    if (useUserStore.persist.hasHydrated()) {
      runRestore()
      return
    }

    return useUserStore.persist.onFinishHydration(runRestore)
  }, [accessToken, restoreSession])

  useEffect(() => {
    const syncCookie = () => {
      const { locale, theme } = useSettingsStore.getState()
      writeSettingsCookie({ locale, theme })
    }

    syncCookie()
    return useSettingsStore.subscribe(syncCookie)
  }, [])

  return (
    <AppProviders>
      <RouterProvider router={router} />
    </AppProviders>
  )
}

async function boot() {
  applyTheme(getStoredTheme())

  const ssrLocale = window.__SSR_LOCALE__
  if (ssrLocale) {
    useSettingsStore.setState({ locale: ssrLocale })
    syncI18nLocale(ssrLocale)
  } else {
    syncI18nLocale(useSettingsStore.getState().locale)
  }

  initWebVitalsReporting()

  const lazyRouteIds = window.__SSR_LAZY_ROUTES__
  if (lazyRouteIds?.length) {
    await preloadLazyRoutes(lazyRouteIds)
  } else if (document.getElementById('root')?.hasChildNodes()) {
    await preloadLazyRoutesForPath(window.location.pathname)
  }

  const rootEl = document.getElementById('root')!
  const router = createAppRouter(window.__ROUTER_HYDRATION__)
  const app = (
    <StrictMode>
      <ClientShell router={router} />
    </StrictMode>
  )

  if (rootEl.hasChildNodes()) {
    hydrateRoot(rootEl, app)
  } else {
    createRoot(rootEl).render(app)
  }
}

void boot()
