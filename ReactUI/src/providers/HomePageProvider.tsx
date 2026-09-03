import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
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

function isLoaderFresh(
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

export function HomePageProvider({ children }: { children: ReactNode }) {
  const location = useLocation()
  const loaderData = useRouteLoaderData('shop') as ShopLayoutLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const fetchOptions = useMemo(
    () => resolveHomePageFetchOptions(location.pathname),
    [location.pathname],
  )
  const fetchKey = buildHomePageFetchKey(fetchOptions)
  const loaderFresh = isLoaderFresh(loaderData, locale, fetchKey)

  const [data, setData] = useState<HomePage | null>(() =>
    loaderFresh ? loaderData.homePage : null,
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderData?.error && !loaderFresh ? 'failed' : null,
  )

  const load = useCallback(
    async (force = false) => {
      setLoading(true)
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
    [fetchOptions, locale],
  )

  useEffect(() => {
    if (loaderFresh) {
      setData(loaderData.homePage)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData])

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
