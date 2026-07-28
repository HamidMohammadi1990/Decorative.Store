import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { StoryComment } from '@/models/stories/story.model'

interface StoryInteractionState {
  likedStoryIds: string[]
  likedCommentIds: string[]
  viewedStoryIds: string[]
  followedAccountIds: string[]
  userComments: Record<string, StoryComment[]>
  toggleStoryLike: (storyId: string) => void
  isStoryLiked: (storyId: string) => boolean
  toggleCommentLike: (commentId: string) => void
  isCommentLiked: (commentId: string) => boolean
  markViewed: (storyId: string) => void
  isViewed: (storyId: string) => boolean
  toggleFollow: (accountId: string) => void
  isFollowing: (accountId: string) => boolean
  addComment: (storyId: string, text: string, authorName: string) => void
  getCommentsForStory: (storyId: string, baseComments: StoryComment[]) => StoryComment[]
}

function createCommentId() {
  return `story-c-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`
}

export const useStoryInteractionStore = create<StoryInteractionState>()(
  persist(
    (set, get) => ({
      likedStoryIds: [],
      likedCommentIds: [],
      viewedStoryIds: [],
      followedAccountIds: [],
      userComments: {},

      toggleStoryLike: (storyId) =>
        set((state) => {
          const liked = state.likedStoryIds.includes(storyId)
          return {
            likedStoryIds: liked
              ? state.likedStoryIds.filter((id) => id !== storyId)
              : [...state.likedStoryIds, storyId],
          }
        }),

      isStoryLiked: (storyId) => get().likedStoryIds.includes(storyId),

      toggleCommentLike: (commentId) =>
        set((state) => {
          const liked = state.likedCommentIds.includes(commentId)
          return {
            likedCommentIds: liked
              ? state.likedCommentIds.filter((id) => id !== commentId)
              : [...state.likedCommentIds, commentId],
          }
        }),

      isCommentLiked: (commentId) => get().likedCommentIds.includes(commentId),

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

      addComment: (storyId, text, authorName) =>
        set((state) => {
          const trimmed = text.trim()
          if (!trimmed) return state

          const comment: StoryComment = {
            id: createCommentId(),
            authorName,
            date: new Date().toISOString().slice(0, 10),
            text: trimmed,
            likes: 0,
          }

          return {
            userComments: {
              ...state.userComments,
              [storyId]: [...(state.userComments[storyId] ?? []), comment],
            },
          }
        }),

      getCommentsForStory: (storyId, baseComments) => [
        ...(get().userComments[storyId] ?? []),
        ...baseComments,
      ],
    }),
    {
      name: 'diba-story-interactions',
      partialize: (state) => ({
        likedStoryIds: state.likedStoryIds,
        likedCommentIds: state.likedCommentIds,
        viewedStoryIds: state.viewedStoryIds,
        followedAccountIds: state.followedAccountIds,
        userComments: state.userComments,
      }),
    },
  ),
)

export function getDisplayedStoryLikes(baseLikes: number, storyId: string) {
  const liked = useStoryInteractionStore.getState().isStoryLiked(storyId)
  return baseLikes + (liked ? 1 : 0)
}

export function getDisplayedStoryCommentCount(storyId: string, baseCommentsLength: number) {
  const userCount = useStoryInteractionStore.getState().userComments[storyId]?.length ?? 0
  return baseCommentsLength + userCount
}

export function getDisplayedCommentLikes(baseLikes: number, commentId: string) {
  const liked = useStoryInteractionStore.getState().isCommentLiked(commentId)
  return baseLikes + (liked ? 1 : 0)
}
