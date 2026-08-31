import { useTranslation } from 'react-i18next'
import { ADMIN_ROW_NUMBER_CLASS } from '@/components/dashboard/admin/AdminRowNumber'

const HEADER_ROW_CLASS =
  'flex items-center gap-3 border-b border-border bg-surface-muted/50 px-4 py-2.5 text-[11px] font-semibold uppercase tracking-[0.12em] text-text-muted sm:px-5'

export function AdminListGridHeader({
  contentLabel,
  actionsLabel,
  showActions = true,
}: {
  contentLabel?: string
  actionsLabel?: string
  showActions?: boolean
}) {
  const { t } = useTranslation()

  return (
    <li className={HEADER_ROW_CLASS} aria-hidden>
      <span className={ADMIN_ROW_NUMBER_CLASS}>{t('common.grid.row')}</span>
      <span className="min-w-0 flex-1">{contentLabel ?? t('common.grid.content')}</span>
      {showActions ? (
        <span className="shrink-0">{actionsLabel ?? t('common.grid.actions')}</span>
      ) : null}
    </li>
  )
}
