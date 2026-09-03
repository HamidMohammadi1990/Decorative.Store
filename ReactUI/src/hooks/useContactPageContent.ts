import { useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { ContactPageContent } from '@/models/contact/contactPage.model'
import { contactPageService } from '@/services/contactPageService'
import type { ContactPageLoaderData } from '@/routes/loaders/types'
import { useSettingsStore } from '@/stores/settingsStore'

export function useContactPageContent() {
  const locale = useSettingsStore((s) => s.locale)
  const loaderData = useRouteLoaderData('contact') as ContactPageLoaderData | undefined
  const loaderFresh = Boolean(loaderData && loaderData.locale === locale && loaderData.content)

  const [content, setContent] = useState<ContactPageContent | null>(() =>
    loaderFresh ? loaderData!.content : null,
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState(() => loaderFresh ? Boolean(loaderData!.error) : false)

  useEffect(() => {
    if (loaderFresh) {
      setContent(loaderData!.content)
      setError(Boolean(loaderData!.error))
      setLoading(false)
      return
    }

    let cancelled = false

    const load = async () => {
      setLoading(true)
      setError(false)
      try {
        const page = await contactPageService.getPage(locale)
        if (!cancelled) setContent(page)
      } catch {
        if (!cancelled) setError(true)
      } finally {
        if (!cancelled) setLoading(false)
      }
    }

    void load()
    return () => {
      cancelled = true
    }
  }, [locale, loaderFresh, loaderData])

  return { content, loading, error }
}
