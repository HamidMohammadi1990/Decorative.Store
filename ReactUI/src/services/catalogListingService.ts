import type { CatalogListingResponse } from '@/models/catalog/catalogListing.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'
import { buildCatalogListingQuery } from '@/services/mappers/catalogListingMapper'
import { normalizeCatalogListingResponse } from '@/services/mappers/catalogResponseNormalize'

const CATALOG_LISTING_PATH = '/api/v1/catalog/listing'

export const catalogListingService = {
  async getListing(
    path: string,
    locale: Locale,
    searchParams: URLSearchParams = new URLSearchParams(),
  ): Promise<CatalogListingResponse> {
    const normalizedPath = path.replace(/^\/+/, '').replace(/\/+$/, '')
    const filterQuery = buildCatalogListingQuery(searchParams)
    const query = normalizedPath
      ? `?path=${encodeURIComponent(normalizedPath)}${filterQuery}`
      : filterQuery
        ? `?${filterQuery.slice(1)}`
        : ''

    const data = await apiGet<unknown>(`${CATALOG_LISTING_PATH}${query}`, locale)
    return normalizeCatalogListingResponse(data)
  },
}
