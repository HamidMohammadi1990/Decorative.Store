import { useTranslation } from 'react-i18next'
import { buildOrganizationJsonLd, buildWebSiteJsonLd } from '@/components/seo/jsonLdBuilders'
import { BlogHomeSection } from '@/components/home/BlogHomeSection'
import { CategoryNav } from '@/components/home/CategoryNav'
import { DesignServicesStrip } from '@/components/home/DesignServicesStrip'
import { FeaturedShopGrid } from '@/components/home/FeaturedShopGrid'
import { HeroCarousel } from '@/components/home/HeroCarousel'
import { PromoTileStrip } from '@/components/home/PromoTileStrip'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { useHomePage } from '@/hooks/useHomePage'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'
import { usePreloadImage } from '@/hooks/usePreloadImage'

export function HomePage() {
  const { t } = useTranslation()
  const { data, loading } = useHomePage()

  const lcpSrc = data?.hero.slides[0]?.image.src
  usePreloadImage(lcpSrc, Boolean(lcpSrc))

  useShopPageMeta({
    title: t('seo.homeTitle', { defaultValue: 'Home' }),
    description: t('seo.homeDescription'),
    jsonLd: [buildOrganizationJsonLd(), buildWebSiteJsonLd()],
    active: !loading && !!data,
  })

  if (loading && !data) {
    return <PageLoading />
  }

  if (!data) {
    return (
      <Container className="py-20 text-center text-sm text-sale">
        {t('common.error')}
      </Container>
    )
  }

  return (
    <>
      {data.hero.slides.length > 0 && <HeroCarousel data={data.hero} />}
      <DesignServicesStrip data={data.designServices} />
      <PromoTileStrip data={data.promoTiles} />
      <CategoryNav data={data.categoryNav} />
      <FeaturedShopGrid data={data.featuredShop} />
      <BlogHomeSection />
    </>
  )
}
