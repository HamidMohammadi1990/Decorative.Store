import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { BlogComment } from '@/models/blog/blog.model'

interface BlogInteractionState {
  likedPostIds: string[]
  likedCommentIds: string[]
  userComments: Record<string, BlogComment[]>
  markPostLiked: (postId: string) => void
  togglePostLike: (postId: string) => void
  isPostLiked: (postId: string) => boolean
  toggleCommentLike: (commentId: string) => void
  isCommentLiked: (commentId: string) => boolean
  addComment: (postId: string, text: string, authorName: string) => void
  getCommentsForPost: (postId: string, baseComments: BlogComment[]) => BlogComment[]
}

function createCommentId() {
  return `user-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`
}

export const useBlogInteractionStore = create<BlogInteractionState>()(
  persist(
    (set, get) => ({
      likedPostIds: [],
      likedCommentIds: [],
      userComments: {},

      markPostLiked: (postId) =>
        set((state) => {
          if (state.likedPostIds.includes(postId)) return state
          return { likedPostIds: [...state.likedPostIds, postId] }
        }),

      togglePostLike: (postId) =>
        set((state) => {
          const liked = state.likedPostIds.includes(postId)
          return {
            likedPostIds: liked
              ? state.likedPostIds.filter((id) => id !== postId)
              : [...state.likedPostIds, postId],
          }
        }),

      isPostLiked: (postId) => get().likedPostIds.includes(postId),

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

      addComment: (postId, text, authorName) =>
        set((state) => {
          const trimmed = text.trim()
          if (!trimmed) return state

          const comment: BlogComment = {
            id: createCommentId(),
            authorName,
            date: new Date().toISOString().slice(0, 10),
            text: trimmed,
            likes: 0,
          }

          return {
            userComments: {
              ...state.userComments,
              [postId]: [...(state.userComments[postId] ?? []), comment],
            },
          }
        }),

      getCommentsForPost: (postId, baseComments) => [
        ...(get().userComments[postId] ?? []),
        ...baseComments,
      ],
    }),
    {
      name: 'westelm-blog-interactions',
      partialize: (state) => ({
        likedPostIds: state.likedPostIds,
        likedCommentIds: state.likedCommentIds,
        userComments: state.userComments,
      }),
    },
  ),
)

export function getDisplayedPostLikes(baseLikes: number, postId: string) {
  const liked = useBlogInteractionStore.getState().isPostLiked(postId)
  return baseLikes + (liked ? 1 : 0)
}

export function getDisplayedCommentLikes(baseLikes: number, commentId: string) {
  const liked = useBlogInteractionStore.getState().isCommentLiked(commentId)
  return baseLikes + (liked ? 1 : 0)
}
