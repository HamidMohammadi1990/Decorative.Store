export function getHorizontalScrollState(el: HTMLElement) {
  const max = Math.max(0, el.scrollWidth - el.clientWidth)
  const position = Math.abs(el.scrollLeft)
  const tolerance = 4

  return {
    canScrollPrev: position > tolerance,
    canScrollNext: position < max - tolerance,
    max,
  }
}

export function scrollHorizontally(
  el: HTMLElement,
  direction: 'prev' | 'next',
  behavior: ScrollBehavior = 'smooth',
) {
  const step = Math.max(120, Math.round(el.clientWidth * 0.65))
  const isRtl = getComputedStyle(el).direction === 'rtl'
  let delta = direction === 'next' ? step : -step
  if (isRtl) delta = -delta

  el.scrollBy({ left: delta, behavior })
}
