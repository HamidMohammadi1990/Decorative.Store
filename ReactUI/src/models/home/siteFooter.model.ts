import type { AppLink } from '@/models/shared/link.model'

export interface FooterColumn {
  title: string
  links: AppLink[]
}

export interface SiteFooter {
  columns: FooterColumn[]
  newsletterTitle: string
  newsletterPlaceholder: string
  newsletterButton: string
  copyright: string
  legalLinks: AppLink[]
}
