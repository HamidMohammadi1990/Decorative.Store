export function formatAssistantTime(timestamp: number, locale: string): string {
  return new Intl.DateTimeFormat(locale === 'fa' ? 'fa-IR' : 'en-US', {
    hour: 'numeric',
    minute: '2-digit',
  }).format(new Date(timestamp))
}
