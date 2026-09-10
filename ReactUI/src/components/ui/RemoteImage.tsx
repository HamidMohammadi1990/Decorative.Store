import type { CSSProperties, ImgHTMLAttributes, RefObject } from 'react'
import { useLazyImageSource } from '@/hooks/useLazyImageSource'

export interface RemoteImageProps extends Omit<
  ImgHTMLAttributes<HTMLImageElement>,
  'src' | 'loading' | 'fetchPriority' | 'decoding'
> {
  src: string
  alt: string
  /** LCP / hero — skip lazy deferral and use fetchpriority high. */
  priority?: boolean
  loading?: 'eager' | 'lazy'
  fetchPriority?: 'high' | 'low' | 'auto'
  decoding?: 'async' | 'sync' | 'auto'
  rootMargin?: string
  /** Responsive hint for the browser — e.g. "(max-width:640px) 50vw, 33vw" */
  sizes?: string
  aspectRatio?: string
}

function webpCandidate(src: string): string | null {
  if (!src.startsWith('/images/home/')) return null
  if (/\.webp(\?|$)/i.test(src)) return null
  if (/\.(jpg|jpeg|png)$/i.test(src)) return src.replace(/\.(jpg|jpeg|png)$/i, '.webp')
  if (/\.svg$/i.test(src)) return null
  if (!/\.\w+$/.test(src)) return `${src}.webp`
  return null
}

/**
 * Performance-oriented image for plain URL strings (uploads, room backgrounds, etc.).
 */
export function RemoteImage({
  src,
  alt,
  priority = false,
  loading,
  fetchPriority,
  decoding = 'async',
  rootMargin,
  sizes,
  aspectRatio,
  className = '',
  width,
  height,
  style,
  ...rest
}: RemoteImageProps) {
  const resolvedLoading = loading ?? (priority ? 'eager' : 'lazy')
  const resolvedFetchPriority = fetchPriority ?? (priority ? 'high' : undefined)
  const { ref, resolvedSrc, isLoaded } = useLazyImageSource(src, { priority, rootMargin })
  const webpSrc = resolvedSrc ? webpCandidate(resolvedSrc) : null

  const wrapperStyle: CSSProperties | undefined = aspectRatio
    ? { aspectRatio, ...style }
    : style

  const imgClass = `${className}${!isLoaded ? ' bg-surface-muted animate-pulse' : ''}`.trim()

  const imgProps = {
    alt,
    width,
    height,
    loading: resolvedLoading,
    decoding,
    fetchPriority: resolvedFetchPriority,
    sizes,
    className: imgClass,
    ...rest,
  }

  const imgRef = ref as RefObject<HTMLImageElement>

  if (aspectRatio) {
    return (
      <span className="block overflow-hidden" style={wrapperStyle}>
        {webpSrc ? (
          <picture>
            <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
            <img ref={imgRef} {...imgProps} src={resolvedSrc} />
          </picture>
        ) : (
          <img ref={imgRef} {...imgProps} src={resolvedSrc} />
        )}
      </span>
    )
  }

  if (webpSrc) {
    return (
      <picture>
        <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
        <img ref={imgRef} {...imgProps} src={resolvedSrc} />
      </picture>
    )
  }

  return <img ref={imgRef} {...imgProps} src={resolvedSrc} />
}
