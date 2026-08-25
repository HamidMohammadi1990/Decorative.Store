import { useCallback, useEffect, useState } from 'react'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { catalogService } from '@/services/catalogService'
import { wishlistService } from '@/services/wishlistService'
import { useSettingsStore } from '@/stores/settingsStore'
import { useWishlistStore } from '@/stores/wishlistStore'
import { useUserStore } from '@/stores/userStore'

interface UseWishlistProductsResult {
  products: ProductDetail[]
  loading: boolean
  error: string | null
  reload: () => void
}

export function useWishlistProducts(): UseWishlistProductsResult {
  const locale = useSettingsStore((s) => s.locale)
  const slugs = useWishlistStore((s) => s.slugs)
  const removeSlug = useWishlistStore((s) => s.removeSlug)
  const accessToken = useUserStore((s) => s.accessToken)
  const [products, setProducts] = useState<ProductDetail[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (slugs.length === 0) {
      setProducts([])
      setLoading(false)
      setError(null)
      return
    }

    setLoading(true)
    setError(null)

    try {
      const details = await catalogService.getProductsBySlugs(slugs, locale)
      const order = new Map(slugs.map((slug, index) => [slug, index]))
      const sorted = [...details].sort(
        (a, b) => (order.get(a.slug) ?? 0) - (order.get(b.slug) ?? 0),
      )
      setProducts(sorted)

      const missing = slugs.filter((slug) => !details.some((p) => p.slug === slug))
      if (missing.length > 0) {
        for (const slug of missing) {
          removeSlug(slug)
          if (accessToken && accessToken !== 'mock-access-token') {
            try {
              await wishlistService.remove(accessToken, slug)
            } catch {
              // Best-effort cleanup for deleted catalog products.
            }
          }
        }
      }
    } catch {
      setError('failed')
      setProducts([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale, removeSlug, slugs])

  useEffect(() => {
    void load()
  }, [load])

  return { products, loading, error, reload: load }
}
