import { useCallback, useEffect, useState } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { languageService } from '@/services/languageService'

interface UseCurrentLanguageIdOptions {
  /** When false, skips the mount fetch until reload() is called. */
  enabled?: boolean
}

export function useCurrentLanguageId(options: UseCurrentLanguageIdOptions = {}) {
  const { enabled = true } = options
  const { locale } = useLocaleSettings()
  const [languageId, setLanguageId] = useState<number | null>(null)
  const [loading, setLoading] = useState(enabled)
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
    if (!enabled) {
      setLoading(false)
      return
    }
    void reload()
  }, [enabled, reload])

  return { languageId, loading, error, reload, locale }
}
