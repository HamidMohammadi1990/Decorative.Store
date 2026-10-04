import type { CartLine } from '@/models/cart/cartLine.model'
import type { PlacedLayoutItem } from '@/models/room/roomLayout.model'

export function syncRoomLayoutWithCart(
  placed: PlacedLayoutItem[],
  lines: CartLine[],
): PlacedLayoutItem[] {
  const lineById = new Map(lines.map((line) => [line.lineId, line]))
  const valid = placed.filter((item) => lineById.has(item.lineId))

  const kept: PlacedLayoutItem[] = []
  const used = new Map<string, number>()

  for (const item of valid) {
    const line = lineById.get(item.lineId)!
    const count = used.get(item.lineId) ?? 0
    if (count < line.quantity) {
      kept.push({
        ...item,
        image: line.layoutImage ?? line.image,
        title: line.title,
      })
      used.set(item.lineId, count + 1)
    }
  }

  return kept
}

export function countPlacedForLine(placed: PlacedLayoutItem[], lineId: string) {
  return placed.filter((item) => item.lineId === lineId).length
}

export function remainingPlaceable(
  placed: PlacedLayoutItem[],
  line: CartLine,
): number {
  return Math.max(0, line.quantity - countPlacedForLine(placed, line.lineId))
}
