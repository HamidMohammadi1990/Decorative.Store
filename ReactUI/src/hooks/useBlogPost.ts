import { useCallback, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import type { BlogPostDetail, BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import { useSettingsStore } from '@/stores/settingsStore'

export function useBlogPost() {
  const { slug } = useParams<{ slug: string }>()
  const locale = useSettingsStore((s) => s.locale)
  const [post, setPost] = useState<BlogPostDetail | null>(null)
  const [related, setRelated] = useState<BlogPostSummary[]>([])
  const [categoryMap, setCategoryMap] = useState<Record<string, string>>({})
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async () => {
    if (!slug) {
      setError('not-found')
      setLoading(false)
      return
    }

    setLoading(true)
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
    void load()
  }, [load])

  return { post, related, categoryMap, loading, error, reload }
}
