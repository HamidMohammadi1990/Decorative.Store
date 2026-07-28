import { useCallback, useEffect, useRef, useState, type RefObject } from 'react'
import {
  getHorizontalScrollState,
  scrollHorizontally,
} from '@/extensions/horizontalScroll'

interface UseHorizontalScrollArrowsResult {
  scrollRef: RefObject<HTMLDivElement | null>
  canScrollPrev: boolean
  canScrollNext: boolean
  hasOverflow: boolean
  scrollPrev: () => void
  scrollNext: () => void
}

export function useHorizontalScrollArrows(): UseHorizontalScrollArrowsResult {
  const scrollRef = useRef<HTMLDivElement>(null)
  const [canScrollPrev, setCanScrollPrev] = useState(false)
  const [canScrollNext, setCanScrollNext] = useState(false)

  const updateScrollState = useCallback(() => {
    const el = scrollRef.current
    if (!el) return

    const { canScrollPrev: prev, canScrollNext: next } =
      getHorizontalScrollState(el)
    setCanScrollPrev(prev)
    setCanScrollNext(next)
  }, [])

  useEffect(() => {
    const el = scrollRef.current
    if (!el) return

    updateScrollState()

    const onScroll = () => updateScrollState()
    el.addEventListener('scroll', onScroll, { passive: true })

    const ro = new ResizeObserver(() => updateScrollState())
    ro.observe(el)

    return () => {
      el.removeEventListener('scroll', onScroll)
      ro.disconnect()
    }
  }, [updateScrollState])

  const scrollPrev = useCallback(() => {
    const el = scrollRef.current
    if (el) scrollHorizontally(el, 'prev')
  }, [])

  const scrollNext = useCallback(() => {
    const el = scrollRef.current
    if (el) scrollHorizontally(el, 'next')
  }, [])

  return {
    scrollRef,
    canScrollPrev,
    canScrollNext,
    hasOverflow: canScrollPrev || canScrollNext,
    scrollPrev,
    scrollNext,
  }
}
