import { useEffect, useRef } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useUserStoryStore } from '@/stores/userStoryStore'
import { useIsAuthenticated, useUserStore } from '@/stores/userStore'

/** Load the user's stories from the backend when authenticated. */
export function useUserStorySync() {
  const isAuthenticated = useIsAuthenticated()
  const { locale } = useLocaleSettings()
  const loadStories = useUserStoryStore((state) => state.loadStories)
  const clearStories = useUserStoryStore((state) => state.clearStories)
  const syncedTokenRef = useRef<string | null>(null)

  useEffect(() => {
    if (!isAuthenticated) {
      syncedTokenRef.current = null
      clearStories()
      return
    }

    void (async () => {
      const accessToken = useUserStore.getState().accessToken
      if (!accessToken || accessToken === 'mock-access-token') return
      if (syncedTokenRef.current === accessToken) return

      syncedTokenRef.current = accessToken
      await loadStories(accessToken, locale)
    })()
  }, [clearStories, isAuthenticated, loadStories, locale])
}
