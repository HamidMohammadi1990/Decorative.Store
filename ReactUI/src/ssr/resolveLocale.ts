import type { Locale } from '@/models/shared/locale.model'
import { readSettingsFromCookieHeader } from '@/extensions/settingsCookie'

/** Resolve storefront locale from SSR request (cookie → Accept-Language → en). */
export function resolveLocale(request: Request): Locale {
  const fromCookie = readSettingsFromCookieHeader(request.headers.get('cookie'))
  if (fromCookie) return fromCookie.locale

  const acceptLanguage = request.headers.get('accept-language') ?? ''
  if (/\bfa(-IR)?\b/i.test(acceptLanguage)) return 'fa'

  return 'en'
}
