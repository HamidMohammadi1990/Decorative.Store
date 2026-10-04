import type { HydrationState } from 'react-router-dom'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'
import { languageService } from '@/services/languageService'

/** Reuse SSR-resolved language id on the client so /language/search is not called again. */
export function seedLanguageFromHydration(hydrationData?: HydrationState) {
  const shopLoader = hydrationData?.loaderData?.shop as ShopLayoutLoaderData | undefined
  if (!shopLoader?.languageId || shopLoader.languageId <= 0) return
  if (shopLoader.locale !== 'fa' && shopLoader.locale !== 'en') return

  languageService.seedResolvedLanguage(shopLoader.locale, shopLoader.languageId)
}
