/** Resolve API origin for the current runtime (browser proxy vs Node SSR fetch). */
export function resolveApiBaseUrl(): string {
  const configured = (import.meta.env.VITE_API_BASE_URL as string | undefined)?.trim()
  if (configured) return configured.replace(/\/+$/, '')

  if (import.meta.env.SSR && typeof process !== 'undefined') {
    const ssrTarget = process.env?.SSR_API_TARGET?.trim()
    if (ssrTarget) return ssrTarget.replace(/\/+$/, '')
  }

  return ''
}

/**
 * Same-origin URL for img src and public media (/Uploads, /images).
 * Do not prefix SSR_API_TARGET — browsers cannot reach http://api:8080 inside Docker.
 */
export function resolvePublicMediaUrl(url: string): string {
  if (!url) return ''
  const trimmed = url.trim()
  if (trimmed.startsWith('http://') || trimmed.startsWith('https://')) return trimmed
  return trimmed.startsWith('/') ? trimmed : `/${trimmed}`
}

/** @deprecated Prefer resolveApiBaseUrl() — kept for modules that read a module-level constant. */
export const API_BASE_URL = resolveApiBaseUrl()
