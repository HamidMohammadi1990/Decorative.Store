export interface CmsContentTranslation {
  languageId: number
  title: string
  slug?: string
  description?: string
  url?: string
  metaTitle?: string
  metaDescription?: string
}

export interface CmsSectionTypeTranslation {
  languageId: number
  name: string
}

export interface AdminCmsSectionType {
  id: string
  isActive: boolean
  name: string
  adminDescription?: string
  translations: CmsSectionTypeTranslation[]
}

export interface CreateCmsSectionTypeInput {
  languageId: number
  name: string
  isActive: boolean
  adminDescription?: string
}

export interface AdminCmsPageTypeGuide {
  type: number
  adminDescription: string
}

export interface UpdateCmsSectionTypeInput extends CreateCmsSectionTypeInput {
  id: string
}

export interface AdminCmsPage {
  id: string
  type: number
  isActive: boolean
  title: string
  slug: string
  metaTitle?: string
  metaDescription?: string
  adminDescription?: string
  translations: CmsContentTranslation[]
}

export interface CreateCmsPageInput {
  languageId: number
  title: string
  slug: string
  type: number
  isActive: boolean
  metaTitle?: string
  metaDescription?: string
  adminDescription?: string
}

export interface UpdateCmsPageInput extends CreateCmsPageInput {
  id: string
}

export interface AdminCmsSection {
  id: string
  sectionTypeId: string
  parentId: string | null
  imageUrl?: string
  startDateOnUtc?: string | null
  endDateOnUtc?: string | null
  isActive: boolean
  title: string
  description?: string
  url: string
  sectionTypeName?: string
  parentTitle?: string | null
  adminDescription?: string
  translations: CmsContentTranslation[]
}

export interface CreateCmsSectionInput {
  languageId: number
  sectionTypeId: string
  parentId?: string | null
  title: string
  description?: string
  url: string
  imageUrl?: string
  startDateOnUtc?: string | null
  endDateOnUtc?: string | null
  isActive: boolean
  adminDescription?: string
}

export interface UpdateCmsSectionInput extends CreateCmsSectionInput {
  id: string
}

export interface AdminCmsSectionItem {
  id: string
  sectionId: string
  priority: number
  icon?: string
  imageUrl?: string
  isActive: boolean
  title: string
  description?: string
  url?: string
  sectionTitle?: string
  sectionTypeName?: string
  adminDescription?: string
  translations: CmsContentTranslation[]
}

export interface CreateCmsSectionItemInput {
  languageId: number
  sectionId: string
  title: string
  priority: number
  icon?: string
  imageUrl?: string
  url?: string
  description?: string
  isActive: boolean
  adminDescription?: string
}

export interface UpdateCmsSectionItemInput extends CreateCmsSectionItemInput {
  id: string
}

export interface AdminCmsPageSection {
  id: string
  pageId: string
  sectionId: string
  priority: number
  pageTitle?: string
  pageSlug?: string
  sectionTitle?: string
  sectionTypeName?: string
  adminDescription?: string
}

export interface CreateCmsPageSectionInput {
  pageId: string
  sectionId: string
  priority: number
  adminDescription?: string
}

export interface UpdateCmsPageSectionInput extends CreateCmsPageSectionInput {
  id: string
}
