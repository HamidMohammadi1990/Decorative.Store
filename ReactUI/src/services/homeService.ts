import type { HomePage } from '@/models/home/homePage.model'
import type { Locale } from '@/models/shared/locale.model'
import { getHomeMock, getNavigationMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { catalogProductService } from '@/services/catalogProductService'
import { categoryService } from '@/services/categoryService'
import { mapCategoryTreeToCategoryNav, mapCategoryTreeToNav } from '@/services/mappers/categoryTreeMapper'
import { buildFeaturedShopGrid } from '@/services/mappers/featuredShopMapper'
import { buildHeroCarouselFromProducts } from '@/services/mappers/heroCarouselMapper'
import { mapHomePage } from '@/services/mappers/homeMapper'

const STATIC_NAV_IDS = new Set(['blog'])

const pageCache = new Map<string, HomePage>()
const pageRequests = new Map<string, Promise<HomePage>>()

function pageCacheKey(locale: Locale, skipCatalogNav: boolean, skipHomeCatalogContent: boolean) {
  return `${locale}:${skipCatalogNav ? 'blog' : 'shop'}:${skipHomeCatalogContent ? 'header' : 'full'}`
}

function getStaticPrimaryNav(locale: Locale) {
  return getNavigationMock(locale).primaryNav.filter((item) => STATIC_NAV_IDS.has(item.id))
}

async function loadHeroCarousel(locale: Locale) {
  try {
    const products = await catalogProductService.getNewestProducts(locale, 4)
    if (products.length > 0) {
      return buildHeroCarouselFromProducts(products, locale)
    }
  } catch {
    // keep mock hero from home json
  }
  return undefined
}

async function loadFeaturedShop(locale: Locale, fallback: HomePage['featuredShop']) {
  try {
    const collections = await catalogProductService.getFeaturedCollections(locale)
    if (collections.length > 0) {
      return buildFeaturedShopGrid(collections, fallback)
    }
  } catch {
    // keep mock featured shop
  }
  return fallback
}

async function buildPage(
  locale: Locale,
  skipCatalogNav: boolean,
  skipHomeCatalogContent: boolean,
): Promise<HomePage> {
  const page = mapHomePage(getHomeMock(locale))
  const staticNav = getStaticPrimaryNav(locale)

  let pageWithContent = page
  if (!skipHomeCatalogContent) {
    const [hero, featuredShop] = await Promise.all([
      loadHeroCarousel(locale),
      loadFeaturedShop(locale, page.featuredShop),
    ])
    pageWithContent = {
      ...page,
      ...(hero ? { hero } : {}),
      featuredShop,
    }
  }

  if (skipCatalogNav) {
    return {
      ...pageWithContent,
      header: {
        ...pageWithContent.header,
        primaryNav: staticNav,
      },
    }
  }

  try {
    const categoryTree = await categoryService.getTree(locale)
    const catalogNav = mapCategoryTreeToNav(categoryTree)
    const dynamicCategoryNav = mapCategoryTreeToCategoryNav(categoryTree)

    return {
      ...pageWithContent,
      categoryNav: dynamicCategoryNav.items.length > 0 ? dynamicCategoryNav : pageWithContent.categoryNav,
      header: {
        ...pageWithContent.header,
        primaryNav: [...staticNav, ...catalogNav],
      },
    }
  } catch {
    return mockFetch(async () => ({
      ...pageWithContent,
      header: {
        ...pageWithContent.header,
        primaryNav: getNavigationMock(locale).primaryNav,
      },
    }))
  }
}

export const homeService = {
  async getPage(
    locale: Locale,
    options: { force?: boolean; skipCatalogNav?: boolean; skipHomeCatalogContent?: boolean } = {},
  ): Promise<HomePage> {
    const { force = false, skipCatalogNav = false, skipHomeCatalogContent = false } = options
    const cacheKey = pageCacheKey(locale, skipCatalogNav, skipHomeCatalogContent)

    if (!force) {
      const cached = pageCache.get(cacheKey)
      if (cached) return cached

      const inFlight = pageRequests.get(cacheKey)
      if (inFlight) return inFlight
    } else {
      pageCache.delete(cacheKey)
    }

    const request = buildPage(locale, skipCatalogNav, skipHomeCatalogContent)
    pageRequests.set(cacheKey, request)

    try {
      const page = await request
      pageCache.set(cacheKey, page)
      return page
    } catch (error) {
      pageCache.delete(cacheKey)
      throw error
    } finally {
      pageRequests.delete(cacheKey)
    }
  },
}
