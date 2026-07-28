export interface AppLink {
  label: string
  href: string
  external?: boolean
}

import type { MegaMenuColumn } from '@/models/shared/megaMenu.model'

export interface NavLinkGroup {
  id: string
  label: string
  href?: string
  /** Simple single-column list */
  children?: AppLink[]
  /** Multi-column mega menu panel */
  columns?: MegaMenuColumn[]
}
