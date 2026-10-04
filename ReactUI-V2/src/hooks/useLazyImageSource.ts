import { useEffect, useRef, useState } from 'react'

interface UseLazyImageSourceOptions {
  /** When true, load immediately (LCP / above-the-fold). */
  priority?: boolean
  /** IntersectionObserver root margin — load slightly before entering viewport. */
  rootMargin?: string
}

function parseVerticalRootMargin(rootMargin: string): number {
  const top = rootMargin.trim().split(/\s+/)[0] ?? '0'
  const value = Number.parseInt(top, 10)
  return Number.isFinite(value) ? value : 0
}

function isNearViewport(rect: DOMRect, marginY: number): boolean {
  if (rect.width <= 0 && rect.height <= 0) return false
  return rect.bottom >= -marginY && rect.top <= window.innerHeight + marginY
}

/**
 * Defers assigning the real image URL until the element is near the viewport.
 * Falls back to immediate load when priority is set or IO is unavailable.
 *
 * The ref must be attached to a real box (e.g. the `<img>`), not `display: contents`.
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

    if (typeof window === 'undefined') {
      setResolvedSrc(src)
      return
    }

    let cancelled = false
    let observer: IntersectionObserver | undefined
    let rafId = 0

    const activate = () => {
      if (!cancelled) setResolvedSrc(src)
    }

    const bind = () => {
      observer?.disconnect()

      const node = ref.current
      if (!node) return false

      const marginY = parseVerticalRootMargin(rootMargin)
      const rect = node.getBoundingClientRect()
      if (isNearViewport(rect, marginY)) {
        activate()
        return true
      }

      if (!('IntersectionObserver' in window)) {
        activate()
        return true
      }

      observer = new IntersectionObserver(
        (entries) => {
          if (cancelled) return
          if (entries.some((entry) => entry.isIntersecting)) {
            activate()
            observer?.disconnect()
          }
        },
        { rootMargin, threshold: 0.01 },
      )
      observer.observe(node)
      return true
    }

    if (!bind()) {
      rafId = window.requestAnimationFrame(() => {
        if (!cancelled) bind()
      })
    }

    return () => {
      cancelled = true
      if (rafId) window.cancelAnimationFrame(rafId)
      observer?.disconnect()
    }
  }, [priority, rootMargin, src])

  useEffect(() => {
    if (priority) setResolvedSrc(src)
  }, [priority, src])

  return { ref, resolvedSrc: resolvedSrc || undefined, isLoaded: Boolean(resolvedSrc) }
}
