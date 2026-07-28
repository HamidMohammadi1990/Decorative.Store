import profileCompletionEn from '@/data/mock/profileCompletion.en.json'
import profileCompletionFa from '@/data/mock/profileCompletion.fa.json'
import type { ProfileCompletionConfig } from '@/models/profile/profileCompletion.model'
import type { AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'

function emptyLocalized() {
  return { en: '', fa: '' }
}

export function buildAdminConfigFromMocks(): AdminProfileCompletionConfig {
  const en = profileCompletionEn as ProfileCompletionConfig
  const fa = profileCompletionFa as ProfileCompletionConfig
  const faById = new Map(fa.questions.map((q) => [q.id, q]))

  return {
    campaignId: en.campaign.id,
    titles: { en: en.campaign.title, fa: fa.campaign.title },
    subtitles: { en: en.campaign.subtitle, fa: fa.campaign.subtitle },
    reward: {
      type: en.campaign.reward.type,
      value: en.campaign.reward.value,
      code: en.campaign.reward.code,
      validDays: en.campaign.reward.validDays,
      descriptions: {
        en: en.campaign.reward.description,
        fa: fa.campaign.reward.description,
      },
    },
    questions: en.questions.map((q) => {
      const faQ = faById.get(q.id)
      return {
        id: q.id,
        type: q.type,
        required: q.required,
        order: q.order,
        labels: { en: q.label, fa: faQ?.label ?? q.label },
        hints: { en: q.hint ?? '', fa: faQ?.hint ?? '' },
        placeholders: { en: q.placeholder ?? '', fa: faQ?.placeholder ?? '' },
        options: (q.options ?? []).map((opt, index) => {
          const faOpt = faQ?.options?.[index]
          return {
            value: opt.value,
            labels: { en: opt.label, fa: faOpt?.label ?? opt.label },
          }
        }),
      }
    }),
  }
}

export function createEmptyAdminQuestion(order: number): AdminProfileCompletionConfig['questions'][number] {
  return {
    id: `question-${Date.now().toString(36)}`,
    type: 'text',
    required: true,
    order,
    labels: emptyLocalized(),
    hints: emptyLocalized(),
    placeholders: emptyLocalized(),
    options: [],
  }
}
