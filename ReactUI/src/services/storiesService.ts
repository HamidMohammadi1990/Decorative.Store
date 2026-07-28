import type { Locale } from '@/models/shared/locale.model'
import type { StoryGroup, UserStoryDraft } from '@/models/stories/story.model'
import { getStoriesMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { userStoryToGroup } from '@/extensions/userStoryToGroup'
import { useUserStoryStore } from '@/stores/userStoryStore'

export const storiesService = {
  async getStories(locale: Locale): Promise<StoryGroup[]> {
    return mockFetch(() => {
      const official = getStoriesMock(locale) as StoryGroup[]
      const userStories = useUserStoryStore
        .getState()
        .stories.filter((s) => s.isActive)
        .map((draft) => userStoryToGroup(draft))

      return [...userStories, ...official]
    })
  },

  async getStoryById(locale: Locale, id: string): Promise<StoryGroup | null> {
    const stories = await this.getStories(locale)
    return stories.find((s) => s.id === id) ?? null
  },

  getUserStories(): UserStoryDraft[] {
    return useUserStoryStore.getState().stories
  },
}
