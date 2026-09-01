import { useCallback, useRef, useState } from 'react'
import type { AssistantFaqItem } from '@/models/admin/assistantFaq.model'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { assistantFaqService } from '@/services/assistantFaqService'

export function useAssistantFaqs() {
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [faqs, setFaqs] = useState<AssistantFaqItem[]>([])
  const [loading, setLoading] = useState(false)
  const requestIdRef = useRef(0)

  const loadFaqs = useCallback(async () => {
    if (languageLoading || languageId == null) return

    const requestId = ++requestIdRef.current
    setLoading(true)

    try {
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
  }, [languageId, languageLoading, locale])

  return { faqs, loading, loadFaqs }
}
