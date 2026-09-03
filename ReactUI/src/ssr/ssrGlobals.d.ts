import type { Locale } from '@/models/shared/locale.model'

declare global {
  interface Window {
    __SSR_LOCALE__?: Locale
    __SSR_LAZY_ROUTES__?: string[]
  }
}

export {}
