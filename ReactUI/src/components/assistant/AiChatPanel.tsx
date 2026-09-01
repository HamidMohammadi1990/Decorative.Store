import { Fragment, useEffect, useRef } from 'react'
import { useTranslation } from 'react-i18next'
import type { AssistantFaqItem } from '@/models/admin/assistantFaq.model'
import type { AssistantChatMessage } from '@/models/assistant/assistantChat.model'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { formatAssistantTime } from '@/extensions/formatAssistantTime'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface AiChatPanelProps {
  open: boolean
  messages: AssistantChatMessage[]
  isTyping: boolean
  suggestions: AssistantFaqItem[]
  suggestionsLoading?: boolean
  onClose: () => void
  onSend: (text: string) => void
  onSuggestion: (id: string) => void
  onReset: () => void
}

export function AiChatPanel({
  open,
  messages,
  isTyping,
  suggestions,
  suggestionsLoading = false,
  onClose,
  onSend,
  onSuggestion,
  onReset,
}: AiChatPanelProps) {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const listRef = useRef<HTMLDivElement>(null)
  const inputRef = useRef<HTMLTextAreaElement>(null)
  const showSuggestions = messages.length <= 1 && !isTyping

  useEffect(() => {
    if (!open) return
    const el = listRef.current
    if (!el) return
    el.scrollTop = el.scrollHeight
  }, [open, messages, isTyping])

  useEffect(() => {
    if (!open) return
    const timer = window.setTimeout(() => inputRef.current?.focus(), 120)
    return () => window.clearTimeout(timer)
  }, [open])

  if (!open) return null

  const handleSubmit = () => {
    const value = inputRef.current?.value ?? ''
    if (!value.trim()) return
    onSend(value)
    if (inputRef.current) {
      inputRef.current.value = ''
      inputRef.current.style.height = 'auto'
    }
  }

  return (
    <Portal>
      <Fragment>
        <button
          type="button"
          className="fixed inset-0 z-[75] bg-black/35 backdrop-blur-[1px] sm:bg-black/15"
          aria-label={t('common.close')}
          onClick={onClose}
        />

        <section
          role="dialog"
          aria-modal="true"
          aria-label={t('assistant.panelLabel')}
          className="fixed inset-x-3 bottom-3 z-[80] flex h-[min(720px,calc(100svh-1.5rem))] max-h-[min(720px,calc(100svh-1.5rem))] flex-col overflow-hidden rounded-2xl border border-border/80 bg-surface shadow-[0_24px_80px_-16px_rgba(0,0,0,0.45)] sm:inset-x-auto sm:end-6 sm:bottom-24 sm:h-[min(640px,calc(100svh-8rem))] sm:w-[min(100vw-3rem,400px)]"
        >
          <header className="relative shrink-0 overflow-hidden border-b border-border/70 px-4 py-3.5">
            <div className="ai-assistant-header-glow pointer-events-none absolute inset-0 opacity-90" aria-hidden />
            <div className="relative flex items-start gap-3">
              <AiAvatar size="md" />
              <div className="min-w-0 flex-1">
                <div className="flex flex-wrap items-center gap-2">
                  <h2 className="text-sm font-semibold text-text sm:text-base">
                    {t('assistant.title')}
                  </h2>
                  <span className="inline-flex items-center gap-1 rounded-full bg-surface/80 px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm ring-1 ring-warm/25 backdrop-blur-sm">
                    <SparkleIcon className="size-3" />
                    {t('assistant.aiBadge')}
                  </span>
                </div>
                <p className="mt-0.5 text-xs text-text-muted">{t('assistant.subtitle')}</p>
              </div>
              <div className="flex shrink-0 items-center gap-1">
                <button
                  type="button"
                  onClick={onReset}
                  className="rounded-full px-2 py-1 text-[11px] font-medium text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                >
                  {t('assistant.newChat')}
                </button>
                <button
                  type="button"
                  onClick={onClose}
                  aria-label={t('common.close')}
                  className="flex size-8 items-center justify-center rounded-full border border-border/80 bg-surface/80 text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                >
                  <CloseIcon />
                </button>
              </div>
            </div>
          </header>

          <div
            ref={listRef}
            className="flex min-h-0 flex-1 flex-col gap-3 overflow-y-auto bg-surface-muted/25 px-3 py-4 sm:px-4"
          >
            {messages.map((message) => (
              <ChatBubble key={message.id} message={message} locale={locale} />
            ))}

            {isTyping && <TypingBubble />}

            {showSuggestions && (
              <div className="mt-1 space-y-2">
                <p className="text-[11px] font-medium uppercase tracking-wide text-text-muted">
                  {t('assistant.suggestionsTitle')}
                </p>
                {suggestionsLoading ? (
                  <p className="text-xs text-text-muted">{t('assistant.loadingSuggestions')}</p>
                ) : suggestions.length > 0 ? (
                  <div className="flex flex-wrap gap-2">
                    {suggestions.map((item) => (
                      <button
                        key={item.id}
                        type="button"
                        onClick={() => onSuggestion(item.id)}
                        className="rounded-full border border-border bg-surface px-3 py-1.5 text-xs font-medium text-text transition-colors hover:border-warm/50 hover:bg-warm-soft hover:text-warm"
                      >
                        {item.question}
                      </button>
                    ))}
                  </div>
                ) : (
                  <p className="text-xs text-text-muted">{t('assistant.noSuggestions')}</p>
                )}
              </div>
            )}
          </div>

          <footer className="shrink-0 border-t border-border/70 bg-surface px-3 py-3 sm:px-4">
            <div className="flex items-end gap-2 rounded-xl border border-border bg-surface-muted/40 p-2 ring-1 ring-black/[0.03] focus-within:border-warm/40 focus-within:ring-warm/15">
              <textarea
                ref={inputRef}
                rows={1}
                placeholder={t('assistant.inputPlaceholder')}
                className="max-h-28 min-h-[2.5rem] flex-1 resize-none bg-transparent px-2 py-2 text-sm text-text outline-none placeholder:text-text-muted"
                onKeyDown={(e) => {
                  if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault()
                    handleSubmit()
                  }
                }}
                onInput={(e) => {
                  const target = e.currentTarget
                  target.style.height = 'auto'
                  target.style.height = `${Math.min(target.scrollHeight, 112)}px`
                }}
              />
              <button
                type="button"
                onClick={handleSubmit}
                disabled={isTyping}
                aria-label={t('assistant.send')}
                className="ai-assistant-send flex size-10 shrink-0 items-center justify-center rounded-xl text-warm-text shadow-sm transition-transform hover:scale-[1.03] active:scale-[0.98] disabled:opacity-50"
              >
                <SendIcon />
              </button>
            </div>
            <p className="mt-2 text-center text-[10px] leading-relaxed text-text-muted">
              {t('assistant.disclaimer')}
            </p>
          </footer>
        </section>
      </Fragment>
    </Portal>
  )
}

