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
      const [detail, relatedPosts] = await Promise.all([
        blogService.getPost(slug, locale),
        blogService.getRelatedPosts(slug, locale),
      ])

      if (!detail) {
        setError('not-found')
        setPost(null)
        setRelated([])
      } else {
        setPost(detail)
        setRelated(relatedPosts)
      }
    } catch {
      setError('failed')
    } finally {
      setLoading(false)
    }
  }, [locale, slug])

  useEffect(() => {
    void load()
  }, [load])

  return { post, related, loading, error }
}
