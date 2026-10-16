import { resolvePublicMediaUrl } from '@/config/api'
import { BLOG_DEFAULT_COVER_SRC } from '@/data/blogDefaultCover'
import type { BlogPostSummary } from '@/models/blog/blog.model'

/** Cover URL for display — tolerates hydration/API shape drift. */
export function getBlogPostCoverSrc(post: BlogPostSummary): string {
  const fromAsset = post.coverImage?.src?.trim()
  if (fromAsset) return resolvePublicMediaUrl(fromAsset)

  const record = post as BlogPostSummary & {
    coverImageUrl?: string
    CoverImageUrl?: string
  }
  const raw = record.coverImageUrl ?? record.CoverImageUrl
  if (typeof raw === 'string' && raw.trim()) {
    return resolvePublicMediaUrl(raw.trim())
  }

  return BLOG_DEFAULT_COVER_SRC
}
