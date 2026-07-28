import type { Locale } from '@/models/shared/locale.model'

export function formatRelativeDate(date: string, locale: Locale) {
  const parsed = new Date(date)
  if (Number.isNaN(parsed.getTime())) return date

  const diffMs = Date.now() - parsed.getTime()
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24))

  if (locale === 'fa') {
    if (diffDays < 1) return 'امروز'
    if (diffDays === 1) return 'دیروز'
    if (diffDays < 7) return `${diffDays} روز پیش`
    if (diffDays < 30) return `${Math.floor(diffDays / 7)} هفته پیش`
    if (diffDays < 365) return `${Math.floor(diffDays / 30)} ماه پیش`
    return `${Math.floor(diffDays / 365)} سال پیش`
  }

  if (diffDays < 1) return 'Today'
  if (diffDays === 1) return 'Yesterday'
  if (diffDays < 7) return `${diffDays} days ago`
  if (diffDays < 30) return `${Math.floor(diffDays / 7)} weeks ago`
  if (diffDays < 365) return `${Math.floor(diffDays / 30)} months ago`
  return `${Math.floor(diffDays / 365)} years ago`
}
