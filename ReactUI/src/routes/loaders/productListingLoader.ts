import type { LoaderFunctionArgs } from 'react-router-dom'
import { catalogService } from '@/services/catalogService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { ProductListingLoaderData } from '@/routes/loaders/types'

export async function productListingLoader({
  request,
}: LoaderFunctionArgs): Promise<ProductListingLoaderData> {
  const locale = resolveLocale(request)
  const url = new URL(request.url)

  // Client: return immediately so navigation is not blocked by the API.
  if (typeof window !== 'undefined') {
    return {
      locale,
      pathname: url.pathname,
      search: url.search,
      data: null,
    }
  }

  try {
    const data = await catalogService.getListing(
      { pathname: url.pathname, searchParams: url.searchParams },
      locale,
    )

    return {
      locale,
      pathname: url.pathname,
      search: url.search,
      data,
      error: data.pathNotFound ? 'not-found' : undefined,
    }
  } catch {
    return {
      locale,
      pathname: url.pathname,
      search: url.search,
      data: null,
      error: 'failed',
    }
  }
}
