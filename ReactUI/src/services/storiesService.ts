import type { Locale } from '@/models/shared/locale.model'
import type { StoryGroup } from '@/models/stories/story.model'
import { getStoriesMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { userStoryToGroup } from '@/extensions/userStoryToGroup'
import { userStoryService } from '@/services/userStoryService'

export const storiesService = {
  async getStories(locale: Locale): Promise<StoryGroup[]> {
    return mockFetch(async () => {
      const official = getStoriesMock(locale) as StoryGroup[]

      try {
        const userStories = (await userStoryService.searchActive(locale))
          .filter((story) => story.isActive)
          .map((draft) => userStoryToGroup(draft))

        return [...userStories, ...official]
      } catch {
        return official
      }
    })
  },

  async getStoryById(locale: Locale, id: string): Promise<StoryGroup | null> {
    const stories = await this.getStories(locale)
    return stories.find((s) => s.id === id) ?? null
  },
}
