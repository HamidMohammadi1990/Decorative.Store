import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface StoryInteractionState {
  viewedStoryIds: string[]
  followedAccountIds: string[]
  markViewed: (storyId: string) => void
  isViewed: (storyId: string) => boolean
  toggleFollow: (accountId: string) => void
  isFollowing: (accountId: string) => boolean
}

export const useStoryInteractionStore = create<StoryInteractionState>()(
  persist(
    (set, get) => ({
      viewedStoryIds: [],
      followedAccountIds: [],

      markViewed: (storyId) => {
        if (get().viewedStoryIds.includes(storyId)) return
        set({ viewedStoryIds: [...get().viewedStoryIds, storyId] })
      },

      isViewed: (storyId) => get().viewedStoryIds.includes(storyId),

      toggleFollow: (accountId) =>
        set((state) => {
          const following = state.followedAccountIds.includes(accountId)
          return {
            followedAccountIds: following
              ? state.followedAccountIds.filter((id) => id !== accountId)
              : [...state.followedAccountIds, accountId],
          }
        }),

      isFollowing: (accountId) => get().followedAccountIds.includes(accountId),
    }),
    {
      name: 'diba-story-interactions',
      partialize: (state) => ({
        viewedStoryIds: state.viewedStoryIds,
        followedAccountIds: state.followedAccountIds,
      }),
    },
  ),
)
