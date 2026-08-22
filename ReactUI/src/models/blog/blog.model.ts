import type { ImageAsset } from '@/models/shared/image.model'

export interface BlogAuthor {
  id: string
  name: string
  role: string
  avatar: ImageAsset
  bio: string
}

export interface BlogCategory {
  id: string
  slug: string
  label: string
  count?: number
}

export interface BlogComment {
  id: string
  authorName: string
  date: string
  text: string
  likes: number
}

export interface BlogPostSummary {
  id: string
  slug: string
  title: string
  excerpt: string
  coverImage: ImageAsset
  categorySlug: string
  categoryLabel?: string
  authorId: string
  publishedAt: string
  readTimeMinutes: number
  likes: number
  commentCount: number
  featured?: boolean
  tags: string[]
}

export interface BlogPostDetail extends BlogPostSummary {
  categoryId: string
  categoryLabel: string
  content: string[]
  gallery: ImageAsset[]
  author: BlogAuthor
  comments: BlogComment[]
}

export interface BlogListingResult {
  categories: BlogCategory[]
  featuredPosts: BlogPostSummary[]
  posts: BlogPostSummary[]
  totalCount: number
  activeCategory?: string
}
