import type { Locale } from '@/models/shared/locale.model'
import type { ContactPageContent } from '@/models/contact/contactPage.model'
import { cmsContactPageSlug } from '@/constants/cmsSectionTypes'
import { applyContactPageFallback } from '@/data/mock/contactPageDefaults'
import { mapCmsPageToContactPage } from '@/services/mappers/cmsContactPageMapper'
import { pageService } from '@/services/pageService'

const cache = new Map<string, ContactPageContent>()
const requests = new Map<string, Promise<ContactPageContent | null>>()

export const contactPageService = {
  async getPage(locale: Locale, options: { force?: boolean } = {}): Promise<ContactPageContent | null> {
    const cacheKey = `${locale}:contact`
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
      const cmsPage = await pageService.getBySlug(cmsContactPageSlug(locale), locale)
      if (!cmsPage) return null
      return applyContactPageFallback(mapCmsPageToContactPage(cmsPage), locale)
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
