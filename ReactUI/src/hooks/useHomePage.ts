import { useCallback, useEffect, useState } from 'react'
import type { HomePage } from '@/models/home/homePage.model'
import { homeService } from '@/services/homeService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseHomePageResult {
  data: HomePage | null
  loading: boolean
  error: string | null
  reload: () => void
}

export function useHomePage(): UseHomePageResult {
  const locale = useSettingsStore((s) => s.locale)
  const [data, setData] = useState<HomePage | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const page = await homeService.getPage(locale)
      setData(page)
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale])

  useEffect(() => {
    void load()
  }, [load])

  return { data, loading, error, reload: load }
}
