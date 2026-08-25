export interface CatalogTranslation {
  languageId: number
  title: string
  slug: string
  description?: string
}

export interface AdminCategory {
  id: string
  code: string
  isActive: boolean
  title: string
  slug: string
  translations: CatalogTranslation[]
}

export interface AdminSubCategory {
  id: string
  code: string
  categoryId: string
  categoryCode: string
  categoryTitle: string
  isActive: boolean
  title: string
  slug: string
  translations: CatalogTranslation[]
}

export interface AdminProductListItem {
  id: string
  productCode: string
  isActive: boolean
  creationDate: string
  title: string
  slug: string
}

export interface AdminProductDetail {
  id: string
  subCategoryId: string
  title: string
  slug: string
  description: string
  productCode: string
  price: number
  compareAtPrice: number | null
  isActive: boolean
  creationDate: string
}

export interface CreateCategoryInput {
  languageId: number
  title: string
  slug: string
  code: string
}

export interface UpdateCategoryInput extends CreateCategoryInput {
  id: string
  isActive: boolean
}

export interface CreateSubCategoryInput {
  languageId: number
  title: string
  slug: string
  code: string
  categoryId: string
}

export interface UpdateSubCategoryInput extends CreateSubCategoryInput {
  id: string
  isActive: boolean
}

export interface CreateProductInput {
  languageId: number
  title: string
  slug: string
  description: string
  productCode: string
  price: number
  compareAtPrice: number | null
  subCategoryId: string
}

export interface UpdateProductInput extends CreateProductInput {
  id: string
  status: boolean
}
