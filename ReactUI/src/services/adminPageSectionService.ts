import type {
  AdminCmsPageSection,
  CreateCmsPageSectionInput,
  UpdateCmsPageSectionInput,
} from '@/models/admin/cms.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost, apiPut } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { readAdminDescription } from '@/services/admin/adminCmsNormalize'

const BASE = '/api/v1/admin/page-section'

function normalizePageSection(data: unknown): AdminCmsPageSection | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const pageId = readEncryptedId(record, 'pageId', 'PageId')
  const sectionId = readEncryptedId(record, 'sectionId', 'SectionId')
  if (!id || !pageId || !sectionId) return null

  return {
    id,
    pageId,
    sectionId,
    priority: readNumberField(record, 'priority', 'Priority'),
    pageTitle: readStringField(record, 'pageTitle', 'PageTitle') || undefined,
    pageSlug: readStringField(record, 'pageSlug', 'PageSlug') || undefined,
    sectionTitle: readStringField(record, 'sectionTitle', 'SectionTitle') || undefined,
    sectionTypeName: readStringField(record, 'sectionTypeName', 'SectionTypeName') || undefined,
    adminDescription: readAdminDescription(record),
  }
}

export const adminPageSectionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      pageId?: string | null
      sectionId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminCmsPageSection>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        pageId: options.pageId ?? null,
        sectionId: options.sectionId ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizePageSection(item))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCmsPageSection | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizePageSection(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateCmsPageSectionInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCmsPageSectionInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },
}
