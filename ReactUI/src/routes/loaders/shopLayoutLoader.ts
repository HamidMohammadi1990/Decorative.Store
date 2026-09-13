import type { LoaderFunctionArgs } from 'react-router-dom'
import { isBlogRoute } from '@/extensions/blogRoute'
import { blogService } from '@/services/blogService'
import { homeService } from '@/services/homeService'
import { storiesService } from '@/services/storiesService'
import {
  buildHomePageFetchKey,
  resolveHomePageFetchOptions,
} from '@/ssr/resolveHomePageFetchOptions'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'

export async function shopLayoutLoader({
  request,
}: LoaderFunctionArgs): Promise<ShopLayoutLoaderData> {
  const locale = resolveLocale(request)
  const pathname = new URL(request.url).pathname
  const options = resolveHomePageFetchOptions(pathname)
  const fetchKey = buildHomePageFetchKey(options)
  const blogNavEnabled = isBlogRoute(pathname)

  const [homeResult, storiesResult, blogNavResult] = await Promise.allSettled([
    homeService.getPage(locale, options),
    storiesService.getStories(locale, null),
    blogNavEnabled ? blogService.getNavCategories(locale) : Promise.resolve([]),
  ])

  const homePage = homeResult.status === 'fulfilled' ? homeResult.value : null
  const stories = storiesResult.status === 'fulfilled' ? storiesResult.value : []
  const blogNavCategories = blogNavResult.status === 'fulfilled' ? blogNavResult.value : []

  if (import.meta.env.SSR) {
    if (homeResult.status === 'rejected') {
      console.error('[shopLayoutLoader] homePage failed:', homeResult.reason)
    }
    if (storiesResult.status === 'rejected') {
      console.error('[shopLayoutLoader] stories failed:', storiesResult.reason)
    }
  }

  return {
    locale,
    fetchKey,
    homePage,
    stories,
    blogNavCategories,
    error: homePage ? undefined : 'failed',
  }
}
