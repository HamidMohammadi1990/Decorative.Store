import { useEffect, type RefObject } from 'react'

const CSS_VAR = '--shop-chrome-height'

export function useShopChromeHeight(
  ref: RefObject<HTMLElement | null>,
  enabled = true,
) {
  useEffect(() => {
    if (!enabled) {
      document.documentElement.style.removeProperty(CSS_VAR)
      return
    }

    const el = ref.current
    if (!el) return

    const sync = () => {
      document.documentElement.style.setProperty(CSS_VAR, `${el.offsetHeight}px`)
    }

    sync()
    const observer = new ResizeObserver(sync)
    observer.observe(el)

    return () => {
      observer.disconnect()
      document.documentElement.style.removeProperty(CSS_VAR)
    }
  }, [ref, enabled])
}
