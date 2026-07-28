import { useEffect } from 'react'
import { applyTheme } from '@/extensions/applyTheme'
import type { ThemeMode } from '@/models/shared/theme.model'
import { useSettingsStore } from '@/stores/settingsStore'

export function useTheme() {
  const theme = useSettingsStore((s) => s.theme)
  const setTheme = useSettingsStore((s) => s.setTheme)

  useEffect(() => {
    applyTheme(theme)

    if (theme !== 'system') return

    const media = window.matchMedia('(prefers-color-scheme: dark)')
    const onChange = () => applyTheme('system')

    media.addEventListener('change', onChange)
    return () => media.removeEventListener('change', onChange)
  }, [theme])

  return {
    theme,
    setTheme: (next: ThemeMode) => setTheme(next),
  }
}
