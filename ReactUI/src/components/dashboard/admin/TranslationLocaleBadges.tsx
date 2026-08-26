import type { CatalogTranslation } from '@/models/admin/catalog.model'
import type { StoreLanguage } from '@/services/languageService'
import { shortLanguageLabel } from '@/extensions/languageCode'
import { translationLanguageIds } from '@/services/admin/adminCatalogNormalize'

export function TranslationLocaleBadges({
  translations,
  languages,
  currentLanguageId,
}: {
  translations: CatalogTranslation[]
  languages: StoreLanguage[]
  currentLanguageId?: number
}) {
  const filledIds = new Set(translationLanguageIds(translations))

  if (languages.length === 0) return null

  return (
    <span className="flex flex-wrap gap-1">
      {languages.map((language) => {
        const filled = filledIds.has(language.id)
        const isCurrent = currentLanguageId != null && language.id === currentLanguageId

        return (
          <span
            key={language.id}
            title={language.name}
            className={`inline-flex rounded-sm px-1.5 py-0.5 text-[10px] font-bold uppercase tracking-wide ${
              filled
                ? isCurrent
                  ? 'bg-warm text-warm-text'
                  : 'bg-warm-soft/80 text-warm'
                : 'border border-dashed border-border bg-surface-muted/50 text-text-muted/70'
            }`}
          >
            {shortLanguageLabel(language.code)}
          </span>
        )
      })}
    </span>
  )
}
