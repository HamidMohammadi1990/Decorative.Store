import { useCallback } from 'react'
import { useTranslation } from 'react-i18next'
import { getWelcomeMessage, fetchAssistantReply } from '@/services/assistantChatService'
import { useAssistantChatStore } from '@/stores/assistantChatStore'
import { useSettingsStore } from '@/stores/settingsStore'
import type { AssistantSuggestionId } from '@/models/assistant/assistantChat.model'

export function useAssistantChat() {
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
    [addMessage, isTyping, locale, setTyping, t],
  )

  const sendSuggestion = useCallback(
    (id: AssistantSuggestionId) => {
      void sendMessage(t(`assistant.suggestions.${id}.prompt`))
    },
    [sendMessage, t],
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
