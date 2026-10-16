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

function EagerLocalImage({
  image,
  className = '',
  loading = 'eager',
  fetchPriority = 'high',
  decoding = 'async',
  sizes,
}: LocalImageProps) {
  const src = image.src?.trim() ?? ''
  if (!src) {
    return <div className={`${className} bg-surface-muted`.trim()} aria-hidden />
  }

  const webpSrc = webpCandidate(src)
  const imgProps = {
    alt: image.alt,
    width: image.width,
    height: image.height,
    loading,
    decoding,
    fetchPriority,
    sizes,
    className,
  }

  if (webpSrc) {
    return (
      <picture>
        <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
        <img {...imgProps} src={src} />
      </picture>
    )
  }

  return <img {...imgProps} src={src} />
}

function LazyLocalImage({
  image,
  className = '',
  loading,
  fetchPriority,
  decoding = 'async',
  sizes,
  rootMargin,
}: LocalImageProps) {
  const resolvedLoading = loading ?? 'lazy'
  const { ref, resolvedSrc, isLoaded } = useLazyImageSource(image.src, { rootMargin })
  const webpSrc = resolvedSrc ? webpCandidate(resolvedSrc) : null
  const imgClass = `${className}${!isLoaded ? ' bg-surface-muted animate-pulse' : ''}`.trim()

  const imgProps = {
    alt: image.alt,
    width: image.width,
    height: image.height,
    loading: resolvedLoading,
    decoding,
    fetchPriority,
    sizes,
    className: imgClass,
  }

  if (webpSrc) {
    return (
      <picture>
        <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
        <img ref={ref as RefObject<HTMLImageElement>} {...imgProps} src={resolvedSrc} />
      </picture>
    )
  }

  return <img ref={ref as RefObject<HTMLImageElement>} {...imgProps} src={resolvedSrc} />
}

export function LocalImage(props: LocalImageProps) {
  if (props.priority) {
    return <EagerLocalImage {...props} />
  }
  return <LazyLocalImage {...props} />
}
