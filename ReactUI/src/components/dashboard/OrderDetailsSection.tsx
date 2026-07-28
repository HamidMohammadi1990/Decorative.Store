import type { ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { DashboardOrder } from '@/models/dashboard/dashboard.model'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { useSettingsStore } from '@/stores/settingsStore'

interface OrderDetailsSectionProps {
  order: DashboardOrder
  formatAmount: (amount: number) => ReactNode
}

export function OrderDetailsSection({ order, formatAmount }: OrderDetailsSectionProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)

  return (
    <div className="border-t border-border bg-surface-muted/20 px-5 py-5 sm:px-6">
      <div className="grid gap-5 lg:grid-cols-2">
        <section className="rounded-sm border border-border bg-surface p-4">
          <h3 className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
            {t('dashboard.orders.detailsShipping')}
          </h3>
          <div className="mt-3 space-y-1 text-sm">
            <p className="font-semibold text-text">
              {order.shippingAddress.label} · {order.shippingAddress.recipient}
            </p>
            {order.shippingAddress.lines.map((line) => (
              <p key={line} className="text-text-muted">
                {line}
              </p>
            ))}
          </div>
          {order.estimatedDelivery && (
            <p className="mt-3 text-xs text-text-muted">
              {t('dashboard.orders.estimatedDelivery', {
                date: formatBlogDate(order.estimatedDelivery, locale),
              })}
            </p>
          )}
        </section>

        <section className="rounded-sm border border-border bg-surface p-4">
          <h3 className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
            {t('dashboard.orders.detailsPayment')}
          </h3>
          <dl className="mt-3 space-y-2 text-sm">
            <div className="flex items-center justify-between gap-3">
              <dt className="text-text-muted">{t('dashboard.orders.paymentMethod')}</dt>
              <dd className="font-medium text-text">
                {t(`dashboard.orders.payment.${order.paymentMethod}`)}
              </dd>
            </div>
            <div className="flex items-center justify-between gap-3">
              <dt className="text-text-muted">{t('dashboard.orders.subtotal')}</dt>
              <dd className="text-text">{formatAmount(order.subtotal)}</dd>
            </div>
            <div className="flex items-center justify-between gap-3">
              <dt className="text-text-muted">{t('dashboard.orders.shippingCost')}</dt>
              <dd className="text-text">
                {order.shippingCost === 0
                  ? t('dashboard.orders.shippingFree')
                  : formatAmount(order.shippingCost)}
              </dd>
            </div>
            <div className="flex items-center justify-between gap-3 border-t border-border pt-2">
              <dt className="font-semibold text-text">{t('dashboard.orders.total')}</dt>
              <dd className="font-semibold text-warm">{formatAmount(order.total)}</dd>
            </div>
          </dl>
        </section>
      </div>

      <section className="mt-5 rounded-sm border border-border bg-surface p-4">
        <h3 className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
          {t('dashboard.orders.detailsTimeline')}
        </h3>
        <ol className="mt-4 space-y-0">
          {order.timeline.map((step, index) => (
            <li key={step.key} className="flex gap-3">
              <div className="flex flex-col items-center">
                <span
                  className={`flex size-7 shrink-0 items-center justify-center rounded-full text-[11px] font-semibold ${
                    step.done
                      ? 'bg-warm text-warm-text'
                      : 'border border-border bg-surface text-text-muted'
                  }`}
                >
                  {step.done ? '✓' : index + 1}
                </span>
                {index < order.timeline.length - 1 && (
                  <span
                    aria-hidden
                    className={`my-1 w-px flex-1 min-h-6 ${
                      step.done ? 'bg-warm/40' : 'bg-border'
                    }`}
                  />
                )}
              </div>
              <div className="min-w-0 pb-4">
                <p className={`text-sm font-medium ${step.done ? 'text-text' : 'text-text-muted'}`}>
                  {t(`dashboard.orders.timeline.${step.key}`)}
                </p>
                {step.date && (
                  <p className="mt-0.5 text-xs text-text-muted">
                    {formatBlogDate(step.date, locale)}
                  </p>
                )}
              </div>
            </li>
          ))}
        </ol>
      </section>

      {order.trackingNumber && (
        <section
          id={`track-${order.id}`}
          className="mt-5 rounded-sm border border-warm/25 bg-warm-soft/40 p-4"
        >
          <h3 className="text-xs font-semibold uppercase tracking-[0.12em] text-warm">
            {t('dashboard.orders.trackingTitle')}
          </h3>
          <div className="mt-3 flex flex-wrap items-center justify-between gap-3">
            <div>
              <p className="font-mono text-sm font-semibold text-text">{order.trackingNumber}</p>
              {order.carrier && (
                <p className="mt-1 text-xs text-text-muted">
                  {t('dashboard.orders.carrier', { name: order.carrier })}
                </p>
              )}
            </div>
            <a
              href={`#track-${order.id}`}
              className="text-xs font-semibold text-warm hover:underline"
              onClick={(e) => e.preventDefault()}
            >
              {t('dashboard.orders.trackLink')}
            </a>
          </div>
        </section>
      )}
    </div>
  )
}
