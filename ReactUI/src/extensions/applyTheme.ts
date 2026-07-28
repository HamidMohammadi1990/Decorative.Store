import type { ResolvedTheme, ThemeMode } from '@/models/shared/theme.model'

const STORAGE_KEY = 'westelm-settings'

export function resolveTheme(mode: ThemeMode): ResolvedTheme {
  if (mode === 'dark') return 'dark'
  if (mode === 'light') return 'light'
  return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
}

export function applyTheme(mode: ThemeMode) {
  const resolved = resolveTheme(mode)
  const root = document.documentElement

  root.classList.toggle('dark', resolved === 'dark')
  root.dataset.theme = mode
  root.style.colorScheme = resolved
}

export function getStoredTheme(): ThemeMode {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    if (!raw) return 'system'

    const parsed = JSON.parse(raw) as { state?: { theme?: ThemeMode } }
    const theme = parsed.state?.theme

    if (theme === 'light' || theme === 'dark' || theme === 'system') {
      return theme
    }
  } catch {
    // ignore malformed storage
  }

  return 'system'
}
