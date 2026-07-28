import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '@/i18n'
import { syncI18nLocale } from '@/i18n'
import { applyTheme, getStoredTheme } from '@/extensions/applyTheme'
import { useSettingsStore } from '@/stores/settingsStore'
import './index.css'
import App from './App.tsx'

applyTheme(getStoredTheme())
syncI18nLocale(useSettingsStore.getState().locale)

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
