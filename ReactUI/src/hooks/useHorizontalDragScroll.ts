import {
  useCallback,
  useRef,
  useState,
  type MouseEvent as ReactMouseEvent,
  type PointerEvent as ReactPointerEvent,
} from 'react'

const DRAG_THRESHOLD_PX = 4

export function useHorizontalDragScroll<T extends HTMLElement>() {
  const ref = useRef<T>(null)
  const isDragging = useRef(false)
  const didDrag = useRef(false)
  const lastX = useRef(0)

  const [isGrabbing, setIsGrabbing] = useState(false)

  const resetDrag = useCallback(() => {
    isDragging.current = false
    setIsGrabbing(false)
  }, [])

  const onPointerDown = useCallback((e: ReactPointerEvent<T>) => {
    const el = ref.current
    if (!el || e.button !== 0) return

    const target = e.target as HTMLElement
    if (target.closest('button, a, input, textarea')) return

    isDragging.current = true
    didDrag.current = false
    lastX.current = e.clientX
    setIsGrabbing(true)
    e.currentTarget.setPointerCapture(e.pointerId)
  }, [])

  const onPointerMove = useCallback((e: ReactPointerEvent<T>) => {
    const el = ref.current
    if (!isDragging.current || !el) return

    const deltaX = e.clientX - lastX.current
    if (Math.abs(deltaX) > DRAG_THRESHOLD_PX) didDrag.current = true

    // scrollBy respects the element's computed direction (ltr/rtl from locale)
    el.scrollBy({ left: -deltaX, behavior: 'auto' })
    lastX.current = e.clientX
  }, [])

  const onPointerUp = useCallback(
    (e: ReactPointerEvent<T>) => {
      if (e.currentTarget.hasPointerCapture(e.pointerId)) {
        e.currentTarget.releasePointerCapture(e.pointerId)
      }
      resetDrag()
    },
    [resetDrag],
  )

  const onLostPointerCapture = useCallback(() => {
    resetDrag()
  }, [resetDrag])

  const onClickCapture = useCallback((e: ReactMouseEvent) => {
    if (didDrag.current) {
      e.preventDefault()
      e.stopPropagation()
      didDrag.current = false
    }
  }, [])

  return {
    ref,
    isGrabbing,
    onPointerDown,
    onPointerMove,
    onPointerUp,
    onLostPointerCapture,
    onClickCapture,
  }
}
