import type { ProductQuestion } from '@/models/catalog/productQuestion.model'

export interface ProductQuestionItem {
  id: string
  question: string
  answer?: string
  askerName: string
  author?: string
  date?: string
  isBuyer?: boolean
  isExpert?: boolean
  answerCount: number
  helpful: number
  notHelpful: number
}

function formatPersonName(
  firstName?: string,
  lastName?: string,
  userName?: string,
): string {
  const fullName = [firstName, lastName].filter(Boolean).join(' ').trim()
  return fullName || userName || ''
}

export function mapProductQuestionToItem(
  question: ProductQuestion,
  locale: string,
): ProductQuestionItem {
  const dateLocale = locale === 'fa' ? 'fa-IR' : 'en-US'
  const hasAnswer = Boolean(question.answer?.trim())
  const answerAuthor = hasAnswer
    ? formatPersonName(
        question.answeredByFirstName,
        question.answeredByLastName,
        question.answeredByUserName,
      )
    : undefined

  const askerName = formatPersonName(
    question.userFirstName,
    question.userLastName,
    question.userName,
  )

  return {
    id: question.id,
    question: question.question,
    answer: question.answer ?? undefined,
    askerName,
    author: answerAuthor ?? askerName,
    date: question.createdOnUtc
      ? new Date(question.createdOnUtc).toLocaleDateString(dateLocale, {
          year: 'numeric',
          month: 'short',
          day: 'numeric',
        })
      : undefined,
    isBuyer: false,
    isExpert: false,
    answerCount: hasAnswer ? 1 : 0,
    helpful: 0,
    notHelpful: 0,
  }
}
