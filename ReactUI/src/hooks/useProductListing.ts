import { useEffect, useState } from 'react'
import { useLocation, useRouteLoaderData } from 'react-router-dom'
import type { ProductListingResult } from '@/models/catalog/listing.model'
import { getCatalogListingCached } from '@/services/catalogListingCache'
import type { ProductListingLoaderData } from '@/routes/loaders/types'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductListingResult {
  data: ProductListingResult | null
  loading: boolean
  error: string | null
  reload: () => void
}

function isLoaderFresh(
  loaderData: ProductListingLoaderData | undefined,
  locale: string,
  pathname: string,
  search: string,
): loaderData is ProductListingLoaderData & { data: ProductListingResult } {
  return Boolean(
    loaderData &&
      loaderData.locale === locale &&
      loaderData.pathname === pathname &&
      loaderData.search === search &&
      loaderData.data,
  )
}

export function useProductListing(): UseProductListingResult {
  const locale = useSettingsStore((s) => s.locale)
  const location = useLocation()
  const loaderData = useRouteLoaderData('product-listing') as ProductListingLoaderData | undefined
  const requestKey = `${locale}|${location.pathname}|${location.search}`
  const loaderFresh = isLoaderFresh(
    loaderData,
    locale,
    location.pathname,
    location.search,
  )

  const [data, setData] = useState<ProductListingResult | null>(() =>
    loaderFresh ? loaderData.data : null,
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderFresh ? (loaderData.error ?? null) : null,
  )
  const [reloadToken, setReloadToken] = useState(0)

  useEffect(() => {
    if (loaderFresh) {
      setData(loaderData.data)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    let cancelled = false
    setLoading(true)
    setData(null)
    setError(null)

    void getCatalogListingCached(
      {
        pathname: location.pathname,
        searchParams: new URLSearchParams(location.search),
      },
      locale,
    )
      .then((result) => {
        if (cancelled) return
        setData(result)
        setError(result.pathNotFound ? 'not-found' : null)
      })
      .catch(() => {
        if (cancelled) return
        setError('failed')
      })
      .finally(() => {
        if (cancelled) return
        setLoading(false)
      })

    return () => {
      cancelled = true
    }
  }, [requestKey, reloadToken, loaderFresh, loaderData, locale, location.pathname, location.search])

  return {
    data,
    loading,
    error,
    reload: () => setReloadToken((value) => value + 1),
  }
}
