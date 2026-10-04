import type { RoomTypeItem } from '@/models/admin/roomType.model'

export interface PlacedLayoutItem {
  id: string
  lineId: string
  sku: string
  title: string
  image: import('@/models/shared/image.model').ImageAsset
  /** Center position as percentage of canvas (0–100) */
  x: number
  y: number
  rotation: number
  scale: number
  widthPct: number
  heightPct: number
  zIndex: number
}

/** Fallback room types when API is unavailable or empty */
export const ROOM_TYPE_FALLBACKS: RoomTypeItem[] = [
  {
    id: 'fallback-living',
    code: 'living',
    title: 'Living room',
    imageUrl: '/images/home/living-room.svg',
    priority: 0,
  },
  {
    id: 'fallback-bedroom',
    code: 'bedroom',
    title: 'Bedroom',
    imageUrl: '/images/home/bedroom.svg',
    priority: 1,
  },
  {
    id: 'fallback-dining',
    code: 'dining',
    title: 'Dining room',
    imageUrl: '/images/home/dining.svg',
    priority: 2,
  },
]

export function findRoomTypeById(items: RoomTypeItem[], id: string | null): RoomTypeItem | null {
  if (!id) return null
  return items.find((item) => item.id === id) ?? null
}
