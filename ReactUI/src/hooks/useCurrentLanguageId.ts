import { useCallback, useEffect, useState } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { languageService } from '@/services/languageService'

export function useCurrentLanguageId() {
  const { locale } = useLocaleSettings()
  const [languageId, setLanguageId] = useState<number | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const reload = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const id = await languageService.resolveLanguageId(locale)
      setLanguageId(id)
    } catch {
      setLanguageId(null)
      setError('language')
    } finally {
      setLoading(false)
    }
  }, [locale])

  useEffect(() => {
    void reload()
  }, [reload])

  return { languageId, loading, error, reload, locale }
}
