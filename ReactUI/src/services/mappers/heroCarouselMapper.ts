import { API_BASE_URL } from '@/config/api'
import i18n from '@/i18n'
import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type { HeroCarousel } from '@/models/home/heroCarousel.model'
import type { Locale } from '@/models/shared/locale.model'

function resolveProductImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  return `${API_BASE_URL}${imageUrl.startsWith('/') ? imageUrl : `/${imageUrl}`}`
}

export function buildHeroCarouselFromProducts(
  products: CatalogListingProduct[],
  locale: Locale,
): HeroCarousel {
  const eyebrow = i18n.t('home.heroEyebrow', { lng: locale })
  const ctaLabel = i18n.t('home.heroCta', { lng: locale })

  return {
    slides: products.map((product) => ({
      id: product.id,
      eyebrow,
      title: product.title,
      image: {
        src: resolveProductImageSrc(product.imageUrl),
        alt: product.imageAlt || product.title,
      },
      cta: {
        label: ctaLabel,
        href: `/product/${product.slug}`,
      },
    })),
  }
}
