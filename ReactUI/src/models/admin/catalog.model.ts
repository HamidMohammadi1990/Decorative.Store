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
  subCategoryId: string
  translations: CatalogTranslation[]
}

export interface AdminProductDetail {
  id: string
  subCategoryId: string
  subCategoryTitle: string
  categoryTitle: string
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

export interface AdminProductFile {
  id: string
  productId: string
  productTitle: string
  title: string
  fileName: string
  imageUrl: string
  isActive: boolean
  isMain: boolean
}

export interface CreateProductFileInput {
  productId: string
  languageId: number
  title: string
  image: File
  isIndex: boolean
}

export interface AdminProductDescription {
  id: string
  productId: string
  languageId: number
  description: string
  productTitle: string
}

export interface CreateProductDescriptionInput {
  productId: string
  languageId: number
  description: string
}

export interface UpdateProductDescriptionInput extends CreateProductDescriptionInput {
  id: string
}

export interface AdminProductComment {
  id: string
  productId: string
  productTitle: string
  userId: string
  authorName: string
  commentTopicId: string
  commentTopicTitle: string
  commentRate: number
  qualityRating: number
  affordableRating: number
  description: string
  isActive: boolean
}

export interface AdminProductQuestion {
  id: string
  productId: string
  productTitle: string
  userId: string
  askerName: string
  question: string
  answer?: string | null
  createdOnUtc?: string
  isActive: boolean
  answeredByName?: string | null
}
