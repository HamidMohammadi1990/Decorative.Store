import { API_BASE_URL } from '@/config/api'
import type { FeaturedShopGrid } from '@/models/home/featuredShop.model'

export interface FeaturedCatalogCollection {
  id: string
  href: string
  imageUrl: string
  imageAlt: string
}

function resolveProductImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  if (imageUrl.startsWith('/')) return imageUrl
  return `${API_BASE_URL}/${imageUrl}`
}

function isCustomCmsUpload(src?: string): boolean {
  if (!src) return false
  return src.includes('/Uploads/Cms/')
}

export function buildFeaturedShopGrid(
  collections: FeaturedCatalogCollection[],
  fallback: FeaturedShopGrid,
): FeaturedShopGrid {
  if (collections.length === 0) return fallback

  const collectionById = new Map(collections.map((collection) => [collection.id, collection]))

  const sections = fallback.sections.map((section) => {
    if (isCustomCmsUpload(section.image?.src)) return section

    const collection = collectionById.get(section.id)
    if (!collection?.imageUrl) return section

    const href = collection.href || section.link?.href || '/'

    return {
      ...section,
      link: {
        ...section.link,
        href,
      },
      image: {
        src: resolveProductImageSrc(collection.imageUrl),
        alt: collection.imageAlt || section.image?.alt || section.title,
      },
    }
  })

  return { sections }
}
