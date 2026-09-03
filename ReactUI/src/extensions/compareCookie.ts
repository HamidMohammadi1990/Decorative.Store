import { MAX_COMPARE_PRODUCTS } from '@/models/catalog/compare.model'

export const COMPARE_COOKIE_NAME = 'diba-compare-slugs'
const COOKIE_MAX_AGE_SEC = 60 * 60 * 24 * 30

function normalizeSlugs(raw: unknown): string[] {
  if (!Array.isArray(raw)) return []
  return raw
    .filter((item): item is string => typeof item === 'string')
    .map((item) => item.trim())
    .filter(Boolean)
    .slice(0, MAX_COMPARE_PRODUCTS)
}

function parseCookieValue(value: string): string[] {
  try {
    return normalizeSlugs(JSON.parse(decodeURIComponent(value)))
  } catch {
    return []
  }
}

export function readCompareSlugsFromCookieHeader(cookieHeader: string | null): string[] {
  if (!cookieHeader) return []

  for (const part of cookieHeader.split(';')) {
    const trimmed = part.trim()
    if (!trimmed.startsWith(`${COMPARE_COOKIE_NAME}=`)) continue
    return parseCookieValue(trimmed.slice(COMPARE_COOKIE_NAME.length + 1))
  }

  return []
}

export function readCompareSlugsFromDocumentCookie(): string[] {
  if (typeof document === 'undefined') return []
  return readCompareSlugsFromCookieHeader(document.cookie)
}

export function writeCompareSlugsToCookie(slugs: string[]) {
  if (typeof document === 'undefined') return

  const normalized = normalizeSlugs(slugs)
  const encoded = encodeURIComponent(JSON.stringify(normalized))
  document.cookie = `${COMPARE_COOKIE_NAME}=${encoded}; Path=/; Max-Age=${COOKIE_MAX_AGE_SEC}; SameSite=Lax`
}
