import type {
  ProfileAnswerValue,
  ProfileCompletionQuestion,
} from '@/models/profile/profileCompletion.model'

export function isAnswerFilled(
  question: ProfileCompletionQuestion,
  value: ProfileAnswerValue | undefined,
): boolean {
  if (value === undefined) return false

  if (question.type === 'multi_select') {
    return Array.isArray(value) && value.length > 0
  }

  return typeof value === 'string' && value.trim().length > 0
}

export function calculateProfileProgress(
  questions: ProfileCompletionQuestion[],
  answers: Record<string, ProfileAnswerValue>,
): {
  percent: number
  answeredCount: number
  totalCount: number
  isComplete: boolean
} {
  const tracked = questions.filter((q) => q.required)
  const totalCount = tracked.length

  if (totalCount === 0) {
    return { percent: 100, answeredCount: 0, totalCount: 0, isComplete: true }
  }

  const answeredCount = tracked.filter((q) => isAnswerFilled(q, answers[q.id])).length
  const percent = Math.round((answeredCount / totalCount) * 100)

  return {
    percent,
    answeredCount,
    totalCount,
    isComplete: answeredCount === totalCount,
  }
}
