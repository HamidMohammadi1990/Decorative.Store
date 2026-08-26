import type { AdminCmsSection, CreateCmsSectionInput, UpdateCmsSectionInput } from '@/models/admin/cms.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { normalizeCmsTranslations, pickCmsTranslation } from '@/services/admin/adminCmsNormalize'

const BASE = '/api/v1/admin/section'

function normalizeSection(data: unknown, languageId?: number): AdminCmsSection | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const sectionTypeId = readEncryptedId(record, 'sectionTypeId', 'SectionTypeId')
  if (!id || !sectionTypeId) return null

  const translations = normalizeCmsTranslations(record.translations ?? record.Translations)
  const translation = pickCmsTranslation(translations, languageId)

  return {
    id,
    sectionTypeId,
    parentId: readOptionalEncryptedId(record, 'parentId', 'ParentId'),
    imageUrl: readStringField(record, 'imageUrl', 'ImageUrl') || undefined,
    startDateOnUtc: readStringField(record, 'startDateOnUtc', 'StartDateOnUtc') || null,
    endDateOnUtc: readStringField(record, 'endDateOnUtc', 'EndDateOnUtc') || null,
    isActive: readIsActive(record),
    title: translation?.title ?? '',
    description: translation?.description,
    url: translation?.url ?? '',
    translations,
  }
}

export const adminSectionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      sectionTypeId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminCmsSection>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        sectionTypeId: options.sectionTypeId ?? null,
        parentId: null,
        title: null,
        url: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeSection(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCmsSection | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    const record = readRecord(data)
    if (!record) return null

    const idValue = readEncryptedId(record, 'id', 'Id')
    const sectionTypeId = readEncryptedId(record, 'sectionTypeId', 'SectionTypeId')
    if (!idValue || !sectionTypeId) return null

    return {
      id: idValue,
      sectionTypeId,
      parentId: readOptionalEncryptedId(record, 'parentId', 'ParentId'),
      imageUrl: readStringField(record, 'imageUrl', 'ImageUrl') || undefined,
      startDateOnUtc: readStringField(record, 'startDateOnUtc', 'StartDateOnUtc') || null,
      endDateOnUtc: readStringField(record, 'endDateOnUtc', 'EndDateOnUtc') || null,
      isActive: readIsActive(record),
      title: readStringField(record, 'title', 'Title'),
      description: readStringField(record, 'description', 'Description') || undefined,
      url: readStringField(record, 'url', 'Url'),
      translations: [],
    }
  },

  async create(accessToken: string, locale: Locale, input: CreateCmsSectionInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCmsSectionInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },
}
