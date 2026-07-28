import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface WishlistState {
  slugs: string[]
  toggle: (slug: string) => 'added' | 'removed'
  remove: (slug: string) => void
  clear: () => void
  isInWishlist: (slug: string) => boolean
  count: () => number
}

export const useWishlistStore = create<WishlistState>()(
  persist(
    (set, get) => ({
      slugs: [],

      toggle: (slug) => {
        const { slugs } = get()
        if (slugs.includes(slug)) {
          set({ slugs: slugs.filter((s) => s !== slug) })
          return 'removed'
        }
        set({ slugs: [...slugs, slug] })
        return 'added'
      },

      remove: (slug) =>
        set((state) => ({
          slugs: state.slugs.filter((s) => s !== slug),
        })),

      clear: () => set({ slugs: [] }),

      isInWishlist: (slug) => get().slugs.includes(slug),

      count: () => get().slugs.length,
    }),
    {
      name: 'diba-wishlist',
      partialize: (state) => ({ slugs: state.slugs }),
    },
  ),
)
