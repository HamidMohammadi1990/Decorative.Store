import { isBlogRoute } from '@/extensions/blogRoute'
import { isHomeRoute } from '@/extensions/homeRoute'

/** CMS page-by-slug is only needed on the storefront home and blog surfaces. */
export function isCmsPageRoute(pathname: string) {
  return isHomeRoute(pathname) || isBlogRoute(pathname)
}
