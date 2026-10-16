import { resolvePublicMediaUrl } from '@/config/api'

export function resolveUserImageUrl(url: string): string {
  if (!url) return ''
  return resolvePublicMediaUrl(url)
}
