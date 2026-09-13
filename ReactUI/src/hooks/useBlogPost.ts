import { useCallback, useEffect, useRef, useState } from 'react'
import { useParams, useRouteLoaderData } from 'react-router-dom'
import type { BlogPostDetail, BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import type { BlogDetailLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

function isLoaderFresh(
  loaderData: BlogDetailLoaderData | undefined,
  slug: string | undefined,
  locale: string,
): loaderData is BlogDetailLoaderData {
  return Boolean(loaderData && loaderData.slug === slug && loaderData.locale === locale)
}

export function useBlogPost() {
  const { slug } = useParams<{ slug: string }>()
  const loaderData = useRouteLoaderData('blog-detail') as BlogDetailLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderFresh = isLoaderFresh(loaderData, slug, locale)

  const [post, setPost] = useState<BlogPostDetail | null>(() =>
    loaderFresh ? (loaderData.post ?? null) : null,
  )
  const [related, setRelated] = useState<BlogPostSummary[]>(() =>
    loaderFresh ? loaderData.related : [],
  )
  const [categoryMap, setCategoryMap] = useState<Record<string, string>>(() =>
    loaderFresh ? loaderData.categoryMap : {},
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [error, setError] = useState<string | null>(() =>
    loaderFresh ? (loaderData.error ?? null) : null,
  )
  const postRef = useRef(post)
  postRef.current = post

  const load = useCallback(async () => {
    if (!slug) {
      setError('not-found')
      setLoading(false)
      return
    }

    if (!postRef.current) {
      setLoading(true)
    }
    setError(null)

    try {
      const { post: detail, related: relatedPosts, categoryMap: labels } =
        await blogService.getPostDetail(slug, locale)

      if (!detail) {
        setError('not-found')
        setPost(null)
        setRelated([])
        setCategoryMap(labels)
        return
      }

      setPost(detail)
      setRelated(relatedPosts)
      setCategoryMap(labels)
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale, slug])

  const reload = useCallback(async () => {
    if (!slug) return

    const { post: detail, related: relatedPosts, categoryMap: labels } =
      await blogService.getPostDetail(slug, locale, true)

    if (!detail) return

    setPost(detail)
    setRelated(relatedPosts)
    setCategoryMap(labels)
  }, [locale, slug])

  useEffect(() => {
    if (loaderFresh) {
      setPost(loaderData.post)
      setRelated(loaderData.related)
      setCategoryMap(loaderData.categoryMap)
      setError(loaderData.error ?? null)
      setLoading(false)
      return
    }

    void load()
  }, [load, loaderFresh, loaderData])

  return { post, related, categoryMap, loading, error, reload }
}
