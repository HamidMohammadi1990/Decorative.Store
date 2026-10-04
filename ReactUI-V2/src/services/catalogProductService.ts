import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type { CatalogProductResponse } from '@/models/catalog/catalogProduct.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'
import {
  normalizeCatalogListingProduct,
  normalizeCatalogProductResponse,
} from '@/services/mappers/catalogResponseNormalize'
import type { FeaturedCatalogCollection } from '@/services/mappers/featuredShopMapper'

const CATALOG_PRODUCT_PATH = '/api/v1/catalog/product'
const CATALOG_RELATED_PATH = '/api/v1/catalog/product/related'
const CATALOG_NEWEST_PATH = '/api/v1/catalog/newest'
const CATALOG_FEATURED_COLLECTIONS_PATH = '/api/v1/catalog/featured-collections'

const productRequests = new Map<string, Promise<CatalogProductResponse>>()
const relatedRequests = new Map<string, Promise<CatalogListingProduct[]>>()
const newestRequests = new Map<string, Promise<CatalogListingProduct[]>>()
const featuredCollectionsRequests = new Map<string, Promise<FeaturedCatalogCollection[]>>()

function productRequestKey(slug: string, locale: Locale) {
  return `${locale}:${slug}`
}

function relatedRequestKey(slug: string, locale: Locale, limit: number) {
  return `${locale}:${slug}:${limit}`
}

function newestRequestKey(locale: Locale, limit: number) {
  return `${locale}:${limit}`
}

function normalizeFeaturedCatalogCollection(data: unknown): FeaturedCatalogCollection {
  const record = (data && typeof data === 'object' ? data : {}) as Record<string, unknown>
  const id = String(record.id ?? record.Id ?? '')
  const href = String(record.href ?? record.Href ?? '')
  const imageUrl = String(record.imageUrl ?? record.ImageUrl ?? '')
  const imageAlt = String(record.imageAlt ?? record.ImageAlt ?? '')

  return { id, href, imageUrl, imageAlt }
}

export const catalogProductService = {
  async getProduct(slug: string, locale: Locale) {
    const key = productRequestKey(slug, locale)
    const inFlight = productRequests.get(key)
    if (inFlight) return inFlight

    const request = (async () => {
      const query = `?slug=${encodeURIComponent(slug)}`
      const data = await apiGet<unknown>(`${CATALOG_PRODUCT_PATH}${query}`, locale)
      return normalizeCatalogProductResponse(data)
    })()

    productRequests.set(key, request)

    try {
      return await request
    } finally {
      productRequests.delete(key)
    }
  },

  async getRelatedProducts(
    slug: string,
    locale: Locale,
    limit = 4,
  ): Promise<CatalogListingProduct[]> {
    const key = relatedRequestKey(slug, locale, limit)
    const inFlight = relatedRequests.get(key)
    if (inFlight) return inFlight

    const request = (async () => {
      const query = `?slug=${encodeURIComponent(slug)}&limit=${limit}`
      const response = await apiGet<{ products?: unknown[]; Products?: unknown[] }>(
        `${CATALOG_RELATED_PATH}${query}`,
        locale,
      )
      const products = response.products ?? response.Products ?? []
      return products.map(normalizeCatalogListingProduct)
    })()

    relatedRequests.set(key, request)

    try {
      return await request
    } finally {
      relatedRequests.delete(key)
    }
  },

  async getNewestProducts(locale: Locale, limit = 4): Promise<CatalogListingProduct[]> {
    const key = newestRequestKey(locale, limit)
    const inFlight = newestRequests.get(key)
    if (inFlight) return inFlight

    const request = (async () => {
      const query = `?limit=${limit}`
      const response = await apiGet<{ products?: unknown[]; Products?: unknown[] }>(
        `${CATALOG_NEWEST_PATH}${query}`,
        locale,
      )
      const products = response.products ?? response.Products ?? []
      return products.map(normalizeCatalogListingProduct)
    })()

    newestRequests.set(key, request)

    try {
      return await request
    } finally {
      newestRequests.delete(key)
    }
  },

  async getFeaturedCollections(locale: Locale): Promise<FeaturedCatalogCollection[]> {
    const key = locale
    const inFlight = featuredCollectionsRequests.get(key)
    if (inFlight) return inFlight

    const request = (async () => {
      const response = await apiGet<{ collections?: unknown[]; Collections?: unknown[] }>(
        CATALOG_FEATURED_COLLECTIONS_PATH,
        locale,
      )
      const collections = response.collections ?? response.Collections ?? []
      return collections.map(normalizeFeaturedCatalogCollection)
    })()

    featuredCollectionsRequests.set(key, request)

    try {
      return await request
    } finally {
      featuredCollectionsRequests.delete(key)
    }
  },

  async getProductsBySlugs(slugs: string[], locale: Locale): Promise<CatalogProductResponse[]> {
    const uniqueSlugs = [...new Set(slugs.filter(Boolean))]
    const products = await Promise.all(uniqueSlugs.map((slug) => this.getProduct(slug, locale)))
    const bySlug = new Map(uniqueSlugs.map((slug, index) => [slug, products[index]]))

    return slugs
      .map((slug) => bySlug.get(slug))
      .filter((product): product is CatalogProductResponse => product !== undefined)
  },
}
