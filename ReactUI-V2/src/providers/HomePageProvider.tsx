import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
  type ReactNode,
} from 'react'
import { useLocation, useRouteLoaderData } from 'react-router-dom'
import type { HomePage } from '@/models/home/homePage.model'
import {
  buildHomePageFetchKey,
  resolveHomePageFetchOptions,
} from '@/ssr/resolveHomePageFetchOptions'
import { homeService } from '@/services/homeService'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'

interface HomePageContextValue {
  data: HomePage | null
  loading: boolean
  error: string | null
  reload: () => void
}

const HomePageContext = createContext<HomePageContextValue | null>(null)

function getLoaderHomeSnapshot(
  loaderData: ShopLayoutLoaderData | undefined,
  fetchKey: string,
): HomePage | null {
  if (!loaderData?.homePage || loaderData.fetchKey !== fetchKey) {
    return null
  }
  return loaderData.homePage
}

function isLoaderLocaleFresh(
  loaderData: ShopLayoutLoaderData | undefined,
  locale: string,
  fetchKey: string,
): loaderData is ShopLayoutLoaderData & { homePage: HomePage } {
  return Boolean(
    loaderData &&
      loaderData.locale === locale &&
      loaderData.fetchKey === fetchKey &&
      loaderData.homePage,
  )
}

function hasShopNavChrome(page: HomePage | null | undefined) {
  return Boolean(page?.header?.primaryNav?.length && page?.categoryNav?.items?.length)
}

export function HomePageProvider({ children }: { children: ReactNode }) {
  const location = useLocation()
  const loaderData = useRouteLoaderData('shop') as ShopLayoutLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const fetchOptions = useMemo(
    () => resolveHomePageFetchOptions(location.pathname),
    [location.pathname],
  )
  const fetchKey = buildHomePageFetchKey(fetchOptions)
  const loaderSnapshot = getLoaderHomeSnapshot(loaderData, fetchKey)
  const loaderLocaleFresh = isLoaderLocaleFresh(loaderData, locale, fetchKey)

  const [data, setData] = useState<HomePage | null>(() => loaderSnapshot)
  const [loading, setLoading] = useState(() => !loaderSnapshot)
  const [error, setError] = useState<string | null>(() =>
    loaderData?.error && !loaderSnapshot ? 'failed' : null,
  )
  const dataRef = useRef(data)
  dataRef.current = data

  const load = useCallback(
    async (force = false) => {
      const needsFreshLocale =
        force ||
        !dataRef.current ||
        loaderData?.locale !== locale ||
        loaderData?.fetchKey !== fetchKey

      if (needsFreshLocale) {
        setLoading(true)
      }
      setError(null)

      try {
        const page = await homeService.getPage(locale, {
          force,
          ...fetchOptions,
        })
        setData(page)
      } catch {
        setError('failed')
      } finally {
        setLoading(false)
      }
    },
    [fetchKey, fetchOptions, loaderData?.fetchKey, loaderData?.locale, locale],
  )

  useEffect(() => {
    if (loaderLocaleFresh) {
      setData(loaderData.homePage)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    // Listing/CMS-lite routes reuse nav chrome only when it already matches the active locale.
    if (
      loaderData?.locale === locale &&
      fetchOptions.skipHomeCatalogContent &&
      fetchOptions.skipCmsPage &&
      hasShopNavChrome(dataRef.current)
    ) {
      setLoading(false)
      return
    }

    void load(true)
  }, [
    fetchKey,
    fetchOptions.skipCmsPage,
    fetchOptions.skipHomeCatalogContent,
    load,
    loaderData,
    loaderLocaleFresh,
    locale,
  ])

  const reload = useCallback(() => {
    void load(true)
  }, [load])

  const value = useMemo(
    () => ({ data, loading, error, reload }),
    [data, loading, error, reload],
  )

  return <HomePageContext.Provider value={value}>{children}</HomePageContext.Provider>
}

export function useHomePage(): HomePageContextValue {
  const context = useContext(HomePageContext)
  if (!context) {
    throw new Error('useHomePage must be used within HomePageProvider')
  }
  return context
}
