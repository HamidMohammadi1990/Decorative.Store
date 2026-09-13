import { useEffect, useState } from 'react'
import { useLocation, useNavigation, useRouteLoaderData } from 'react-router-dom'
import type { ProductListingResult } from '@/models/catalog/listing.model'
import { getCatalogListingCached } from '@/services/catalogListingCache'
import type { ProductListingLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

interface UseProductListingResult {
  data: ProductListingResult | null
  loading: boolean
  error: string | null
  reload: () => void
}

type ClientListingState = {
  data: ProductListingResult | null
  error: string | null
  loading: boolean
  locale: string
  pathname: string
  search: string
}

function matchesLoaderRoute(
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

function sameRoute(
  loaderData: ProductListingLoaderData | undefined,
  pathname: string,
  search: string,
) {
  return Boolean(loaderData && loaderData.pathname === pathname && loaderData.search === search)
}

function resolveClientState(
  clientState: ClientListingState | null,
  locale: string,
  pathname: string,
  search: string,
): ClientListingState | null {
  if (!clientState) return null
  if (
    clientState.locale !== locale ||
    clientState.pathname !== pathname ||
    clientState.search !== search
  ) {
    return null
  }
  return clientState
}

export function useProductListing(): UseProductListingResult {
  const location = useLocation()
  const navigation = useNavigation()
  const loaderData = useRouteLoaderData('product-listing') as ProductListingLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderMatches = matchesLoaderRoute(
    loaderData,
    locale,
    location.pathname,
    location.search,
  )
  const loaderStaleForLocale =
    sameRoute(loaderData, location.pathname, location.search) &&
    loaderData!.locale !== locale

  const [reloadToken, setReloadToken] = useState(0)
  const [clientState, setClientState] = useState<ClientListingState | null>(null)

  const reload = () => setReloadToken((value) => value + 1)

  useEffect(() => {
    const shouldRefetch = reloadToken > 0 || loaderStaleForLocale
    if (!shouldRefetch) return

    let cancelled = false
    setClientState({
      data: null,
      error: null,
      loading: true,
      locale,
      pathname: location.pathname,
      search: location.search,
    })

    void getCatalogListingCached(
      {
        pathname: location.pathname,
        searchParams: new URLSearchParams(location.search),
      },
      locale,
    )
      .then((result) => {
        if (cancelled) return
        setClientState({
          data: result,
          error: result.pathNotFound ? 'not-found' : null,
          loading: false,
          locale,
          pathname: location.pathname,
          search: location.search,
        })
      })
      .catch(() => {
        if (cancelled) return
        setClientState({
          data: null,
          error: 'failed',
          loading: false,
          locale,
          pathname: location.pathname,
          search: location.search,
        })
      })

    return () => {
      cancelled = true
    }
  }, [loaderStaleForLocale, reloadToken, locale, location.pathname, location.search])

  const clientFresh = resolveClientState(
    clientState,
    locale,
    location.pathname,
    location.search,
  )

  if (clientFresh?.loading) {
    return { data: null, loading: true, error: null, reload }
  }

  if (clientFresh && (clientFresh.data || clientFresh.error)) {
    return {
      data: clientFresh.data,
      loading: false,
      error: clientFresh.error,
      reload,
    }
  }

  if (loaderStaleForLocale) {
    return { data: null, loading: true, error: null, reload }
  }

  const loaderPending =
    navigation.state === 'loading' &&
    navigation.location?.pathname === location.pathname

  if (loaderMatches) {
    return {
      data: loaderData.data,
      loading: false,
      error: loaderData.error ?? null,
      reload,
    }
  }

  if (sameRoute(loaderData, location.pathname, location.search)) {
    return {
      data: loaderData!.data ?? null,
      loading: loaderPending,
      error: loaderData!.error ?? null,
      reload,
    }
  }

  return {
    data: null,
    loading: loaderPending,
    error: null,
    reload,
  }
}
