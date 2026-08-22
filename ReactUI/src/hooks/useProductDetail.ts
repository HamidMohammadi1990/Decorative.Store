import { useEffect, useState } from 'react'
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

  useEffect(() => {
    if (!slug) {
      setError('not-found')
      setLoading(false)
      return
    }

    let cancelled = false

    setLoading(true)
    setError(null)

    void Promise.all([
      catalogService.getProduct(slug, locale),
      catalogService.getRelatedProducts(slug, locale),
    ])
      .then(([detail, relatedProducts]) => {
        if (cancelled) return

        if (!detail) {
          setError('not-found')
          setProduct(null)
          setRelated([])
        } else {
          setProduct(detail)
          setRelated(relatedProducts)
        }
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
  }, [locale, slug])

  return { product, related, loading, error }
}
