import { useCallback, useEffect, useRef, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { BlogCategory } from '@/models/blog/blog.model'
import { isBlogLayoutFetchKey } from '@/extensions/blogRoute'
import { blogService } from '@/services/blogService'
import type { ShopLayoutLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

interface UseBlogNavOptions {
  enabled?: boolean
}

export function useBlogNav({ enabled = false }: UseBlogNavOptions = {}) {
  const loaderData = useRouteLoaderData('shop') as ShopLayoutLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderFresh = Boolean(
    enabled &&
      loaderData &&
      loaderData.locale === locale &&
      isBlogLayoutFetchKey(loaderData.fetchKey),
  )

  const [categories, setCategories] = useState<BlogCategory[]>(() =>
    loaderFresh ? loaderData!.blogNavCategories : [],
  )
  const [loading, setLoading] = useState(enabled && !loaderFresh)
  const categoriesRef = useRef(categories)
  categoriesRef.current = categories

  const load = useCallback(async () => {
    if (!enabled) {
      setCategories([])
      setLoading(false)
      return
    }

    if (!categoriesRef.current.length) {
      setLoading(true)
    }

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
    if (!enabled) {
      setCategories([])
      setLoading(false)
      return
    }

    if (loaderFresh) {
      setCategories(loaderData!.blogNavCategories)
      setLoading(false)
      return
    }

    void load()
  }, [enabled, load, loaderFresh, loaderData])

  return { categories, loading }
}
