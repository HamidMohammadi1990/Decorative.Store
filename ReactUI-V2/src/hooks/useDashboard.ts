import { useCallback, useEffect, useState } from 'react'
import type { DashboardData } from '@/models/dashboard/dashboard.model'
import { dashboardService } from '@/services/dashboardService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseDashboardResult {
  data: DashboardData | null
  loading: boolean
  error: string | null
  reload: () => void
}

export function useDashboard(): UseDashboardResult {
  const locale = useSettingsStore((s) => s.locale)
  const [data, setData] = useState<DashboardData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const result = await dashboardService.getData(locale)
      setData(result)
    } catch {
      setError('error')
    } finally {
      setLoading(false)
    }
  }, [locale])

  useEffect(() => {
    void load()
  }, [load])

  return { data, loading, error, reload: load }
}
