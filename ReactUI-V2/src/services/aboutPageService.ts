import type { Locale } from '@/models/shared/locale.model'
import type { AboutPageContent } from '@/models/about/aboutPage.model'
import { cmsAboutPageSlug } from '@/constants/cmsSectionTypes'
import { applyAboutPageFallback } from '@/data/mock/aboutPageDefaults'
import { mapCmsPageToAboutPage } from '@/services/mappers/cmsAboutPageMapper'
import { pageService } from '@/services/pageService'

const requests = new Map<string, Promise<AboutPageContent | null>>()

function cacheKey(locale: Locale) {
  return `${locale}:about`
}

export const aboutPageService = {
  async getPage(locale: Locale, options: { force?: boolean } = {}): Promise<AboutPageContent | null> {
    const key = cacheKey(locale)
    const { force = false } = options

    if (!force) {
      const inFlight = requests.get(key)
      if (inFlight) return inFlight
    }

    const request = (async () => {
      const cmsPage = await pageService.getBySlug(cmsAboutPageSlug(locale), locale, force)
      if (!cmsPage) return null
      return applyAboutPageFallback(mapCmsPageToAboutPage(cmsPage), locale)
    })()

    requests.set(key, request)

    try {
      return await request
    } finally {
      requests.delete(key)
    }
  },
}
