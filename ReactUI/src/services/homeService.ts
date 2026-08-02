import type { HomePage } from '@/models/home/homePage.model'
import type { Locale } from '@/models/shared/locale.model'
import { getHomeMock, getNavigationMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { categoryService } from '@/services/categoryService'
import { mapCategoryTreeToNav } from '@/services/mappers/categoryTreeMapper'
import { mapHomePage } from '@/services/mappers/homeMapper'

const STATIC_NAV_IDS = new Set(['blog'])

function getStaticPrimaryNav(locale: Locale) {
  return getNavigationMock(locale).primaryNav.filter((item) => STATIC_NAV_IDS.has(item.id))
}

export const homeService = {
  async getPage(locale: Locale): Promise<HomePage> {
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
  },
}
