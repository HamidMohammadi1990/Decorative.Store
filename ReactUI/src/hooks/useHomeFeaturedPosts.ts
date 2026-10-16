import { useEffect, useState } from 'react'
import type { BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import type { HomePageLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

export function useHomeFeaturedPosts(loaderData: HomePageLoaderData) {
  const locale = useStorefrontLocale(loaderData.locale)

  const [posts, setPosts] = useState<BlogPostSummary[]>(() => loaderData.featuredPosts ?? [])

  useEffect(() => {
    let cancelled = false

    void blogService
      .getHomeJournalPosts(locale, 3, true)
      .then((journalPosts) => {
        if (!cancelled) setPosts(journalPosts)
      })
      .catch(() => {
        if (!cancelled && loaderData.featuredPosts?.length) {
          setPosts(loaderData.featuredPosts)
        }
      })

    return () => {
      cancelled = true
    }
  }, [locale, loaderData.featuredPosts])

  return posts
}
