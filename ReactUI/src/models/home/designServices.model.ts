import type { AppLink } from '@/models/shared/link.model'

export interface DesignServicesStrip {
  title: string
  cta: AppLink
  featuredLinks: AppLink[]
}
