import { useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'
import type { HomePageLoaderData } from '@/routes/loaders/types'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'

export function useHomeFeaturedPosts() {
  const loaderData = useRouteLoaderData('home') as HomePageLoaderData | undefined
  const locale = useStorefrontLocale(loaderData?.locale)
  const loaderLocaleFresh = loaderData?.locale === locale

  const [posts, setPosts] = useState<BlogPostSummary[]>(
    () => loaderData?.featuredPosts ?? [],
  )

  useEffect(() => {
    if (loaderLocaleFresh) {
      setPosts(loaderData!.featuredPosts)
      return
    }

    let cancelled = false

    void blogService.getFeaturedPosts(locale, 3).then((featuredPosts) => {
      if (cancelled) return
      setPosts(featuredPosts)
    })

    return () => {
      cancelled = true
    }
  }, [locale, loaderLocaleFresh, loaderData])

  return posts
}
