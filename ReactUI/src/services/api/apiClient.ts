import { API_BASE_URL } from '@/config/api'
import type { Locale } from '@/models/shared/locale.model'

interface ApiResult<T> {
  isSuccess: boolean
  statusCode: number
  data?: T
  messages?: Array<{ code: string; message: string }>
}

export function toAcceptLanguage(locale: Locale): string {
  return locale === 'fa' ? 'fa-IR' : 'en-US'
}

export async function apiGet<T>(path: string, locale: Locale): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      Accept: 'application/json',
      'Accept-Language': toAcceptLanguage(locale),
    },
  })

  if (!response.ok) {
    throw new Error(`API request failed with status ${response.status}`)
  }

  const body = (await response.json()) as ApiResult<T>

  if (!body.isSuccess || body.data === undefined) {
    throw new Error('API request returned an unsuccessful result')
  }

  return body.data
}
