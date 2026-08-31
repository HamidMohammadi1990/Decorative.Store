import type { AdminCmsPage, CreateCmsPageInput, UpdateCmsPageInput } from '@/models/admin/cms.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { normalizeCmsTranslations, pickCmsTranslation, readPageType } from '@/services/admin/adminCmsNormalize'

const BASE = '/api/v1/admin/page'

function normalizePage(data: unknown, languageId?: number): AdminCmsPage | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  if (!id) return null

  const translations = normalizeCmsTranslations(record.translations ?? record.Translations)
  const translation = pickCmsTranslation(translations, languageId)

  return {
    id,
    type: readPageType(record.type ?? record.Type),
    isActive: readIsActive(record),
    title: translation?.title ?? '',
    slug: translation?.slug ?? '',
    metaTitle: translation?.metaTitle,
    metaDescription: translation?.metaDescription,
    translations,
  }
}

export const adminPageService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminCmsPage>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        slug: null,
        type: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizePage(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCmsPage | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    const record = readRecord(data)
    if (!record) return null

    const idValue = readEncryptedId(record, 'id', 'Id')
    if (!idValue) return null

    return {
      id: idValue,
      type: readPageType(record.type ?? record.Type),
      isActive: readIsActive(record),
      title: readStringField(record, 'title', 'Title'),
      slug: readStringField(record, 'slug', 'Slug'),
      metaTitle: readStringField(record, 'metaTitle', 'MetaTitle') || undefined,
      metaDescription: readStringField(record, 'metaDescription', 'MetaDescription') || undefined,
      translations: [],
    }
  },

  async create(accessToken: string, locale: Locale, input: CreateCmsPageInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCmsPageInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },
}
