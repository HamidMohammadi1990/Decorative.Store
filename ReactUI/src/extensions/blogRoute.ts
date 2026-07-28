export function isBlogRoute(pathname: string) {
  return pathname === '/blog' || pathname.startsWith('/blog/')
}
