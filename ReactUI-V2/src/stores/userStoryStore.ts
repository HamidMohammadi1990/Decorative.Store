import { create } from 'zustand'
import type { Locale } from '@/models/shared/locale.model'
import type { UserStoryDraft } from '@/models/stories/story.model'
import { userStoryService } from '@/services/userStoryService'

interface UserStoryState {
  stories: UserStoryDraft[]
  isLoading: boolean
  loadStories: (accessToken: string, locale: Locale) => Promise<void>
  clearStories: () => void
  setStories: (stories: UserStoryDraft[]) => void
}

export const useUserStoryStore = create<UserStoryState>()((set) => ({
  stories: [],
  isLoading: false,

  loadStories: async (accessToken, locale) => {
    set({ isLoading: true })
    try {
      const stories = await userStoryService.getMy(accessToken, locale)
      set({ stories, isLoading: false })
    } catch {
      set({ isLoading: false })
    }
  },

  clearStories: () => set({ stories: [], isLoading: false }),

  setStories: (stories) => set({ stories }),
}))
