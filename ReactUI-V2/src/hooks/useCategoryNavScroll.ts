import {
  useCallback,
  useEffect,
  useRef,
  useState,
  type MouseEvent,
  type RefObject,
} from 'react'
import {
  getHorizontalScrollState,
  scrollHorizontally,
} from '@/extensions/horizontalScroll'

const DRAG_THRESHOLD_PX = 4

interface UseCategoryNavScrollResult {
  scrollRef: RefObject<HTMLDivElement | null>
  isGrabbing: boolean
  canScrollPrev: boolean
  canScrollNext: boolean
  scrollPrev: () => void
  scrollNext: () => void
  onMouseDown: (e: MouseEvent<HTMLDivElement>) => void
  onClickCapture: (e: MouseEvent<HTMLDivElement>) => void
}

export function useCategoryNavScroll(): UseCategoryNavScrollResult {
  const scrollRef = useRef<HTMLDivElement>(null)
  const isDragging = useRef(false)
  const didDrag = useRef(false)
  const startX = useRef(0)
  const startScrollLeft = useRef(0)
  const targetScrollLeft = useRef(0)
  const rafId = useRef(0)
  const [isGrabbing, setIsGrabbing] = useState(false)
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

  const stopRaf = useCallback(() => {
    if (rafId.current) {
      cancelAnimationFrame(rafId.current)
      rafId.current = 0
    }
  }, [])

  const startRaf = useCallback(() => {
    const tick = () => {
      const el = scrollRef.current
      if (!el || !isDragging.current) {
        rafId.current = 0
        return
      }
      el.scrollLeft = targetScrollLeft.current
      updateScrollState()
      rafId.current = requestAnimationFrame(tick)
    }
    if (!rafId.current) {
      rafId.current = requestAnimationFrame(tick)
    }
  }, [updateScrollState])

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

  useEffect(() => {
    const onMouseMove = (e: globalThis.MouseEvent) => {
      if (!isDragging.current) return

      const delta = e.pageX - startX.current
      if (Math.abs(delta) > DRAG_THRESHOLD_PX) {
        didDrag.current = true
      }

      targetScrollLeft.current = startScrollLeft.current - delta
      startRaf()
    }

    const endDrag = () => {
      if (!isDragging.current) return
      isDragging.current = false
      stopRaf()
      setIsGrabbing(false)
      updateScrollState()
    }

    window.addEventListener('mousemove', onMouseMove, { passive: true })
    window.addEventListener('mouseup', endDrag)

    return () => {
      window.removeEventListener('mousemove', onMouseMove)
      window.removeEventListener('mouseup', endDrag)
      stopRaf()
    }
  }, [startRaf, stopRaf, updateScrollState])

  const scrollPrev = useCallback(() => {
    const el = scrollRef.current
    if (!el) return
    scrollHorizontally(el, 'prev')
  }, [])

  const scrollNext = useCallback(() => {
    const el = scrollRef.current
    if (!el) return
    scrollHorizontally(el, 'next')
  }, [])

  const onMouseDown = useCallback(
    (e: MouseEvent<HTMLDivElement>) => {
      if (e.button !== 0) return
      const el = scrollRef.current
      if (!el) return

      stopRaf()
      isDragging.current = true
      didDrag.current = false
      startX.current = e.pageX
      startScrollLeft.current = el.scrollLeft
      targetScrollLeft.current = el.scrollLeft
      setIsGrabbing(true)
      startRaf()
    },
    [startRaf, stopRaf],
  )

  const onClickCapture = useCallback((e: MouseEvent<HTMLDivElement>) => {
    if (didDrag.current) {
      e.preventDefault()
      e.stopPropagation()
      didDrag.current = false
    }
  }, [])

  return {
    scrollRef,
    isGrabbing,
    canScrollPrev,
    canScrollNext,
    scrollPrev,
    scrollNext,
    onMouseDown,
    onClickCapture,
  }
}
