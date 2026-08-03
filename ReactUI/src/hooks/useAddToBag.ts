import { useCallback, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import type { ImageAsset } from '@/models/shared/image.model'
import type { Money } from '@/models/shared/money.model'
import { savePendingCartAdd } from '@/extensions/pendingCartAdd'
import { cartService, mapServerCartToLines } from '@/services/cartService'
import { openLoginModal } from '@/stores/authModalStore'
import { useCartStore } from '@/stores/cartStore'
import { useAccessToken, useIsAuthenticated } from '@/stores/userStore'

export interface AddToBagItem {
  sku: string
  title: string
  image: ImageAsset
  unitPrice: Money
  quantity?: number
  inStock?: boolean
  productSlug?: string
}

export type AddToBagStatus = 'idle' | 'adding' | 'added'

export function useAddToBag() {
  const navigate = useNavigate()
  const location = useLocation()
  const isAuthenticated = useIsAuthenticated()
  const accessToken = useAccessToken()
  const addLine = useCartStore((s) => s.addLine)
  const setLines = useCartStore((s) => s.setLines)
  const [status, setStatus] = useState<AddToBagStatus>('idle')

  const addToBag = useCallback(
    async (item: AddToBagItem) => {
      const quantity = item.quantity ?? 1
      const returnUrl = `${location.pathname}${location.search}`

      if (item.inStock === false && item.productSlug) {
        navigate(`/product/${item.productSlug}`)
        return false
      }

      if (!isAuthenticated) {
        savePendingCartAdd({
          sku: item.sku,
          title: item.title,
          image: item.image,
          unitPrice: item.unitPrice,
          quantity,
          returnUrl,
        })
        openLoginModal()
        return false
      }

      setStatus('adding')

      try {
        if (accessToken && accessToken !== 'mock-access-token') {
          const cart = await cartService.addItem(accessToken, item.sku, quantity)
          setLines(mapServerCartToLines(cart))
        } else {
          addLine({
            sku: item.sku,
            title: item.title,
            image: item.image,
            unitPrice: item.unitPrice,
            quantity,
          })
        }

        setStatus('added')
        window.setTimeout(() => setStatus('idle'), 1800)
        return true
      } catch (error) {
        setStatus('idle')

        if (cartService.isConfigurationRequired(error) && item.productSlug) {
          navigate(`/product/${item.productSlug}`)
          return false
        }

        throw error
      }
    },
    [
      accessToken,
      addLine,
      isAuthenticated,
      location.pathname,
      location.search,
      navigate,
      setLines,
    ],
  )

  return { addToBag, status, isAuthenticated }
}
