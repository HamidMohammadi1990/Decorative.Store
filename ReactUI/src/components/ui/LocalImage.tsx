import type { RefObject } from 'react'
import type { ImageAsset } from '@/models/shared/image.model'
import { useLazyImageSource } from '@/hooks/useLazyImageSource'

interface LocalImageProps {
  image: ImageAsset
  className?: string
  loading?: 'eager' | 'lazy'
  /** Shorthand for LCP images — eager load + fetchpriority high. */
  priority?: boolean
  fetchPriority?: 'high' | 'low' | 'auto'
  decoding?: 'async' | 'sync' | 'auto'
  sizes?: string
  rootMargin?: string
}

function webpCandidate(src: string): string | null {
  if (!src.startsWith('/images/home/')) return null
  if (/\.webp(\?|$)/i.test(src)) return null
  if (/\.(jpg|jpeg|png)$/i.test(src)) return src.replace(/\.(jpg|jpeg|png)$/i, '.webp')
  if (/\.svg$/i.test(src)) return null
  if (!/\.\w+$/.test(src)) return `${src}.webp`
  return null
}

export function LocalImage({
  image,
  className = '',
  loading,
  priority = false,
  fetchPriority,
  decoding = 'async',
  sizes,
  rootMargin,
}: LocalImageProps) {
  const resolvedLoading = loading ?? (priority ? 'eager' : 'lazy')
  const resolvedFetchPriority = fetchPriority ?? (priority ? 'high' : undefined)
  const { ref, resolvedSrc, isLoaded } = useLazyImageSource(image.src, {
    priority,
    rootMargin,
  })
  const webpSrc = resolvedSrc ? webpCandidate(resolvedSrc) : null

  const imgClass = `${className}${!isLoaded ? ' bg-surface-muted animate-pulse' : ''}`.trim()

  const imgProps = {
    alt: image.alt,
    width: image.width,
    height: image.height,
    loading: resolvedLoading,
    decoding,
    fetchPriority: resolvedFetchPriority,
    sizes,
    className: imgClass,
  }

  if (webpSrc) {
    return (
      <span ref={ref as RefObject<HTMLSpanElement>} className="contents">
        <picture>
          <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
          <img {...imgProps} src={resolvedSrc} />
        </picture>
      </span>
    )
  }

  return (
    <span ref={ref as RefObject<HTMLSpanElement>} className="contents">
      <img {...imgProps} src={resolvedSrc} />
    </span>
  )
}
