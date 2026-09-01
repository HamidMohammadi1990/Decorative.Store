import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ContactPageSections } from '@/components/contact/ContactPageSections'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import type { ContactPageContent } from '@/models/contact/contactPage.model'
import { contactPageService } from '@/services/contactPageService'
import { useSettingsStore } from '@/stores/settingsStore'

export function ContactPage() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const [content, setContent] = useState<ContactPageContent | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(false)

  useEffect(() => {
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

  return <ContactPageSections content={content} />
}
