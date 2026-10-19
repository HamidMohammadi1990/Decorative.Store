import { CMS_SECTION_TYPES } from '@/constants/cmsSectionTypes'

/** True when src points at an uploaded CMS file (not static /images/ mock). */
export function isCmsUploadedImage(src: string): boolean {
  const value = src.trim()
  if (!value) return false
  return /\/Uploads\/Cms\//i.test(value) || /^Uploads\/Cms\//i.test(value)
}

/** Bundled storefront placeholders under /images/home/ (not CMS uploads). */
export function isStockHomeMediaSrc(src: string): boolean {
  const value = src.trim().toLowerCase()
  if (!value) return true
  if (isCmsUploadedImage(src)) return false
  return value.startsWith('/images/home/')
}

export function resolveCmsSectionTypeKey(sectionTypeName: string): string {
  const trimmed = sectionTypeName.trim()
  if (!trimmed) return ''

  const known = Object.values(CMS_SECTION_TYPES) as string[]
  const match = known.find((key) => key.toLowerCase() === trimmed.toLowerCase())
  if (match) return match

  const compact = trimmed.replace(/\s+/g, '')
  const compactMatch = known.find((key) => key.toLowerCase() === compact.toLowerCase())
  return compactMatch ?? trimmed
}
