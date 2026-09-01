import { useEffect } from 'react'
import { useAddressStore } from '@/stores/addressStore'
import { useIsAuthenticated } from '@/stores/userStore'

/** Clear cached addresses when the user signs out. */
export function useAddressSync() {
  const isAuthenticated = useIsAuthenticated()
  const clearAddresses = useAddressStore((state) => state.clearAddresses)

  useEffect(() => {
    if (!isAuthenticated) {
      clearAddresses()
    }
  }, [clearAddresses, isAuthenticated])
}
