import type { ProfileCompletionConfig } from '@/models/profile/profileCompletion.model'
import type { AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'
import type { Locale } from '@/models/shared/locale.model'

export function resolveProfileCompletionConfig(
  admin: AdminProfileCompletionConfig,
  locale: Locale,
): ProfileCompletionConfig {
  const questions = [...admin.questions]
    .sort((a, b) => a.order - b.order)
    .map((q) => ({
      id: q.id,
      type: q.type,
      required: q.required,
      order: q.order,
      label: q.labels[locale],
      hint: q.hints[locale] || undefined,
      placeholder: q.placeholders[locale] || undefined,
      options:
        q.options.length > 0
          ? q.options.map((opt) => ({
              value: opt.value,
              label: opt.labels[locale],
            }))
          : undefined,
    }))

  return {
    campaign: {
      id: admin.campaignId,
      title: admin.titles[locale],
      subtitle: admin.subtitles[locale],
      reward: {
        type: admin.reward.type,
        value: admin.reward.value,
        code: admin.reward.code,
        validDays: admin.reward.validDays,
        description: admin.reward.descriptions[locale],
      },
    },
    questions,
  }
}
