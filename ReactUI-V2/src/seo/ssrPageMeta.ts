import type { PageMetaInput } from '@/seo/pageMetaManager'

let activeMeta: PageMetaInput | null = null

/** Records meta during SSR render (last write wins). */
export function registerSsrPageMeta(meta: PageMetaInput) {
  if (!import.meta.env.SSR) return
  activeMeta = meta
}

export function consumeSsrPageMeta(): PageMetaInput | null {
  const meta = activeMeta
  activeMeta = null
  return meta
}

export function resetSsrPageMeta() {
  activeMeta = null
}
