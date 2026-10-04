import { useCallback, useEffect, useState } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { languageService, type StoreLanguage } from '@/services/languageService'

export function useStoreLanguages() {
  const { locale } = useLocaleSettings()
  const [languages, setLanguages] = useState<StoreLanguage[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const reload = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const items = await languageService.search(locale)
      setLanguages(items)
    } catch {
      setLanguages([])
      setError('languages')
    } finally {
      setLoading(false)
    }
  }, [locale])

  useEffect(() => {
    void reload()
  }, [reload])

  return { languages, loading, error, reload }
}
