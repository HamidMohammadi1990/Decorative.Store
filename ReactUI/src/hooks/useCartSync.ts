import { useEffect, useRef } from 'react'
import { hasPendingCartAdd } from '@/extensions/pendingCartAdd'
import { cartService, mapServerCartToLines } from '@/services/cartService'
import { useCartStore } from '@/stores/cartStore'
import { useAccessToken, useUserStore } from '@/stores/userStore'

/** Load the server-side pending order into the local cart drawer. */
export function useCartSync() {
  const user = useUserStore((s) => s.user)
  const accessToken = useAccessToken()
  const setLines = useCartStore((s) => s.setLines)
  const syncedTokenRef = useRef<string | null>(null)

  useEffect(() => {
    if (!user || !accessToken || accessToken === 'mock-access-token') {
      syncedTokenRef.current = null
      return
    }

    if (syncedTokenRef.current === accessToken) return
    if (hasPendingCartAdd()) return

    syncedTokenRef.current = accessToken

    void cartService
      .getCart(accessToken)
      .then((cart) => setLines(mapServerCartToLines(cart)))
      .catch(() => {
        // Keep the existing local cart when the server cart cannot be loaded.
      })
  }, [accessToken, setLines, user])
}
