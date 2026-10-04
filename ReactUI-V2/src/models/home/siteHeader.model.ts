import type { NavLinkGroup } from '@/models/shared/link.model'

export interface SiteHeader {
  brandLabel: string
  primaryNav: NavLinkGroup[]
  accountLabel: string
  cartLabel: string
  searchPlaceholder: string
}
