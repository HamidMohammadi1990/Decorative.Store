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
  aboutHero: 'AboutHero',
  aboutStoryBlock: 'AboutStoryBlock',
  aboutStatsStrip: 'AboutStatsStrip',
  aboutValuesGrid: 'AboutValuesGrid',
  aboutTimeline: 'AboutTimeline',
  aboutCtaStrip: 'AboutCtaStrip',
  contactHero: 'ContactHero',
  contactMethodsGrid: 'ContactMethodsGrid',
  contactLocationsGrid: 'ContactLocationsGrid',
  contactFormIntro: 'ContactFormIntro',
  contentHero: 'ContentHero',
  contentBodyBlock: 'ContentBodyBlock',
  contentStepsGrid: 'ContentStepsGrid',
  contentCtaStrip: 'ContentCtaStrip',
} as const

export const CMS_CONTENT_SECTION_TYPES = new Set<string>([
  CMS_SECTION_TYPES.contentHero,
  CMS_SECTION_TYPES.contentBodyBlock,
  CMS_SECTION_TYPES.contentStepsGrid,
  CMS_SECTION_TYPES.contentCtaStrip,
])

export const CMS_CONTACT_SECTION_TYPES = new Set<string>([
  CMS_SECTION_TYPES.contactHero,
  CMS_SECTION_TYPES.contactMethodsGrid,
  CMS_SECTION_TYPES.contactLocationsGrid,
  CMS_SECTION_TYPES.contactFormIntro,
])

export const CMS_ABOUT_SECTION_TYPES = new Set<string>([
  CMS_SECTION_TYPES.aboutHero,
  CMS_SECTION_TYPES.aboutStoryBlock,
  CMS_SECTION_TYPES.aboutStatsStrip,
  CMS_SECTION_TYPES.aboutValuesGrid,
  CMS_SECTION_TYPES.aboutTimeline,
  CMS_SECTION_TYPES.aboutCtaStrip,
])

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

export function cmsBlogPageSlug(_locale: 'en' | 'fa') {
  return 'blog'
}

export function cmsAboutPageSlug(_locale: 'en' | 'fa') {
  return 'about'
}

export function cmsContactPageSlug(_locale: 'en' | 'fa') {
  return 'contact'
}
