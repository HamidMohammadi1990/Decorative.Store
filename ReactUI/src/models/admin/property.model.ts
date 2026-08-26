import type { PropertyTypeValue } from '@/models/checkout/checkout.model'
import type { CatalogTranslation } from '@/models/admin/catalog.model'

export interface AdminPropertyCategory {
  id: string
  code: string
  isActive: boolean
  title: string
  translations: CatalogTranslation[]
}

export interface CreatePropertyCategoryInput {
  languageId: number
  code: string
  title: string
}

export interface UpdatePropertyCategoryInput extends CreatePropertyCategoryInput {
  id: string
  isActive: boolean
}

export interface AdminProperty {
  id: string
  code: string
  parentId: string | null
  propertyCategoryId: string
  propertyCategoryCode: string
  priority: number
  propertyType: PropertyTypeValue
  isActive: boolean
  title: string
  description?: string
  translations: CatalogTranslation[]
}

export interface AdminPropertyItem {
  id: string
  code: string
  propertyId: string
  propertyCode: string
  propertyCategoryId: string
  propertyType: PropertyTypeValue
  priority: number
  isActive: boolean
  title: string
  translations: CatalogTranslation[]
}

export interface AdminProductProperty {
  id: string
  productId: string
  propertyId: string
  propertyItemId: string | null
  isActive: boolean
}

export interface CreatePropertyInput {
  languageId: number
  code: string
  title: string
  description?: string | null
  parentId?: string | null
  priority: number
  propertyCategoryId: string
  propertyType: PropertyTypeValue
}

export interface UpdatePropertyInput extends CreatePropertyInput {
  id: string
  status: boolean
}

export interface CreatePropertyItemInput {
  languageId: number
  code: string
  title: string
  propertyId: string
  priority: number
}

export interface UpdatePropertyItemInput extends CreatePropertyItemInput {
  id: string
  status: boolean
}

export interface CreateProductPropertyInput {
  productId: string
  propertyId: string
  propertyItemId?: string | null
  isActive: boolean
}

export interface UpdateProductPropertyInput extends CreateProductPropertyInput {
  id: string
}

export const PROPERTY_TYPE_OPTIONS: PropertyTypeValue[] = [1, 2, 3, 4, 5, 6, 7]

export function propertyTypeUsesItems(type: PropertyTypeValue): boolean {
  return type === 4 || type === 7
}
