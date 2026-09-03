import { isBlogRoute } from '@/extensions/blogRoute'
import { isAboutRoute } from '@/extensions/aboutRoute'
import { isContactRoute } from '@/extensions/contactRoute'
import { resolveCmsContentSlug } from '@/extensions/cmsContentRoute'
import { isCmsPageRoute } from '@/extensions/cmsPageRoute'
import { isHomeRoute } from '@/extensions/homeRoute'

export interface HomePageFetchOptions {
  skipCatalogNav: boolean
  skipHomeCatalogContent: boolean
  skipCmsPage: boolean
  cmsSlug: string
}

export function resolveHomePageFetchOptions(pathname: string): HomePageFetchOptions {
  const contentSlug = resolveCmsContentSlug(pathname)

  const skipCatalogNav = isBlogRoute(pathname)
  const skipHomeCatalogContent = !isHomeRoute(pathname)
  const skipCmsPage = !isCmsPageRoute(pathname)
  const cmsSlug = isAboutRoute(pathname)
    ? 'about'
    : isContactRoute(pathname)
      ? 'contact'
      : contentSlug ?? (isBlogRoute(pathname) ? 'blog' : 'shop')

  return { skipCatalogNav, skipHomeCatalogContent, skipCmsPage, cmsSlug }
}

export function buildHomePageFetchKey(options: HomePageFetchOptions): string {
  return [
    options.skipCatalogNav ? '1' : '0',
    options.skipHomeCatalogContent ? '1' : '0',
    options.skipCmsPage ? '1' : '0',
    options.cmsSlug,
  ].join(':')
}
