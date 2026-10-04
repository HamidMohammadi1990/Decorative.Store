import type { LoaderFunctionArgs } from 'react-router-dom'
import { catalogSearchService } from '@/services/catalogSearchService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { SearchPageLoaderData } from '@/routes/loaders/types'

const MIN_QUERY_LENGTH = 2
const RESULT_LIMIT = 100

export async function searchPageLoader({
  request,
}: LoaderFunctionArgs): Promise<SearchPageLoaderData> {
  const locale = resolveLocale(request)
  const url = new URL(request.url)
  const query = (url.searchParams.get('q') ?? '').trim()

  if (query.length < MIN_QUERY_LENGTH) {
    return { locale, query, data: null }
  }

  try {
    const data = await catalogSearchService.search(query, locale, RESULT_LIMIT)
    return { locale, query, data }
  } catch {
    return { locale, query, data: null, error: 'failed' }
  }
}
