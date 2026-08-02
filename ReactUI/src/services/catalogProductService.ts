import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type { CatalogProductResponse } from '@/models/catalog/catalogProduct.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'

const CATALOG_PRODUCT_PATH = '/api/v1/catalog/product'
const CATALOG_RELATED_PATH = '/api/v1/catalog/product/related'

export const catalogProductService = {
  async getProduct(slug: string, locale: Locale): Promise<CatalogProductResponse> {
    const query = `?slug=${encodeURIComponent(slug)}`
    return apiGet<CatalogProductResponse>(`${CATALOG_PRODUCT_PATH}${query}`, locale)
  },

  async getRelatedProducts(
    slug: string,
    locale: Locale,
    limit = 4,
  ): Promise<CatalogListingProduct[]> {
    const query = `?slug=${encodeURIComponent(slug)}&limit=${limit}`
    const response = await apiGet<{ products: CatalogListingProduct[] }>(
      `${CATALOG_RELATED_PATH}${query}`,
      locale,
    )
    return response.products
  },
}
