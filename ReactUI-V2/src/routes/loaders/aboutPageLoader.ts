import type { LoaderFunctionArgs } from 'react-router-dom'
import { aboutPageService } from '@/services/aboutPageService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { AboutPageLoaderData } from '@/routes/loaders/types'

export async function aboutPageLoader({
  request,
}: LoaderFunctionArgs): Promise<AboutPageLoaderData> {
  const locale = resolveLocale(request)

  try {
    const content = await aboutPageService.getPage(locale)
    return { locale, content }
  } catch {
    return { locale, content: null, error: 'failed' }
  }
}
