import { resolveSameOriginMediaSrc } from '@/utils/publicUploadUrl'

export function resolveUserImageUrl(url: string): string {
  if (!url) return ''
  return resolveSameOriginMediaSrc(url, 'Users')
}
