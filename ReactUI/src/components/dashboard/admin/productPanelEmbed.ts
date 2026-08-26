export interface ProductPanelEmbedProps {
  embedded?: boolean
  productId?: string
  productTitle?: string
  productCode?: string
}

export function embeddedProductLabel(
  title?: string,
  code?: string,
): string {
  if (title && code) return `${title} (${code})`
  return title || code || ''
}
