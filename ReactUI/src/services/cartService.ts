import { API_BASE_URL } from '@/config/api'
import type { CartLine } from '@/models/cart/cartLine.model'
import { apiGetAuth, apiPost } from '@/services/api/apiClient'
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

function resolveImageSrc(imageUrl?: string | null): string {
  if (!imageUrl) return '/images/home/new-arrivals.svg'
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  return `${API_BASE_URL}${imageUrl.startsWith('/') ? imageUrl : `/${imageUrl}`}`
}

export function mapServerCartToLines(cart: ServerCartResponse): CartLine[] {
  return cart.items.map((item) => ({
    lineId: item.orderItemId,
    sku: item.productId,
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

  isConfigurationRequired(error: unknown) {
    return error instanceof ApiError && error.hasCode('ProductRequiresConfiguration')
  },
}
