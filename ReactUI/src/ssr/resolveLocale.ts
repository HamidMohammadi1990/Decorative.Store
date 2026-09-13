import type { Locale } from '@/models/shared/locale.model'
import { readSettingsFromCookieHeader } from '@/extensions/settingsCookie'

function readLocaleHintFromUrl(request: Request): Locale | null {
  const url = new URL(request.url)
  const hint = url.searchParams.get('hl')?.trim().toLowerCase()
  if (hint === 'fa' || hint === 'fa-ir') return 'fa'
  if (hint === 'en' || hint === 'en-us') return 'en'
  return null
}

/** Resolve storefront locale from SSR request (hl query → cookie → Accept-Language → en). */
export function resolveLocale(request: Request): Locale {
  const fromHint = readLocaleHintFromUrl(request)
  if (fromHint) return fromHint

  const fromCookie = readSettingsFromCookieHeader(request.headers.get('cookie'))
  if (fromCookie) return fromCookie.locale

  const acceptLanguage = request.headers.get('accept-language') ?? ''
  if (/\bfa(-IR)?\b/i.test(acceptLanguage)) return 'fa'

  return 'en'
}
