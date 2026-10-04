/** Removes locale hint query params from canonical URLs. */
export function stripLocaleHintFromPath(path: string): string {
  const [pathname, search = ''] = path.split('?')
  if (!search) return pathname

  const params = new URLSearchParams(search)
  params.delete('hl')
  const nextSearch = params.toString()
  return nextSearch ? `${pathname}?${nextSearch}` : pathname
}
