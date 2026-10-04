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
  brand?: string
  category?: string
  locale?: string
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
  if (input.brand) {
    product.brand = {
      '@type': 'Brand',
      name: input.brand,
    }
  }
  if (input.category) product.category = input.category
  if (input.locale) product.inLanguage = input.locale === 'fa' ? 'fa-IR' : 'en-US'
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
  locale?: string
  image?: string
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'WebPage',
    name: input.name,
    description: input.description,
    url: input.url,
    ...(input.image ? { primaryImageOfPage: input.image, image: input.image } : {}),
    ...(input.locale ? { inLanguage: input.locale === 'fa' ? 'fa-IR' : 'en-US' } : {}),
    isPartOf: {
      '@type': 'WebSite',
      name: SITE_NAME,
      url: absoluteUrl('/'),
    },
  }
}

export function buildWebApplicationJsonLd(input: {
  name: string
  description: string
  url: string
  image?: string
  locale?: string
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'WebApplication',
    name: input.name,
    description: input.description,
    url: input.url,
    ...(input.image ? { image: input.image, screenshot: input.image } : {}),
    ...(input.locale ? { inLanguage: input.locale === 'fa' ? 'fa-IR' : 'en-US' } : {}),
    applicationCategory: 'DesignApplication',
    operatingSystem: 'Any',
    browserRequirements: 'Requires JavaScript',
    offers: {
      '@type': 'Offer',
      price: '0',
      priceCurrency: 'USD',
    },
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
  dateModified?: string
  locale?: string
  keywords?: string[]
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'Article',
    headline: input.headline,
    description: input.description,
    image: input.image,
    url: input.url,
    mainEntityOfPage: input.url,
    datePublished: input.datePublished,
    ...(input.dateModified ? { dateModified: input.dateModified } : {}),
    ...(input.locale ? { inLanguage: input.locale === 'fa' ? 'fa-IR' : 'en-US' } : {}),
    ...(input.keywords?.length ? { keywords: input.keywords.join(', ') } : {}),
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

export function buildCollectionPageJsonLd(input: {
  name: string
  description: string
  url: string
  locale?: string
}) {
  return {
    '@context': 'https://schema.org',
    '@type': 'CollectionPage',
    name: input.name,
    description: input.description,
    url: input.url,
    ...(input.locale ? { inLanguage: input.locale === 'fa' ? 'fa-IR' : 'en-US' } : {}),
    isPartOf: {
      '@type': 'WebSite',
      name: SITE_NAME,
      url: absoluteUrl('/'),
    },
  }
}
