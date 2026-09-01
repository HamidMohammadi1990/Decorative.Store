import type { Locale } from '@/models/shared/locale.model'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import { applyContentPageFallback } from '@/data/mock/contentPageDefaults'
import { mapCmsPageToContentPage } from '@/services/mappers/cmsContentPageMapper'
import { pageService } from '@/services/pageService'

const cache = new Map<string, CmsContentPageContent>()
const requests = new Map<string, Promise<CmsContentPageContent | null>>()

export const cmsContentPageService = {
  async getPage(
    slug: string,
    locale: Locale,
    options: { force?: boolean } = {},
  ): Promise<CmsContentPageContent | null> {
    const cacheKey = `${locale}:${slug}`
    const { force = false } = options

    if (!force) {
      const cached = cache.get(cacheKey)
      if (cached) return cached

      const inFlight = requests.get(cacheKey)
      if (inFlight) return inFlight
    } else {
      cache.delete(cacheKey)
    }

    const request = (async () => {
      const cmsPage = await pageService.getBySlug(slug, locale)
      if (!cmsPage) return null
      return applyContentPageFallback(mapCmsPageToContentPage(cmsPage), slug, locale)
    })()

    requests.set(cacheKey, request)

    try {
      const page = await request
      if (page) cache.set(cacheKey, page)
      return page
    } finally {
      requests.delete(cacheKey)
    }
  },
}
