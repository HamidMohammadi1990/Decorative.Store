/** Maps storefront paths to CMS page slugs for content pages (legal, design services, generic). */
export const CMS_CONTENT_PATH_TO_SLUG: Record<string, string> = {
  '/privacy': 'privacy',
  '/terms': 'terms',
  '/legal': 'legal',
  '/returns': 'returns',
  '/delivery': 'delivery',
  '/promo-terms': 'promo-terms',
  '/sustainability': 'sustainability',
  '/careers': 'careers',
  '/stores': 'stores',
  '/design-services': 'design-services',
}

export const CMS_CONTENT_SLUGS = new Set(Object.values(CMS_CONTENT_PATH_TO_SLUG))

export function resolveCmsContentSlug(pathname: string): string | null {
  const direct = CMS_CONTENT_PATH_TO_SLUG[pathname]
  if (direct) return direct

  if (pathname.startsWith('/p/')) {
    const slug = pathname.slice(3).split('/')[0]?.trim()
    return slug || null
  }

  return null
}

export function isCmsContentRoute(pathname: string): boolean {
  return resolveCmsContentSlug(pathname) !== null
}

export function cmsContentPagePath(slug: string): string {
  const entry = Object.entries(CMS_CONTENT_PATH_TO_SLUG).find(([, value]) => value === slug)
  return entry?.[0] ?? `/p/${slug}`
}
