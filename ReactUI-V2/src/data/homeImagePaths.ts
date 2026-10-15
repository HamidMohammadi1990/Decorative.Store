/**
 * Single source of truth for homepage image paths (served from /public).
 * After `npm run download:images`, files are .jpg; placeholders use .svg.
 */
export const HOME_IMAGE_PATHS = {
  livingRoom: '/images/home/living-room',
  bedroom: '/images/home/bedroom',
  velvetSofa: '/images/home/velvet-sofa',
  dining: '/images/home/dining',
  bedroomSet: '/images/home/bedroom-set',
  newArrivals: '/images/home/new-arrivals',
  tradeInterior: '/images/home/trade-interior',
  inStock: '/images/home/in-stock',
  collaboration: '/images/home/collaboration',
  commercialOffice: '/images/home/commercial-office',
} as const

/** Prefer JPEG only after `npm run download:images`; repo ships SVG placeholders. */
export function homeImageSrc(basePath: string, preferJpg?: boolean): string {
  const useJpg =
    preferJpg ??
    (import.meta.env.VITE_HOME_IMAGES_PREFER_JPG as string | undefined)?.trim() === 'true'
  return useJpg ? `${basePath}.jpg` : `${basePath}.svg`
}
