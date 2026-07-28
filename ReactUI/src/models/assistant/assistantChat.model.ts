export type AssistantMessageRole = 'user' | 'assistant'

export interface AssistantChatMessage {
  id: string
  role: AssistantMessageRole
  content: string
  createdAt: number
}

export type AssistantIntent =
  | 'greeting'
  | 'shipping'
  | 'returns'
  | 'orders'
  | 'recommend'
  | 'payment'
  | 'hours'
  | 'fallback'

export type AssistantSuggestionId =
  | 'recommend'
  | 'shipping'
  | 'returns'
  | 'orders'
  | 'payment'
