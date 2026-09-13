const CATALOG_LISTING_SKIP_PREFIXES = [
  '/cart',
  '/checkout',
  '/account',
  '/compare',
  '/room-layout',
] as const

/** Full document navigation for SSR (preserves query string). */
export function navigateSsr(href: string) {
  window.location.assign(href)
}

/** Shop routes that should use full document navigation so SSR renders HTML + loader data. */
export function isSsrFullPageHref(href: string): boolean {
  if (!href.startsWith('/') || href.startsWith('//')) return false
  if (href === '/') return true
  return !CATALOG_LISTING_SKIP_PREFIXES.some((prefix) => href.startsWith(prefix))
}

/** Catalog listing paths (excludes home, blog, and product detail). */
export function isCatalogListingHref(href: string): boolean {
  if (!isSsrFullPageHref(href) || href === '/') return false
  return !href.startsWith('/blog') && !href.startsWith('/product/')
}

/** Product detail paths. */
export function isProductDetailHref(href: string): boolean {
  return href.startsWith('/product/') && !href.startsWith('//')
}
