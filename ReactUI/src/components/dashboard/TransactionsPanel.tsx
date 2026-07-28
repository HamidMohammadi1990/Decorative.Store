import { useTranslation } from 'react-i18next'
import type { DashboardTransaction } from '@/models/dashboard/dashboard.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { TransactionsIcon } from '@/components/dashboard/DashboardIcons'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { resolveActiveCurrency } from '@/extensions/resolveActiveCurrency'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface TransactionsPanelProps {
  transactions: DashboardTransaction[]
}

export function TransactionsPanel({ transactions }: TransactionsPanelProps) {
  const { t } = useTranslation()
  const { locale, currency } = useLocaleSettings()
  const activeCurrency = resolveActiveCurrency(locale, currency)

  const formatAmount = (amount: number) => (
    <PriceDisplay money={{ amount, currencyCode: 'IRT' }} currency={activeCurrency} />
  )

  if (transactions.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.transactions.title')}
          description={t('dashboard.transactions.description')}
          icon={<TransactionsIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<TransactionsIcon size={28} />}
          title={t('dashboard.transactions.emptyTitle')}
          message={t('dashboard.transactions.emptyMessage')}
        />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.transactions.title')}
        description={t('dashboard.transactions.description')}
        icon={<TransactionsIcon size={22} />}
      />

      <div className="overflow-hidden rounded-sm border border-border bg-surface shadow-sm">
        <div className="hidden border-b border-border bg-surface-muted/30 px-5 py-3 text-xs font-semibold uppercase tracking-wider text-text-muted sm:grid sm:grid-cols-[1fr_auto_auto] sm:gap-4 sm:px-6">
          <span>{t('dashboard.transactions.colDescription')}</span>
          <span className="text-end">{t('dashboard.transactions.colDate')}</span>
          <span className="w-28 text-end">{t('dashboard.transactions.colAmount')}</span>
        </div>

        <ul className="divide-y divide-border">
          {transactions.map((tx) => (
            <li
              key={tx.id}
              className="px-5 py-4 transition-colors hover:bg-surface-muted/40 sm:grid sm:grid-cols-[1fr_auto_auto] sm:items-center sm:gap-4 sm:px-6"
            >
              <div className="flex items-start gap-3">
                <span
                  className={`mt-0.5 flex size-9 shrink-0 items-center justify-center rounded-sm text-sm font-semibold ${
                    tx.type === 'credit' ? 'bg-accent/10 text-accent' : 'bg-sale/10 text-sale'
                  }`}
                >
                  {tx.type === 'credit' ? '↑' : '↓'}
                </span>
                <div className="min-w-0">
                  <p className="text-sm font-medium text-text">{tx.description}</p>
                  <p className="mt-0.5 text-xs text-text-muted sm:hidden">
                    {formatBlogDate(tx.date, locale)}
                  </p>
                  <p className="mt-0.5 font-mono text-[11px] text-text-muted/80">{tx.id}</p>
                </div>
              </div>
              <span className="mt-2 hidden text-sm text-text-muted sm:mt-0 sm:block sm:text-end">
                {formatBlogDate(tx.date, locale)}
              </span>
              <span
                className={`mt-1 inline-flex items-center justify-end gap-0.5 text-sm font-semibold sm:mt-0 sm:w-28 sm:text-end ${
                  tx.type === 'credit' ? 'text-accent' : 'text-sale'
                }`}
              >
                <span>{tx.type === 'credit' ? '+' : '−'}</span>
                {formatAmount(tx.amount)}
              </span>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
