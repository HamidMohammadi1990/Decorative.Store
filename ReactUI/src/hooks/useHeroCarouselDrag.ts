import {
  useCallback,
  useEffect,
  useRef,
  useState,
  type MouseEvent as ReactMouseEvent,
  type PointerEvent as ReactPointerEvent,
} from 'react'

const DRAG_THRESHOLD_PX = 48
const MAX_DRAG_OFFSET_PX = 120
const DRAG_START_PX = 6

interface UseHeroCarouselDragOptions {
  slideCount: number
  index: number
  isRtl: boolean
  onIndexChange: (index: number) => void
}

export function useHeroCarouselDrag({
  slideCount,
  index,
  isRtl,
  onIndexChange,
}: UseHeroCarouselDragOptions) {
  const isDragging = useRef(false)
  const didDrag = useRef(false)
  const startX = useRef(0)
  const startY = useRef(0)
  const startIndex = useRef(0)
  const dragOffsetRef = useRef(0)
  const onIndexChangeRef = useRef(onIndexChange)

  const [dragOffset, setDragOffset] = useState(0)
  const [isGrabbing, setIsGrabbing] = useState(false)

  useEffect(() => {
    onIndexChangeRef.current = onIndexChange
  }, [onIndexChange])

  const resetDrag = useCallback(() => {
    isDragging.current = false
    didDrag.current = false
    dragOffsetRef.current = 0
    setDragOffset(0)
    setIsGrabbing(false)
  }, [])

  const clampOffset = useCallback(
    (value: number, fromIndex: number) => {
      const draggingToNext = isRtl ? value > 0 : value < 0
      const draggingToPrev = isRtl ? value < 0 : value > 0
      const atStart = fromIndex === 0 && draggingToPrev
      const atEnd = fromIndex === slideCount - 1 && draggingToNext

      if (atStart || atEnd) {
        return value * 0.25
      }

      return Math.max(-MAX_DRAG_OFFSET_PX, Math.min(MAX_DRAG_OFFSET_PX, value))
    },
    [isRtl, slideCount],
  )

  const finishDrag = useCallback(() => {
    if (!isDragging.current) return

    const currentOffset = dragOffsetRef.current
    const fromIndex = startIndex.current
    resetDrag()

    if (isRtl) {
      if (currentOffset > DRAG_THRESHOLD_PX && fromIndex < slideCount - 1) {
        onIndexChangeRef.current(fromIndex + 1)
      } else if (currentOffset < -DRAG_THRESHOLD_PX && fromIndex > 0) {
        onIndexChangeRef.current(fromIndex - 1)
      }
      return
    }

    if (currentOffset < -DRAG_THRESHOLD_PX && fromIndex < slideCount - 1) {
      onIndexChangeRef.current(fromIndex + 1)
    } else if (currentOffset > DRAG_THRESHOLD_PX && fromIndex > 0) {
      onIndexChangeRef.current(fromIndex - 1)
    }
  }, [isRtl, resetDrag, slideCount])

  useEffect(() => {
    const onPointerMove = (e: PointerEvent) => {
      if (!isDragging.current) return

      const deltaX = e.clientX - startX.current
      const deltaY = e.clientY - startY.current

      if (!didDrag.current) {
        if (Math.abs(deltaX) < DRAG_START_PX && Math.abs(deltaY) < DRAG_START_PX) {
          return
        }

        if (Math.abs(deltaY) > Math.abs(deltaX)) {
          resetDrag()
          return
        }

        didDrag.current = true
      }

      const nextOffset = clampOffset(deltaX, startIndex.current)
      dragOffsetRef.current = nextOffset
      setDragOffset(nextOffset)
    }

    const onPointerUp = () => finishDrag()

    window.addEventListener('pointermove', onPointerMove, { passive: true })
    window.addEventListener('pointerup', onPointerUp)
    window.addEventListener('pointercancel', onPointerUp)

    return () => {
      window.removeEventListener('pointermove', onPointerMove)
      window.removeEventListener('pointerup', onPointerUp)
      window.removeEventListener('pointercancel', onPointerUp)
    }
  }, [clampOffset, finishDrag, resetDrag])

  useEffect(() => {
    resetDrag()
  }, [index, isRtl, resetDrag])

  const onPointerDown = useCallback(
    (e: ReactPointerEvent<HTMLDivElement>) => {
      if (slideCount <= 1 || e.button !== 0) return

      isDragging.current = true
      didDrag.current = false
      startX.current = e.clientX
      startY.current = e.clientY
      startIndex.current = index
      dragOffsetRef.current = 0
      setDragOffset(0)
      setIsGrabbing(true)
      e.currentTarget.setPointerCapture(e.pointerId)
    },
    [index, slideCount],
  )

  const onPointerUp = useCallback(
    (e: ReactPointerEvent<HTMLDivElement>) => {
      if (e.currentTarget.hasPointerCapture(e.pointerId)) {
        e.currentTarget.releasePointerCapture(e.pointerId)
      }
      finishDrag()
    },
    [finishDrag],
  )

  const onLostPointerCapture = useCallback(() => {
    finishDrag()
  }, [finishDrag])

  const onClickCapture = useCallback((e: ReactMouseEvent) => {
    if (didDrag.current) {
      e.preventDefault()
      e.stopPropagation()
      didDrag.current = false
    }
  }, [])

  return {
    dragOffset,
    isGrabbing,
    onPointerDown,
    onPointerUp,
    onLostPointerCapture,
    onClickCapture,
  }
}
