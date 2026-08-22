import { useEffect, useRef } from 'react'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useAddressStore } from '@/stores/addressStore'
import { useIsAuthenticated, useUserStore } from '@/stores/userStore'

/** Load saved user addresses from the backend when authenticated. */
export function useAddressSync() {
  const isAuthenticated = useIsAuthenticated()
  const { locale } = useLocaleSettings()
  const loadAddresses = useAddressStore((state) => state.loadAddresses)
  const clearAddresses = useAddressStore((state) => state.clearAddresses)
  const syncedTokenRef = useRef<string | null>(null)

  useEffect(() => {
    if (!isAuthenticated) {
      syncedTokenRef.current = null
      clearAddresses()
      return
    }

    void (async () => {
      const accessToken = useUserStore.getState().accessToken
      if (!accessToken || accessToken === 'mock-access-token') return
      if (syncedTokenRef.current === accessToken) return

      syncedTokenRef.current = accessToken
      await loadAddresses(accessToken, locale)
    })()
  }, [clearAddresses, isAuthenticated, loadAddresses, locale])
}
