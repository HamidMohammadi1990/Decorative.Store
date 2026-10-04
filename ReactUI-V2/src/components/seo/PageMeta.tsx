import { usePageMeta } from '@/hooks/usePageMeta'
import type { PageMetaInput } from '@/seo/pageMetaManager'

interface PageMetaProps extends PageMetaInput {
  /** When false, meta tags are not applied (e.g. loading state). */
  active?: boolean
}

export function PageMeta({ active = true, ...meta }: PageMetaProps) {
  usePageMeta(active ? meta : null)
  return null
}
