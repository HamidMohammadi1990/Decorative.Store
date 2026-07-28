import type { AppLink } from '@/models/shared/link.model'

export interface UtilityBar {
  phoneLabel: string
  phoneHref: string
  links: AppLink[]
}
