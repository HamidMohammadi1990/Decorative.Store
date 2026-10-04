import { useCallback, useEffect, useState } from 'react'
import type { DashboardCoupon } from '@/models/dashboard/dashboard.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { discountService } from '@/services/discountService'
import { useUserStore } from '@/stores/userStore'

interface UseMyDiscountsResult {
  coupons: DashboardCoupon[]
  loading: boolean
  error: string | null
  reload: () => void
}

export function useMyDiscounts(): UseMyDiscountsResult {
  const { locale } = useLocaleSettings()
  const accessToken = useUserStore((state) => state.accessToken)
  const [coupons, setCoupons] = useState<DashboardCoupon[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setCoupons([])
      setLoading(false)
      setError(null)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const items = await discountService.getMyAvailable(accessToken, locale)
      setCoupons(items)
    } catch {
      setCoupons([])
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale])

  useEffect(() => {
    void load()
  }, [load])

  return { coupons, loading, error, reload: load }
}
