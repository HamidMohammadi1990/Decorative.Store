export function inferLayoutFootprint(title: string): { widthPct: number; heightPct: number } {
  const t = title.toLowerCase()

  if (/\b(rug|carpet|runner)\b/.test(t) || /فرش|قالی/.test(title)) {
    return { widthPct: 42, heightPct: 28 }
  }
  if (/\b(sofa|sectional|chesterfield)\b/.test(t) || /مبل|کاناپه/.test(title)) {
    return { widthPct: 34, heightPct: 20 }
  }
  if (/\b(bed|bedframe)\b/.test(t) || /تخت/.test(title)) {
    return { widthPct: 32, heightPct: 24 }
  }
  if (/\b(table|desk|dining)\b/.test(t) || /میز/.test(title)) {
    return { widthPct: 24, heightPct: 18 }
  }
  if (/\b(chair|armchair|stool)\b/.test(t) || /صندلی|مبل راحتی/.test(title)) {
    return { widthPct: 16, heightPct: 16 }
  }
  if (/\b(lamp|vase|ceramic)\b/.test(t) || /لامپ|گلدان/.test(title)) {
    return { widthPct: 11, heightPct: 11 }
  }
  if (/\b(duvet|pillow|linen|curtain)\b/.test(t) || /روتختی|کوسن|پرده/.test(title)) {
    return { widthPct: 20, heightPct: 14 }
  }

  return { widthPct: 18, heightPct: 18 }
}
