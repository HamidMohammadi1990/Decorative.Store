/** When true, all storefront pages are noindex + robots.txt blocks crawlers. */
export function isSearchIndexingBlocked(): boolean {
  if (typeof process !== 'undefined' && process.env.BLOCK_SEARCH_INDEXING) {
    const v = process.env.BLOCK_SEARCH_INDEXING.trim().toLowerCase()
    return v === 'true' || v === '1' || v === 'yes'
  }

  const viteFlag = import.meta.env.VITE_BLOCK_SEARCH_INDEXING
  if (typeof viteFlag === 'string') {
    const v = viteFlag.trim().toLowerCase()
    return v === 'true' || v === '1' || v === 'yes'
  }

  return false
}

export function resolvePageNoindex(pageNoindex?: boolean): boolean {
  return Boolean(pageNoindex) || isSearchIndexingBlocked()
}
