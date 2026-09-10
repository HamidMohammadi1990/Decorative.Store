import type {
  AdminUser,
  AdminUserGender,
  CreateAdminUserInput,
  UpdateAdminUserInput,
} from '@/models/admin/user.model'
import type { Locale } from '@/models/shared/locale.model'
import { API_BASE_URL } from '@/config/api'
import { apiDelete, apiPost, apiPut, toAcceptLanguage } from '@/services/api/apiClient'
import { normalizeApiEnvelope, readBooleanField, readRecord, readStringField } from '@/services/api/apiNormalize'
import { ApiError } from '@/services/api/apiTypes'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/account'

function resolveUserProfileImageUrl(url: string): string {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) return url
  if (url.startsWith('/')) return `${API_BASE_URL}${url}`
  return `${API_BASE_URL}/${url}`
}

function readGender(record: Record<string, unknown>): AdminUserGender {
  const raw = record.gender ?? record.Gender
  if (typeof raw === 'number') return raw === 2 ? 2 : 1
  if (raw === 'Male') return 2
  return 1
}

function normalizeAdminUser(data: unknown): AdminUser | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const userName = readStringField(record, 'userName', 'UserName')
  if (!id || !userName) return null

  return {
    id,
    userName,
    firstName: readStringField(record, 'firstName', 'FirstName'),
    lastName: readStringField(record, 'lastName', 'LastName'),
    email: readStringField(record, 'email', 'Email'),
    phoneNumber: readStringField(record, 'phoneNumber', 'PhoneNumber'),
    gender: readGender(record),
    emailConfirmed: readBooleanField(record, 'emailConfirmed', 'EmailConfirmed'),
    phoneNumberConfirmed: readBooleanField(record, 'phoneNumberConfirmed', 'PhoneNumberConfirmed'),
    loginPermission: readBooleanField(record, 'loginPermission', 'LoginPermission'),
    isActive: readBooleanField(record, 'isActive', 'IsActive'),
    lastLoginDateOnUtc:
      readStringField(record, 'lastLoginDateOnUtc', 'LastLoginDateOnUtc') || null,
    profileImageFileName:
      readStringField(record, 'profileImageFileName', 'ProfileImageFileName') || undefined,
    profileImageUrl: resolveUserProfileImageUrl(
      readStringField(record, 'profileImageUrl', 'ProfileImageUrl'),
    ) || undefined,
  }
}

export const adminUserService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      userName?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminUser>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        userName: options.userName ?? null,
        firstName: null,
        lastName: null,
        email: null,
        emailConfirmed: null,
        phoneNumber: null,
        phoneNumberConfirmed: null,
        loginPermission: null,
        gender: null,
        isActive: options.isActive ?? null,
        refundMethod: null,
        cityId: null,
        economicCode: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 20),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminUser)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminUserInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateAdminUserInput): Promise<void> {
    const payload = {
      ...input,
      password: input.password?.trim() ? input.password : null,
    }
    await apiPut(`${BASE}/update`, payload, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },

  async uploadProfileImage(
    accessToken: string,
    locale: Locale,
    file: File,
  ): Promise<{ imageFileName: string; imageUrl: string }> {
    const formData = new FormData()
    formData.append('image', file)

    const response = await fetch(`${API_BASE_URL}${BASE}/upload-profile-image`, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Accept-Language': toAcceptLanguage(locale),
        Authorization: `Bearer ${accessToken}`,
      },
      body: formData,
    })

    const responseBody = await response.json()
    if (!response.ok) {
      throw new ApiError(response.status, normalizeApiEnvelope(responseBody).messages)
    }

    const envelope = normalizeApiEnvelope(responseBody)
    const record = readRecord(envelope.data)
    const imageFileName = readStringField(record ?? {}, 'imageFileName', 'ImageFileName')
    const imageUrl = readStringField(record ?? {}, 'imageUrl', 'ImageUrl')
    if (!imageFileName || !imageUrl) throw new ApiError(response.status, envelope.messages)

    return {
      imageFileName,
      imageUrl: resolveUserProfileImageUrl(imageUrl),
    }
  },
}
