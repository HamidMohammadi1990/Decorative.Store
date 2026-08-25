import type { ImageAsset } from '@/models/shared/image.model'

export type StoryMediaType = 'image' | 'video'

export interface StoryMedia {
  type: StoryMediaType
  src: string
  alt: string
  poster?: string
  durationMs?: number
}

export interface StorySlide {
  id: string
  media: StoryMedia
  caption?: string
}

export interface StoryComment {
  id: string
  authorName: string
  authorAvatar?: string
  date: string
  text: string
  likes: number
}

export interface StoryGroup {
  id: string
  slug: string
  title: string
  avatar: ImageAsset
  coverImage: ImageAsset
  slides: StorySlide[]
  productSlugs: string[]
  likes: number
  comments: StoryComment[]
  publishedAt: string
  isOfficial?: boolean
  ownerId?: string
}

export interface UserStoryDraft {
  id: string
  title: string
  caption: string
  mediaType: StoryMediaType
  mediaPath: string
  mediaSrc: string
  mediaAlt: string
  posterSrc?: string
  productSlugs: string[]
  createdAt: string
  isActive: boolean
  ownerName?: string
}

export type UserStoryInput = {
  title: string
  caption: string
  mediaType: StoryMediaType
  mediaPath: string
  mediaAlt: string
  posterPath?: string
  productSlug?: string
  isActive?: boolean
}
