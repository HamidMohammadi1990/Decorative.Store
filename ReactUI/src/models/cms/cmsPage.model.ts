export interface CmsSectionItem {
  title: string
  priority: number
  icon?: string | null
  imageUrl?: string | null
  url?: string | null
  description?: string | null
}

export interface CmsPageSection {
  sectionTypeId: number
  sectionTypeName: string
  priority: number
  title: string
  description?: string | null
  url: string
  imageUrl?: string | null
  items: CmsSectionItem[]
}

export interface CmsPage {
  slug: string
  title: string
  type: number
  metaTitle?: string | null
  metaDescription?: string | null
  sections: CmsPageSection[]
}
