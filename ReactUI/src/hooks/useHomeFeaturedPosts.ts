import { useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import type { HomePageLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

export function useHomeFeaturedPosts() {
  const loaderData = useRouteLoaderData('home') as HomePageLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)

  const [posts, setPosts] = useState<BlogPostSummary[]>(
    () => loaderData?.featuredPosts ?? [],
  )

  useEffect(() => {
    let cancelled = false

    void blogService.getHomeJournalPosts(locale, 3).then((journalPosts) => {
      if (cancelled) return
      setPosts(journalPosts)
    })

    return () => {
      cancelled = true
    }
  }, [locale])

  return posts
}
