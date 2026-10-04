import type { Locale } from '@/models/shared/locale.model'
import type { AboutPageContent } from '@/models/about/aboutPage.model'
import { cmsAboutPageSlug } from '@/constants/cmsSectionTypes'
import { applyAboutPageFallback } from '@/data/mock/aboutPageDefaults'
import { mapCmsPageToAboutPage } from '@/services/mappers/cmsAboutPageMapper'
import { pageService } from '@/services/pageService'

const cache = new Map<string, AboutPageContent>()
const requests = new Map<string, Promise<AboutPageContent | null>>()

export const aboutPageService = {
  async getPage(locale: Locale, options: { force?: boolean } = {}): Promise<AboutPageContent | null> {
    const cacheKey = `${locale}:about`
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
      const cmsPage = await pageService.getBySlug(cmsAboutPageSlug(locale), locale)
      if (!cmsPage) return null
      return applyAboutPageFallback(mapCmsPageToAboutPage(cmsPage), locale)
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
