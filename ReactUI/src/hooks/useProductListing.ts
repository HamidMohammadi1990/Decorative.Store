import { useCallback, useEffect, useState } from 'react'
import { useLocation, useRouteLoaderData } from 'react-router-dom'
import type { ProductListingResult } from '@/models/catalog/listing.model'
import { catalogService } from '@/services/catalogService'
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

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const result = await catalogService.getListing(
        {
          pathname: location.pathname,
          searchParams: new URLSearchParams(location.search),
        },
        locale,
      )
      setData(result)
      if (result.pathNotFound) {
        setError('not-found')
      }
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale, location.pathname, location.search])

  useEffect(() => {
    if (loaderFresh) {
      setData(loaderData.data)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData])

  return { data, loading, error, reload: load }
}
