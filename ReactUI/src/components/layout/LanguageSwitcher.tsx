import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import type { Locale } from '@/models/shared/locale.model'

const LOCALE_OPTIONS: { value: Locale; short: string; fullKey: 'english' | 'persian' }[] = [
  { value: 'en', short: 'EN', fullKey: 'english' },
  { value: 'fa', short: 'FA', fullKey: 'persian' },
]

export function LanguageSwitcher() {
  const { locale, switchLocale, t } = useLocaleSettings()

  return (
    <div
      role="group"
      aria-label={t('common.language')}
      className="inline-flex items-center gap-1.5 rounded-full border border-warm-muted bg-warm-soft/60 p-0.5"
    >
      <GlobeIcon className="ms-1.5 hidden text-warm sm:block" />
      {LOCALE_OPTIONS.map((opt) => {
        const isActive = locale === opt.value

        return (
          <button
            key={opt.value}
            type="button"
            aria-pressed={isActive}
            aria-label={t(`common.${opt.fullKey}`)}
            title={t(`common.${opt.fullKey}`)}
            onClick={() => void switchLocale(opt.value)}
            className={`min-w-9 rounded-full px-2.5 py-1 text-xs font-semibold tracking-wide transition-all ${
              isActive
                ? 'bg-warm text-warm-text shadow-sm'
                : 'text-text-muted hover:text-warm'
            }`}
          >
            {opt.short}
          </button>
        )
      })}
    </div>
  )
}

function GlobeIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      width="14"
      height="14"
      viewBox="0 0 14 14"
      fill="none"
      aria-hidden
      className={className}
    >
      <circle cx="7" cy="7" r="5.25" stroke="currentColor" strokeWidth="1.1" />
      <path
        d="M1.75 7h10.5M7 1.75c1.5 1.75 2.25 3.7 2.25 5.25S8.5 10.5 7 12.25M7 1.75C5.5 3.5 4.75 5.45 4.75 7S5.5 10.5 7 12.25"
        stroke="currentColor"
        strokeWidth="1.1"
        strokeLinecap="round"
      />
    </svg>
  )
}
