import { useEffect, useState } from 'react'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { useStoreLanguages } from '@/hooks/useStoreLanguages'

export function useAdminContentLanguage() {
  const { languageId, loading: languageLoading } = useCurrentLanguageId()
  const { languages, loading: languagesLoading } = useStoreLanguages()
  const [contentLanguageId, setContentLanguageId] = useState<number | null>(null)

  useEffect(() => {
    if (languageId != null) {
      setContentLanguageId(languageId)
    }
  }, [languageId])

  return {
    contentLanguageId,
    setContentLanguageId,
    languages,
    loading: languageLoading || languagesLoading,
  }
}
