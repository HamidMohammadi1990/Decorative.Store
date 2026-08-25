import type { Locale } from '@/models/shared/locale.model'
import type { StoryGroup } from '@/models/stories/story.model'
import { userStoryToGroup } from '@/extensions/userStoryToGroup'
import { userStoryService } from '@/services/userStoryService'

export const storiesService = {
  /** Homepage / shop strip: active stories from backend only. */
  async getStories(locale: Locale): Promise<StoryGroup[]> {
    const items = await userStoryService.searchActive(locale, 40)
    return items
      .filter((story) => story.isActive && story.id && story.mediaSrc)
      .map((draft) => userStoryToGroup(draft))
  },

  async getStoryById(locale: Locale, id: string): Promise<StoryGroup | null> {
    const stories = await this.getStories(locale)
    return stories.find((s) => s.id === id) ?? null
  },
}
