import type { Locale } from '@/models/shared/locale.model'
import type { ThemeMode } from '@/models/shared/theme.model'

export const SETTINGS_STORAGE_KEY = 'westelm-settings'
const COOKIE_MAX_AGE_SEC = 60 * 60 * 24 * 365

interface SettingsCookieState {
  locale: Locale
  theme: ThemeMode
}

function parseSettingsCookieValue(value: string): SettingsCookieState | null {
  try {
    const parsed = JSON.parse(decodeURIComponent(value)) as { state?: SettingsCookieState }
    const locale = parsed.state?.locale
    const theme = parsed.state?.theme
    if (locale !== 'fa' && locale !== 'en') return null
    if (theme !== 'light' && theme !== 'dark' && theme !== 'system') return null
    return { locale, theme }
  } catch {
    return null
  }
}

export function readSettingsFromCookieHeader(cookieHeader: string | null): SettingsCookieState | null {
  if (!cookieHeader) return null

  for (const part of cookieHeader.split(';')) {
    const trimmed = part.trim()
    if (!trimmed.startsWith(`${SETTINGS_STORAGE_KEY}=`)) continue
    return parseSettingsCookieValue(trimmed.slice(SETTINGS_STORAGE_KEY.length + 1))
  }

  return null
}

export function writeSettingsCookie(state: SettingsCookieState) {
  if (typeof document === 'undefined') return

  const payload = encodeURIComponent(JSON.stringify({ state }))
  document.cookie = `${SETTINGS_STORAGE_KEY}=${payload}; Path=/; Max-Age=${COOKIE_MAX_AGE_SEC}; SameSite=Lax`
}
