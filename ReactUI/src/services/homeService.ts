import type { HomePage } from '@/models/home/homePage.model'
import type { Locale } from '@/models/shared/locale.model'
import { cmsBlogPageSlug, cmsShopPageSlug } from '@/constants/cmsSectionTypes'
import { getHomeMock, getNavigationMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'
import { catalogProductService } from '@/services/catalogProductService'
import { categoryService } from '@/services/categoryService'
import { mapCmsPageToHomePage } from '@/services/mappers/cmsPageMapper'
import { mapCategoryTreeToCategoryNav, mapCategoryTreeToNav } from '@/services/mappers/categoryTreeMapper'
import { buildFeaturedShopGrid } from '@/services/mappers/featuredShopMapper'
import { buildHeroCarouselFromProducts } from '@/services/mappers/heroCarouselMapper'
import { mapHomePage } from '@/services/mappers/homeMapper'
import { pageService } from '@/services/pageService'

const STATIC_NAV_IDS = new Set(['blog'])

const pageCache = new Map<string, HomePage>()
const pageRequests = new Map<string, Promise<HomePage>>()

function pageCacheKey(
  locale: Locale,
  skipCatalogNav: boolean,
  skipHomeCatalogContent: boolean,
  skipCmsPage: boolean,
) {
  return `${locale}:${skipCatalogNav ? 'blog' : 'shop'}:${skipHomeCatalogContent ? 'header' : 'full'}:${skipCmsPage ? 'mock' : 'cms'}`
}

function getStaticPrimaryNav(locale: Locale) {
  return getNavigationMock(locale).primaryNav.filter((item) => STATIC_NAV_IDS.has(item.id))
}

function mergeHomePage(base: HomePage, override: Partial<HomePage>): HomePage {
  return {
    ...base,
    ...override,
    header: override.header ? { ...base.header, ...override.header } : base.header,
    footer: override.footer ? { ...base.footer, ...override.footer } : base.footer,
  }
}

/** CMS overrides mock; empty CMS slices keep mock fallbacks so the shop still renders. */
function applyCmsOnMock(mock: HomePage, cms: HomePage): HomePage {
  let page = mergeHomePage(mock, cms)

  if (!cms.hero?.slides?.length) page.hero = mock.hero
  if (!cms.promoTiles?.tiles?.length) page.promoTiles = mock.promoTiles
  if (!cms.categoryNav?.items?.length) page.categoryNav = mock.categoryNav
  if (!cms.featuredShop?.sections?.length) page.featuredShop = mock.featuredShop
  if (!cms.designServices?.title?.trim()) page.designServices = mock.designServices

  if (!cms.promoAnnouncement?.message?.trim()) {
    page.promoAnnouncement = mock.promoAnnouncement
  }

  const utilityLinks = cms.utilityBar?.links ?? []
  if (!cms.utilityBar?.phoneLabel?.trim() && utilityLinks.length === 0) {
    page.utilityBar = mock.utilityBar
  }

  if (!cms.header?.brandLabel?.trim()) {
    page.header = {
      ...mock.header,
      ...page.header,
      brandLabel: mock.header.brandLabel,
      searchPlaceholder: page.header.searchPlaceholder || mock.header.searchPlaceholder,
      accountLabel: page.header.accountLabel || mock.header.accountLabel,
      cartLabel: page.header.cartLabel || mock.header.cartLabel,
      primaryNav:
        page.header.primaryNav.length > 0 ? page.header.primaryNav : mock.header.primaryNav,
    }
  }

  const footerColumns = cms.footer?.columns ?? []
  if (footerColumns.length === 0 && !cms.footer?.newsletterTitle?.trim()) {
    page.footer = mock.footer
  }

  return page
}

async function loadCmsPage(
  locale: Locale,
  includeHomeContent: boolean,
  useBlogSlug: boolean,
): Promise<HomePage | null> {
  try {
    const slug = useBlogSlug ? cmsBlogPageSlug(locale) : cmsShopPageSlug(locale)
    const cmsPage = await pageService.getBySlug(slug, locale)
    if (!cmsPage) return null
    return mapCmsPageToHomePage(cmsPage, { includeHomeContent })
  } catch {
    return null
  }
}

async function loadHeroCarousel(locale: Locale) {
  try {
    const products = await catalogProductService.getNewestProducts(locale, 4)
    if (products.length > 0) {
      return buildHeroCarouselFromProducts(products, locale)
    }
  } catch {
    // keep CMS hero
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
    // keep CMS featured shop
  }
  return fallback
}

async function buildPage(
  locale: Locale,
  skipCatalogNav: boolean,
  skipHomeCatalogContent: boolean,
  skipCmsPage: boolean,
): Promise<HomePage> {
  const mockPage = mapHomePage(getHomeMock(locale))

  try {
    let page = mockPage
    if (!skipCmsPage) {
      const cmsPage = await loadCmsPage(locale, !skipHomeCatalogContent, skipCatalogNav)
      if (cmsPage) {
        page = applyCmsOnMock(mockPage, cmsPage)
      }
    }

    const staticNav = getStaticPrimaryNav(locale)

    let pageWithContent = page
    if (!skipHomeCatalogContent) {
      const [hero, featuredShop] = await Promise.all([
        loadHeroCarousel(locale),
        loadFeaturedShop(locale, page.featuredShop),
      ])
      pageWithContent = mergeHomePage(page, {
        ...(hero ? { hero } : {}),
        featuredShop,
      })
    }

    if (skipCatalogNav) {
      return mergeHomePage(pageWithContent, {
        header: {
          primaryNav: staticNav,
        },
      })
    }

    try {
      const categoryTree = await categoryService.getTree(locale)
      const catalogNav = mapCategoryTreeToNav(categoryTree)
      const dynamicCategoryNav = mapCategoryTreeToCategoryNav(categoryTree)

      return mergeHomePage(pageWithContent, {
        categoryNav:
          dynamicCategoryNav.items.length > 0 ? dynamicCategoryNav : pageWithContent.categoryNav,
        header: {
          primaryNav: [...staticNav, ...catalogNav],
        },
      })
    } catch {
      return mockFetch(async () =>
        mergeHomePage(pageWithContent, {
          header: {
            primaryNav: getNavigationMock(locale).primaryNav,
          },
        }),
      )
    }
  } catch {
    return mockPage
  }
}

export const homeService = {
  async getPage(
    locale: Locale,
    options: {
      force?: boolean
      skipCatalogNav?: boolean
      skipHomeCatalogContent?: boolean
      skipCmsPage?: boolean
    } = {},
  ): Promise<HomePage> {
    const {
      force = false,
      skipCatalogNav = false,
      skipHomeCatalogContent = false,
      skipCmsPage = false,
    } = options
    const cacheKey = pageCacheKey(locale, skipCatalogNav, skipHomeCatalogContent, skipCmsPage)

    if (!force) {
      const cached = pageCache.get(cacheKey)
      if (cached) return cached

      const inFlight = pageRequests.get(cacheKey)
      if (inFlight) return inFlight
    } else {
      pageCache.delete(cacheKey)
    }

    const request = buildPage(locale, skipCatalogNav, skipHomeCatalogContent, skipCmsPage)
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
