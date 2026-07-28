import type { HomePage } from '@/models/home/homePage.model'
import type { Locale } from '@/models/shared/locale.model'
import { getHomeMock, getNavigationMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { mapHomePage } from '@/services/mappers/homeMapper'

export const homeService = {
  async getPage(locale: Locale): Promise<HomePage> {
    return mockFetch(async () => {
      const page = mapHomePage(getHomeMock(locale))
      const navigation = getNavigationMock(locale)

      return {
        ...page,
        header: {
          ...page.header,
          primaryNav: navigation.primaryNav,
        },
      }
    })
  },
}
