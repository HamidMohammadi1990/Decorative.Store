import type { StoryGroup, UserStoryDraft } from '@/models/stories/story.model'

export function userStoryToGroup(draft: UserStoryDraft): StoryGroup {
  const slideId = `${draft.id}-slide`
  const coverSrc = draft.posterSrc ?? draft.mediaSrc

  return {
    id: draft.id,
    slug: draft.id,
    title: draft.ownerName || draft.title,
    avatar: { src: coverSrc, alt: draft.title },
    coverImage: { src: coverSrc, alt: draft.title },
    slides: [
      {
        id: slideId,
        caption: draft.caption || undefined,
        media: {
          type: draft.mediaType,
          src: draft.mediaSrc,
          alt: draft.mediaAlt || draft.title,
          poster: draft.posterSrc,
          durationMs: draft.mediaType === 'video' ? 15000 : 5000,
        },
      },
    ],
    productSlugs: draft.productSlugs,
    likes: draft.likeCount ?? 0,
    commentCount: draft.commentCount ?? 0,
    isLikedByCurrentUser: draft.isLikedByCurrentUser ?? false,
    comments: [],
    publishedAt: draft.createdAt.slice(0, 10),
    ownerId: draft.ownerName || 'user',
  }
}
