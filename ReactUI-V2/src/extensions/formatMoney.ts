import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { Money } from '@/models/shared/money.model'

/** Formats a numeric amount only — pair with {@link PriceDisplay} for the Toman label. */
export function formatMoney(money: Money, currency: CurrencyConfig): string {
  const numberLocale = currency?.numberLocale ?? 'en-US'
  return new Intl.NumberFormat(numberLocale, {
    maximumFractionDigits: 0,
  }).format(money.amount)
}
