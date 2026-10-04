import type { AppLink } from '@/models/shared/link.model'

export interface PromoAnnouncement {
  id: string
  message: string
  link: AppLink
}
