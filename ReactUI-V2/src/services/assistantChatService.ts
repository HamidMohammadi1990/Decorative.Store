import i18n from '@/i18n'
import { detectAssistantIntent } from '@/extensions/detectAssistantIntent'
import type { AssistantIntent } from '@/models/assistant/assistantChat.model'
import type { Locale } from '@/models/shared/locale.model'

const RESPONSE_DELAY_MS = { min: 700, max: 1400 }

function wait(ms: number) {
  return new Promise((resolve) => window.setTimeout(resolve, ms))
}

function getReplyKey(intent: AssistantIntent): string {
  return `assistant.responses.${intent}`
}

export async function fetchAssistantReply(
  userMessage: string,
  locale: Locale,
): Promise<{ content: string; intent: AssistantIntent }> {
  const intent = detectAssistantIntent(userMessage)
  const delay =
    RESPONSE_DELAY_MS.min +
    Math.random() * (RESPONSE_DELAY_MS.max - RESPONSE_DELAY_MS.min)

  await wait(delay)

  const content = i18n.t(getReplyKey(intent), { lng: locale })

  return { content, intent }
}

export function getWelcomeMessage(locale: Locale): string {
  return i18n.t('assistant.welcome', { lng: locale })
}
