import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { AssistantChatMessage } from '@/models/assistant/assistantChat.model'

interface AssistantChatState {
  isOpen: boolean
  isTyping: boolean
  messages: AssistantChatMessage[]
  open: () => void
  close: () => void
  toggle: () => void
  setTyping: (typing: boolean) => void
  addMessage: (message: Omit<AssistantChatMessage, 'id' | 'createdAt'> & Partial<Pick<AssistantChatMessage, 'id' | 'createdAt'>>) => void
  clearMessages: () => void
}

export const useAssistantChatStore = create<AssistantChatState>()(
  persist(
    (set) => ({
      isOpen: false,
      isTyping: false,
      messages: [],

      open: () => set({ isOpen: true }),
      close: () => set({ isOpen: false }),
      toggle: () => set((s) => ({ isOpen: !s.isOpen })),

      setTyping: (typing) => set({ isTyping: typing }),

      addMessage: (message) =>
        set((state) => ({
          messages: [
            ...state.messages,
            {
              id: message.id ?? `msg-${Date.now()}-${Math.random().toString(36).slice(2, 7)}`,
              createdAt: message.createdAt ?? Date.now(),
              role: message.role,
              content: message.content,
            },
          ],
        })),

      clearMessages: () => set({ messages: [], isTyping: false }),
    }),
    {
      name: 'diba-assistant-chat',
      partialize: (state) => ({ messages: state.messages }),
    },
  ),
)
