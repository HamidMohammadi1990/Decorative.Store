import { useCallback, useEffect, useState } from 'react'
import type { StoryGroup } from '@/models/stories/story.model'
import { storiesService } from '@/services/storiesService'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'

interface UseStoriesResult {
  stories: StoryGroup[]
  loading: boolean
  error: string | null
  reload: () => void
}

export function useStories(): UseStoriesResult {
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useUserStore((s) => s.accessToken)
  const [stories, setStories] = useState<StoryGroup[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await storiesService.getStories(locale, accessToken)
      setStories(data)
    } catch {
      setError('failed')
      setStories([])
    } finally {
      setLoading(false)
    }
  }, [locale, accessToken])

  useEffect(() => {
    void load()
  }, [load])

  return { stories, loading, error, reload: load }
}
