import type { ProfileQuestionType, ProfileRewardType } from '@/models/profile/profileCompletion.model'

export interface LocalizedText {
  en: string
  fa: string
}

export interface AdminQuestionOption {
  value: string
  labels: LocalizedText
}

export interface AdminProfileQuestion {
  id: string
  type: ProfileQuestionType
  required: boolean
  order: number
  labels: LocalizedText
  hints: LocalizedText
  placeholders: LocalizedText
  options: AdminQuestionOption[]
}

export interface AdminProfileCompletionConfig {
  campaignId: string
  titles: LocalizedText
  subtitles: LocalizedText
  reward: {
    type: ProfileRewardType
    value: number
    code: string
    descriptions: LocalizedText
    validDays: number
  }
  questions: AdminProfileQuestion[]
}
