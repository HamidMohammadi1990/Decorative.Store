import type {
  AdminBlogPostDetail,
  AdminBlogPostListItem,
  CreateBlogPostInput,
  UpdateBlogPostInput,
} from '@/models/admin/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost, apiPut } from '@/services/api/apiClient'
import {
  readBooleanField,
  readNumberField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/blog-post'

function readAuthorName(record: Record<string, unknown>): string {
  const first = readStringField(record, 'userFirstName', 'UserFirstName')
  const last = readStringField(record, 'userLastName', 'UserLastName')
  return [first, last].filter(Boolean).join(' ').trim()
}

function normalizeBlogPostListItem(data: unknown): AdminBlogPostListItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const title = readStringField(record, 'title', 'Title')
  const slug = readStringField(record, 'slug', 'Slug')
  const categoryId =
    readOptionalEncryptedId(record, 'categoryId', 'CategoryId') ??
    readOptionalEncryptedId(record, 'blogPostCategoryId', 'BlogPostCategoryId') ??
    ''
  if (!id || !title) return null

  return {
    id,
    title,
    slug,
    categoryId,
    categoryTitle: readStringField(record, 'categoryTitle', 'CategoryTitle'),
    metaDescription: readStringField(record, 'metaDescription', 'MetaDescription'),
    content: readStringField(record, 'content', 'Content'),
    readingTimeInMinutes: readNumberField(record, 'readingTimeInMinutes', 'ReadingTimeInMinutes'),
    authorName: readAuthorName(record),
    createdOnUtc: readStringField(record, 'createdOnUtc', 'CreatedOnUtc'),
    updatedOnUtc: readStringField(record, 'updatedOnUtc', 'UpdatedOnUtc') || null,
    publishedOnUtc: readStringField(record, 'publishedOnUtc', 'PublishedOnUtc') || null,
    isActive: readIsActive(record),
    isPublished: readBooleanField(record, 'isPublished', 'IsPublished'),
  }
}

function normalizeBlogPostDetail(data: unknown): AdminBlogPostDetail | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const title = readStringField(record, 'title', 'Title')
  const categoryId =
    readOptionalEncryptedId(record, 'blogPostCategoryId', 'BlogPostCategoryId') ??
    readOptionalEncryptedId(record, 'categoryId', 'CategoryId') ??
    ''
  if (!id || !code || !title) return null

  return {
    id,
    code,
    title,
    slug: readStringField(record, 'slug', 'Slug'),
    categoryId,
    metaDescription: readStringField(record, 'metaDescription', 'MetaDescription'),
    seoKeywords: readStringField(record, 'seoKeywords', 'SeoKeywords'),
    content: readStringField(record, 'content', 'Content'),
    readingTimeInMinutes: readNumberField(record, 'readingTimeInMinutes', 'ReadingTimeInMinutes'),
    createdOnUtc: readStringField(record, 'createdOnUtc', 'CreatedOnUtc'),
    updatedOnUtc: readStringField(record, 'updatedOnUtc', 'UpdatedOnUtc') || null,
    publishedOnUtc: readStringField(record, 'publishedOnUtc', 'PublishedOnUtc') || null,
    isActive: readIsActive(record),
    isPublished: readBooleanField(record, 'isPublished', 'IsPublished'),
    isFeatured: readBooleanField(record, 'isFeatured', 'IsFeatured'),
  }
}

export const adminBlogPostService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      categoryId?: string | null
      isPublished?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminBlogPostListItem>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        slug: null,
        categoryId: options.categoryId ?? null,
        isActive: null,
        isPublished: options.isPublished ?? null,
        userId: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeBlogPostListItem)
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminBlogPostDetail | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeBlogPostDetail(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateBlogPostInput): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        languageId: input.languageId,
        code: input.code,
        categoryId: input.categoryId,
        title: input.title,
        slug: input.slug,
        metaDescription: input.metaDescription,
        seoKeywords: input.seoKeywords,
        content: input.content,
        readingTimeInMinutes: input.readingTimeInMinutes,
        isFeatured: input.isFeatured,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateBlogPostInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        languageId: input.languageId,
        code: input.code,
        categoryId: input.categoryId,
        title: input.title,
        slug: input.slug,
        metaDescription: input.metaDescription,
        seoKeywords: input.seoKeywords,
        content: input.content,
        readingTimeInMinutes: input.readingTimeInMinutes,
        isFeatured: input.isFeatured,
      },
      { locale, accessToken },
    )
  },

  async publish(accessToken: string, locale: Locale, id: string): Promise<void> {
    await apiPost(`${BASE}/publish`, { id }, { locale, accessToken })
  },
}
