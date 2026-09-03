import { useEffect, useState } from 'react'
import { useParams, useRouteLoaderData } from 'react-router-dom'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import { catalogService } from '@/services/catalogService'
import type { ProductDetailLoaderData } from '@/routes/loaders/types'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductDetailResult {
  product: ProductDetail | null
  related: ProductSummary[]
  loading: boolean
  error: string | null
}

function isLoaderFresh(
  loaderData: ProductDetailLoaderData | undefined,
  slug: string | undefined,
  locale: string,
): loaderData is ProductDetailLoaderData {
  return Boolean(loaderData && loaderData.slug === slug && loaderData.locale === locale)
}

export function useProductDetail(): UseProductDetailResult {
  const { slug } = useParams<{ slug: string }>()
  const locale = useSettingsStore((s) => s.locale)
  const loaderData = useRouteLoaderData('product-detail') as ProductDetailLoaderData | undefined
  const loaderFresh = isLoaderFresh(loaderData, slug, locale)

  const [product, setProduct] = useState<ProductDetail | null>(() =>
    loaderFresh ? (loaderData.product ?? null) : null,
  )
  const [related, setRelated] = useState<ProductSummary[]>(() =>
    loaderFresh ? loaderData.related : [],
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderFresh ? (loaderData.error ?? null) : null,
  )

  useEffect(() => {
    if (!slug) {
      setError('not-found')
      setProduct(null)
      setRelated([])
      setLoading(false)
      return
    }

    if (loaderFresh) {
      setProduct(loaderData.product)
      setRelated(loaderData.related)
      setError(loaderData.error ?? null)
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
  }, [locale, slug, loaderFresh, loaderData])

  return { product, related, loading, error }
}
