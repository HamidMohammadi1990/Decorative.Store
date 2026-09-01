import type {
  AdminCmsSectionItem,
  CreateCmsSectionItemInput,
  UpdateCmsSectionItemInput,
} from '@/models/admin/cms.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost, apiPut } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { normalizeCmsTranslations, pickCmsTranslation } from '@/services/admin/adminCmsNormalize'

const BASE = '/api/v1/admin/section-item'

function normalizeSectionItem(data: unknown, languageId?: number): AdminCmsSectionItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const sectionId = readEncryptedId(record, 'sectionId', 'SectionId')
  if (!id || !sectionId) return null

  const translations = normalizeCmsTranslations(record.translations ?? record.Translations)
  const translation = pickCmsTranslation(translations, languageId)

  return {
    id,
    sectionId,
    priority: readNumberField(record, 'priority', 'Priority'),
    icon: readStringField(record, 'icon', 'Icon') || undefined,
    imageUrl: readStringField(record, 'imageUrl', 'ImageUrl') || undefined,
    isActive: readIsActive(record),
    title: translation?.title ?? '',
    description: translation?.description,
    url: translation?.url,
    sectionTitle: readStringField(record, 'sectionTitle', 'SectionTitle') || undefined,
    sectionTypeName: readStringField(record, 'sectionTypeName', 'SectionTypeName') || undefined,
    translations,
  }
}

export const adminSectionItemService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      sectionId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminCmsSectionItem>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        sectionId: options.sectionId ?? null,
        title: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeSectionItem(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCmsSectionItem | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    const record = readRecord(data)
    if (!record) return null

    const idValue = readEncryptedId(record, 'id', 'Id')
    const sectionId = readEncryptedId(record, 'sectionId', 'SectionId')
    if (!idValue || !sectionId) return null

    return {
      id: idValue,
      sectionId,
      priority: readNumberField(record, 'priority', 'Priority'),
      icon: readStringField(record, 'icon', 'Icon') || undefined,
      imageUrl: readStringField(record, 'imageUrl', 'ImageUrl') || undefined,
      isActive: readIsActive(record),
      title: readStringField(record, 'title', 'Title'),
      description: readStringField(record, 'description', 'Description') || undefined,
      url: readStringField(record, 'url', 'Url') || undefined,
      translations: [],
    }
  },

  async create(accessToken: string, locale: Locale, input: CreateCmsSectionItemInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCmsSectionItemInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },
}
