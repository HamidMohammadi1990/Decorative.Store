import { useTranslation } from 'react-i18next'
import { AiChatPanel } from '@/components/assistant/AiChatPanel'
import { useAssistantChat } from '@/hooks/useAssistantChat'
import { useAssistantFaqs } from '@/hooks/useAssistantFaqs'
import { useCompareStore } from '@/stores/compareStore'

export function AiAssistantWidget() {
  const { t } = useTranslation()
  const compareCount = useCompareStore((s) => s.slugs.length)
  const { faqs, loading: faqsLoading, loadFaqs } = useAssistantFaqs()
  const {
    isOpen,
    isTyping,
    messages,
    openChat,
    close,
    sendMessage,
    sendSuggestion,
    resetChat,
  } = useAssistantChat(faqs)

  const handleFabClick = () => {
    if (isOpen) {
      close()
      return
    }
    void loadFaqs()
    openChat()
  }

  const bottomOffset = compareCount > 0 ? 'bottom-24 sm:bottom-28' : 'bottom-4 sm:bottom-6'

  return (
    <>
      <button
        type="button"
        onClick={handleFabClick}
        aria-expanded={isOpen}
        aria-label={isOpen ? t('assistant.close') : t('assistant.open')}
        title={!isOpen ? t('assistant.launchHint') : undefined}
        className={`ai-assistant-fab group pointer-events-auto fixed end-4 flex size-14 items-center justify-center rounded-full text-warm-text shadow-[0_12px_40px_-8px_rgba(154,116,72,0.65)] transition-transform hover:scale-[1.04] active:scale-[0.97] sm:end-6 sm:size-[3.75rem] ${bottomOffset} ${
          isOpen ? 'z-[90] ring-2 ring-warm/40 ring-offset-2 ring-offset-surface' : 'z-[55]'
        }`}
      >
        {!isOpen && (
          <span className="pointer-events-none absolute bottom-[calc(100%+0.625rem)] end-0 hidden w-max max-w-[12rem] rounded-xl border border-border/70 bg-surface px-3 py-2 text-xs font-medium leading-snug text-text opacity-0 shadow-lg transition-opacity duration-200 group-hover:opacity-100 group-focus-visible:opacity-100 sm:block">
            {t('assistant.launchHint')}
            <span
              className="absolute end-4 top-full border-4 border-transparent border-t-surface"
              aria-hidden
            />
          </span>
        )}

        <span className="ai-assistant-fab-ring pointer-events-none absolute inset-0 rounded-full" aria-hidden />
        {isOpen ? <CloseFabIcon /> : <SparkleFabIcon />}
        {!isOpen && (
          <span className="absolute -end-0.5 -top-0.5 flex size-5 items-center justify-center rounded-full bg-surface text-[9px] font-bold uppercase tracking-wide text-warm ring-2 ring-surface">
            AI
          </span>
        )}
      </button>

      <AiChatPanel
        open={isOpen}
        messages={messages}
        isTyping={isTyping}
        suggestions={faqs}
        suggestionsLoading={faqsLoading}
        onClose={close}
        onSend={(text) => void sendMessage(text)}
        onSuggestion={sendSuggestion}
        onReset={resetChat}
      />
    </>
  )
}

function SparkleFabIcon() {
  return (
    <svg width="26" height="26" viewBox="0 0 24 24" fill="none" aria-hidden className="relative z-[1]">
      <path
        d="M12 2.5 13.8 9.2 20.5 11 13.8 12.8 12 19.5 10.2 12.8 3.5 11 10.2 9.2 12 2.5Z"
        fill="currentColor"
      />
      <path
        d="M18.8 14.8 19.6 17.5 22.3 18.3 19.6 19.1 18.8 21.8 18 19.1 15.3 18.3 18 17.5 18.8 14.8Z"
        fill="currentColor"
        opacity="0.85"
      />
    </svg>
  )
}

function CloseFabIcon() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" aria-hidden className="relative z-[1]">
      <path d="M7 7l10 10M17 7 7 17" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
    </svg>
  )
}
