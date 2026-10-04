import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'

const CATALOG_SEARCH_PATH = '/api/v1/catalog/search'

export const catalogSearchService = {
  async search(query: string, locale: Locale, limit = 8): Promise<CatalogSearchResponse> {
    const q = query.trim()
    if (!q) {
      return { categories: [], subCategories: [], products: [] }
    }

    const params = new URLSearchParams({ q, limit: String(limit) })
    return apiGet<CatalogSearchResponse>(`${CATALOG_SEARCH_PATH}?${params}`, locale)
  },
}
