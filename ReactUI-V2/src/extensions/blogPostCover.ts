import { homeImageSrc, HOME_IMAGE_PATHS } from '@/data/homeImagePaths'
import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'
import type { BlogPostSummary } from '@/models/blog/blog.model'
import { resolveSameOriginMediaSrc } from '@/utils/publicUploadUrl'

/** Placeholder when API has no cover — repo ships .svg under /images/home/. */
export const BLOG_DEFAULT_COVER_SRC = homeImageSrc(HOME_IMAGE_PATHS.livingRoom, false)

function resolveCoverUrl(raw: string): string {
  const normalized = resolveSameOriginMediaSrc(raw, 'BlogPosts')
  return resolveHomeImageSrc(normalized)
}

/** Cover URL for display — tolerates hydration/API shape drift. */
export function getBlogPostCoverSrc(post: BlogPostSummary): string {
  const fromAsset = post.coverImage?.src?.trim()
  if (fromAsset) return resolveCoverUrl(fromAsset)

  const record = post as BlogPostSummary & {
    coverImageUrl?: string
    CoverImageUrl?: string
  }
  const raw = record.coverImageUrl ?? record.CoverImageUrl
  if (typeof raw === 'string' && raw.trim()) {
    return resolveCoverUrl(raw.trim())
  }

  return BLOG_DEFAULT_COVER_SRC
}
