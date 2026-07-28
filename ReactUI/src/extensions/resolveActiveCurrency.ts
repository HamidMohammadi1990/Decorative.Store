import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { Locale } from '@/models/shared/locale.model'
import { getCurrencyConfig, isValidCurrencyConfig } from '@/services/currencyService'

/** Sync fallback while async currency hydration is pending or incomplete. */
export function resolveActiveCurrency(
  locale: Locale,
  currency: CurrencyConfig | null | undefined,
): CurrencyConfig {
  return isValidCurrencyConfig(currency) ? currency : getCurrencyConfig(locale)
}
