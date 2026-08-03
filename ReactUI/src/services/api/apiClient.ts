import { API_BASE_URL } from '@/config/api'
import type { Locale } from '@/models/shared/locale.model'
import { ApiError } from '@/services/api/apiTypes'
import { normalizeApiEnvelope } from '@/services/api/apiNormalize'

export function toAcceptLanguage(locale: Locale): string {
  return locale === 'fa' ? 'fa-IR' : 'en-US'
}

interface RequestOptions {
  locale?: Locale
  accessToken?: string | null
  body?: unknown
  method?: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE'
}

async function requestApi<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { locale, accessToken, body, method = 'GET' } = options

  const headers: Record<string, string> = {
    Accept: 'application/json',
  }

  if (locale) {
    headers['Accept-Language'] = toAcceptLanguage(locale)
  }

  if (accessToken) {
    headers.Authorization = `Bearer ${accessToken}`
  }

  if (body !== undefined) {
    headers['Content-Type'] = 'application/json'
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    method,
    headers,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  })

  const responseBody = await response.json()
  const { isSuccess, data, messages } = normalizeApiEnvelope<T>(responseBody)

  if (!response.ok || !isSuccess || data === undefined) {
    throw new ApiError(response.status, messages)
  }

  return data
}

export async function apiGet<T>(
  path: string,
  locale: Locale,
  accessToken?: string | null,
): Promise<T> {
  return requestApi<T>(path, { locale, accessToken, method: 'GET' })
}

export async function apiGetAuth<T>(path: string, accessToken: string): Promise<T> {
  return requestApi<T>(path, { accessToken, method: 'GET' })
}

export async function apiPost<T>(
  path: string,
  body: unknown,
  options: { locale?: Locale; accessToken?: string | null } = {},
): Promise<T> {
  return requestApi<T>(path, {
    locale: options.locale,
    accessToken: options.accessToken,
    body,
    method: 'POST',
  })
}

export async function apiDelete<T>(path: string, accessToken: string, body?: unknown): Promise<T> {
  return requestApi<T>(path, { accessToken, body, method: 'DELETE' })
}
