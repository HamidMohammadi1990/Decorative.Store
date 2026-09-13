import type { ListingQuery, ProductListingResult } from '@/models/catalog/listing.model'
import type { Locale } from '@/models/shared/locale.model'
import { catalogService } from '@/services/catalogService'

interface CacheEntry {
  promise: Promise<ProductListingResult>
  result?: ProductListingResult
}

const cache = new Map<string, CacheEntry>()

function buildCacheKey(locale: Locale, pathname: string, search: string) {
  return `${locale}|${pathname}|${search}`
}

function startListingRequest(query: ListingQuery, locale: Locale): Promise<ProductListingResult> {
  const key = buildCacheKey(locale, query.pathname, query.searchParams.toString())
  const existing = cache.get(key)
  if (existing?.result) return Promise.resolve(existing.result)
  if (existing?.promise) return existing.promise

  const entry: CacheEntry = {
    promise: catalogService.getListing(query, locale).then((result) => {
      entry.result = result
      return result
    }),
  }
  cache.set(key, entry)
  return entry.promise
}

export async function getCatalogListingCached(
  query: ListingQuery,
  locale: Locale,
): Promise<ProductListingResult> {
  return startListingRequest(query, locale)
}

export function clearCatalogListingCache() {
  cache.clear()
}
