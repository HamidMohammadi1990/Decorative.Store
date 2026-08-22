import { useEffect } from 'react'
import { consumePendingCartAdd } from '@/extensions/pendingCartAdd'
import { cartService, mapServerCartToLines } from '@/services/cartService'
import { useCartStore } from '@/stores/cartStore'
import { useAccessToken, useUserStore } from '@/stores/userStore'

/** After sign-in, apply a product the user tried to add while logged out. */
export function useResumePendingCartAdd() {
  const user = useUserStore((s) => s.user)
  const accessToken = useAccessToken()
  const addLine = useCartStore((s) => s.addLine)
  const setLines = useCartStore((s) => s.setLines)
  const openCart = useCartStore((s) => s.openCart)

  useEffect(() => {
    if (!user || !accessToken) return

    const pending = consumePendingCartAdd()
    if (!pending) return

    if (accessToken === 'mock-access-token') {
      addLine({
        sku: pending.sku,
        title: pending.title,
        image: pending.image,
        unitPrice: pending.unitPrice,
        quantity: pending.quantity,
      })
      return
    }

    void cartService
      .addItem(accessToken, pending.sku, pending.quantity)
      .then((cart) => {
        setLines(mapServerCartToLines(cart))
        openCart()
      })
      .catch(() => {
        addLine({
          sku: pending.sku,
          title: pending.title,
          image: pending.image,
          unitPrice: pending.unitPrice,
          quantity: pending.quantity,
        })
      })
  }, [accessToken, addLine, openCart, setLines, user])
}
