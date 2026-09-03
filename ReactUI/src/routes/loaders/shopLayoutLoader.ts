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

  try {
    const [homePage, stories, blogNavCategories] = await Promise.all([
      homeService.getPage(locale, options),
      storiesService.getStories(locale, null),
      blogNavEnabled ? blogService.getNavCategories(locale) : Promise.resolve([]),
    ])

    return { locale, fetchKey, homePage, stories, blogNavCategories }
  } catch {
    return {
      locale,
      fetchKey,
      homePage: null,
      stories: [],
      blogNavCategories: [],
      error: 'failed',
    }
  }
}
