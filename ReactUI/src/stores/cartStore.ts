import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { CartLine } from '@/models/cart/cartLine.model'

interface CartState {
  lines: CartLine[]
  isOpen: boolean
  openCart: () => void
  closeCart: () => void
  setLines: (lines: CartLine[]) => void
  addLine: (line: Omit<CartLine, 'lineId'>) => void
  removeLine: (lineId: string) => void
  updateQuantity: (lineId: string, quantity: number) => void
  clearCart: () => void
  itemCount: () => number
}

export const useCartStore = create<CartState>()(
  persist(
    (set, get) => ({
      lines: [],
      isOpen: false,

      openCart: () => set({ isOpen: true }),
      closeCart: () => set({ isOpen: false }),

      setLines: (lines) => set({ lines }),

      addLine: (line) => {
        set((state) => {
          const existing = state.lines.find((l) => l.sku === line.sku)
          if (existing) {
            return {
              lines: state.lines.map((l) =>
                l.sku === line.sku
                  ? { ...l, quantity: l.quantity + line.quantity }
                  : l,
              ),
              isOpen: true,
            }
          }

          const lineId = `${line.sku}-${Date.now()}`
          return {
            lines: [...state.lines, { ...line, lineId }],
            isOpen: true,
          }
        })
      },

      removeLine: (lineId) =>
        set((state) => ({
          lines: state.lines.filter((l) => l.lineId !== lineId),
        })),

      updateQuantity: (lineId, quantity) =>
        set((state) => ({
          lines: state.lines.map((l) =>
            l.lineId === lineId ? { ...l, quantity: Math.max(1, quantity) } : l,
          ),
        })),

      clearCart: () => set({ lines: [] }),

      itemCount: () =>
        get().lines.reduce((sum, line) => sum + line.quantity, 0),
    }),
    {
      name: 'westelm-cart',
      partialize: (state) => ({ lines: state.lines }),
    },
  ),
)
