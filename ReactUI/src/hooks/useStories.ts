import { useCallback, useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { StoryGroup } from '@/models/stories/story.model'
import { storiesService } from '@/services/storiesService'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'
import { useUserStore } from '@/stores/userStore'

interface UseStoriesResult {
  stories: StoryGroup[]
  loading: boolean
  error: string | null
  reload: () => void
}

export function useStories(): UseStoriesResult {
  const accessToken = useUserStore((s) => s.accessToken)
  const loaderData = useRouteLoaderData('shop') as ShopLayoutLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderFresh = Boolean(loaderData && loaderData.locale === locale)

  const [stories, setStories] = useState<StoryGroup[]>(() =>
    loaderFresh ? loaderData!.stories : [],
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
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
    if (loaderFresh && !accessToken) {
      setStories(loaderData!.stories)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData, accessToken])

  return { stories, loading, error, reload: load }
}
