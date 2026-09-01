import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AboutPageSections } from '@/components/about/AboutPageSections'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import type { AboutPageContent } from '@/models/about/aboutPage.model'
import { aboutPageService } from '@/services/aboutPageService'
import { useSettingsStore } from '@/stores/settingsStore'

export function AboutPage() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const [content, setContent] = useState<AboutPageContent | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(false)

  useEffect(() => {
    let cancelled = false

    const load = async () => {
      setLoading(true)
      setError(false)
      try {
        const page = await aboutPageService.getPage(locale)
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
  }, [locale])

  useEffect(() => {
    if (!content) return
    document.title = content.metaTitle?.trim() || content.title
  }, [content])

  if (loading) {
    return <PageLoading />
  }

  if (error || !content) {
    return (
      <Container className="py-20 text-center text-sm text-sale">
        {t('common.error')}
      </Container>
    )
  }

  return <AboutPageSections content={content} />
}
