import { useCallback, useEffect, useRef, useState } from 'react'
import { useParams, useRouteLoaderData } from 'react-router-dom'
import type { BlogListingResult } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import type { BlogListingLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

function readListingLoaderData(
  indexData: BlogListingLoaderData | undefined,
  categoryData: BlogListingLoaderData | undefined,
  categorySlug: string | undefined,
): BlogListingLoaderData | undefined {
  if (categorySlug) return categoryData
  return indexData
}

function isLoaderFresh(
  loaderData: BlogListingLoaderData | undefined,
  locale: string,
  categorySlug: string | undefined,
): loaderData is BlogListingLoaderData & { data: BlogListingResult } {
  return Boolean(
    loaderData &&
      loaderData.locale === locale &&
      loaderData.categorySlug === categorySlug &&
      loaderData.data,
  )
}

export function useBlogListing() {
  const { categorySlug } = useParams<{ categorySlug?: string }>()
  const indexLoaderData = useRouteLoaderData('blog-index') as BlogListingLoaderData | undefined
  const categoryLoaderData = useRouteLoaderData('blog-category') as BlogListingLoaderData | undefined
  const loaderData = readListingLoaderData(indexLoaderData, categoryLoaderData, categorySlug)
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderFresh = isLoaderFresh(loaderData, locale, categorySlug)

  const [data, setData] = useState<BlogListingResult | null>(() =>
    loaderFresh ? loaderData.data : null,
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderFresh ? (loaderData.error ?? null) : null,
  )
  const dataRef = useRef(data)
  dataRef.current = data

  const load = useCallback(async () => {
    if (!dataRef.current) {
      setLoading(true)
    }
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
    if (loaderFresh) {
      setData(loaderData.data)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData])

  return { data, loading, error, categorySlug }
}
