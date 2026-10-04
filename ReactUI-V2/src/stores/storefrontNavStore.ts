import { create } from 'zustand'

interface StorefrontNavState {
  menuOpen: boolean
  setMenuOpen: (open: boolean) => void
  openMenu: () => void
  closeMenu: () => void
}

export const useStorefrontNavStore = create<StorefrontNavState>((set) => ({
  menuOpen: false,
  setMenuOpen: (open) => set({ menuOpen: open }),
  openMenu: () => set({ menuOpen: true }),
  closeMenu: () => set({ menuOpen: false }),
}))
