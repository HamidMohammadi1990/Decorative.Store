import { useEffect, useRef } from 'react'
import type { StorySlide } from '@/models/stories/story.model'
import { LocalImage } from '@/components/ui/LocalImage'

interface StorySlidePlayerProps {
  slide: StorySlide
  isActive: boolean
  paused: boolean
  onComplete: () => void
  onProgress?: (progress: number) => void
}

const DEFAULT_IMAGE_MS = 5000

export function StorySlidePlayer({
  slide,
  isActive,
  paused,
  onComplete,
  onProgress,
}: StorySlidePlayerProps) {
  const videoRef = useRef<HTMLVideoElement>(null)
  const rafRef = useRef<number>(0)
  const startRef = useRef(0)
  const elapsedRef = useRef(0)
  const onCompleteRef = useRef(onComplete)
  const onProgressRef = useRef(onProgress)

  const isVideo = slide.media.type === 'video'
  const durationMs = slide.media.durationMs ?? (isVideo ? 15000 : DEFAULT_IMAGE_MS)

  onCompleteRef.current = onComplete
  onProgressRef.current = onProgress

  useEffect(() => {
    cancelAnimationFrame(rafRef.current)
    startRef.current = 0
    elapsedRef.current = 0
    onProgressRef.current?.(0)
  }, [slide.id])

  useEffect(() => {
    if (!isActive) {
      cancelAnimationFrame(rafRef.current)
      return
    }

    if (isVideo) {
      const video = videoRef.current
      if (!video) return

      if (paused) {
        video.pause()
      } else {
        void video.play().catch(() => undefined)
      }
      return
    }

    if (paused) {
      cancelAnimationFrame(rafRef.current)
      return
    }

    const tick = (timestamp: number) => {
      if (!startRef.current) startRef.current = timestamp
      const delta = timestamp - startRef.current
      startRef.current = timestamp
      elapsedRef.current += delta

      const progress = Math.min(1, elapsedRef.current / durationMs)
      onProgressRef.current?.(progress)

      if (progress >= 1) {
        onCompleteRef.current()
        return
      }

      rafRef.current = requestAnimationFrame(tick)
    }

    startRef.current = 0
    elapsedRef.current = 0
    rafRef.current = requestAnimationFrame(tick)

    return () => cancelAnimationFrame(rafRef.current)
  }, [isActive, isVideo, paused, durationMs, slide.id])

  if (isVideo) {
    return (
      <video
        ref={videoRef}
        src={slide.media.src}
        poster={slide.media.poster}
        className="size-full object-cover"
        playsInline
        muted
        onTimeUpdate={(e) => {
          const video = e.currentTarget
          if (!video.duration) return
          onProgressRef.current?.(video.currentTime / video.duration)
        }}
        onEnded={() => onCompleteRef.current()}
      />
    )
  }

  return (
    <LocalImage
      image={{ src: slide.media.src, alt: slide.media.alt }}
      className="size-full object-cover"
      loading="eager"
    />
  )
}
