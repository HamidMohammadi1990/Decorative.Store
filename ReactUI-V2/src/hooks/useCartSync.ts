import { useEffect, useRef } from 'react'
import { useLocation } from 'react-router-dom'
import { hasPendingCartAdd } from '@/extensions/pendingCartAdd'
import { cartService, mapServerCartToLines } from '@/services/cartService'
import { useCartStore } from '@/stores/cartStore'
import { useIsAuthenticated } from '@/stores/userStore'

/** Load the server-side pending order into the local cart drawer. */
export function useCartSync() {
  const location = useLocation()
  const isAuthenticated = useIsAuthenticated()
  const setLines = useCartStore((s) => s.setLines)
  const syncedTokenRef = useRef<string | null>(null)

  useEffect(() => {
    if (location.pathname === '/checkout' || location.pathname === '/cart' || location.pathname.startsWith('/account/dashboard/cart')) return

    if (!isAuthenticated) {
      syncedTokenRef.current = null
      return
    }

    if (hasPendingCartAdd()) return

    void (async () => {
      const { useUserStore } = await import('@/stores/userStore')
      const accessToken = useUserStore.getState().accessToken
      if (!accessToken || accessToken === 'mock-access-token') return
      if (syncedTokenRef.current === accessToken) return

      syncedTokenRef.current = accessToken

      try {
        const cart = await cartService.getCart(accessToken)
        setLines(mapServerCartToLines(cart))
      } catch {
        syncedTokenRef.current = null
        // Keep the existing local cart when the server cart cannot be loaded.
      }
    })()
  }, [isAuthenticated, location.pathname, setLines])
}
