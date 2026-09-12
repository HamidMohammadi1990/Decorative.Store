import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { CartLine } from '@/models/cart/cartLine.model'
import { inferLayoutFootprint } from '@/extensions/inferLayoutFootprint'
import { syncRoomLayoutWithCart } from '@/extensions/syncRoomLayoutWithCart'
import type { PlacedLayoutItem } from '@/models/room/roomLayout.model'

interface RoomLayoutState {
  roomTypeId: string | null
  placedItems: PlacedLayoutItem[]
  selectedId: string | null
  showGrid: boolean
  setRoomTypeId: (id: string) => void
  setShowGrid: (show: boolean) => void
  selectItem: (id: string | null) => void
  syncWithCart: (lines: CartLine[]) => void
  placeFromLine: (line: CartLine, x?: number, y?: number) => string | null
  updateItem: (id: string, patch: Partial<PlacedLayoutItem>) => void
  removeItem: (id: string) => void
  rotateItem: (id: string, delta: number) => void
  scaleItem: (id: string, delta: number) => void
  bringToFront: (id: string) => void
  clearLayout: () => void
}

function createPlacementId() {
  return `placed-${Date.now().toString(36)}-${Math.random().toString(36).slice(2, 6)}`
}

export const useRoomLayoutStore = create<RoomLayoutState>()(
  persist(
    (set, get) => ({
      roomTypeId: null,
      placedItems: [],
      selectedId: null,
      showGrid: true,

      setRoomTypeId: (id) => set({ roomTypeId: id }),
      setShowGrid: (show) => set({ showGrid: show }),
      selectItem: (id) => set({ selectedId: id }),

      syncWithCart: (lines) =>
        set((state) => ({
          placedItems: syncRoomLayoutWithCart(state.placedItems, lines),
          selectedId: state.selectedId
            && syncRoomLayoutWithCart(state.placedItems, lines).some((i) => i.id === state.selectedId)
            ? state.selectedId
            : null,
        })),

      placeFromLine: (line, x = 50, y = 55) => {
        const state = get()
        const placedCount = state.placedItems.filter((i) => i.lineId === line.lineId).length
        if (placedCount >= line.quantity) return null

        const { widthPct, heightPct } = inferLayoutFootprint(line.title)
        const maxZ = state.placedItems.reduce((max, item) => Math.max(max, item.zIndex), 0)
        const id = createPlacementId()
        const item: PlacedLayoutItem = {
          id,
          lineId: line.lineId,
          sku: line.sku,
          title: line.title,
          image: line.layoutImage ?? line.image,
          x,
          y,
          rotation: 0,
          scale: 1,
          widthPct,
          heightPct,
          zIndex: maxZ + 1,
        }

        set((s) => ({
          placedItems: [...s.placedItems, item],
          selectedId: id,
        }))

        return id
      },

      updateItem: (id, patch) =>
        set((state) => ({
          placedItems: state.placedItems.map((item) =>
            item.id === id ? { ...item, ...patch } : item,
          ),
        })),

      removeItem: (id) =>
        set((state) => ({
          placedItems: state.placedItems.filter((item) => item.id !== id),
          selectedId: state.selectedId === id ? null : state.selectedId,
        })),

      rotateItem: (id, delta) =>
        set((state) => ({
          placedItems: state.placedItems.map((item) =>
            item.id === id ? { ...item, rotation: item.rotation + delta } : item,
          ),
        })),

      scaleItem: (id, delta) =>
        set((state) => ({
          placedItems: state.placedItems.map((item) =>
            item.id === id
              ? { ...item, scale: Math.min(1.6, Math.max(0.6, item.scale + delta)) }
              : item,
          ),
        })),

      bringToFront: (id) =>
        set((state) => {
          const maxZ = state.placedItems.reduce((max, item) => Math.max(max, item.zIndex), 0)
          return {
            placedItems: state.placedItems.map((item) =>
              item.id === id ? { ...item, zIndex: maxZ + 1 } : item,
            ),
          }
        }),

      clearLayout: () => set({ placedItems: [], selectedId: null }),
    }),
    {
      name: 'diba-room-layout',
      partialize: (state) => ({
        roomTypeId: state.roomTypeId,
        placedItems: state.placedItems,
        showGrid: state.showGrid,
      }),
    },
  ),
)
