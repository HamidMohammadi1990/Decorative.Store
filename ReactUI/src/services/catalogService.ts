import type { ProductSummary } from '@/models/catalog/product.model'
import type { Locale } from '@/models/shared/locale.model'
import { enrichProductDetail } from '@/extensions/productDetailContent'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type {
  FilterFacet,
  ListingBreadcrumb,
  ListingQuery,
  ProductListingResult,
  SortOptionId,
} from '@/models/catalog/listing.model'
import type { ParsedListingFilters } from '@/extensions/listingFilters'
import { getProductsMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import {
  PRICE_BUCKETS,
  getProductPriceBounds,
  parseListingFilters,
  productMatchesFilters,
  toActiveFiltersRecord,
} from '@/extensions/listingFilters'
import { getColorSwatch } from '@/extensions/colorSwatches'
import {
  productMatchesCollection,
  productMatchesPathSegments,
  resolveListingPath,
} from '@/extensions/resolveListingPath'
import { catalogListingService } from '@/services/catalogListingService'
import { mapCatalogListingProduct } from '@/services/mappers/catalogListingMapper'

const PAGE_SIZE = 12

const CATEGORY_LABELS: Record<Locale, Record<string, string>> = {
  en: {
    sofas: 'Sofas & Armchairs',
    furniture: 'Furniture',
    garden: 'Garden',
    'rugs-curtains': 'Rugs & Curtains',
    rugs: 'Rugs',
    'bed-linen': 'Bed Linen',
    bath: 'Bath',
    lighting: 'Lighting',
    decor: 'Cushions & Decor',
    'art-mirrors': 'Art & Mirrors',
    'kitchen-dining': 'Kitchen & Dining',
    gifts: 'Gifts',
    sale: 'Sale',
    new: 'New Arrivals',
    bedroom: 'Bedroom',
    'home-office': 'Home Office',
    dining: 'Dining Room',
    'living-room': 'Living Room',
    'mid-century': 'Mid-Century',
    'in-stock': 'In Stock',
    'best-sellers': 'Best Sellers',
    clearance: 'Clearance',
  },
  fa: {
    sofas: 'مبل و صندلی',
    furniture: 'مبلمان',
    garden: 'باغ و فضای باز',
    'rugs-curtains': 'فرش و پرده',
    rugs: 'فرش',
    'bed-linen': 'ملحفه و روتختی',
    bath: 'حمام',
    lighting: 'روشنایی',
    decor: 'کوسن و دکور',
    'art-mirrors': 'هنر و آینه',
    'kitchen-dining': 'آشپزخانه و ناهارخوری',
    gifts: 'هدایا',
    sale: 'حراج',
    new: 'جدید',
    bedroom: 'اتاق خواب',
    'home-office': 'دفتر خانگی',
    dining: 'ناهارخوری',
    'living-room': 'اتاق نشیمن',
    'mid-century': 'میدسنچری',
    'in-stock': 'موجود در انبار',
    'best-sellers': 'پرفروش‌ها',
    clearance: 'تخفیف ویژه',
  },
}

const COLLECTION_LABELS: Record<Locale, Record<string, string>> = {
  en: {
    sale: 'Sale',
    new: 'New Arrivals',
    'in-stock': 'In Stock & Ready to Ship',
    'best-sellers': 'Best Sellers',
    clearance: 'Clearance',
    shop: 'Shop',
    collaborations: 'Collaborations',
  },
  fa: {
    sale: 'حراج',
    new: 'محصولات جدید',
    'in-stock': 'موجود و آماده ارسال',
    'best-sellers': 'پرفروش‌ها',
    clearance: 'تخفیف ویژه',
    shop: 'فروشگاه',
    collaborations: 'همکاری‌ها',
  },
}

const FACET_LABELS: Record<Locale, Record<string, string>> = {
  en: {
    price: 'Price',
    color: 'Colour',
    size: 'Size',
    material: 'Material',
    room: 'Room',
    inStock: 'Availability',
    onSale: 'Offers',
    isNew: 'Newness',
  },
  fa: {
    price: 'قیمت',
    color: 'رنگ',
    size: 'اندازه',
    material: 'جنس',
    room: 'اتاق',
    inStock: 'موجودی',
    onSale: 'پیشنهادها',
    isNew: 'تازه‌ها',
  },
}

const VALUE_LABELS: Record<Locale, Record<string, string>> = {
  en: {
    green: 'Green',
    grey: 'Grey',
    walnut: 'Walnut',
    oak: 'Oak',
    white: 'White',
    beige: 'Beige',
    cream: 'Cream',
    natural: 'Natural',
    brass: 'Brass',
    clear: 'Clear',
    neutral: 'Neutral',
    taupe: 'Taupe',
    charcoal: 'Charcoal',
    teak: 'Teak',
    '3-seater': '3 Seater',
    '2-seater': '2 Seater',
    armchair: 'Armchair',
    sectional: 'Sectional',
    velvet: 'Velvet',
    linen: 'Linen',
    wood: 'Wood',
    wool: 'Wool',
    ceramic: 'Ceramic',
    rattan: 'Rattan',
    metal: 'Metal',
    glass: 'Glass',
    cotton: 'Cotton',
    fabric: 'Fabric',
    'performance-fabric': 'Performance fabric',
    'living-room': 'Living room',
    bedroom: 'Bedroom',
    dining: 'Dining',
    garden: 'Garden',
    'home-office': 'Home office',
    inStock: 'In stock only',
    onSale: 'On sale',
    isNew: 'New arrivals',
    'under-5m': 'Under 5M',
    '5m-10m': '5M – 10M',
    '10m-15m': '10M – 15M',
    '15m-20m': '15M – 20M',
    'over-20m': 'Over 20M',
  },
  fa: {
    green: 'سبز',
    grey: 'خاکستری',
    walnut: 'گردویی',
    oak: 'بلوط',
    white: 'سفید',
    beige: 'بژ',
    cream: 'کرم',
    natural: 'طبیعی',
    brass: 'برنجی',
    clear: 'شفاف',
    neutral: 'خنثی',
    taupe: 'قهوه‌ای روشن',
    charcoal: 'ذغالی',
    teak: 'چوبی',
    '3-seater': '۳ نفره',
    '2-seater': '۲ نفره',
    armchair: 'صندلی راحتی',
    sectional: 'گوشه',
    velvet: 'مخمل',
    linen: 'کتان',
    wood: 'چوب',
    wool: 'پشم',
    ceramic: 'سرامیک',
    rattan: 'حصیری',
    metal: 'فلز',
    glass: 'شیشه',
    cotton: 'نخی',
    fabric: 'پارچه',
    'performance-fabric': 'پارچه مقاوم',
    'living-room': 'نشیمن',
    bedroom: 'خواب',
    dining: 'ناهارخوری',
    garden: 'باغ',
    'home-office': 'دفتر',
    inStock: 'فقط موجود',
    onSale: 'حراج',
    isNew: 'محصولات جدید',
    'under-5m': 'زیر ۵ میلیون',
    '5m-10m': '۵ تا ۱۰ میلیون',
    '10m-15m': '۱۰ تا ۱۵ میلیون',
    '15m-20m': '۱۵ تا ۲۰ میلیون',
    'over-20m': 'بالای ۲۰ میلیون',
  },
}

function labelFor(map: Record<string, string>, key: string) {
  return map[key] ?? key.replace(/-/g, ' ')
}

function getSortOptions(locale: Locale) {
  const labels =
    locale === 'fa'
      ? {
          featured: 'پیشنهادی',
          'price-asc': 'قیمت: کم به زیاد',
          'price-desc': 'قیمت: زیاد به کم',
          newest: 'جدیدترین',
        }
      : {
          featured: 'Featured',
          'price-asc': 'Price: Low to High',
          'price-desc': 'Price: High to Low',
          newest: 'Newest',
        }

  return (['featured', 'price-asc', 'price-desc', 'newest'] as SortOptionId[]).map(
    (id) => ({ id, label: labels[id] }),
  )
}

function buildTitle(locale: Locale, collection: string | null, segments: string[]) {
  const catLabels = CATEGORY_LABELS[locale]
  const colLabels = COLLECTION_LABELS[locale]

  if (collection && segments.length === 0) {
    return colLabels[collection] ?? collection
  }

  if (collection && segments.length > 0) {
    const cat = labelFor(catLabels, segments[0])
    const col = colLabels[collection] ?? collection
    return locale === 'fa' ? `${col} — ${cat}` : `${col} — ${cat}`
  }

  if (segments.length === 1) {
    return labelFor(catLabels, segments[0])
  }

  if (segments.length >= 2) {
    const parent = labelFor(catLabels, segments[0])
    const child = labelFor(catLabels, segments[1])
    return `${parent} — ${child}`
  }

  return locale === 'fa' ? 'همه محصولات' : 'All Products'
}

function buildBreadcrumbs(
  locale: Locale,
  collection: string | null,
  segments: string[],
): ListingBreadcrumb[] {
  const home = locale === 'fa' ? 'خانه' : 'Home'
  const crumbs: ListingBreadcrumb[] = [{ label: home, href: '/' }]
  const catLabels = CATEGORY_LABELS[locale]
  const colLabels = COLLECTION_LABELS[locale]

  let path = ''
  if (collection) {
    path = `/${collection}`
    crumbs.push({
      label: colLabels[collection] ?? collection,
      href: path,
    })
  }

  segments.forEach((segment) => {
    path += `/${segment}`
    crumbs.push({
      label: labelFor(catLabels, segment),
      href: path,
    })
  })

  return crumbs
}

function sortProducts(products: ProductSummary[], sort: SortOptionId) {
  const copy = [...products]
  switch (sort) {
    case 'price-asc':
      return copy.sort((a, b) => a.price.amount - b.price.amount)
    case 'price-desc':
      return copy.sort((a, b) => b.price.amount - a.price.amount)
    case 'newest':
      return copy.sort((a, b) => Number(b.isNew) - Number(a.isNew))
    default:
      return copy
  }
}

function countForFacetOption(
  products: ProductSummary[],
  filters: ParsedListingFilters,
  facetId: string,
  optionValue: string,
) {
  return products.filter((product) => {
    if (!productMatchesFilters(product, filters, facetId)) return false

    if (facetId === 'price') {
      const bucket = PRICE_BUCKETS.find((b) => b.id === optionValue)
      if (!bucket) return false
      return (
        product.price.amount >= bucket.min && product.price.amount <= bucket.max
      )
    }
    if (facetId === 'inStock') return product.inStock
    if (facetId === 'onSale') return product.onSale
    if (facetId === 'isNew') return product.isNew

    const values = product.facets[facetId as keyof typeof product.facets] ?? []
    return values.includes(optionValue)
  }).length
}

function buildFacets(
  locale: Locale,
  products: ProductSummary[],
  filters: ParsedListingFilters,
): FilterFacet[] {
  if (products.length === 0) return []

  const facets: FilterFacet[] = []
  const valueLabels = VALUE_LABELS[locale]
  const facetLabels = FACET_LABELS[locale]
  const priceBounds = getProductPriceBounds(products)

  const priceBucketOptions = PRICE_BUCKETS.map((bucket) => ({
    value: bucket.id,
    label: labelFor(valueLabels, bucket.id),
    count: countForFacetOption(products, filters, 'price', bucket.id),
  })).filter((option) => option.count > 0)

  facets.push({
    id: 'price',
    label: facetLabels.price,
    type: 'range',
    options: priceBucketOptions,
    range: {
      min: priceBounds.min,
      max: priceBounds.max,
      step: 1,
      selectedMin: filters.minPrice,
      selectedMax: filters.maxPrice,
    },
  })

  const inStockCount = countForFacetOption(products, filters, 'inStock', 'true')
  if (inStockCount > 0) {
    facets.push({
      id: 'inStock',
      label: facetLabels.inStock,
      type: 'checkbox',
      options: [
        {
          value: 'true',
          label: valueLabels.inStock,
          count: inStockCount,
        },
      ],
    })
  }

  const onSaleCount = countForFacetOption(products, filters, 'onSale', 'true')
  if (onSaleCount > 0) {
    facets.push({
      id: 'onSale',
      label: facetLabels.onSale,
      type: 'checkbox',
      options: [
        {
          value: 'true',
          label: valueLabels.onSale,
          count: onSaleCount,
        },
      ],
    })
  }

  const isNewCount = countForFacetOption(products, filters, 'isNew', 'true')
  if (isNewCount > 0) {
    facets.push({
      id: 'isNew',
      label: facetLabels.isNew,
      type: 'checkbox',
      options: [
        {
          value: 'true',
          label: valueLabels.isNew,
          count: isNewCount,
        },
      ],
    })
  }

  const attributeFacetIds = ['color', 'size', 'material', 'room'] as const

  for (const facetId of attributeFacetIds) {
    const values = new Set<string>()
    for (const product of products) {
      for (const value of product.facets[facetId] ?? []) {
        values.add(value)
      }
    }
    if (values.size === 0) continue

    const options = [...values]
      .sort((a, b) => a.localeCompare(b))
      .map((value) => ({
        value,
        label: labelFor(valueLabels, value),
        count: countForFacetOption(products, filters, facetId, value),
        swatch: facetId === 'color' ? getColorSwatch(value) : undefined,
      }))
      .filter((option) => option.count > 0)

    if (options.length === 0) continue

    facets.push({
      id: facetId,
      label: facetLabels[facetId],
      type: facetId === 'color' ? 'color' : 'checkbox',
      options,
    })
  }

  return facets
}

async function getMockListing(
  query: ListingQuery,
  locale: Locale,
): Promise<ProductListingResult> {
  const { pathname, searchParams } = query
  const { collection, segments } = resolveListingPath(pathname)
  const allProducts = getProductsMock(locale) as ProductSummary[]

  let filtered = allProducts.filter((product) => {
    if (
      !productMatchesCollection(collection, {
        onSale: product.onSale,
        isNew: product.isNew,
        inStock: product.inStock,
      })
    ) {
      return false
    }
    return productMatchesPathSegments(
      product.categorySlugs,
      product.subcategorySlug,
      product.facets,
      segments,
    )
  })

  const parsedFilters = parseListingFilters(searchParams)
  filtered = filtered.filter((p) => productMatchesFilters(p, parsedFilters))

  const sort = (searchParams.get('sort') as SortOptionId) || 'featured'
  filtered = sortProducts(filtered, sort)

  const page = Math.max(1, Number(searchParams.get('page')) || 1)
  const totalCount = filtered.length
  const start = (page - 1) * PAGE_SIZE
  const pageProducts = filtered.slice(start, start + PAGE_SIZE)

  const categoryProducts = allProducts.filter((product) => {
    if (
      !productMatchesCollection(collection, {
        onSale: product.onSale,
        isNew: product.isNew,
        inStock: product.inStock,
      })
    ) {
      return false
    }
    return productMatchesPathSegments(
      product.categorySlugs,
      product.subcategorySlug,
      product.facets,
      segments,
    )
  })

  const facets = buildFacets(locale, categoryProducts, parsedFilters)

  const pathNotFound =
    segments.length > 0 &&
    categoryProducts.length === 0 &&
    collection !== 'collaborations'

  return {
    title: buildTitle(locale, collection, segments),
    breadcrumbs: buildBreadcrumbs(locale, collection, segments),
    products: pageProducts,
    facets,
    sortOptions: getSortOptions(locale),
    totalCount,
    page,
    pageSize: PAGE_SIZE,
    activeFilters: toActiveFiltersRecord(parsedFilters),
    pathNotFound,
  }
}

export const catalogService = {
  async getListing(query: ListingQuery, locale: Locale): Promise<ProductListingResult> {
    const { pathname, searchParams } = query
    const catalogPath = pathname.replace(/^\/+/, '').replace(/\/+$/, '')

    try {
      const listing = await catalogListingService.getListing(catalogPath, locale)

      if (listing.pathNotFound) {
        return {
          title: listing.title,
          breadcrumbs: listing.breadcrumbs,
          products: [],
          facets: [],
          sortOptions: getSortOptions(locale),
          totalCount: 0,
          page: 1,
          pageSize: PAGE_SIZE,
          activeFilters: {},
          pathNotFound: true,
        }
      }

      let filtered = listing.products.map(mapCatalogListingProduct)
      const parsedFilters = parseListingFilters(searchParams)
      filtered = filtered.filter((product) => productMatchesFilters(product, parsedFilters))

      const sort = (searchParams.get('sort') as SortOptionId) || 'featured'
      filtered = sortProducts(filtered, sort)

      const page = Math.max(1, Number(searchParams.get('page')) || 1)
      const totalCount = filtered.length
      const start = (page - 1) * PAGE_SIZE
      const pageProducts = filtered.slice(start, start + PAGE_SIZE)
      const facets = buildFacets(locale, listing.products.map(mapCatalogListingProduct), parsedFilters)

      return {
        title: listing.title,
        breadcrumbs: listing.breadcrumbs,
        products: pageProducts,
        facets,
        sortOptions: getSortOptions(locale),
        totalCount,
        page,
        pageSize: PAGE_SIZE,
        activeFilters: toActiveFiltersRecord(parsedFilters),
        pathNotFound: false,
      }
    } catch {
      return mockFetch(async () => getMockListing(query, locale))
    }
  },

  async getProduct(slug: string, locale: Locale): Promise<ProductDetail | null> {
    return mockFetch(async () => {
      const products = getProductsMock(locale) as ProductSummary[]
      const product = products.find((p) => p.slug === slug)
      if (!product) return null
      return enrichProductDetail(product, locale)
    })
  },

  async getProductsBySlugs(slugs: string[], locale: Locale): Promise<ProductDetail[]> {
    return mockFetch(async () => {
      if (slugs.length === 0) return []
      const products = getProductsMock(locale) as ProductSummary[]
      const order = new Map(slugs.map((slug, index) => [slug, index]))
      return products
        .filter((p) => slugs.includes(p.slug))
        .sort((a, b) => (order.get(a.slug) ?? 0) - (order.get(b.slug) ?? 0))
        .map((p) => enrichProductDetail(p, locale))
    })
  },

  async getRelatedProducts(
    slug: string,
    locale: Locale,
    limit = 4,
  ): Promise<ProductSummary[]> {
    return mockFetch(async () => {
      const products = getProductsMock(locale) as ProductSummary[]
      const current = products.find((p) => p.slug === slug)
      if (!current) return []

      const primaryCategory = current.categorySlugs[0]
      return products
        .filter(
          (p) =>
            p.slug !== slug &&
            p.categorySlugs.some((c) => c === primaryCategory),
        )
        .slice(0, limit)
    })
  },
}
