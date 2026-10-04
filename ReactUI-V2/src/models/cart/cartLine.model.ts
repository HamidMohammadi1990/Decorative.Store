import type { ImageAsset } from '@/models/shared/image.model'
import type { Money } from '@/models/shared/money.model'

/** Minimal product snapshot stored in cart — not a catalog Product model */
export interface CartLine {
  lineId: string
  sku: string
  slug?: string
  title: string
  image: ImageAsset
  layoutImage?: ImageAsset
  unitPrice: Money
  quantity: number
}
