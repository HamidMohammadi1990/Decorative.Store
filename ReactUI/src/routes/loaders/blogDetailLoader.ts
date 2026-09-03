import type { LoaderFunctionArgs } from 'react-router-dom'
import { blogService } from '@/services/blogService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { BlogDetailLoaderData } from '@/routes/loaders/types'

export async function blogDetailLoader({
  request,
  params,
}: LoaderFunctionArgs): Promise<BlogDetailLoaderData> {
  const locale = resolveLocale(request)
  const slug = params.slug?.trim()

  if (!slug) {
    return {
      locale,
      slug: '',
      post: null,
      related: [],
      categoryMap: {},
      error: 'not-found',
    }
  }

  try {
    const { post, related, categoryMap } = await blogService.getPostDetail(slug, locale)

    if (!post) {
      return { locale, slug, post: null, related: [], categoryMap, error: 'not-found' }
    }

    return { locale, slug, post, related, categoryMap }
  } catch {
    return {
      locale,
      slug,
      post: null,
      related: [],
      categoryMap: {},
      error: 'failed',
    }
  }
}
