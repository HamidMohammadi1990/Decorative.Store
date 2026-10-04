import { useCallback } from 'react'
import { useTranslation } from 'react-i18next'
import type { AssistantFaqItem } from '@/models/admin/assistantFaq.model'
import { getWelcomeMessage, fetchAssistantReply } from '@/services/assistantChatService'
import { useAssistantChatStore } from '@/stores/assistantChatStore'
import { useSettingsStore } from '@/stores/settingsStore'

function wait(ms: number) {
  return new Promise((resolve) => window.setTimeout(resolve, ms))
}

export function useAssistantChat(faqs: AssistantFaqItem[] = []) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const isOpen = useAssistantChatStore((s) => s.isOpen)
  const isTyping = useAssistantChatStore((s) => s.isTyping)
  const messages = useAssistantChatStore((s) => s.messages)
  const open = useAssistantChatStore((s) => s.open)
  const close = useAssistantChatStore((s) => s.close)
  const toggle = useAssistantChatStore((s) => s.toggle)
  const setTyping = useAssistantChatStore((s) => s.setTyping)
  const addMessage = useAssistantChatStore((s) => s.addMessage)
  const clearMessages = useAssistantChatStore((s) => s.clearMessages)

  const findFaqMatch = useCallback(
    (text: string) => {
      const normalized = text.trim().toLowerCase()
      if (!normalized) return null
      return (
        faqs.find((item) => item.question.trim().toLowerCase() === normalized) ??
        faqs.find((item) => item.id === text) ??
        null
      )
    },
    [faqs],
  )

  const replyWithFaq = useCallback(
    async (faq: AssistantFaqItem) => {
      addMessage({ role: 'user', content: faq.question })
      setTyping(true)
      try {
        const delay = 700 + Math.random() * 700
        await wait(delay)
        addMessage({ role: 'assistant', content: faq.answer })
      } finally {
        setTyping(false)
      }
    },
    [addMessage, setTyping],
  )

  const ensureWelcome = useCallback(() => {
    const current = useAssistantChatStore.getState().messages
    if (current.length > 0) return

    addMessage({
      role: 'assistant',
      content: getWelcomeMessage(locale),
    })
  }, [addMessage, locale])

  const openChat = useCallback(() => {
    open()
    ensureWelcome()
  }, [open, ensureWelcome])

  const sendMessage = useCallback(
    async (rawText: string) => {
      const text = rawText.trim()
      if (!text || isTyping) return

      const matchedFaq = findFaqMatch(text)
      if (matchedFaq) {
        await replyWithFaq(matchedFaq)
        return
      }

      addMessage({ role: 'user', content: text })
      setTyping(true)

      try {
        const { content } = await fetchAssistantReply(text, locale)
        addMessage({ role: 'assistant', content })
      } catch {
        addMessage({
          role: 'assistant',
          content: t('assistant.errorReply'),
        })
      } finally {
        setTyping(false)
      }
    },
    [addMessage, findFaqMatch, isTyping, locale, replyWithFaq, setTyping, t],
  )

  const sendSuggestion = useCallback(
    (id: string) => {
      const faq = faqs.find((item) => item.id === id)
      if (!faq) return
      void replyWithFaq(faq)
    },
    [faqs, replyWithFaq],
  )

  const resetChat = useCallback(() => {
    clearMessages()
    addMessage({
      role: 'assistant',
      content: getWelcomeMessage(locale),
    })
  }, [addMessage, clearMessages, locale])

  return {
    isOpen,
    isTyping,
    messages,
    openChat,
    close,
    toggle,
    sendMessage,
    sendSuggestion,
    resetChat,
  }
}
