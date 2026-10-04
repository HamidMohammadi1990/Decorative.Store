import { HOME_IMAGE_PATHS, homeImageSrc } from '@/data/homeImagePaths'

type HomeImageKey = keyof typeof HOME_IMAGE_PATHS

const KEY_BY_PATH = Object.fromEntries(
  (Object.entries(HOME_IMAGE_PATHS) as [HomeImageKey, string][]).map(
    ([key, path]) => [path, key],
  ),
) as Record<string, HomeImageKey>

/**
 * Resolves mock/API image src to a local public path.
 * External URLs are left unchanged; local bases try .jpg then .svg.
 */
export function resolveHomeImageSrc(src: string): string {
  if (!src.startsWith('/images/home/')) {
    return src
  }

  const base = src.replace(/\.(jpg|jpeg|svg|webp)$/i, '')
  const key = KEY_BY_PATH[base]
  if (!key) return src

  return homeImageSrc(HOME_IMAGE_PATHS[key])
}
