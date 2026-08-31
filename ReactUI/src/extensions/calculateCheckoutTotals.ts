import type { CartLine } from '@/models/cart/cartLine.model'

export type FulfillmentType = 'delivery' | 'pickup'
export type DeliveryMethod = 'standard' | 'express'

export interface CheckoutTotals {
  itemCount: number
  subtotal: number
  shipping: number
  tax: number
  total: number
  shippingIsFree: boolean
  fulfillment: FulfillmentType
}

const FREE_SHIPPING_THRESHOLD = 5_000_000
export { FREE_SHIPPING_THRESHOLD }
const STANDARD_SHIPPING = 290_000
const EXPRESS_SHIPPING = 490_000
const TAX_RATE = 0.2

export function getCartSubtotal(lines: CartLine[]) {
  return lines.reduce((sum, line) => sum + line.unitPrice.amount * line.quantity, 0)
}

export function calculateCheckoutTotals(
  lines: CartLine[],
  fulfillment: FulfillmentType,
  delivery: DeliveryMethod = 'standard',
): CheckoutTotals {
  const itemCount = lines.reduce((sum, line) => sum + line.quantity, 0)
  const subtotal = getCartSubtotal(lines)

  let shipping = 0
  let shippingIsFree = false

  if (fulfillment === 'delivery') {
    shippingIsFree = delivery === 'standard' && subtotal >= FREE_SHIPPING_THRESHOLD
    shipping =
      delivery === 'express'
        ? EXPRESS_SHIPPING
        : shippingIsFree
          ? 0
          : STANDARD_SHIPPING
  }

  const taxable = subtotal + shipping
  const tax = Math.round(taxable * TAX_RATE * 100) / 100
  const total = Math.round((taxable + tax) * 100) / 100

  return { itemCount, subtotal, shipping, tax, total, shippingIsFree, fulfillment }
}
