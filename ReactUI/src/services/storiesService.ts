import type { Locale } from '@/models/shared/locale.model'
import type { StoryGroup } from '@/models/stories/story.model'
import { groupUserStoriesToGroups } from '@/extensions/groupUserStoriesToGroups'
import { userStoryService } from '@/services/userStoryService'

export const storiesService = {
  /** Homepage / shop strip: active stories from backend only. */
  async getStories(locale: Locale, accessToken?: string | null): Promise<StoryGroup[]> {
    const items = await userStoryService.searchActive(locale, 40, accessToken)
    const drafts = items.filter((story) => story.isActive && story.id && story.mediaSrc)
    return groupUserStoriesToGroups(drafts)
  },

  async getStoryById(locale: Locale, id: string, accessToken?: string | null): Promise<StoryGroup | null> {
    const stories = await this.getStories(locale, accessToken)
    return stories.find((s) => s.id === id || s.slides.some((slide) => slide.id.startsWith(id))) ?? null
  },
}
