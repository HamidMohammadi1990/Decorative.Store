export interface AdminAssistantFaq {
  id: string
  languageId: number
  question: string
  answer: string
  priority: number
  isActive: boolean
}

export interface AssistantFaqItem {
  id: string
  question: string
  answer: string
  priority: number
}

export interface CreateAdminAssistantFaqInput {
  languageId: number
  question: string
  answer: string
  priority: number
}

export interface UpdateAdminAssistantFaqInput {
  id: string
  languageId: number
  question: string
  answer: string
  priority: number
  isActive: boolean
}
