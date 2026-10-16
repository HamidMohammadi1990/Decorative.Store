import { HOME_IMAGE_PATHS, homeImageSrc } from '@/data/homeImagePaths'

/** Placeholder when API has no coverImageUrl (use .svg — jpg may be missing on server). */
export const BLOG_DEFAULT_COVER_SRC = homeImageSrc(HOME_IMAGE_PATHS.livingRoom, false)
