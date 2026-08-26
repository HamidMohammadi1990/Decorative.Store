export const CMS_SECTION_TYPES = {
  promoAnnouncement: 'PromoAnnouncement',
  utilityBar: 'UtilityBar',
  siteHeader: 'SiteHeader',
  designServicesStrip: 'DesignServicesStrip',
  heroCarousel: 'HeroCarousel',
  promoTileStrip: 'PromoTileStrip',
  categoryNav: 'CategoryNav',
  featuredShopGrid: 'FeaturedShopGrid',
  footerColumn: 'FooterColumn',
  siteFooter: 'SiteFooter',
} as const

export const CMS_CHROME_SECTION_TYPES = new Set<string>([
  CMS_SECTION_TYPES.promoAnnouncement,
  CMS_SECTION_TYPES.utilityBar,
  CMS_SECTION_TYPES.siteHeader,
  CMS_SECTION_TYPES.footerColumn,
  CMS_SECTION_TYPES.siteFooter,
])

export const CMS_HOME_SECTION_TYPES = new Set<string>([
  CMS_SECTION_TYPES.designServicesStrip,
  CMS_SECTION_TYPES.heroCarousel,
  CMS_SECTION_TYPES.promoTileStrip,
  CMS_SECTION_TYPES.categoryNav,
  CMS_SECTION_TYPES.featuredShopGrid,
])

export function cmsShopPageSlug(_locale: 'en' | 'fa') {
  return 'shop'
}
