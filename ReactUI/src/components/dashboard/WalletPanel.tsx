import { useTranslation } from 'react-i18next'
import type { WalletInfo, DashboardTransaction } from '@/models/dashboard/dashboard.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardStatCard } from '@/components/dashboard/DashboardStatCard'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardSection } from '@/components/dashboard/DashboardSection'
import { WalletIcon, TransactionsIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { resolveActiveCurrency } from '@/extensions/resolveActiveCurrency'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface WalletPanelProps {
  wallet: WalletInfo
  recentTransactions: DashboardTransaction[]
}

export function WalletPanel({ wallet, recentTransactions }: WalletPanelProps) {
  const { t } = useTranslation()
  const { locale, currency } = useLocaleSettings()
  const activeCurrency = resolveActiveCurrency(locale, currency)

  const balanceFormatted = (
    <PriceDisplay
      money={{ amount: wallet.balance, currencyCode: wallet.currencyCode }}
      currency={activeCurrency}
      iconSize={18}
    />
  )

  const credits = recentTransactions
    .filter((tx) => tx.type === 'credit')
    .reduce((sum, tx) => sum + tx.amount, 0)

  const debits = recentTransactions
    .filter((tx) => tx.type === 'debit')
    .reduce((sum, tx) => sum + tx.amount, 0)

  const formatAmount = (amount: number) => (
    <PriceDisplay
      money={{ amount, currencyCode: wallet.currencyCode }}
      currency={activeCurrency}
    />
  )

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.wallet.title')}
        description={t('dashboard.wallet.description')}
        icon={<WalletIcon size={22} />}
        action={
          <div className="flex flex-wrap gap-2">
            <Button variant="warm">{t('dashboard.wallet.topUp')}</Button>
            <Button variant="secondary">{t('dashboard.wallet.withdraw')}</Button>
          </div>
        }
      />

      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <DashboardStatCard
          label={t('dashboard.wallet.balanceLabel')}
          value={balanceFormatted}
          hint={t('dashboard.wallet.balanceHint')}
          icon={<WalletIcon size={20} />}
          accent
        />
        <DashboardStatCard
          label={t('dashboard.wallet.creditsLabel')}
          value={formatAmount(credits)}
          hint={t('dashboard.wallet.creditsHint')}
          icon={<TransactionsIcon size={20} />}
        />
        <DashboardStatCard
          label={t('dashboard.wallet.debitsLabel')}
          value={formatAmount(debits)}
          hint={t('dashboard.wallet.debitsHint')}
          icon={<TransactionsIcon size={20} />}
        />
      </div>

      <DashboardSection
        title={t('dashboard.wallet.recentTitle')}
        icon={<TransactionsIcon size={18} />}
        className="mt-8"
      >
        {recentTransactions.length === 0 ? (
          <div className="p-6">
            <DashboardEmptyState
              icon={<TransactionsIcon size={28} />}
              title={t('dashboard.transactions.emptyTitle')}
              message={t('dashboard.transactions.emptyMessage')}
            />
          </div>
        ) : (
          <ul className="divide-y divide-border">
            {recentTransactions.slice(0, 5).map((tx) => (
              <li
                key={tx.id}
                className="flex items-center justify-between gap-4 px-5 py-4 transition-colors hover:bg-surface-muted/40 sm:px-6"
              >
                <div className="flex min-w-0 items-center gap-3">
                  <span
                    className={`flex size-9 shrink-0 items-center justify-center rounded-sm text-sm font-semibold ${
                      tx.type === 'credit'
                        ? 'bg-accent/10 text-accent'
                        : 'bg-sale/10 text-sale'
                    }`}
                  >
                    {tx.type === 'credit' ? '↑' : '↓'}
                  </span>
                  <div className="min-w-0">
                    <p className="truncate text-sm font-medium text-text">{tx.description}</p>
                    <p className="mt-0.5 text-xs text-text-muted">
                      {formatBlogDate(tx.date, locale)}
                    </p>
                  </div>
                </div>
                <span
                  className={`inline-flex shrink-0 items-center gap-0.5 text-sm font-semibold ${
                    tx.type === 'credit' ? 'text-accent' : 'text-sale'
                  }`}
                >
                  <span>{tx.type === 'credit' ? '+' : '−'}</span>
                  {formatAmount(tx.amount)}
                </span>
              </li>
            ))}
          </ul>
        )}
      </DashboardSection>
    </div>
  )
}
