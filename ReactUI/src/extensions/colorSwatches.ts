export const COLOR_SWATCHES: Record<string, string> = {
  green: '#4a6741',
  grey: '#9ca3af',
  walnut: '#5c4033',
  oak: '#c4a574',
  white: '#f5f5f5',
  beige: '#d4c4a8',
  cream: '#fffdd0',
  natural: '#e8dcc8',
  brass: '#b5a642',
  clear: '#d6eaf5',
  neutral: '#d9d0c7',
  taupe: '#b8a99a',
  charcoal: '#36454f',
  teak: '#8b6914',
}

export function getColorSwatch(colorSlug: string) {
  return COLOR_SWATCHES[colorSlug] ?? '#d1d5db'
}
