import { useEffect, useRef, useState } from 'react'

interface UseLazyImageSourceOptions {
  /** When true, load immediately (LCP / above-the-fold). */
  priority?: boolean
  /** IntersectionObserver root margin — load slightly before entering viewport. */
  rootMargin?: string
}

/**
 * Defers assigning the real image URL until the element is near the viewport.
 * Falls back to immediate load when priority is set or IO is unavailable.
 */
export function useLazyImageSource(
  src: string,
  { priority = false, rootMargin = '280px 0px' }: UseLazyImageSourceOptions = {},
) {
  const ref = useRef<HTMLElement | null>(null)
  const [resolvedSrc, setResolvedSrc] = useState(priority ? src : '')

  useEffect(() => {
    if (priority || !src) {
      setResolvedSrc(src)
      return
    }

    if (typeof window === 'undefined' || !('IntersectionObserver' in window)) {
      setResolvedSrc(src)
      return
    }

    const node = ref.current
    if (!node) {
      setResolvedSrc(src)
      return
    }

    let cancelled = false
    const observer = new IntersectionObserver(
      (entries) => {
        if (cancelled) return
        if (entries.some((entry) => entry.isIntersecting)) {
          setResolvedSrc(src)
          observer.disconnect()
        }
      },
      { rootMargin, threshold: 0.01 },
    )

    observer.observe(node)
    return () => {
      cancelled = true
      observer.disconnect()
    }
  }, [priority, rootMargin, src])

  useEffect(() => {
    if (priority) setResolvedSrc(src)
  }, [priority, src])

  return { ref, resolvedSrc: resolvedSrc || undefined, isLoaded: Boolean(resolvedSrc) }
}
