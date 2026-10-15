/** Turn stored media (filename, /Uploads/..., or dev absolute URL) into a same-origin path. */
export function toPublicUploadPath(url: string): string {
  const trimmed = url.trim()
  if (!trimmed) return ''

  if (/^https?:\/\//i.test(trimmed)) {
    try {
      const pathname = new URL(trimmed).pathname
      if (pathname.startsWith('/Uploads/') || pathname.startsWith('/images/')) return pathname
    } catch {
      return trimmed
    }
    return trimmed
  }

  if (trimmed.startsWith('/')) return trimmed
  return trimmed
}

/**
 * Browser-safe media URL for img src — never uses SSR internal hosts (e.g. http://api:8080).
 */
export function resolveSameOriginMediaSrc(url: string, uploadFolder = 'Products'): string {
  if (!url?.trim()) return ''
  const path = toPublicUploadPath(url)
  if (!path) return ''
  if (path.startsWith('/images/') || path.startsWith('/Uploads/')) return path
  if (/^https?:\/\//i.test(path)) return path
  return `/Uploads/${uploadFolder}/${path.replace(/^\/+/, '')}`
}

export function resolveProductImageSrc(imageUrl: string): string {
  return resolveSameOriginMediaSrc(imageUrl, 'Products')
}
