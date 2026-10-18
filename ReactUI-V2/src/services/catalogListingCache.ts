import type { ListingQuery, ProductListingResult } from '@/models/catalog/listing.model'
import type { Locale } from '@/models/shared/locale.model'
import { catalogService } from '@/services/catalogService'

/** Dedupe in-flight listing requests only — do not keep stale product titles after admin edits. */
const inFlight = new Map<string, Promise<ProductListingResult>>()

function buildCacheKey(locale: Locale, pathname: string, search: string) {
  return `${locale}|${pathname}|${search}`
}

function startListingRequest(query: ListingQuery, locale: Locale): Promise<ProductListingResult> {
  const key = buildCacheKey(locale, query.pathname, query.searchParams.toString())
  const existing = inFlight.get(key)
  if (existing) return existing

  const promise = catalogService.getListing(query, locale).finally(() => {
    inFlight.delete(key)
  })
  inFlight.set(key, promise)
  return promise
}

export async function getCatalogListingCached(
  query: ListingQuery,
  locale: Locale,
): Promise<ProductListingResult> {
  return startListingRequest(query, locale)
}

export function clearCatalogListingCache() {
  inFlight.clear()
}
