import { StrictMode, useEffect } from 'react'
import { createRoot } from 'react-dom/client'
import '@/i18n'
import { syncI18nLocale } from '@/i18n'
import { applyTheme, getStoredTheme } from '@/extensions/applyTheme'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'
import './index.css'
import App from './App.tsx'

applyTheme(getStoredTheme())
syncI18nLocale(useSettingsStore.getState().locale)

function Bootstrap() {
  const restoreSession = useUserStore((s) => s.restoreSession)

  useEffect(() => {
    void restoreSession()
  }, [restoreSession])

  return <App />
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Bootstrap />
  </StrictMode>,
)
