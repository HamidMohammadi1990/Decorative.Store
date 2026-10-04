import { useCallback, useEffect, useState } from 'react'
import {
  cartService,
  mapServerCartToLines,
  type ServerCartSummary,
} from '@/services/cartService'
import { useCartMutations } from '@/hooks/useCartMutations'
import { useCartStore } from '@/stores/cartStore'
import { useAccessToken } from '@/stores/userStore'

export function useCartPage() {
  const accessToken = useAccessToken()
  const lines = useCartStore((s) => s.lines)
  const setLines = useCartStore((s) => s.setLines)
  const clearCartLocal = useCartStore((s) => s.clearCart)
  const { removeLine, updateQuantity, mutating, isServerCart } = useCartMutations()

  const [summary, setSummary] = useState<ServerCartSummary | null>(null)
  const [trackingCode, setTrackingCode] = useState<number | null>(null)
  const [loading, setLoading] = useState(true)
  const [clearing, setClearing] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const applyCartResponse = useCallback(
    (cart: Awaited<ReturnType<typeof cartService.getCart>>) => {
      setLines(mapServerCartToLines(cart))
      setSummary(cart.summary)
      setTrackingCode(cart.trackingCode ?? null)
    },
    [setLines],
  )

  const refresh = useCallback(async () => {
    if (!isServerCart) {
      setSummary(null)
      setTrackingCode(null)
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const cart = await cartService.getCart(accessToken!)
      applyCartResponse(cart)
    } catch {
      setError('loadFailed')
    } finally {
      setLoading(false)
    }
  }, [accessToken, applyCartResponse, isServerCart])

  useEffect(() => {
    void refresh()
  }, [refresh])

  const clearCart = useCallback(async () => {
    if (!isServerCart) {
      clearCartLocal()
      return
    }

    setClearing(true)
    setError(null)

    try {
      for (const line of [...lines]) {
        await cartService.removeItem(accessToken!, line.lineId)
      }
      applyCartResponse(await cartService.getCart(accessToken!))
    } catch {
      setError('updateFailed')
    } finally {
      setClearing(false)
    }
  }, [accessToken, applyCartResponse, clearCartLocal, isServerCart, lines])

  return {
    lines,
    summary,
    trackingCode,
    loading,
    mutating: mutating || clearing,
    error,
    refresh,
    removeLine,
    updateQuantity,
    clearCart,
    isServerCart,
  }
}