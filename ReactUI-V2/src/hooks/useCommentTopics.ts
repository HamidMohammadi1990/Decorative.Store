import { useCallback, useEffect, useState } from 'react'
import type { CommentTopic } from '@/models/catalog/commentTopic.model'
import { commentTopicService } from '@/services/commentTopicService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseCommentTopicsResult {
  topics: CommentTopic[]
  loading: boolean
  error: string | null
  reload: () => Promise<void>
}

export function useCommentTopics(enabled = true): UseCommentTopicsResult {
  const locale = useSettingsStore((s) => s.locale)
  const [topics, setTopics] = useState<CommentTopic[]>([])
  const [loading, setLoading] = useState(enabled)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!enabled) {
      setTopics([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const result = await commentTopicService.search(locale, { pageNumber: 1, pageSize: 50 })
      setTopics(result.items)
    } catch {
      setTopics([])
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [enabled, locale])

  useEffect(() => {
    void load()
  }, [load])

  return { topics, loading, error, reload: load }
}
