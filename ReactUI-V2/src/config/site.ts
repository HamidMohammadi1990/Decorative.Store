import type { Locale } from '@/models/shared/locale.model'

export const SITE_NAME = 'Diba Gallery'

export const SITE_TAGLINE: Record<Locale, string> = {
  en: 'Contemporary Furniture & Interiors',
  fa: 'مبلمان و دکوراسیون معاصر',
}

export const DEFAULT_DESCRIPTION: Record<Locale, string> = {
  en: 'Shop contemporary furniture, rugs, and home décor. Design-led collections, room layout tools, and interior inspiration.',
  fa: 'خرید مبلمان معاصر، فرش و دکوراسیون منزل. کالکشن‌های طراحی‌شده، چیدمان اتاق و الهام دکوراسیون.',
}

/** Public storefront origin — set VITE_SITE_URL in production (e.g. https://dibagallery.com). */
export function getSiteOrigin(): string {
  const configured = (import.meta.env.VITE_SITE_URL as string | undefined)?.trim()
  if (configured) return configured.replace(/\/+$/, '')
  if (typeof window !== 'undefined') return window.location.origin
  return ''
}

export function absoluteUrl(path: string): string {
  const normalized = path.startsWith('/') ? path : `/${path}`
  const origin = getSiteOrigin()
  return origin ? `${origin}${normalized}` : normalized
}

export function buildPageTitle(title: string | undefined, locale: Locale): string {
  const trimmed = title?.trim()
  if (trimmed) return `${trimmed} — ${SITE_NAME}`
  return `${SITE_NAME} — ${SITE_TAGLINE[locale]}`
}

export const DEFAULT_OG_IMAGE = '/images/home/living-room.jpg'

export const TWITTER_HANDLE = '@dibagallery'
