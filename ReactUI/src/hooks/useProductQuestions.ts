import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  mapProductQuestionToItem,
  type ProductQuestionItem,
} from '@/extensions/productQuestions'
import { productQuestionService } from '@/services/productQuestionService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductQuestionsResult {
  questions: ProductQuestionItem[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useProductQuestions(productId: string | undefined): UseProductQuestionsResult {
  const locale = useSettingsStore((s) => s.locale)
  const { i18n } = useTranslation()
  const [questions, setQuestions] = useState<ProductQuestionItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!productId) {
      setQuestions([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const result = await productQuestionService.search(productId, locale, {
        pageNumber: 1,
        pageSize: 50,
      })
      setQuestions(
        result.items.map((item) => mapProductQuestionToItem(item, i18n.language)),
      )
    } catch {
      setError('failed')
      setQuestions([])
    } finally {
      setLoading(false)
    }
  }, [i18n.language, locale, productId])

  useEffect(() => {
    void load()
  }, [load])

  return { questions, loading, error, reload: load }
}
