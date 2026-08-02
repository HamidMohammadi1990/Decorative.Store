import type { CatalogListingResponse } from '@/models/catalog/catalogListing.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'

const CATALOG_LISTING_PATH = '/api/v1/catalog/listing'

export const catalogListingService = {
  async getListing(path: string, locale: Locale): Promise<CatalogListingResponse> {
    const normalizedPath = path.replace(/^\/+/, '').replace(/\/+$/, '')
    const query = normalizedPath ? `?path=${encodeURIComponent(normalizedPath)}` : ''
    return apiGet<CatalogListingResponse>(`${CATALOG_LISTING_PATH}${query}`, locale)
  },
}
