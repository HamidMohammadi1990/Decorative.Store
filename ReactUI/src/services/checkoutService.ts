import type { CheckoutOrderData } from '@/models/checkout/checkout.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'

const CHECKOUT_PATH = '/api/v1/order/checkout'

export const checkoutService = {
  async getProductCheckout(
    productId: string,
    locale: Locale,
    accessToken?: string | null,
  ): Promise<CheckoutOrderData> {
    return apiPost<CheckoutOrderData>(
      CHECKOUT_PATH,
      { productId },
      { locale, accessToken },
    )
  },
}
