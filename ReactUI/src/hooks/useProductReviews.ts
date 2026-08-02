import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  mapProductCommentToReview,
  type ProductReviewItem,
} from '@/extensions/productReviews'
import { productCommentService } from '@/services/productCommentService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductReviewsResult {
  reviews: ProductReviewItem[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useProductReviews(productId: string | undefined): UseProductReviewsResult {
  const locale = useSettingsStore((s) => s.locale)
  const { i18n } = useTranslation()
  const [reviews, setReviews] = useState<ProductReviewItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!productId) {
      setReviews([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const result = await productCommentService.search(productId, locale, {
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
  }, [i18n.language, locale, productId])

  useEffect(() => {
    void load()
  }, [load])

  return { reviews, loading, error, reload: load }
}
