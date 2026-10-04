import { useCallback, useEffect, useState } from 'react'
import { cartService, type ServerCartSummary } from '@/services/cartService'
import { useAccessToken, useIsAuthenticated } from '@/stores/userStore'

export function useCheckoutDiscount() {
  const isAuthenticated = useIsAuthenticated()
  const accessToken = useAccessToken()

  const [summary, setSummary] = useState<ServerCartSummary | null>(null)
  const [appliedCode, setAppliedCode] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [applying, setApplying] = useState(false)
  const [errorKey, setErrorKey] = useState<string | null>(null)

  const refresh = useCallback(async () => {
    if (!isAuthenticated || !accessToken || accessToken === 'mock-access-token') {
      setSummary(null)
      setAppliedCode(null)
      return
    }

    setLoading(true)
    setErrorKey(null)

    try {
      const cart = await cartService.getCart(accessToken)
      setSummary(cart.summary)
      if (!cart.summary.isDiscountApplied) {
        setAppliedCode(null)
      }
    } catch {
      setSummary(null)
    } finally {
      setLoading(false)
    }
  }, [accessToken, isAuthenticated])

  useEffect(() => {
    void refresh()
  }, [refresh])

  const applyDiscount = useCallback(
    async (code: string) => {
      if (!accessToken || accessToken === 'mock-access-token') return false

      const trimmed = code.trim()
      if (!trimmed) return false

      setApplying(true)
      setErrorKey(null)

      try {
        const result = await cartService.validateDiscount(accessToken, trimmed)
        setAppliedCode(result.discountCode)
        setSummary({
          totalPrice: result.totalPrice,
          discountAmount: result.discountAmount,
          finalPrice: result.finalPrice,
          isDiscountApplied: result.discountAmount > 0,
          isDiscountInvalidated: false,
          discountInvalidationMessage: null,
        })
        return true
      } catch (error) {
        const codeKey = cartService.getDiscountErrorCode(error)
        setErrorKey(codeKey ? `checkout.discountErrors.${codeKey}` : 'checkout.discountErrors.generic')
        return false
      } finally {
        setApplying(false)
      }
    },
    [accessToken],
  )

  const removeDiscount = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') return false

    setApplying(true)
    setErrorKey(null)

    try {
      const cart = await cartService.removeDiscount(accessToken)
      setSummary(cart.summary)
      setAppliedCode(null)
      return true
    } catch {
      setErrorKey('checkout.discountErrors.generic')
      return false
    } finally {
      setApplying(false)
    }
  }, [accessToken])

  return {
    summary,
    appliedCode,
    loading,
    applying,
    errorKey,
    applyDiscount,
    removeDiscount,
    refresh,
    canApply: isAuthenticated && Boolean(accessToken) && accessToken !== 'mock-access-token',
  }
}
