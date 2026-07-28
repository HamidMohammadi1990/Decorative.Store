import type { StoryGroup, UserStoryDraft } from '@/models/stories/story.model'

export function userStoryToGroup(draft: UserStoryDraft): StoryGroup {
  const slideId = `${draft.id}-slide`

  return {
    id: draft.id,
    slug: draft.id,
    title: draft.title,
    avatar: { src: draft.mediaSrc, alt: draft.title },
    coverImage: { src: draft.posterSrc ?? draft.mediaSrc, alt: draft.title },
    slides: [
      {
        id: slideId,
        caption: draft.caption || undefined,
        media: {
          type: draft.mediaType,
          src: draft.mediaSrc,
          alt: draft.mediaAlt,
          poster: draft.posterSrc,
          durationMs: draft.mediaType === 'video' ? 15000 : 5000,
        },
      },
    ],
    productSlugs: draft.productSlugs,
    likes: 0,
    comments: [],
    publishedAt: draft.createdAt.slice(0, 10),
    ownerId: 'user',
  }
}
