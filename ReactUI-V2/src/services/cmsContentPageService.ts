import type { Locale } from '@/models/shared/locale.model'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import { applyContentPageFallback, getContentPageMock } from '@/data/mock/contentPageDefaults'
import { mapCmsPageToContentPage } from '@/services/mappers/cmsContentPageMapper'
import { pageService } from '@/services/pageService'

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
      const inFlight = requests.get(cacheKey)
      if (inFlight) return inFlight
    }

    const request = (async () => {
      const cmsPage = await pageService.getBySlug(slug, locale, force)
      if (!cmsPage) return getContentPageMock(slug, locale)
      return applyContentPageFallback(mapCmsPageToContentPage(cmsPage), slug, locale)
    })()

    requests.set(cacheKey, request)

    try {
      return await request
    } finally {
      requests.delete(cacheKey)
    }
  },
}
