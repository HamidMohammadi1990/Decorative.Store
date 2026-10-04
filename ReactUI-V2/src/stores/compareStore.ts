import { create } from 'zustand'
import { writeCompareSlugsToCookie } from '@/extensions/compareCookie'
import { persist } from 'zustand/middleware'
import { MAX_COMPARE_PRODUCTS } from '@/models/catalog/compare.model'

interface CompareState {
  slugs: string[]
  toggle: (slug: string) => 'added' | 'removed' | 'full'
  remove: (slug: string) => void
  clear: () => void
  isInCompare: (slug: string) => boolean
  count: () => number
  isFull: () => boolean
}

export const useCompareStore = create<CompareState>()(
  persist(
    (set, get) => ({
      slugs: [],

      toggle: (slug) => {
        const { slugs } = get()
        if (slugs.includes(slug)) {
          set({ slugs: slugs.filter((s) => s !== slug) })
          return 'removed'
        }
        if (slugs.length >= MAX_COMPARE_PRODUCTS) return 'full'
        set({ slugs: [...slugs, slug] })
        return 'added'
      },

      remove: (slug) =>
        set((state) => ({
          slugs: state.slugs.filter((s) => s !== slug),
        })),

      clear: () => set({ slugs: [] }),

      isInCompare: (slug) => get().slugs.includes(slug),

      count: () => get().slugs.length,

      isFull: () => get().slugs.length >= MAX_COMPARE_PRODUCTS,
    }),
    {
      name: 'diba-compare',
      partialize: (state) => ({ slugs: state.slugs }),
      onRehydrateStorage: () => (state) => {
        if (state) writeCompareSlugsToCookie(state.slugs)
      },
    },
  ),
)

useCompareStore.subscribe((state, prev) => {
  if (state.slugs !== prev.slugs) writeCompareSlugsToCookie(state.slugs)
})
