import type { LoaderFunctionArgs } from 'react-router-dom'
import { readCompareSlugsFromCookieHeader } from '@/extensions/compareCookie'
import { catalogService } from '@/services/catalogService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { ComparePageLoaderData } from '@/routes/loaders/types'

export async function comparePageLoader({
  request,
}: LoaderFunctionArgs): Promise<ComparePageLoaderData> {
  const locale = resolveLocale(request)
  const slugs = readCompareSlugsFromCookieHeader(request.headers.get('cookie'))

  if (slugs.length === 0) {
    return { locale, slugs, products: [] }
  }

  try {
    const products = await catalogService.getProductsBySlugs(slugs, locale)
    return { locale, slugs, products }
  } catch {
    return { locale, slugs, products: [], error: 'failed' }
  }
}
