export type MarketingPromoType = 1 | 2

export interface AdminMarketingPromo {
  id: string
  languageId: number
  promoType: MarketingPromoType
  title: string
  subtitle?: string | null
  linkLabel: string
  linkHref: string
  imageFileName: string
  priority: number
  isActive: boolean
}

export interface MarketingPromoStrip {
  tiles: {
    id: string
    promoType: MarketingPromoType
    title: string
    subtitle?: string | null
    imageUrl: string
    linkLabel: string
    linkHref: string
  }[]
  disclaimer?: string | null
  disclaimerLink?: { label: string; href: string } | null
}

export interface CreateAdminMarketingPromoInput {
  languageId: number
  promoType: MarketingPromoType
  title: string
  subtitle?: string | null
  linkLabel: string
  linkHref: string
  imageFileName: string
  priority: number
}

export interface UpdateAdminMarketingPromoInput extends CreateAdminMarketingPromoInput {
  id: string
  isActive: boolean
}

export interface UpdateMarketingStripDisclaimerInput {
  languageId: number
  disclaimer?: string | null
  disclaimerLinkLabel?: string | null
  disclaimerLinkHref?: string | null
}
