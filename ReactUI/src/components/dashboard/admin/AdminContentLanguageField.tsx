import { useTranslation } from 'react-i18next'
import { shortLanguageLabel } from '@/extensions/languageCode'
import type { StoreLanguage } from '@/services/languageService'
import { AdminField, adminInputClass } from '@/components/dashboard/admin/adminFormShared'

export function AdminContentLanguageField({
  value,
  onChange,
  languages,
  disabled = false,
  className = '',
}: {
  value: number | null
  onChange: (languageId: number) => void
  languages: StoreLanguage[]
  disabled?: boolean
  className?: string
}) {
  const { t } = useTranslation()

  return (
    <div className={className}>
      <AdminField label={t('dashboard.contentLocale.fieldLanguage')}>
        <select
          value={value ?? ''}
          disabled={disabled || value == null}
          onChange={(e) => onChange(Number(e.target.value))}
          className={adminInputClass}
        >
          {languages.map((language) => (
            <option key={language.id} value={language.id}>
              {shortLanguageLabel(language.code)} — {language.name}
            </option>
          ))}
        </select>
      </AdminField>
      <p className="mt-1.5 text-xs leading-relaxed text-text-muted">
        {t('dashboard.contentLocale.fieldHint')}
      </p>
    </div>
  )
}
