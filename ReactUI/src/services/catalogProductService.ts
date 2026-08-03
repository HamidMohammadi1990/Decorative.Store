import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'
import {
  normalizeCatalogListingProduct,
  normalizeCatalogProductResponse,
} from '@/services/mappers/catalogResponseNormalize'

const CATALOG_PRODUCT_PATH = '/api/v1/catalog/product'
const CATALOG_RELATED_PATH = '/api/v1/catalog/product/related'

export const catalogProductService = {
  async getProduct(slug: string, locale: Locale) {
    const query = `?slug=${encodeURIComponent(slug)}`
    const data = await apiGet<unknown>(`${CATALOG_PRODUCT_PATH}${query}`, locale)
    return normalizeCatalogProductResponse(data)
  },

  async getRelatedProducts(
    slug: string,
    locale: Locale,
    limit = 4,
  ): Promise<CatalogListingProduct[]> {
    const query = `?slug=${encodeURIComponent(slug)}&limit=${limit}`
    const response = await apiGet<{ products?: unknown[]; Products?: unknown[] }>(
      `${CATALOG_RELATED_PATH}${query}`,
      locale,
    )
    const products = response.products ?? response.Products ?? []
    return products.map(normalizeCatalogListingProduct)
  },
}
