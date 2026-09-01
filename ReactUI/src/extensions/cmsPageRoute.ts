import { isBlogRoute } from '@/extensions/blogRoute'
import { isAboutRoute } from '@/extensions/aboutRoute'
import { isContactRoute } from '@/extensions/contactRoute'
import { isCmsContentRoute } from '@/extensions/cmsContentRoute'
import { isHomeRoute } from '@/extensions/homeRoute'

/** CMS page-by-slug is needed on storefront routes that load their own page chrome from CMS. */
export function isCmsPageRoute(pathname: string) {
  return (
    isHomeRoute(pathname) ||
    isBlogRoute(pathname) ||
    isAboutRoute(pathname) ||
    isContactRoute(pathname) ||
    isCmsContentRoute(pathname)
  )
}
