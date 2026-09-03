import type { LoaderFunctionArgs } from 'react-router-dom'
import { blogService } from '@/services/blogService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { BlogListingLoaderData } from '@/routes/loaders/types'

export async function blogListingLoader({
  request,
  params,
}: LoaderFunctionArgs): Promise<BlogListingLoaderData> {
  const locale = resolveLocale(request)
  const categorySlug = params.categorySlug?.trim() || undefined

  try {
    const data = await blogService.getListing(locale, categorySlug)
    return { locale, categorySlug, data }
  } catch {
    return { locale, categorySlug, data: null, error: 'failed' }
  }
}
