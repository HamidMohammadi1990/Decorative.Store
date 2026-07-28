import { useCallback, useEffect, useState } from 'react'
import type { BlogCategory } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import { useSettingsStore } from '@/stores/settingsStore'

interface UseBlogNavOptions {
  enabled?: boolean
}

export function useBlogNav({ enabled = true }: UseBlogNavOptions = {}) {
  const locale = useSettingsStore((s) => s.locale)
  const [categories, setCategories] = useState<BlogCategory[]>([])
  const [loading, setLoading] = useState(enabled)

  const load = useCallback(async () => {
    if (!enabled) return

    setLoading(true)

    try {
      const result = await blogService.getNavCategories(locale)
      setCategories(result)
    } catch {
      setCategories([])
    } finally {
      setLoading(false)
    }
  }, [enabled, locale])

  useEffect(() => {
    void load()
  }, [load])

  return { categories, loading }
}
