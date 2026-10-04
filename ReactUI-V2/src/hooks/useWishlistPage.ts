import { useCallback, useEffect, useState } from 'react'
import { useWishlistProducts } from '@/hooks/useWishlistProducts'
import { useSettingsStore } from '@/stores/settingsStore'
import { useWishlistStore } from '@/stores/wishlistStore'
import { useUserStore } from '@/stores/userStore'

export function useWishlistPage() {
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useUserStore((s) => s.accessToken)
  const slugs = useWishlistStore((s) => s.slugs)
  const isSyncing = useWishlistStore((s) => s.isLoading)
  const loadWishlist = useWishlistStore((s) => s.loadWishlist)
  const clearAllRemote = useWishlistStore((s) => s.clearAllRemote)

  const { products, loading: productsLoading, error: productsError, reload: reloadProducts } =
    useWishlistProducts()

  const [syncDone, setSyncDone] = useState(false)
  const [syncError, setSyncError] = useState(false)

  const syncWishlist = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setSyncError(false)
      setSyncDone(true)
      return
    }

    setSyncError(false)
    try {
      await loadWishlist(accessToken, locale)
    } catch {
      setSyncError(true)
    } finally {
      setSyncDone(true)
    }
  }, [accessToken, locale, loadWishlist])

  useEffect(() => {
    setSyncDone(false)
    void syncWishlist()
  }, [syncWishlist])

  const reload = useCallback(() => {
    setSyncDone(false)
    void syncWishlist().then(() => reloadProducts())
  }, [reloadProducts, syncWishlist])

  const loading =
    !syncDone || isSyncing || (syncDone && !syncError && slugs.length > 0 && productsLoading)

  const error = syncError || productsError !== null

  return {
    products,
    slugs,
    loading,
    error,
    reload,
    clearAllRemote,
    accessToken,
  }
}
