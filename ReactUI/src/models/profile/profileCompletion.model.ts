export type ProfileQuestionType =
  | 'text'
  | 'textarea'
  | 'single_select'
  | 'multi_select'
  | 'date'
  | 'tel'

export interface ProfileQuestionOption {
  value: string
  label: string
}

export interface ProfileCompletionQuestion {
  id: string
  type: ProfileQuestionType
  label: string
  hint?: string
  placeholder?: string
  required: boolean
  order: number
  options?: ProfileQuestionOption[]
}

export type ProfileRewardType = 'percent_discount' | 'fixed_discount'

export interface ProfileCompletionReward {
  type: ProfileRewardType
  value: number
  code: string
  description: string
  validDays: number
}

export interface ProfileCompletionCampaign {
  id: string
  title: string
  subtitle: string
  reward: ProfileCompletionReward
}

export interface ProfileCompletionConfig {
  campaign: ProfileCompletionCampaign
  questions: ProfileCompletionQuestion[]
}

export type ProfileAnswerValue = string | string[]
