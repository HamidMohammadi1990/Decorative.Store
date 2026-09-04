import type { Locale } from '@/models/shared/locale.model'

declare global {
  interface Window {
    __SSR_LOCALE__?: Locale
    __SSR_LAZY_ROUTES__?: string[]
  }

  // Used by src/config/api.ts during SSR bundle evaluation.
  // eslint-disable-next-line no-var
  var process: {
    env?: Record<string, string | undefined>
  }
}

export {}
