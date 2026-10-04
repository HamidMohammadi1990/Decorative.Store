import type { AssistantIntent } from '@/models/assistant/assistantChat.model'

const INTENT_PATTERNS: Record<Exclude<AssistantIntent, 'greeting' | 'fallback'>, RegExp[]> = {
  shipping: [
    /\bship(ping)?\b/i,
    /\bdeliver(y|ies)?\b/i,
    /\btracking\b/i,
    /ارسال/,
    /تحویل/,
    /پست/,
    /پیک/,
  ],
  returns: [
    /\breturn(s|ing)?\b/i,
    /\brefund(s|ing)?\b/i,
    /\bexchange\b/i,
    /بازگشت/,
    /مرجوع/,
    /استرداد/,
    /تعویض/,
  ],
  orders: [
    /\border(s)?\b/i,
    /\btrack(ing)?\b/i,
    /سفارش/,
    /پیگیری/,
    /وضعیت/,
  ],
  recommend: [
    /\brecommend\b/i,
    /\bsuggest\b/i,
    /\brug(s)?\b/i,
    /\bsofa\b/i,
    /\bdecor\b/i,
    /پیشنهاد/,
    /معرفی/,
    /فرش/,
    /مبل/,
    /دکور/,
  ],
  payment: [
    /\bpay(ment)?\b/i,
    /\binstallment(s)?\b/i,
    /\bcard\b/i,
    /پرداخت/,
    /قسط/,
    /کارت/,
    /درگاه/,
  ],
  hours: [
    /\bstore hours\b/i,
    /\bopening\b/i,
    /\bcontact\b/i,
    /ساعت/,
    /تماس/,
    /شعبه/,
    /پشتیبانی/,
  ],
}

export function detectAssistantIntent(message: string): AssistantIntent {
  const trimmed = message.trim()
  if (!trimmed) return 'fallback'

  const greetingPattern =
    /^(hi|hello|hey|سلام|درود|صبح بخیر|عصر بخیر|وقت بخیر)\b/i
  if (greetingPattern.test(trimmed)) return 'greeting'

  for (const [intent, patterns] of Object.entries(INTENT_PATTERNS) as [
    Exclude<AssistantIntent, 'greeting' | 'fallback'>,
    RegExp[],
  ][]) {
    if (patterns.some((pattern) => pattern.test(trimmed))) {
      return intent
    }
  }

  return 'fallback'
}
