import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeAdminPaged } from '@/services/admin/adminCatalogNormalize'

const PUBLIC_BASE = '/api/v1/newsletter'
const ADMIN_BASE = '/api/v1/admin/marketing-promo'

export interface AdminNewsletterSubscriber {
  id: number
  email: string
  languageId: number
  subscribedAtUtc: string
  isActive: boolean
}

function normalizeSubscriber(data: unknown): AdminNewsletterSubscriber | null {
  if (!data || typeof data !== 'object') return null
  const record = data as Record<string, unknown>
  const email = typeof record.email === 'string' ? record.email : typeof record.Email === 'string' ? record.Email : ''
  if (!email) return null

  return {
    id: Number(record.id ?? record.Id ?? 0),
    email,
    languageId: Number(record.languageId ?? record.LanguageId ?? 0),
    subscribedAtUtc: String(record.subscribedAtUtc ?? record.SubscribedAtUtc ?? ''),
    isActive: Boolean(record.isActive ?? record.IsActive ?? true),
  }
}

export const newsletterService = {
  async subscribe(locale: Locale, languageId: number, email: string): Promise<void> {
    await apiPost(`${PUBLIC_BASE}/subscribe`, { languageId, email: email.trim() }, { locale })
  },
}

export const adminNewsletterService = {
  async getSubscribers(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      email?: string | null
    } = {},
  ) {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/newsletter-subscribers`,
      {
        languageId: options.languageId ?? null,
        email: options.email ?? null,
        pagination: {
          pageNumber: options.pageNumber ?? 1,
          pageSize: options.pageSize ?? 20,
        },
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeSubscriber)
  },
}
