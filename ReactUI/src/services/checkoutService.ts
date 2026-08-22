import type { CheckoutOrderData } from '@/models/checkout/checkout.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'

const CHECKOUT_PATH = '/api/v1/order/checkout'

const checkoutCache = new Map<string, CheckoutOrderData>()
const checkoutRequests = new Map<string, Promise<CheckoutOrderData>>()

function requestKey(productId: string, locale: Locale, userId?: string | null) {
  return `${locale}:${userId ?? 'guest'}:${productId}`
}

export const checkoutService = {
  async getProductCheckout(
    productId: string,
    locale: Locale,
    accessToken?: string | null,
    userId?: string | null,
  ): Promise<CheckoutOrderData> {
    const key = requestKey(productId, locale, userId)

    const cached = checkoutCache.get(key)
    if (cached) return cached

    const inFlight = checkoutRequests.get(key)
    if (inFlight) return inFlight

    const request = apiPost<CheckoutOrderData>(
      CHECKOUT_PATH,
      { productId },
      { locale, accessToken },
    )

    checkoutRequests.set(key, request)

    try {
      const data = await request
      checkoutCache.set(key, data)
      return data
    } catch (error) {
      checkoutCache.delete(key)
      throw error
    } finally {
      checkoutRequests.delete(key)
    }
  },
}
