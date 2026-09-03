import { useMatches } from 'react-router-dom'
import type { CmsContentPageLoaderData } from '@/routes/loaders/types'

/** Finds CMS loader data from the active route match (static or dynamic slug routes). */
export function useCmsContentLoaderData(
  slug: string,
): CmsContentPageLoaderData | undefined {
  const matches = useMatches()

  for (const match of matches) {
    const data = match.data as CmsContentPageLoaderData | undefined
    if (data?.slug === slug) return data
  }

  return undefined
}
