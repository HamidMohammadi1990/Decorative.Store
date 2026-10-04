import { useTranslation } from 'react-i18next'
import { buildOrganizationJsonLd, buildWebSiteJsonLd } from '@/components/seo/jsonLdBuilders'
import { BlogHomeSection } from '@/components/home/BlogHomeSection'
import { FeaturedShopGrid } from '@/components/home/FeaturedShopGrid'
import { PromoTileStrip } from '@/components/home/PromoTileStrip'
import { ZaraHomeShell } from '@/components/storefront/zara/ZaraHomeShell'
import { PageLoading } from '@/components/ui/Spinner'
import { useHomePage } from '@/hooks/useHomePage'
import { useShopPageMeta, resolveOgImageSrc } from '@/hooks/useShopPageMeta'
import { DEFAULT_OG_IMAGE } from '@/config/site'
import { usePreloadImage } from '@/hooks/usePreloadImage'

export function HomePage() {
  const { t } = useTranslation()
  const { data, loading } = useHomePage()

  const lcpSrc = data?.hero.slides[0]?.image.src
  usePreloadImage(lcpSrc, Boolean(lcpSrc))

  const heroImage = data?.hero.slides[0]?.image

  useShopPageMeta({
    title: t('seo.homeTitle', { defaultValue: 'Home' }),
    description: t('seo.homeDescription'),
    image: resolveOgImageSrc(heroImage?.src ?? DEFAULT_OG_IMAGE),
    imageAlt: heroImage?.alt ?? t('seo.homeTitle'),
    keywords: t('seo.homeKeywords'),
    jsonLd: [buildOrganizationJsonLd(), buildWebSiteJsonLd()],
    active: !loading && !!data,
  })

  if (loading && !data) {
    return <PageLoading />
  }

  if (!data) {
    return (
      <div className="sf-container py-20 text-center text-sm text-sale">
        {t('common.error')}
      </div>
    )
  }

  return (
    <>
      <ZaraHomeShell header={data.header} hero={data.hero} categoryNav={data.categoryNav} />
      <div className="bg-surface text-text">
        <section className="border-t border-border">
          <PromoTileStrip data={data.promoTiles} />
        </section>
        <section className="border-t border-border py-4">
          <FeaturedShopGrid data={data.featuredShop} />
        </section>
        <section className="border-t border-border">
          <BlogHomeSection />
        </section>
      </div>
    </>
  )
}
