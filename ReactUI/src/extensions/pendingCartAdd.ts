import type { ImageAsset } from '@/models/shared/image.model'
import type { Money } from '@/models/shared/money.model'

const STORAGE_KEY = 'westelm-pending-cart-add'

export interface PendingCartAdd {
  sku: string
  title: string
  image: ImageAsset
  unitPrice: Money
  quantity: number
  returnUrl: string
}

export function savePendingCartAdd(item: PendingCartAdd) {
  try {
    sessionStorage.setItem(STORAGE_KEY, JSON.stringify(item))
  } catch {
    // Ignore quota or privacy mode errors.
  }
}

export function hasPendingCartAdd() {
  try {
    return sessionStorage.getItem(STORAGE_KEY) !== null
  } catch {
    return false
  }
}

export function consumePendingCartAdd(): PendingCartAdd | null {
  try {
    const raw = sessionStorage.getItem(STORAGE_KEY)
    if (!raw) return null
    sessionStorage.removeItem(STORAGE_KEY)
    return JSON.parse(raw) as PendingCartAdd
  } catch {
    sessionStorage.removeItem(STORAGE_KEY)
    return null
  }
}
