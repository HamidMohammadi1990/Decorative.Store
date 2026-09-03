import { useCallback, useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { catalogService } from '@/services/catalogService'
import type { ComparePageLoaderData } from '@/routes/loaders/types'
import { useCompareStore } from '@/stores/compareStore'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseCompareProductsResult {
  products: ProductDetail[]
  loading: boolean
  error: string | null
  reload: () => void
}

function slugsKey(slugs: string[]) {
  return slugs.join('\0')
}

function isLoaderFresh(
  loaderData: ComparePageLoaderData | undefined,
  locale: string,
  slugs: string[],
): loaderData is ComparePageLoaderData {
  return Boolean(
    loaderData &&
      loaderData.locale === locale &&
      slugsKey(loaderData.slugs) === slugsKey(slugs),
  )
}

export function useCompareProducts(): UseCompareProductsResult {
  const locale = useSettingsStore((s) => s.locale)
  const slugs = useCompareStore((s) => s.slugs)
  const loaderData = useRouteLoaderData('compare') as ComparePageLoaderData | undefined
  const loaderFresh = isLoaderFresh(loaderData, locale, slugs)

  const [products, setProducts] = useState<ProductDetail[]>(() =>
    loaderFresh ? loaderData.products : [],
  )
  const [loading, setLoading] = useState(() => slugs.length > 0 && !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderFresh ? (loaderData.error ?? null) : null,
  )

  const load = useCallback(async () => {
    if (slugs.length === 0) {
      setProducts([])
      setLoading(false)
      setError(null)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const details = await catalogService.getProductsBySlugs(slugs, locale)
      setProducts(details)

      const missing = slugs.filter((slug) => !details.some((p) => p.slug === slug))
      if (missing.length > 0) {
        useCompareStore.setState({
          slugs: slugs.filter((slug) => !missing.includes(slug)),
        })
      }
    } catch {
      setError('failed')
      setProducts([])
    } finally {
      setLoading(false)
    }
  }, [locale, slugs])

  useEffect(() => {
    if (slugs.length === 0) {
      setProducts([])
      setLoading(false)
      setError(null)
      return
    }

    if (loaderFresh) {
      setProducts(loaderData.products)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData, slugs.length])

  return { products, loading, error, reload: load }
}
