import type { SiteHeader as SiteHeaderModel } from '@/models/home/siteHeader.model'
import type { HeroCarousel as HeroCarouselModel } from '@/models/home/heroCarousel.model'
import type { CategoryNav as CategoryNavModel } from '@/models/home/categoryNav.model'
import { ZaraStoreHeader } from '@/components/storefront/zara/ZaraStoreHeader'
import { ZaraHeroStage } from '@/components/storefront/zara/ZaraHeroStage'

interface ZaraHomeShellProps {
  header: SiteHeaderModel
  hero: HeroCarouselModel
  categoryNav?: CategoryNavModel
}

/** Hero + overlay header share one grid cell (Zara-style), not stacked in document flow. */
export function ZaraHomeShell({ header, hero, categoryNav }: ZaraHomeShellProps) {
  return (
    <div className="grid h-svh w-full grid-cols-1 grid-rows-1">
      <ZaraHeroStage
        hero={hero}
        categoryNav={categoryNav}
        className="col-start-1 row-start-1 h-svh w-full"
      />
      <div className="pointer-events-none col-start-1 row-start-1 z-20 flex h-svh w-full flex-col">
        <div className="pointer-events-auto">
          <ZaraStoreHeader data={header} variant="overlay" />
        </div>
      </div>
    </div>
  )
}
