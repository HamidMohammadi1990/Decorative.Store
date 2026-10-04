import { useCallback, useRef, useState } from 'react'
import type { AssistantFaqItem } from '@/models/admin/assistantFaq.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { assistantFaqService } from '@/services/assistantFaqService'
import { languageService } from '@/services/languageService'

export function useAssistantFaqs() {
  const { locale } = useLocaleSettings()
  const [faqs, setFaqs] = useState<AssistantFaqItem[]>([])
  const [loading, setLoading] = useState(false)
  const requestIdRef = useRef(0)

  const loadFaqs = useCallback(async () => {
    const requestId = ++requestIdRef.current
    setLoading(true)

    try {
      const languageId = await languageService.resolveLanguageId(locale)
      const items = await assistantFaqService.search(locale, languageId)
      if (requestIdRef.current === requestId) {
        setFaqs(items)
      }
    } catch {
      if (requestIdRef.current === requestId) {
        setFaqs([])
      }
    } finally {
      if (requestIdRef.current === requestId) {
        setLoading(false)
      }
    }
  }, [locale])

  return { faqs, loading, loadFaqs }
}
