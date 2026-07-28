import type { ImageAsset } from '@/models/shared/image.model'

export type RoomPresetId = 'living' | 'bedroom' | 'dining'

export interface RoomPreset {
  id: RoomPresetId
  imageSrc: string
}

export interface PlacedLayoutItem {
  id: string
  lineId: string
  sku: string
  title: string
  image: ImageAsset
  /** Center position as percentage of canvas (0–100) */
  x: number
  y: number
  rotation: number
  scale: number
  widthPct: number
  heightPct: number
  zIndex: number
}

export const ROOM_PRESETS: RoomPreset[] = [
  { id: 'living', imageSrc: '/images/home/living-room.svg' },
  { id: 'bedroom', imageSrc: '/images/home/bedroom.svg' },
  { id: 'dining', imageSrc: '/images/home/dining.svg' },
]
