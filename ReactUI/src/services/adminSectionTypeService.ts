import type {
  AdminCmsSectionType,
  CreateCmsSectionTypeInput,
  UpdateCmsSectionTypeInput,
} from '@/models/admin/cms.model'
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
import { normalizeSectionTypeTranslations, pickSectionTypeName } from '@/services/admin/adminCmsNormalize'

const BASE = '/api/v1/admin/section-type'

function normalizeSectionType(data: unknown, languageId?: number): AdminCmsSectionType | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  if (!id) return null

  const translations = normalizeSectionTypeTranslations(record.translations ?? record.Translations)

  return {
    id,
    isActive: readIsActive(record),
    name: pickSectionTypeName(translations, languageId) || readStringField(record, 'name', 'Name'),
    translations,
  }
}

export const adminSectionTypeService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminCmsSectionType>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        name: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeSectionType(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCmsSectionType | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    const record = readRecord(data)
    if (!record) return null

    const idValue = readEncryptedId(record, 'id', 'Id')
    if (!idValue) return null

    return {
      id: idValue,
      isActive: readIsActive(record),
      name: readStringField(record, 'name', 'Name'),
      translations: [],
    }
  },

  async create(accessToken: string, locale: Locale, input: CreateCmsSectionTypeInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCmsSectionTypeInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },
}
