import { useEffect, useRef } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useWishlistStore } from '@/stores/wishlistStore'
import { useIsAuthenticated, useUserStore } from '@/stores/userStore'

/** Load the user's wishlist from the backend when authenticated. */
export function useWishlistSync() {
  const isAuthenticated = useIsAuthenticated()
  const { locale } = useLocaleSettings()
  const loadWishlist = useWishlistStore((state) => state.loadWishlist)
  const clear = useWishlistStore((state) => state.clear)
  const syncedKeyRef = useRef<string | null>(null)

  useEffect(() => {
    if (!isAuthenticated) {
      syncedKeyRef.current = null
      clear()
      return
    }

    void (async () => {
      const accessToken = useUserStore.getState().accessToken
      if (!accessToken || accessToken === 'mock-access-token') return

      const syncKey = `${accessToken}:${locale}`
      if (syncedKeyRef.current === syncKey) return

      syncedKeyRef.current = syncKey
      try {
        await loadWishlist(accessToken, locale)
      } catch {
        syncedKeyRef.current = null
      }
    })()
  }, [clear, isAuthenticated, loadWishlist, locale])
}
