import type { LoaderFunctionArgs } from 'react-router-dom'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { HomePageLoaderData } from '@/routes/loaders/types'

/** Journal posts load client-side in BlogHomeSection (same API as `/blog`). */
export async function homePageLoader({
  request,
}: LoaderFunctionArgs): Promise<HomePageLoaderData> {
  const locale = resolveLocale(request)
  return { locale, featuredPosts: [] }
}
