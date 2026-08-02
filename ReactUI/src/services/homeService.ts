import type { HomePage } from '@/models/home/homePage.model'
import type { Locale } from '@/models/shared/locale.model'
import { getHomeMock, getNavigationMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { categoryService } from '@/services/categoryService'
import { mapCategoryTreeToNav } from '@/services/mappers/categoryTreeMapper'
import { mapHomePage } from '@/services/mappers/homeMapper'

const STATIC_NAV_IDS = new Set(['blog'])

const pageCache = new Map<Locale, HomePage>()
const pageRequests = new Map<Locale, Promise<HomePage>>()

function getStaticPrimaryNav(locale: Locale) {
  return getNavigationMock(locale).primaryNav.filter((item) => STATIC_NAV_IDS.has(item.id))
}

async function buildPage(locale: Locale): Promise<HomePage> {
  const page = mapHomePage(getHomeMock(locale))
  const staticNav = getStaticPrimaryNav(locale)

  try {
    const categoryTree = await categoryService.getTree(locale)
    const catalogNav = mapCategoryTreeToNav(categoryTree)

    return {
      ...page,
      header: {
        ...page.header,
        primaryNav: [...staticNav, ...catalogNav],
      },
    }
  } catch {
    return mockFetch(async () => ({
      ...page,
      header: {
        ...page.header,
        primaryNav: getNavigationMock(locale).primaryNav,
      },
    }))
  }
}

export const homeService = {
  async getPage(locale: Locale, force = false): Promise<HomePage> {
    if (!force) {
      const cached = pageCache.get(locale)
      if (cached) return cached

      const inFlight = pageRequests.get(locale)
      if (inFlight) return inFlight
    } else {
      pageCache.delete(locale)
    }

    const request = buildPage(locale)
    pageRequests.set(locale, request)

    try {
      const page = await request
      pageCache.set(locale, page)
      return page
    } catch (error) {
      pageCache.delete(locale)
      throw error
    } finally {
      pageRequests.delete(locale)
    }
  },
}
