import { useCallback, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import type { BlogListingResult } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import { useSettingsStore } from '@/stores/settingsStore'

export function useBlogListing() {
  const { categorySlug } = useParams<{ categorySlug?: string }>()
  const locale = useSettingsStore((s) => s.locale)
  const [data, setData] = useState<BlogListingResult | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)

    try {
      const result = await blogService.getListing(locale, categorySlug)
      setData(result)
    } catch {
      setError('failed')
      setData(null)
    } finally {
      setLoading(false)
    }
  }, [categorySlug, locale])

  useEffect(() => {
    void load()
  }, [load])

  return { data, loading, error, categorySlug }
}
