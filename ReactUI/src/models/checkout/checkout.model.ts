export const PropertyType = {
  Boolean: 1,
  HasParents: 2,
  Numeric: 3,
  NumericWithItem: 4,
  Dimensions: 5,
  Text: 6,
  Select: 7,
} as const

export type PropertyTypeValue = (typeof PropertyType)[keyof typeof PropertyType]

export interface CheckoutPriceField {
  amount: number
  currencyCode: string
}

export interface CheckoutPropertyRule {
  isMandatory: boolean
  description?: string | null
  minQuantity?: number
  maxQuantity?: number
  minLength?: number
  maxLength?: number
  minWidth?: number
  maxWidth?: number
  minHeight?: number
  maxHeight?: number
}

export interface CheckoutPropertyItem {
  id: string
  title: string
  price: CheckoutPriceField
}

export interface CheckoutPropertyDependency {
  parentId: string
  dependentId: string
}

export interface CheckoutProperty {
  id: string
  title: string
  propertyType: PropertyTypeValue
  dependentPropertyId?: string | null
  price: CheckoutPriceField
  items: CheckoutPropertyItem[]
  dependencies: CheckoutPropertyDependency[]
  parents: CheckoutProperty[]
  rule?: CheckoutPropertyRule | null
}

export interface CheckoutPropertyGroup {
  categoryTitle: string
  properties: CheckoutProperty[]
}

export interface CheckoutProductImage {
  title: string
  url: string
}

export interface CheckoutProduct {
  id: string
  title: string
  slug: string
  description: string
  productCode: string
  subCategoryTitle: string
  images: CheckoutProductImage[]
}

export interface CheckoutPostType {
  id: string
  title: string
  description: string
}

export interface CheckoutUserAddress {
  id: string
  title: string
  address: string
}

export interface CheckoutAttachmentRestriction {
  isRequired: boolean
  minWidth: number
  maxWidth: number
  minHeight: number
  maxHeight: number
  minHorizontalResolution: number
  maxHorizontalResolution: number
  minVerticalResolution: number
  maxVerticalResolution: number
  colorMode: number
  maxFileSizeInMegaBytes: number
}

export interface CheckoutAttachment {
  id: string
  title: string
  description: string
  restriction: CheckoutAttachmentRestriction
}

export interface CheckoutOrderData {
  product: CheckoutProduct
  properties: CheckoutPropertyGroup[]
  postTypes: CheckoutPostType[]
  addresses: CheckoutUserAddress[]
  attachments: CheckoutAttachment[]
}

export interface CheckoutProductSession {
  productId: string
  productTitle: string
  data: CheckoutOrderData
}
