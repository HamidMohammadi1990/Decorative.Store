import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import { useLocation } from 'react-router-dom'
import type { HomePage } from '@/models/home/homePage.model'
import { isBlogRoute } from '@/extensions/blogRoute'
import { isCmsPageRoute } from '@/extensions/cmsPageRoute'
import { isHomeRoute } from '@/extensions/homeRoute'
import { homeService } from '@/services/homeService'
import { useSettingsStore } from '@/stores/settingsStore'

interface HomePageContextValue {
  data: HomePage | null
  loading: boolean
  error: string | null
  reload: () => void
}

const HomePageContext = createContext<HomePageContextValue | null>(null)

export function HomePageProvider({ children }: { children: ReactNode }) {
  const locale = useSettingsStore((s) => s.locale)
  const location = useLocation()
  const skipCatalogNav = isBlogRoute(location.pathname)
  const skipHomeCatalogContent = !isHomeRoute(location.pathname)
  const skipCmsPage = !isCmsPageRoute(location.pathname)
  const [data, setData] = useState<HomePage | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(
    async (force = false) => {
      setLoading(true)
      setError(null)
      try {
        const page = await homeService.getPage(locale, {
          force,
          skipCatalogNav,
          skipHomeCatalogContent,
          skipCmsPage,
        })
        setData(page)
      } catch {
        setError('failed')
      } finally {
        setLoading(false)
      }
    },
    [locale, skipCatalogNav, skipHomeCatalogContent, skipCmsPage],
  )

  useEffect(() => {
    void load()
  }, [load])

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
