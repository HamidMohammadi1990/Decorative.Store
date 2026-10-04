import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { Locale } from '@/models/shared/locale.model'
import type { ThemeMode } from '@/models/shared/theme.model'
import { currencyService, isValidCurrencyConfig } from '@/services/currencyService'

interface SettingsState {
  locale: Locale
  theme: ThemeMode
  currency: CurrencyConfig | null
  setLocale: (locale: Locale) => Promise<void>
  setTheme: (theme: ThemeMode) => void
  hydrateCurrency: () => Promise<void>
}

export const useSettingsStore = create<SettingsState>()(
  persist(
    (set, get) => ({
      locale: 'en',
      theme: 'system',
      currency: null,

      setLocale: async (locale) => {
        set({ locale })
        const currency = await currencyService.getActive(locale)
        set({ currency })
      },

      setTheme: (theme) => set({ theme }),

      hydrateCurrency: async () => {
        if (isValidCurrencyConfig(get().currency)) return
        const currency = await currencyService.getActive(get().locale)
        set({ currency })
      },
    }),
    {
      name: 'westelm-settings',
      partialize: (state) => ({ locale: state.locale, theme: state.theme }),
    },
  ),
)
