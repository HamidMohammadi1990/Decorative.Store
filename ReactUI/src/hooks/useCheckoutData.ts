import { useEffect, useMemo, useRef, useState } from 'react'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { CheckoutProductSession, CheckoutUserAddress } from '@/models/checkout/checkout.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { checkoutService } from '@/services/checkoutService'
import { useUserStore } from '@/stores/userStore'

interface UseCheckoutDataResult {
  sessions: CheckoutProductSession[]
  addresses: CheckoutUserAddress[]
  loading: boolean
  error: string | null
}

function buildProductIdsKey(lines: CartLine[]) {
  return [...new Set(lines.map((line) => line.sku))].sort().join('|')
}

export function useCheckoutData(lines: CartLine[]): UseCheckoutDataResult {
  const { locale } = useLocaleSettings()
  const userId = useUserStore((s) => s.user?.id ?? '')
  const [sessions, setSessions] = useState<CheckoutProductSession[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [authReady, setAuthReady] = useState(() => useUserStore.persist.hasHydrated())
  const productTitlesRef = useRef(new Map<string, string>())

  const productIdsKey = useMemo(() => buildProductIdsKey(lines), [lines])

  useEffect(() => {
    if (useUserStore.persist.hasHydrated()) {
      setAuthReady(true)
      return
    }

    return useUserStore.persist.onFinishHydration(() => {
      setAuthReady(true)
    })
  }, [])

  useEffect(() => {
    for (const line of lines) {
      productTitlesRef.current.set(line.sku, line.title)
    }
  }, [lines])

  useEffect(() => {
    if (!authReady || !productIdsKey) {
      if (!productIdsKey) {
        setSessions([])
        setLoading(false)
        setError(null)
      }
      return
    }

    const productIds = productIdsKey.split('|')
    const accessToken = useUserStore.getState().accessToken
    let cancelled = false

    setLoading(true)
    setError(null)

    void Promise.all(
      productIds.map(async (productId) => {
        const data = await checkoutService.getProductCheckout(
          productId,
          locale,
          accessToken,
          userId,
        )
        return {
          productId,
          productTitle: productTitlesRef.current.get(productId) ?? data.product.title,
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
  }, [authReady, locale, productIdsKey, userId])

  const addresses = useMemo(() => {
    const map = new Map<string, CheckoutUserAddress>()
    for (const session of sessions) {
      for (const address of session.data.addresses) {
        map.set(address.id, address)
      }
    }
    return [...map.values()]
  }, [sessions])

  return { sessions, addresses, loading, error }
}
