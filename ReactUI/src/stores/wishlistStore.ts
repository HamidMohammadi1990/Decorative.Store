import { create } from 'zustand'
import type { Locale } from '@/models/shared/locale.model'
import { wishlistService } from '@/services/wishlistService'

interface WishlistState {
  slugs: string[]
  isLoading: boolean
  setSlugs: (slugs: string[]) => void
  clear: () => void
  addSlug: (slug: string) => void
  removeSlug: (slug: string) => void
  isInWishlist: (slug: string) => boolean
  count: () => number
  loadWishlist: (accessToken: string, locale: Locale) => Promise<void>
  toggleRemote: (accessToken: string, locale: Locale, slug: string) => Promise<'added' | 'removed'>
}

export const useWishlistStore = create<WishlistState>()((set, get) => ({
  slugs: [],
  isLoading: false,

  setSlugs: (slugs) => set({ slugs }),

  clear: () => set({ slugs: [], isLoading: false }),

  addSlug: (slug) =>
    set((state) => ({
      slugs: state.slugs.includes(slug) ? state.slugs : [...state.slugs, slug],
    })),

  removeSlug: (slug) =>
    set((state) => ({
      slugs: state.slugs.filter((item) => item !== slug),
    })),

  isInWishlist: (slug) => get().slugs.includes(slug),

  count: () => get().slugs.length,

  loadWishlist: async (accessToken, locale) => {
    set({ isLoading: true })
    try {
      const slugs = await wishlistService.getMy(accessToken, locale)
      set({ slugs, isLoading: false })
    } catch {
      set({ isLoading: false })
    }
  },

  toggleRemote: async (accessToken, locale, slug) => {
    const isCurrentlyWishlisted = get().isInWishlist(slug)

    if (isCurrentlyWishlisted) {
      await wishlistService.remove(accessToken, slug)
      get().removeSlug(slug)
      return 'removed'
    }

    await wishlistService.add(accessToken, locale, slug)
    get().addSlug(slug)
    return 'added'
  },
}))
