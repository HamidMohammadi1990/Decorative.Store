import { absoluteUrl, SITE_NAME } from '@/config/site'

export function buildOrganizationJsonLd() {
  return {
    '@context': 'https://schema.org',
    '@type': 'Organization',
    name: SITE_NAME,
    url: absoluteUrl('/'),
    logo: absoluteUrl('/favicon.svg'),
  }
}

export function buildWebSiteJsonLd() {
  return {
    '@context': 'https://schema.org',
    '@type': 'WebSite',
    name: SITE_NAME,
    url: absoluteUrl('/'),
    potentialAction: {
      '@type': 'SearchAction',
      target: `${absoluteUrl('/search')}?q={search_term_string}`,
      'query-input': 'required name=search_term_string',
    },
  }
}

export function buildBreadcrumbJsonLd(items: { name: string; path?: string }[]) {
  return {
    '@context': 'https://schema.org',
    '@type': 'BreadcrumbList',
    itemListElement: items.map((item, index) => ({
      '@type': 'ListItem',
      position: index + 1,
      name: item.name,
      ...(item.path ? { item: absoluteUrl(item.path) } : {}),
    })),
  }
}

export function buildProductJsonLd(input: {
  name: string
  description: string
  image: string
  url: string
  sku?: string
  price: number
  currency: string
  inStock: boolean
  rating?: number
  reviewCount?: number
}) {
  const offer: Record<string, unknown> = {
    '@type': 'Offer',
    url: input.url,
    priceCurrency: input.currency,
    price: input.price,
    availability: input.inStock
      ? 'https://schema.org/InStock'
      : 'https://schema.org/OutOfStock',
  }

  const product: Record<string, unknown> = {
    '@context': 'https://schema.org',
    '@type': 'Product',
    name: input.name,
    description: input.description,
    image: input.image,
    url: input.url,
    offers: offer,
  }

  if (input.sku) product.sku = input.sku
  if (input.rating != null && input.reviewCount != null && input.reviewCount > 0) {
    product.aggregateRating = {
      '@type': 'AggregateRating',
      ratingValue: input.rating,
      reviewCount: input.reviewCount,
    }
  }

  return product
}

export function buildItemListJsonLd(input: {
  name: string
  url: string
  items: { name: string; url: string; image?: string; position: number }[]
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'ItemList',
    name: input.name,
    url: input.url,
    numberOfItems: input.items.length,
    itemListElement: input.items.map((item) => ({
      '@type': 'ListItem',
      position: item.position,
      url: item.url,
      name: item.name,
      ...(item.image ? { image: item.image } : {}),
    })),
  }
}

export function buildWebPageJsonLd(input: {
  name: string
  description: string
  url: string
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'WebPage',
    name: input.name,
    description: input.description,
    url: input.url,
    isPartOf: {
      '@type': 'WebSite',
      name: SITE_NAME,
      url: absoluteUrl('/'),
    },
  }
}

export function buildArticleJsonLd(input: {
  headline: string
  description: string
  image: string
  url: string
  datePublished: string
  authorName: string
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'Article',
    headline: input.headline,
    description: input.description,
    image: input.image,
    url: input.url,
    datePublished: input.datePublished,
    author: {
      '@type': 'Person',
      name: input.authorName,
    },
    publisher: {
      '@type': 'Organization',
      name: SITE_NAME,
      logo: {
        '@type': 'ImageObject',
        url: absoluteUrl('/favicon.svg'),
      },
    },
  }
}
