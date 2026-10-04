import type { CategoryNav } from '@/models/home/categoryNav.model'
import type { DesignServicesStrip } from '@/models/home/designServices.model'
import type { FeaturedShopGrid } from '@/models/home/featuredShop.model'
import type { HeroCarousel } from '@/models/home/heroCarousel.model'
import type { PromoAnnouncement } from '@/models/home/promoAnnouncement.model'
import type { PromoTileStrip } from '@/models/home/promoTiles.model'
import type { SiteFooter } from '@/models/home/siteFooter.model'
import type { SiteHeader } from '@/models/home/siteHeader.model'
import type { UtilityBar } from '@/models/home/utilityBar.model'

export interface HomePage {
  promoAnnouncement: PromoAnnouncement
  utilityBar: UtilityBar
  header: SiteHeader
  designServices: DesignServicesStrip
  hero: HeroCarousel
  promoTiles: PromoTileStrip
  categoryNav: CategoryNav
  featuredShop: FeaturedShopGrid
  footer: SiteFooter
}
