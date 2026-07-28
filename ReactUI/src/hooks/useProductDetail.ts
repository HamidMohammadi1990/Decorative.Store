import { useCallback, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import { catalogService } from '@/services/catalogService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductDetailResult {
  product: ProductDetail | null
  related: ProductSummary[]
  loading: boolean
  error: string | null
}

export function useProductDetail(): UseProductDetailResult {
  const { slug } = useParams<{ slug: string }>()
  const locale = useSettingsStore((s) => s.locale)
  const [product, setProduct] = useState<ProductDetail | null>(null)
  const [related, setRelated] = useState<ProductSummary[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!slug) {
      setError('not-found')
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const [detail, relatedProducts] = await Promise.all([
        catalogService.getProduct(slug, locale),
        catalogService.getRelatedProducts(slug, locale),
      ])

      if (!detail) {
        setError('not-found')
        setProduct(null)
        setRelated([])
      } else {
        setProduct(detail)
        setRelated(relatedProducts)
      }
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale, slug])

  useEffect(() => {
    void load()
  }, [load])

  return { product, related, loading, error }
}
