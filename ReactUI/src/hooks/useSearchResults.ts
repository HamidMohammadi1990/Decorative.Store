import { useEffect, useState } from 'react'
import { useSearchParams, useRouteLoaderData } from 'react-router-dom'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import { catalogSearchService } from '@/services/catalogSearchService'
import type { SearchPageLoaderData } from '@/routes/loaders/types'
import { useSettingsStore } from '@/stores/settingsStore'

const MIN_QUERY_LENGTH = 2
const RESULT_LIMIT = 100

function isLoaderFresh(
  loaderData: SearchPageLoaderData | undefined,
  locale: string,
  query: string,
): loaderData is SearchPageLoaderData {
  return Boolean(loaderData && loaderData.locale === locale && loaderData.query === query)
}

export function useSearchResults() {
  const locale = useSettingsStore((s) => s.locale)
  const [searchParams] = useSearchParams()
  const query = (searchParams.get('q') ?? '').trim()
  const loaderData = useRouteLoaderData('search') as SearchPageLoaderData | undefined
  const loaderFresh = isLoaderFresh(loaderData, locale, query) && query.length >= MIN_QUERY_LENGTH

  const [data, setData] = useState<CatalogSearchResponse | null>(() =>
    loaderFresh ? (loaderData.data ?? null) : null,
  )
  const [loading, setLoading] = useState(() => query.length >= MIN_QUERY_LENGTH && !loaderFresh)

  useEffect(() => {
    if (query.length < MIN_QUERY_LENGTH) {
      setData(null)
      setLoading(false)
      return
    }

    if (loaderFresh) {
      setData(loaderData.data ?? null)
      setLoading(false)
      return
    }

    let cancelled = false
    setLoading(true)

    catalogSearchService
      .search(query, locale, RESULT_LIMIT)
      .then((response) => {
        if (!cancelled) setData(response)
      })
      .catch(() => {
        if (!cancelled) setData(null)
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })

    return () => {
      cancelled = true
    }
  }, [query, locale, loaderFresh, loaderData])

  return { query, data, loading }
}
