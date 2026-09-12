import { useTranslation } from 'react-i18next'

export function AccessDeniedPanel() {
  const { t } = useTranslation()

  return (
    <div className="rounded-sm border border-dashed border-border bg-surface-muted/30 px-6 py-14 text-center">
      <p className="text-base font-semibold text-text">{t('dashboard.accessDenied.title')}</p>
      <p className="mx-auto mt-2 max-w-md text-sm leading-relaxed text-text-muted">
        {t('dashboard.accessDenied.message')}
      </p>
    </div>
  )
}
