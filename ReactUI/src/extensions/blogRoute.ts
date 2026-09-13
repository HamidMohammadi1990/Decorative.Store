export function isBlogRoute(pathname: string) {
  return pathname === '/blog' || pathname.startsWith('/blog/')
}

/** Shop layout loader fetchKey prefix when `skipCatalogNav` is set (blog routes only). */
export function isBlogLayoutFetchKey(fetchKey: string | undefined): boolean {
  return Boolean(fetchKey?.startsWith('1:'))
}
