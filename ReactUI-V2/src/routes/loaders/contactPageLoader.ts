import type { LoaderFunctionArgs } from 'react-router-dom'
import { contactPageService } from '@/services/contactPageService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { ContactPageLoaderData } from '@/routes/loaders/types'

export async function contactPageLoader({
  request,
}: LoaderFunctionArgs): Promise<ContactPageLoaderData> {
  const locale = resolveLocale(request)

  try {
    const content = await contactPageService.getPage(locale)
    return { locale, content }
  } catch {
    return { locale, content: null, error: 'failed' }
  }
}
