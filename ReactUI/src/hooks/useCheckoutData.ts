import { useEffect, useMemo, useState } from 'react'
import type { CartLine } from '@/models/cart/cartLine.model'
import type {
  CheckoutDeliveryType,
  CheckoutProductSession,
  CheckoutUserAddress,
} from '@/models/checkout/checkout.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { checkoutService } from '@/services/checkoutService'
import { useAccessToken } from '@/stores/userStore'

interface UseCheckoutDataResult {
  sessions: CheckoutProductSession[]
  deliveryTypes: CheckoutDeliveryType[]
  addresses: CheckoutUserAddress[]
  loading: boolean
  error: string | null
}

export function useCheckoutData(lines: CartLine[]): UseCheckoutDataResult {
  const { locale } = useLocaleSettings()
  const accessToken = useAccessToken()
  const [sessions, setSessions] = useState<CheckoutProductSession[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const productIds = useMemo(
    () => [...new Set(lines.map((line) => line.sku))],
    [lines],
  )

  useEffect(() => {
    if (productIds.length === 0) {
      setSessions([])
      setLoading(false)
      setError(null)
      return
    }

    let cancelled = false
    setLoading(true)
    setError(null)

    void Promise.all(
      productIds.map(async (productId) => {
        const data = await checkoutService.getProductCheckout(productId, locale, accessToken)
        const line = lines.find((item) => item.sku === productId)
        return {
          productId,
          productTitle: line?.title ?? data.product.title,
          data,
        }
      }),
    )
      .then((nextSessions) => {
        if (!cancelled) {
          setSessions(nextSessions)
          setLoading(false)
        }
      })
      .catch(() => {
        if (!cancelled) {
          setSessions([])
          setError('checkout.loadError')
          setLoading(false)
        }
      })

    return () => {
      cancelled = true
    }
  }, [accessToken, lines, locale, productIds])

  const deliveryTypes = useMemo(() => {
    const map = new Map<string, CheckoutDeliveryType>()
    for (const session of sessions) {
      for (const deliveryType of session.data.deliveryTypes) {
        map.set(deliveryType.id, deliveryType)
      }
    }
    return [...map.values()]
  }, [sessions])

  const addresses = useMemo(() => {
    const map = new Map<string, CheckoutUserAddress>()
    for (const session of sessions) {
      for (const address of session.data.addresses) {
        map.set(address.id, address)
      }
    }
    return [...map.values()]
  }, [sessions])

  return { sessions, deliveryTypes, addresses, loading, error }
}
