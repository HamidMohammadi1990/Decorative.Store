import { useCallback, useState } from 'react'
import { cartService, mapServerCartToLines } from '@/services/cartService'
import { useCartStore } from '@/stores/cartStore'
import { useAccessToken } from '@/stores/userStore'

export function useCartMutations() {
  const accessToken = useAccessToken()
  const setLines = useCartStore((s) => s.setLines)
  const removeLineLocal = useCartStore((s) => s.removeLine)
  const updateQuantityLocal = useCartStore((s) => s.updateQuantity)
  const [mutating, setMutating] = useState(false)

  const isServerCart = Boolean(accessToken && accessToken !== 'mock-access-token')

  const applyCartResponse = useCallback(
    (cart: Awaited<ReturnType<typeof cartService.getCart>>) => {
      setLines(mapServerCartToLines(cart))
    },
    [setLines],
  )

  const removeLine = useCallback(
    async (lineId: string) => {
      if (!isServerCart) {
        removeLineLocal(lineId)
        return
      }

      setMutating(true)
      try {
        const cart = await cartService.removeItem(accessToken!, lineId)
        applyCartResponse(cart)
      } finally {
        setMutating(false)
      }
    },
    [accessToken, applyCartResponse, isServerCart, removeLineLocal],
  )

  const updateQuantity = useCallback(
    async (lineId: string, quantity: number) => {
      const line = useCartStore.getState().lines.find((item) => item.lineId === lineId)
      if (!line || quantity === line.quantity) return

      if (quantity < 1) {
        await removeLine(lineId)
        return
      }

      if (!isServerCart) {
        updateQuantityLocal(lineId, quantity)
        return
      }

      setMutating(true)
      try {
        const cart = await cartService.updateItemQuantity(accessToken!, lineId, quantity)
        applyCartResponse(cart)
      } finally {
        setMutating(false)
      }
    },
    [accessToken, applyCartResponse, isServerCart, removeLine, updateQuantityLocal],
  )

  return {
    removeLine,
    updateQuantity,
    mutating,
    isServerCart,
  }
}
