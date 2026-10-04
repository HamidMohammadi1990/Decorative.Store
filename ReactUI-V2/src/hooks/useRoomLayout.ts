import { useEffect } from 'react'
import { useCartStore } from '@/stores/cartStore'
import { useRoomLayoutStore } from '@/stores/roomLayoutStore'
import { remainingPlaceable } from '@/extensions/syncRoomLayoutWithCart'

export function useRoomLayout() {
  const lines = useCartStore((s) => s.lines)
  const roomTypeId = useRoomLayoutStore((s) => s.roomTypeId)
  const placedItems = useRoomLayoutStore((s) => s.placedItems)
  const selectedId = useRoomLayoutStore((s) => s.selectedId)
  const showGrid = useRoomLayoutStore((s) => s.showGrid)
  const setRoomTypeId = useRoomLayoutStore((s) => s.setRoomTypeId)
  const setShowGrid = useRoomLayoutStore((s) => s.setShowGrid)
  const selectItem = useRoomLayoutStore((s) => s.selectItem)
  const syncWithCart = useRoomLayoutStore((s) => s.syncWithCart)
  const placeFromLine = useRoomLayoutStore((s) => s.placeFromLine)
  const updateItem = useRoomLayoutStore((s) => s.updateItem)
  const removeItem = useRoomLayoutStore((s) => s.removeItem)
  const rotateItem = useRoomLayoutStore((s) => s.rotateItem)
  const scaleItem = useRoomLayoutStore((s) => s.scaleItem)
  const bringToFront = useRoomLayoutStore((s) => s.bringToFront)
  const clearLayout = useRoomLayoutStore((s) => s.clearLayout)

  useEffect(() => {
    syncWithCart(lines)
  }, [lines, syncWithCart])

  const paletteLines = lines.map((line) => ({
    line,
    remaining: remainingPlaceable(placedItems, line),
  }))

  const selectedItem = placedItems.find((item) => item.id === selectedId) ?? null

  return {
    lines,
    paletteLines,
    roomTypeId,
    placedItems,
    selectedItem,
    selectedId,
    showGrid,
    setRoomTypeId,
    setShowGrid,
    selectItem,
    placeFromLine,
    updateItem,
    removeItem,
    rotateItem,
    scaleItem,
    bringToFront,
    clearLayout,
  }
}
