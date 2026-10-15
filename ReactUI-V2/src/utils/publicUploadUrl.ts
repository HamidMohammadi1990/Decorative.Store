/** Turn stored media (filename, /Uploads/..., or dev absolute URL) into a same-origin path. */
export function toPublicUploadPath(url: string): string {
  const trimmed = url.trim()
  if (!trimmed) return ''

  if (/^https?:\/\//i.test(trimmed)) {
    try {
      const pathname = new URL(trimmed).pathname
      if (pathname.startsWith('/Uploads/')) return pathname
    } catch {
      return trimmed
    }
    return trimmed
  }

  if (trimmed.startsWith('/')) return trimmed
  return trimmed
}
