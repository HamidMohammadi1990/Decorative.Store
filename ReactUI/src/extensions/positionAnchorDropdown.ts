const VIEWPORT_PADDING = 16
const ANCHOR_GAP = 6

export function positionAnchorDropdown(
  panel: HTMLElement,
  anchorEl: HTMLElement,
  alignEnd = true,
) {
  const isRtl = document.documentElement.dir === 'rtl'
  const preferAlignEnd = isRtl ? !alignEnd : alignEnd

  const rect = anchorEl.getBoundingClientRect()
  const inner = panel.querySelector('[data-dropdown-inner]') as HTMLElement | null
  const panelWidth =
    inner?.getBoundingClientRect().width ?? panel.getBoundingClientRect().width

  panel.style.top = `${rect.bottom + ANCHOR_GAP}px`
  panel.style.maxWidth = `calc(100vw - ${VIEWPORT_PADDING * 2}px)`
  panel.style.right = 'auto'

  if (panelWidth <= 0) return

  let left = preferAlignEnd ? rect.right - panelWidth : rect.left
  const maxLeft = window.innerWidth - VIEWPORT_PADDING - panelWidth
  const minLeft = VIEWPORT_PADDING
  left = Math.max(minLeft, Math.min(left, maxLeft))

  panel.style.left = `${left}px`
}
