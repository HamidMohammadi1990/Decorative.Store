import { useEffect } from 'react'

/** Injects `<link rel="preload" as="image">` for LCP candidates. */
export function usePreloadImage(href: string | undefined, priority = false) {
  useEffect(() => {
    if (!priority || !href || typeof document === 'undefined') return

    const existing = document.querySelector(`link[rel="preload"][href="${href}"]`)
    if (existing) return

    const link = document.createElement('link')
    link.rel = 'preload'
    link.as = 'image'
    link.href = href
    document.head.appendChild(link)

    return () => {
      link.remove()
    }
  }, [href, priority])
}
