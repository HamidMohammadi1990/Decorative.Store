import type { ProductFacets } from '@/models/catalog/product.model'

const COLLECTIONS = new Set([
  'sale',
  'new',
  'in-stock',
  'best-sellers',
  'clearance',
  'shop',
  'collaborations',
])

export interface ResolvedListingPath {
  collection: string | null
  segments: string[]
  pathname: string
}

export function resolveListingPath(pathname: string): ResolvedListingPath {
  const segments = pathname.split('/').filter(Boolean)
  let collection: string | null = null
  let rest = [...segments]

  if (rest.length > 0 && COLLECTIONS.has(rest[0])) {
    collection = rest[0]
    rest = rest.slice(1)
  }

  return { collection, segments: rest, pathname }
}

export function slugMatchesCategory(productSlug: string, pathSlug: string) {
  return productSlug === pathSlug || productSlug.startsWith(`${pathSlug}-`)
}

export function productMatchesPathSegments(
  categorySlugs: string[],
  subcategorySlug: string | undefined,
  facets: ProductFacets,
  segments: string[],
) {
  if (segments.length === 0) return true

  const primary = segments[0]
  const primaryMatch = categorySlugs.some(
    (slug) => slug === primary || slugMatchesCategory(slug, primary),
  )
  if (!primaryMatch) return false

  if (segments.length === 1) return true

  const secondary = segments[1]
  const facetValues = [
    ...(facets.size ?? []),
    ...(facets.color ?? []),
    ...(facets.material ?? []),
    ...(facets.room ?? []),
  ]

  const secondaryMatch =
    subcategorySlug === secondary ||
    (subcategorySlug != null &&
      (subcategorySlug.endsWith(`-${secondary}`) ||
        subcategorySlug.startsWith(`${secondary}-`))) ||
    categorySlugs.some(
      (slug) => slug === secondary || slugMatchesCategory(slug, secondary),
    ) ||
    facetValues.includes(secondary)

  return secondaryMatch
}

export function productMatchesCollection(
  collection: string | null,
  flags: { onSale: boolean; isNew: boolean; inStock: boolean },
) {
  if (!collection) return true
  if (collection === 'sale' || collection === 'clearance') return flags.onSale
  if (collection === 'new') return flags.isNew
  if (collection === 'in-stock') return flags.inStock
  if (collection === 'best-sellers') return true
  return true
}
