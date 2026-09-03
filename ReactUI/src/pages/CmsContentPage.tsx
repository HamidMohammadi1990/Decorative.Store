import { useMemo } from 'react'
import { useParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { CmsContentPageSections } from '@/components/content/CmsContentPageSections'
import { buildWebPageJsonLd } from '@/components/seo/jsonLdBuilders'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { NotFoundPage } from '@/pages/NotFoundPage'
import { cmsContentPagePath } from '@/extensions/cmsContentRoute'
import { absoluteUrl } from '@/config/site'
import { useCmsContentPage } from '@/hooks/useCmsContentPage'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'

interface CmsContentPageProps {
  slug?: string
}

export function CmsContentPage({ slug: slugProp }: CmsContentPageProps) {
  const { t } = useTranslation()
  const { slug: paramSlug } = useParams()
  const slug = (slugProp ?? paramSlug ?? '').trim()
  const { content, loading, notFound } = useCmsContentPage(slug)
  const pagePath = slug ? cmsContentPagePath(slug) : undefined

  const jsonLd = useMemo(() => {
    if (!content || !pagePath) return null
    return buildWebPageJsonLd({
      name: content.title,
      description: content.metaDescription ?? content.hero.subtitle ?? '',
      url: absoluteUrl(pagePath),
    })
  }, [content, pagePath])

  useShopPageMeta({
    title: content?.metaTitle?.trim() || content?.title,
    description: content?.metaDescription ?? undefined,
    path: pagePath,
    active: Boolean(content),
    jsonLd,
  })

  if (!slug || notFound) {
    return <NotFoundPage />
  }

  if (loading && !content) {
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
