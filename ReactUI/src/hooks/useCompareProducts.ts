import { useCallback, useEffect, useState } from 'react'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { catalogService } from '@/services/catalogService'
import { useCompareStore } from '@/stores/compareStore'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseCompareProductsResult {
  products: ProductDetail[]
  loading: boolean
  error: string | null
  reload: () => void
}

export function useCompareProducts(): UseCompareProductsResult {
  const locale = useSettingsStore((s) => s.locale)
  const slugs = useCompareStore((s) => s.slugs)
  const [products, setProducts] = useState<ProductDetail[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

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
    void load()
  }, [load])

  return { products, loading, error, reload: load }
}