function ChatBubble({
  message,
  locale,
}: {
  message: AssistantChatMessage
  locale: string
}) {
  const isUser = message.role === 'user'

  return (
    <div className={`flex gap-2 ${isUser ? 'flex-row-reverse' : 'flex-row'}`}>
      {!isUser && <AiAvatar size="sm" className="mt-0.5" />}
      <div className={`flex max-w-[85%] flex-col gap-1 ${isUser ? 'items-end' : 'items-start'}`}>
        <div
          className={`rounded-2xl px-3.5 py-2.5 text-sm leading-relaxed shadow-sm ${
            isUser
              ? 'rounded-ee-sm bg-warm text-warm-text'
              : 'rounded-es-sm border border-border/70 bg-surface text-text'
          }`}
        >
          {message.content}
        </div>
        <time
          dateTime={new Date(message.createdAt).toISOString()}
          className="px-1 text-[10px] text-text-muted"
        >
          {formatAssistantTime(message.createdAt, locale)}
        </time>
      </div>
    </div>
  )
}

function TypingBubble() {
  const { t } = useTranslation()

  return (
    <div className="flex gap-2">
      <AiAvatar size="sm" className="mt-0.5" />
      <div className="rounded-es-sm border border-border/70 bg-surface px-3.5 py-3 shadow-sm">
        <span className="sr-only">{t('assistant.typing')}</span>
        <span className="flex items-center gap-1" aria-hidden>
          <span className="ai-typing-dot size-1.5 rounded-full bg-text-muted" />
          <span className="ai-typing-dot size-1.5 rounded-full bg-text-muted [animation-delay:120ms]" />
          <span className="ai-typing-dot size-1.5 rounded-full bg-text-muted [animation-delay:240ms]" />
        </span>
      </div>
    </div>
  )
}

function AiAvatar({ size = 'md', className = '' }: { size?: 'sm' | 'md'; className?: string }) {
  const dim = size === 'sm' ? 'size-7' : 'size-10'

  return (
    <span
      className={`${dim} ai-assistant-avatar relative flex shrink-0 items-center justify-center rounded-full text-warm-text shadow-sm ${className}`}
      aria-hidden
    >
      <SparkleIcon className={size === 'sm' ? 'size-3.5' : 'size-[18px]'} />
    </span>
  )
}

function SparkleIcon({ className = '' }: { className?: string }) {
  return (
    <svg viewBox="0 0 24 24" fill="none" aria-hidden className={className}>
      <path
        d="M12 2.5 13.6 8.4 19.5 10 13.6 11.6 12 17.5 10.4 11.6 4.5 10 10.4 8.4 12 2.5Z"
        fill="currentColor"
        opacity="0.95"
      />
      <path
        d="M18.5 14.5 19.2 16.8 21.5 17.5 19.2 18.2 18.5 20.5 17.8 18.2 15.5 17.5 17.8 16.8 18.5 14.5Z"
        fill="currentColor"
        opacity="0.75"
      />
    </svg>
  )
}

function SendIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4.5 12.5 19 5.5 16 12.5 19 19.5 4.5 12.5Z"
        stroke="currentColor"
        strokeWidth="1.6"
        strokeLinejoin="round"
      />
      <path d="M16 12.5H8" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
    </svg>
  )
}
