import { useCallback, useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useRouteLoaderData } from 'react-router-dom'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'
import {
  mapProductQuestionToItem,
  type ProductQuestionItem,
} from '@/extensions/productQuestions'
import { productQuestionService } from '@/services/productQuestionService'
import type { ProductDetailLoaderData } from '@/routes/loaders/types'

interface UseProductQuestionsResult {
  questions: ProductQuestionItem[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useProductQuestions(productId: string | undefined): UseProductQuestionsResult {
  const loaderData = useRouteLoaderData('product-detail') as ProductDetailLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const { i18n } = useTranslation()
  const loaderFresh = Boolean(
    loaderData &&
      loaderData.locale === locale &&
      loaderData.product?.id === productId,
  )

  const [questions, setQuestions] = useState<ProductQuestionItem[]>(() =>
    loaderFresh ? loaderData!.questions : [],
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(null)
  const questionsRef = useRef(questions)
  questionsRef.current = questions

  const load = useCallback(async () => {
    if (!productId) {
      setQuestions([])
      setLoading(false)
      return
    }

    if (!questionsRef.current.length) {
      setLoading(true)
    }
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
    if (loaderFresh) {
      setQuestions(loaderData!.questions)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData])

  return { questions, loading, error, reload: load }
}
