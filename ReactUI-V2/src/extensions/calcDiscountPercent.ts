import type { Money } from '@/models/shared/money.model'

export function calcDiscountPercent(price: Money, compareAtPrice: Money): number | null {
  if (compareAtPrice.amount <= 0 || price.amount >= compareAtPrice.amount) return null
  return Math.round((1 - price.amount / compareAtPrice.amount) * 100)
}
