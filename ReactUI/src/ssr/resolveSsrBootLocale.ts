import type { HydrationState } from 'react-router-dom'
import type { Locale } from '@/models/shared/locale.model'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'

function isLocale(value: unknown): value is Locale {
  return value === 'fa' || value === 'en'
}

/** Locale from SSR boot scripts or hydrated router loader data. */
export function resolveSsrBootLocale(
  ssrLocale?: Locale,
  hydrationData?: HydrationState,
): Locale | undefined {
  if (isLocale(ssrLocale)) return ssrLocale

  const shopLoader = hydrationData?.loaderData?.shop as ShopLayoutLoaderData | undefined
  if (isLocale(shopLoader?.locale)) return shopLoader.locale

  return undefined
}
