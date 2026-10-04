import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { Locale } from '@/models/shared/locale.model'
import { mockFetch } from '@/services/api/mockClient'

/** Mock currency config — replace with API when backend is ready */
const CURRENCY_BY_LOCALE: Record<Locale, CurrencyConfig> = {
  en: { code: 'IRT', symbol: 'تومان', numberLocale: 'en-US' },
  fa: { code: 'IRT', symbol: 'تومان', numberLocale: 'fa-IR' },
}

export function getCurrencyConfig(locale: Locale): CurrencyConfig {
  return CURRENCY_BY_LOCALE[locale]
}

export function isValidCurrencyConfig(
  currency: CurrencyConfig | null | undefined,
): currency is CurrencyConfig {
  return Boolean(currency?.code && currency?.numberLocale)
}

export const currencyService = {
  async getActive(locale: Locale): Promise<CurrencyConfig> {
    return mockFetch(async () => getCurrencyConfig(locale))
  },
}
