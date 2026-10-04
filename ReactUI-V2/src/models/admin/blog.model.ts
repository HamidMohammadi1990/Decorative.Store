import type { CatalogTranslation } from '@/models/admin/catalog.model'

export interface AdminBlogPostCategory {
  id: string
  code: string
  isActive: boolean
  title: string
  slug: string
  translations: CatalogTranslation[]
}

export interface CreateBlogPostCategoryInput {
  languageId: number
  code: string
  title: string
  slug: string
}

export interface UpdateBlogPostCategoryInput extends CreateBlogPostCategoryInput {
  id: string
  isActive: boolean
}

export interface AdminTag {
  id: string
  title: string
  isActive: boolean
}

export interface CreateTagInput {
  title: string
}

export interface UpdateTagInput {
  id: string
  title: string
  isActive: boolean
}

export interface AdminBlogPostListItem {
  id: string
  title: string
  slug: string
  categoryId: string
  categoryTitle: string
  metaDescription: string
  content: string
  readingTimeInMinutes: number
  authorName: string
  createdOnUtc: string
  updatedOnUtc: string | null
  publishedOnUtc: string | null
  isActive: boolean
  isPublished: boolean
}

export interface AdminBlogPostDetail {
  id: string
  code: string
  title: string
  slug: string
  categoryId: string
  metaDescription: string
  seoKeywords: string
  content: string
  readingTimeInMinutes: number
  createdOnUtc: string
  updatedOnUtc: string | null
  publishedOnUtc: string | null
  isActive: boolean
  isPublished: boolean
  isFeatured: boolean
}

export interface CreateBlogPostInput {
  languageId: number
  code: string
  categoryId: string
  title: string
  slug: string
  metaDescription: string
  seoKeywords: string
  content: string
  readingTimeInMinutes: number
  isFeatured: boolean
}

export interface UpdateBlogPostInput extends CreateBlogPostInput {
  id: string
}

export interface AdminBlogPostFile {
  id: string
  blogPostId: string
  blogPostTitle: string
  title: string
  fileName: string
  imageUrl: string
  isActive: boolean
  isMain: boolean
}

export interface CreateBlogPostFileInput {
  blogPostId: string
  languageId: number
  title: string
  image: File
  isIndex: boolean
}

export interface AdminBlogPostTag {
  id: string
  tagId: string
  tagTitle: string
  blogPostId: string
  blogPostTitle: string
}

export interface CreateBlogPostTagInput {
  tagId: string
  blogPostId: string
}

export interface UpdateBlogPostTagInput extends CreateBlogPostTagInput {
  id: string
}

export interface AdminBlogPostComment {
  id: string
  parentId: string | null
  content: string
  blogPostId: string
  blogPostTitle: string
  authorName: string
  createdOnUtc: string
  approvedOnUtc: string | null
  isApproved: boolean
}
