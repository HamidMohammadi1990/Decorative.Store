import { useCallback, useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom'
import type { ProductListingResult } from '@/models/catalog/listing.model'
import { catalogService } from '@/services/catalogService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseProductListingResult {
  data: ProductListingResult | null
  loading: boolean
  error: string | null
  reload: () => void
}

export function useProductListing(): UseProductListingResult {
  const locale = useSettingsStore((s) => s.locale)
  const location = useLocation()
  const [data, setData] = useState<ProductListingResult | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const result = await catalogService.getListing(
        {
          pathname: location.pathname,
          searchParams: new URLSearchParams(location.search),
        },
        locale,
      )
      setData(result)
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale, location.pathname, location.search])

  useEffect(() => {
    void load()
  }, [load])

  return { data, loading, error, reload: load }
}
