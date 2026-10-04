import { useEffect, useMemo } from 'react'
import type { PageMetaInput } from '@/seo/pageMetaManager'
import { applyPageMeta, resetPageMeta } from '@/seo/pageMetaManager'
import { registerSsrPageMeta } from '@/seo/ssrPageMeta'

function serializeMeta(meta: PageMetaInput | null | undefined): string {
  if (!meta) return ''
  return JSON.stringify(meta)
}

/** Applies document head tags (title, meta, OG, canonical, JSON-LD) for the current page. */
export function usePageMeta(meta: PageMetaInput | null | undefined) {
  const key = useMemo(() => serializeMeta(meta), [meta])

  if (import.meta.env.SSR && meta) {
    registerSsrPageMeta(meta)
  }

  useEffect(() => {
    if (!meta) return
    applyPageMeta(meta)
    return () => resetPageMeta()
  }, [key, meta])
}
