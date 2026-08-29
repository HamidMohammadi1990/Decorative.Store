import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type {
  CatalogProductFeature,
  CatalogProductImage,
  CatalogProductResponse,
} from '@/models/catalog/catalogProduct.model'
import type { CommentTopic } from '@/models/catalog/commentTopic.model'
import {
  readBooleanField,
  readNumberField,
  readOptionalNumberField,
  readOptionalStringField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'

function normalizeCatalogProductImage(data: unknown): CatalogProductImage {
  const record = readRecord(data) ?? {}
  return {
    url: readStringField(record, 'url', 'Url'),
    alt: readStringField(record, 'alt', 'Alt'),
  }
}

function normalizeCatalogProductFeature(data: unknown): CatalogProductFeature {
  const record = readRecord(data) ?? {}
  return {
    label: readStringField(record, 'label', 'Label'),
    value: readStringField(record, 'value', 'Value'),
    groupTitle: readOptionalStringField(record, 'groupTitle', 'GroupTitle') ?? undefined,
  }
}

export function normalizeCatalogListingProduct(data: unknown): CatalogListingProduct {
  const record = readRecord(data) ?? {}

  return {
    id: readStringField(record, 'id', 'Id'),
    title: readStringField(record, 'title', 'Title'),
    slug: readStringField(record, 'slug', 'Slug'),
    imageUrl: readStringField(record, 'imageUrl', 'ImageUrl'),
    imageAlt: readStringField(record, 'imageAlt', 'ImageAlt'),
    price: readNumberField(record, 'price', 'Price'),
    currencyCode: readStringField(record, 'currencyCode', 'CurrencyCode') || 'IRT',
    compareAtPrice: readOptionalNumberField(record, 'compareAtPrice', 'CompareAtPrice'),
    inStock: readBooleanField(record, 'inStock', 'InStock'),
    onSale: readBooleanField(record, 'onSale', 'OnSale'),
    isNew: readBooleanField(record, 'isNew', 'IsNew'),
    categorySlug: readStringField(record, 'categorySlug', 'CategorySlug'),
    subCategorySlug: readStringField(record, 'subCategorySlug', 'SubCategorySlug'),
  }
}

export function normalizeCatalogProductResponse(data: unknown): CatalogProductResponse {
  const record = readRecord(data) ?? {}
  const images = Array.isArray(record.images ?? record.Images)
    ? (record.images ?? record.Images).map(normalizeCatalogProductImage)
    : []
  const features = Array.isArray(record.features ?? record.Features)
    ? (record.features ?? record.Features).map(normalizeCatalogProductFeature)
    : []
  const longDescriptions = Array.isArray(record.longDescriptions ?? record.LongDescriptions)
    ? (record.longDescriptions ?? record.LongDescriptions).filter(
        (item): item is string => typeof item === 'string',
      )
    : []

  return {
    notFound: readBooleanField(record, 'notFound', 'NotFound'),
    id: readStringField(record, 'id', 'Id'),
    title: readStringField(record, 'title', 'Title'),
    slug: readStringField(record, 'slug', 'Slug'),
    imageUrl: readStringField(record, 'imageUrl', 'ImageUrl'),
    imageAlt: readStringField(record, 'imageAlt', 'ImageAlt'),
    price: readNumberField(record, 'price', 'Price'),
    currencyCode: readStringField(record, 'currencyCode', 'CurrencyCode') || 'IRT',
    compareAtPrice: readOptionalNumberField(record, 'compareAtPrice', 'CompareAtPrice'),
    inStock: readBooleanField(record, 'inStock', 'InStock'),
    onSale: readBooleanField(record, 'onSale', 'OnSale'),
    isNew: readBooleanField(record, 'isNew', 'IsNew'),
    categorySlug: readStringField(record, 'categorySlug', 'CategorySlug'),
    subCategorySlug: readStringField(record, 'subCategorySlug', 'SubCategorySlug'),
    description: readStringField(record, 'description', 'Description'),
    longDescriptions,
    images,
    features,
    reviewCount: readNumberField(record, 'reviewCount', 'ReviewCount'),
    averageRating: readOptionalNumberField(record, 'averageRating', 'AverageRating'),
    satisfactionPercent: readOptionalNumberField(record, 'satisfactionPercent', 'SatisfactionPercent'),
    purchaseCount: readNumberField(record, 'purchaseCount', 'PurchaseCount'),
  }
}

export function normalizeCommentTopic(data: unknown): CommentTopic {
  const record = readRecord(data) ?? {}

  return {
    id: readStringField(record, 'id', 'Id'),
    title: readStringField(record, 'title', 'Title'),
    priority: readNumberField(record, 'priority', 'Priority'),
    isActive: readBooleanField(record, 'isActive', 'IsActive'),
  }
}
