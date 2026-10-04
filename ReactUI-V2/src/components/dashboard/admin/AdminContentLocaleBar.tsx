import { useTranslation } from 'react-i18next'
import { localeFromLanguageCode, shortLanguageLabel } from '@/extensions/languageCode'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useStoreLanguages } from '@/hooks/useStoreLanguages'
import type { Locale } from '@/models/shared/locale.model'
import { InlineLoading } from '@/components/ui/Spinner'

export function AdminContentLocaleBar({ className = '' }: { className?: string }) {
  const { t } = useTranslation()
  const { locale, switchLocale } = useLocaleSettings()
  const { languages, loading } = useStoreLanguages()

  const handleSwitch = (next: Locale) => {
    if (next !== locale) void switchLocale(next)
  }

  return (
    <div
      className={`flex flex-col gap-2 rounded-sm border border-border bg-surface-muted/35 px-3 py-2.5 sm:flex-row sm:items-center sm:justify-between ${className}`}
    >
      <p className="text-xs leading-relaxed text-text-muted">
        {t('dashboard.contentLocale.editingHint')}
      </p>
      {loading ? (
        <InlineLoading label={t('dashboard.contentLocale.loadingLanguages')} />
      ) : (
        <div className="flex flex-wrap items-center gap-1.5">
          {languages.map((language) => {
            const languageLocale = localeFromLanguageCode(language.code)
            if (!languageLocale) return null

            const isActive = languageLocale === locale
            return (
              <button
                key={language.id}
                type="button"
                onClick={() => handleSwitch(languageLocale)}
                className={`inline-flex items-center gap-1.5 rounded-sm px-2.5 py-1 text-[11px] transition-colors ${
                  isActive
                    ? 'bg-warm text-warm-text shadow-sm'
                    : 'border border-border bg-surface text-text-muted hover:border-warm/40 hover:bg-warm-soft/50 hover:text-warm'
                }`}
                aria-pressed={isActive}
              >
                <span className="font-semibold uppercase tracking-wide">
                  {shortLanguageLabel(language.code)}
                </span>
                <span className="hidden font-normal normal-case sm:inline">{language.name}</span>
              </button>
            )
          })}
        </div>
      )}
    </div>
  )
}
