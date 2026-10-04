import type { LoaderFunctionArgs } from 'react-router-dom'
import { blogService } from '@/services/blogService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { HomePageLoaderData } from '@/routes/loaders/types'

export async function homePageLoader({
  request,
}: LoaderFunctionArgs): Promise<HomePageLoaderData> {
  const locale = resolveLocale(request)

  try {
    const featuredPosts = await blogService.getFeaturedPosts(locale, 3)
    return { locale, featuredPosts }
  } catch {
    return { locale, featuredPosts: [] }
  }
}
