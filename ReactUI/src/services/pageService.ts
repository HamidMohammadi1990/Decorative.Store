import type { Locale } from '@/models/shared/locale.model'
import type { CmsPage } from '@/models/cms/cmsPage.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeCmsPage } from '@/services/mappers/cmsPageMapper'

const PAGE_GET_BY_SLUG_PATH = '/api/v1/page/get-by-slug'

const pageCache = new Map<string, CmsPage>()
const pageRequests = new Map<string, Promise<CmsPage | null>>()

function cacheKey(locale: Locale, slug: string) {
  return `${locale}:${slug}`
}

export const pageService = {
  async getBySlug(slug: string, locale: Locale, force = false): Promise<CmsPage | null> {
    const key = cacheKey(locale, slug)

    if (!force) {
      const cached = pageCache.get(key)
      if (cached) return cached

      const inFlight = pageRequests.get(key)
      if (inFlight) return inFlight
    } else {
      pageCache.delete(key)
    }

    const request = apiPost<unknown>(PAGE_GET_BY_SLUG_PATH, { slug }, { locale })
      .then((data) => normalizeCmsPage(data))
      .catch(() => null)

    pageRequests.set(key, request)

    try {
      const result = await request
      if (result) {
        pageCache.set(key, result)
      }
      return result
    } finally {
      pageRequests.delete(key)
    }
  },
}
