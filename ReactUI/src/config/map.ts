/** Default map center — Tehran */
export const DEFAULT_MAP_CENTER = {
  latitude: 35.6892,
  longitude: 51.389,
} as const

export const DEFAULT_MAP_ZOOM = 13

/** Override with VITE_MAP_TILE_URL for providers such as Neshan or Map.ir */
export const MAP_TILE_URL =
  import.meta.env.VITE_MAP_TILE_URL ?? 'https://tile.openstreetmap.org/{z}/{x}/{y}.png'

export const MAP_TILE_ATTRIBUTION =
  import.meta.env.VITE_MAP_TILE_ATTRIBUTION ??
  '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'

/** ~1 cm precision — matches decimal(10,7) in the database */
export function roundCoordinate(value: number): number {
  return Math.round(value * 1e7) / 1e7
}

export interface MapCoordinates {
  latitude: number
  longitude: number
}

export function isValidMapCoordinates(
  coords: Partial<MapCoordinates> | null | undefined,
): coords is MapCoordinates {
  if (!coords) return false
  const { latitude, longitude } = coords
  return (
    typeof latitude === 'number' &&
    Number.isFinite(latitude) &&
    latitude >= -90 &&
    latitude <= 90 &&
    typeof longitude === 'number' &&
    Number.isFinite(longitude) &&
    longitude >= -180 &&
    longitude <= 180
  )
}
