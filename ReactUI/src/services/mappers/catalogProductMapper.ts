import { API_BASE_URL } from '@/config/api'
import type { CatalogProductResponse } from '@/models/catalog/catalogProduct.model'
import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type {
  ProductDetail,
  ProductFeature,
  ProductFeatureGroup,
} from '@/models/catalog/productDetail.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import type { Locale } from '@/models/shared/locale.model'
import { mapCatalogListingProduct } from '@/services/mappers/catalogListingMapper'

function resolveProductImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  return `${API_BASE_URL}${imageUrl.startsWith('/') ? imageUrl : `/${imageUrl}`}`
}

function toListingProductShape(product: CatalogProductResponse): CatalogListingProduct {
  return {
    id: product.id,
    title: product.title,
    slug: product.slug,
    imageUrl: product.imageUrl,
    imageAlt: product.imageAlt,
    price: product.price,
    currencyCode: product.currencyCode,
    compareAtPrice: product.compareAtPrice,
    inStock: product.inStock,
    onSale: product.onSale,
    isNew: product.isNew,
    categorySlug: product.categorySlug,
    subCategorySlug: product.subCategorySlug,
  }
}

function buildFeatureGroups(features: CatalogProductResponse['features']): ProductFeatureGroup[] {
  const groups = new Map<string, ProductFeature[]>()

  for (const feature of features) {
    const groupKey = feature.groupTitle?.trim() || 'default'
    const list = groups.get(groupKey) ?? []
    list.push({ label: feature.label, value: feature.value })
    groups.set(groupKey, list)
  }

  return [...groups.entries()].map(([key, groupFeatures], index) => ({
    id: `group-${index}`,
    title: key === 'default' ? '' : key,
    features: groupFeatures,
  }))
}

export function buildProductDetailFromSummary(product: ProductSummary): ProductDetail {
  return {
    ...product,
    description: '',
    longDescription: [],
    highlights: [],
    images: product.image.src ? [product.image] : [],
    features: [],
    featureGroups: [],
  }
}

export function mapCatalogProductToDetail(
  product: CatalogProductResponse,
  locale: Locale,
): ProductDetail | null {
  void locale

  if (product.notFound) return null

  const summary = mapCatalogListingProduct(toListingProductShape(product))
  const apiImages =
    product.images.length > 0
      ? product.images.map((image) => ({
          src: resolveProductImageSrc(image.url),
          alt: image.alt || product.title,
        }))
      : summary.image.src
        ? [summary.image]
        : []

  const flatFeatures: ProductFeature[] = product.features.map((feature) => ({
    label: feature.label,
    value: feature.value,
  }))

  const featureGroups = buildFeatureGroups(product.features)

  const longDescription =
    product.longDescriptions.length > 0
      ? product.longDescriptions
      : product.description
        ? [product.description]
        : []

  return {
    ...summary,
    description: product.description,
    longDescription,
    highlights: [],
    images: apiImages,
    features: flatFeatures,
    featureGroups,
    reviewCount: product.reviewCount,
    averageRating: product.averageRating,
    satisfactionPercent: product.satisfactionPercent,
    purchaseCount: product.purchaseCount,
  }
}

export function mapRelatedCatalogProducts(products: CatalogListingProduct[]): ProductSummary[] {
  return products.map(mapCatalogListingProduct)
}
