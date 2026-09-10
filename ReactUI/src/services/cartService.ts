import { API_BASE_URL } from '@/config/api'
import type { CartLine } from '@/models/cart/cartLine.model'
import { apiGetAuth, apiPost, apiPatch, apiDelete } from '@/services/api/apiClient'
import { ApiError } from '@/services/api/apiTypes'

const CART_PATH = '/api/v1/order/cart'
const CART_ITEMS_PATH = '/api/v1/order/cart/items'

export interface ServerCartItem {
  orderItemId: string
  productId: string
  slug: string
  title: string
  imageUrl?: string | null
  imageAlt?: string | null
  quantity: number
  unitPrice: number
  currencyCode: string
}

export interface ServerCartSummary {
  totalPrice: number
  discountAmount: number
  finalPrice: number
  isDiscountApplied: boolean
  isDiscountInvalidated: boolean
  discountInvalidationMessage?: string | null
}

export interface ServerCartResponse {
  orderId?: string | null
  trackingCode?: number | null
  summary: ServerCartSummary
  items: ServerCartItem[]
}

export interface ValidateDiscountResponse {
  discountCode: string
  totalPrice: number
  discountAmount: number
  finalPrice: number
}

function resolveImageSrc(imageUrl?: string | null): string {
  if (!imageUrl?.trim()) return '/images/home/new-arrivals.svg'
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  if (imageUrl.startsWith('/')) return `${API_BASE_URL}${imageUrl}`
  return `${API_BASE_URL}/Uploads/Products/${imageUrl.replace(/^\/+/, '')}`
}

export function mapServerCartToLines(cart: ServerCartResponse): CartLine[] {
  return cart.items.map((item) => ({
    lineId: item.orderItemId,
    sku: item.productId,
    slug: item.slug || undefined,
    title: item.title,
    image: {
      src: resolveImageSrc(item.imageUrl),
      alt: item.imageAlt || item.title,
    },
    unitPrice: {
      amount: item.unitPrice,
      currencyCode: item.currencyCode,
    },
    quantity: item.quantity,
  }))
}

interface RemoveOrderItemResponse {
  orderId?: string | null
  trackingCode?: number | null
  isOrderDeleted: boolean
  cart: ServerCartSummary
}

export const cartService = {
  async getCart(accessToken: string): Promise<ServerCartResponse> {
    return apiGetAuth<ServerCartResponse>(CART_PATH, accessToken)
  },

  async addItem(
    accessToken: string,
    productId: string,
    quantity: number,
  ): Promise<ServerCartResponse> {
    return apiPost<ServerCartResponse>(
      CART_ITEMS_PATH,
      { productId, quantity },
      { accessToken },
    )
  },

  async updateItemQuantity(
    accessToken: string,
    orderItemId: string,
    quantity: number,
  ): Promise<ServerCartResponse> {
    return apiPatch<ServerCartResponse>(
      CART_ITEMS_PATH,
      { orderItemId, quantity },
      accessToken,
    )
  },

  async removeItem(accessToken: string, orderItemId: string): Promise<ServerCartResponse> {
    await apiDelete<RemoveOrderItemResponse>(
      '/api/v1/order/item',
      accessToken,
      { orderItemId },
    )
    return this.getCart(accessToken)
  },

  async validateDiscount(
    accessToken: string,
    discountCode: string,
  ): Promise<ValidateDiscountResponse> {
    const data = await apiPost<Record<string, unknown>>(
      '/api/v1/order/validate-discount',
      { discountCode: discountCode.trim() },
      { accessToken },
    )

    return {
      discountCode: String(data.discountCode ?? data.DiscountCode ?? ''),
      totalPrice: Number(data.totalPrice ?? data.TotalPrice ?? 0),
      discountAmount: Number(data.discountAmount ?? data.DiscountAmount ?? 0),
      finalPrice: Number(data.finalPrice ?? data.FinalPrice ?? 0),
    }
  },

  async removeDiscount(accessToken: string): Promise<ServerCartResponse> {
    await apiDelete<ServerCartSummary>('/api/v1/order/discount', accessToken)
    return this.getCart(accessToken)
  },

  isConfigurationRequired(error: unknown) {
    return error instanceof ApiError && error.hasCode('ProductRequiresConfiguration')
  },

  getDiscountErrorCode(error: unknown): string | null {
    if (!(error instanceof ApiError)) return null
    const code = error.messages[0]?.code
    return code ?? null
  },
}
