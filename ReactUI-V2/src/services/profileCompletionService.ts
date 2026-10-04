import { resolveProfileCompletionConfig } from '@/extensions/resolveProfileCompletionConfig'
import type { ProfileCompletionConfig } from '@/models/profile/profileCompletion.model'
import type { AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'
import type { Locale } from '@/models/shared/locale.model'

export function getProfileCompletionConfig(
  locale: Locale,
  adminConfig: AdminProfileCompletionConfig,
): ProfileCompletionConfig {
  return resolveProfileCompletionConfig(adminConfig, locale)
}
