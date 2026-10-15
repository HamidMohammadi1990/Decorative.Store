function parseBlockFlag(raw: string | undefined): boolean {
  if (!raw) return false
  const v = raw.trim().toLowerCase()
  return v === 'true' || v === '1' || v === 'yes'
}

/** When true, all storefront pages are noindex + robots.txt blocks crawlers. */
export function isSearchIndexingBlocked(): boolean {
  const fromNode =
    typeof process !== 'undefined' ? process.env?.BLOCK_SEARCH_INDEXING : undefined
  if (parseBlockFlag(fromNode)) return true

  const viteFlag = import.meta.env.VITE_BLOCK_SEARCH_INDEXING
  if (typeof viteFlag === 'string' && parseBlockFlag(viteFlag)) return true

  return false
}

export function resolvePageNoindex(pageNoindex?: boolean): boolean {
  return Boolean(pageNoindex) || isSearchIndexingBlocked()
}
