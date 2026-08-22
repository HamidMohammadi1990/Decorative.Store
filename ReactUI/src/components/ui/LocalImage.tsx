import type { ImageAsset } from '@/models/shared/image.model'

interface LocalImageProps {
  image: ImageAsset
  className?: string
  loading?: 'eager' | 'lazy'
}

export function LocalImage({ image, className = '', loading = 'lazy' }: LocalImageProps) {
  return (
    <img
      src={image.src}
      alt={image.alt}
      width={image.width}
      height={image.height}
      loading={loading}
      className={className}
    />
  )
}
