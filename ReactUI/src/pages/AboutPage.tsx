import { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { AboutPageSections } from '@/components/about/AboutPageSections'
import { buildWebPageJsonLd } from '@/components/seo/jsonLdBuilders'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { absoluteUrl } from '@/config/site'
import { useAboutPageContent } from '@/hooks/useAboutPageContent'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'

export function AboutPage() {
  const { t } = useTranslation()
  const { content, loading, error } = useAboutPageContent()

  const jsonLd = useMemo(() => {
    if (!content) return null
    return buildWebPageJsonLd({
      name: content.title,
      description: content.metaDescription ?? content.hero.subtitle ?? '',
      url: absoluteUrl('/about'),
    })
  }, [content])

  useShopPageMeta({
    title: content?.metaTitle?.trim() || content?.title,
    description: content?.metaDescription ?? undefined,
    path: '/about',
    active: Boolean(content),
    jsonLd,
  })

  if (loading && !content) {
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
