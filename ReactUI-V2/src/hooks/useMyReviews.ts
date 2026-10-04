import { useCallback, useEffect, useState } from 'react'
import type { DashboardReview } from '@/models/dashboard/dashboard.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { productCommentService } from '@/services/productCommentService'
import { useUserStore } from '@/stores/userStore'

interface UseMyReviewsResult {
  reviews: DashboardReview[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useMyReviews(): UseMyReviewsResult {
  const { locale } = useLocaleSettings()
  const accessToken = useUserStore((state) => state.accessToken)
  const [reviews, setReviews] = useState<DashboardReview[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const reload = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setReviews([])
      setLoading(false)
      setError(null)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const data = await productCommentService.getMy(accessToken, locale)
      setReviews(data)
    } catch {
      setError('failed')
      setReviews([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale])

  useEffect(() => {
    void reload()
  }, [reload])

  return {
    reviews,
    loading,
    error,
    reload,
  }
}
