import type { StoryGroup, UserStoryDraft } from '@/models/stories/story.model'

function draftToSlide(draft: UserStoryDraft) {
  return {
    id: `${draft.id}-slide`,
    caption: draft.caption || undefined,
    media: {
      type: draft.mediaType,
      src: draft.mediaSrc,
      alt: draft.mediaAlt || draft.title,
      poster: draft.posterSrc,
      durationMs: draft.mediaType === 'video' ? 15000 : 5000,
    },
  }
}

/** Groups active user stories by owner into one ring per user (multi-slide story). */
export function groupUserStoriesToGroups(drafts: UserStoryDraft[]): StoryGroup[] {
  const byOwner = new Map<string, UserStoryDraft[]>()

  for (const draft of drafts) {
    const key = draft.userId ?? draft.ownerName ?? draft.id
    const list = byOwner.get(key) ?? []
    list.push(draft)
    byOwner.set(key, list)
  }

  return Array.from(byOwner.values()).map((ownerStories) => {
    const sorted = [...ownerStories].sort((a, b) => a.createdAt.localeCompare(b.createdAt))
    const primary = sorted[0]
    const coverSrc = primary.posterSrc ?? primary.mediaSrc
    const productSlugs = [
      ...new Set(sorted.flatMap((story) => story.productSlugs).filter(Boolean)),
    ]

    return {
      id: primary.id,
      slug: primary.id,
      title: primary.ownerName || primary.title,
      avatar: { src: coverSrc, alt: primary.title },
      coverImage: { src: coverSrc, alt: primary.title },
      slides: sorted.map(draftToSlide),
      productSlugs,
      likes: sorted.reduce((sum, story) => sum + (story.likeCount ?? 0), 0),
      commentCount: sorted.reduce((sum, story) => sum + (story.commentCount ?? 0), 0),
      isLikedByCurrentUser: sorted.some((story) => story.isLikedByCurrentUser),
      comments: [],
      publishedAt: primary.createdAt.slice(0, 10),
      ownerId: primary.userId ?? primary.ownerName ?? 'user',
    }
  })
}
