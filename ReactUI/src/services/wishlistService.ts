import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiGet, apiPost } from '@/services/api/apiClient'
import { readRecord } from '@/services/api/apiNormalize'

const PRODUCT_WISHLIST_MY_PATH = '/api/v1/product-wishlist/my'
const PRODUCT_WISHLIST_CREATE_PATH = '/api/v1/product-wishlist/create'
const PRODUCT_WISHLIST_DELETE_PATH = '/api/v1/product-wishlist/delete'
const PRODUCT_WISHLIST_CLEAR_PATH = '/api/v1/product-wishlist/clear'

function normalizeWishlistSlugs(data: unknown): string[] {
  const record = readRecord(data) ?? {}
  const raw = record.slugs ?? record.Slugs
  if (!Array.isArray(raw)) return []

  return raw
    .map((item) => (typeof item === 'string' ? item.trim() : ''))
    .filter(Boolean)
}

export const wishlistService = {
  async getMy(accessToken: string, locale: Locale): Promise<string[]> {
    const result = await apiGet<unknown>(PRODUCT_WISHLIST_MY_PATH, locale, accessToken)
    return normalizeWishlistSlugs(result)
  },

  async add(accessToken: string, locale: Locale, slug: string) {
    await apiPost(PRODUCT_WISHLIST_CREATE_PATH, { slug }, { locale, accessToken })
  },

  async remove(accessToken: string, slug: string) {
    await apiDelete(PRODUCT_WISHLIST_DELETE_PATH, accessToken, { slug })
  },

  async clearAll(accessToken: string) {
    await apiDelete(PRODUCT_WISHLIST_CLEAR_PATH, accessToken)
  },
}
