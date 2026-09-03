import type { LoaderFunctionArgs } from 'react-router-dom'
import { CMS_CONTENT_PATH_TO_SLUG } from '@/extensions/cmsContentRoute'
import { cmsContentPageService } from '@/services/cmsContentPageService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { CmsContentPageLoaderData } from '@/routes/loaders/types'

export async function cmsContentPageLoader({
  request,
  params,
}: LoaderFunctionArgs): Promise<CmsContentPageLoaderData> {
  const locale = resolveLocale(request)
  const pathname = new URL(request.url).pathname
  const slug = (params.slug?.trim() || CMS_CONTENT_PATH_TO_SLUG[pathname] || '').trim()

  if (!slug) {
    return { locale, slug: '', content: null, notFound: true }
  }

  try {
    const content = await cmsContentPageService.getPage(slug, locale)

    if (!content?.hero.title?.trim()) {
      return { locale, slug, content: null, notFound: true }
    }

    return { locale, slug, content }
  } catch {
    return { locale, slug, content: null, notFound: true, error: 'failed' }
  }
}
