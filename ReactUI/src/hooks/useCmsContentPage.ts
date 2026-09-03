import { useEffect, useState } from 'react'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import { cmsContentPageService } from '@/services/cmsContentPageService'
import { useCmsContentLoaderData } from '@/hooks/useCmsContentLoaderData'
import { useSettingsStore } from '@/stores/settingsStore'

export function useCmsContentPage(slug: string) {
  const locale = useSettingsStore((s) => s.locale)
  const loaderData = useCmsContentLoaderData(slug)
  const loaderFresh = Boolean(
    loaderData && loaderData.locale === locale && loaderData.slug === slug && loaderData.content,
  )

  const [content, setContent] = useState<CmsContentPageContent | null>(() =>
    loaderFresh ? loaderData!.content : null,
  )
  const [loading, setLoading] = useState(() => !loaderFresh && Boolean(slug))
  const [notFound, setNotFound] = useState(() => loaderData?.notFound ?? false)

  useEffect(() => {
    if (!slug) {
      setNotFound(true)
      setLoading(false)
      return
    }

    if (loaderFresh) {
      setContent(loaderData!.content)
      setNotFound(false)
      setLoading(false)
      return
    }

    if (loaderData && loaderData.locale === locale && loaderData.slug === slug && loaderData.notFound) {
      setNotFound(true)
      setContent(null)
      setLoading(false)
      return
    }

    let cancelled = false

    const load = async () => {
      setLoading(true)
      setNotFound(false)
      try {
        const page = await cmsContentPageService.getPage(slug, locale)
        if (cancelled) return
        if (!page?.hero.title?.trim()) {
          setNotFound(true)
          setContent(null)
        } else {
          setContent(page)
        }
      } catch {
        if (!cancelled) setNotFound(true)
      } finally {
        if (!cancelled) setLoading(false)
      }
    }

    void load()
    return () => {
      cancelled = true
    }
  }, [locale, slug, loaderFresh, loaderData])

  return { content, loading, notFound }
}
