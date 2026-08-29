import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  mapProductCommentToReview,
  type ProductReviewItem,
} from '@/extensions/productReviews'
import { catalogProductService } from '@/services/catalogProductService'
import { productCommentService } from '@/services/productCommentService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductReviewsResult {
  reviews: ProductReviewItem[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useProductReviews(
  productId: string | undefined,
  productSlug?: string,
): UseProductReviewsResult {
  const locale = useSettingsStore((s) => s.locale)
  const { i18n } = useTranslation()
  const [reviews, setReviews] = useState<ProductReviewItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    let resolvedProductId = productId?.trim() || ''

    if (!resolvedProductId && productSlug) {
      try {
        const catalogProduct = await catalogProductService.getProduct(productSlug, locale)
        resolvedProductId = catalogProduct.id?.trim() || ''
      } catch {
        resolvedProductId = ''
      }
    }

    if (!resolvedProductId) {
      setReviews([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const result = await productCommentService.search(resolvedProductId, locale, {
        pageNumber: 1,
        pageSize: 50,
      })
      setReviews(
        result.items.map((item) => mapProductCommentToReview(item, i18n.language)),
      )
    } catch {
      setError('failed')
      setReviews([])
    } finally {
      setLoading(false)
    }
  }, [i18n.language, locale, productId, productSlug])

  useEffect(() => {
    void load()
  }, [load])

  return { reviews, loading, error, reload: load }
}
