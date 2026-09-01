import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { CmsContentPageSections } from '@/components/content/CmsContentPageSections'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { NotFoundPage } from '@/pages/NotFoundPage'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import { cmsContentPageService } from '@/services/cmsContentPageService'
import { useSettingsStore } from '@/stores/settingsStore'

interface CmsContentPageProps {
  slug?: string
}

export function CmsContentPage({ slug: slugProp }: CmsContentPageProps) {
  const { t } = useTranslation()
  const { slug: paramSlug } = useParams()
  const slug = (slugProp ?? paramSlug ?? '').trim()
  const locale = useSettingsStore((s) => s.locale)
  const [content, setContent] = useState<CmsContentPageContent | null>(null)
  const [loading, setLoading] = useState(true)
  const [notFound, setNotFound] = useState(false)

  useEffect(() => {
    if (!slug) {
      setNotFound(true)
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
  }, [locale, slug])

  useEffect(() => {
    if (!content) return
    document.title = content.metaTitle?.trim() || content.title
  }, [content])

  if (!slug || notFound) {
    return <NotFoundPage />
  }

  if (loading) {
    return <PageLoading />
  }

  if (!content) {
    return (
      <Container className="py-20 text-center text-sm text-sale">
        {t('common.error')}
      </Container>
    )
  }

  return <CmsContentPageSections content={content} />
}
