import type {
  AdminBlogPostCategory,
  CreateBlogPostCategoryInput,
  UpdateBlogPostCategoryInput,
} from '@/models/admin/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  normalizeTranslations,
  paginationBody,
  pickTranslation,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/blog-post-category'

function normalizeBlogPostCategory(data: unknown, languageId?: number): AdminBlogPostCategory | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const translation = pickTranslation(translations, languageId)

  return {
    id,
    code,
    isActive: readIsActive(record),
    title: translation?.title ?? code,
    slug: translation?.slug ?? '',
    translations,
  }
}

export const adminBlogPostCategoryService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminBlogPostCategory>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        slug: null,
        code: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeBlogPostCategory(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminBlogPostCategory | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    const record = readRecord(data)
    if (!record) return null

    const idValue = readEncryptedId(record, 'id', 'Id')
    const code = readStringField(record, 'code', 'Code')
    if (!idValue || !code) return null

    return {
      id: idValue,
      code,
      isActive: readIsActive(record),
      title: readStringField(record, 'title', 'Title'),
      slug: readStringField(record, 'slug', 'Slug'),
      translations: [],
    }
  },

  async create(accessToken: string, locale: Locale, input: CreateBlogPostCategoryInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateBlogPostCategoryInput): Promise<void> {
    await apiPost(
      `${BASE}/update`,
      {
        id: input.id,
        languageId: input.languageId,
        code: input.code,
        title: input.title,
        slug: input.slug,
        isActive: input.isActive,
      },
      { locale, accessToken },
    )
  },
}
