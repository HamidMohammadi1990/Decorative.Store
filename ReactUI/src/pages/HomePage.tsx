import { useTranslation } from 'react-i18next'
import { BlogHomeSection } from '@/components/home/BlogHomeSection'
import { CategoryNav } from '@/components/home/CategoryNav'
import { DesignServicesStrip } from '@/components/home/DesignServicesStrip'
import { FeaturedShopGrid } from '@/components/home/FeaturedShopGrid'
import { HeroCarousel } from '@/components/home/HeroCarousel'
import { PromoTileStrip } from '@/components/home/PromoTileStrip'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { useHomePage } from '@/hooks/useHomePage'

export function HomePage() {
  const { t } = useTranslation()
  const { data, loading, error } = useHomePage()

  if (loading) {
    return <PageLoading />
  }

  if (error || !data) {
    return (
      <Container className="py-20 text-center text-sm text-sale">
        {t('common.error')}
      </Container>
    )
  }

  return (
    <>
      <HeroCarousel data={data.hero} />
      <DesignServicesStrip data={data.designServices} />
      <PromoTileStrip data={data.promoTiles} />
      <CategoryNav data={data.categoryNav} />
      <FeaturedShopGrid data={data.featuredShop} />
      <BlogHomeSection />
    </>
  )
}
