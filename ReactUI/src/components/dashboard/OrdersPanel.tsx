import { useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { DashboardOrder, OrderSortOption, OrderStatusFilter } from '@/models/dashboard/dashboard.model'
import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'
import {
  countOrdersByStatus,
  filterDashboardOrders,
} from '@/extensions/filterDashboardOrders'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { OrderDetailsSection } from '@/components/dashboard/OrderDetailsSection'
import { OrderFiltersBar } from '@/components/dashboard/OrderFiltersBar'
import { OrderStatusBadge } from '@/components/dashboard/StatusBadge'
import { OrdersIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { resolveActiveCurrency } from '@/extensions/resolveActiveCurrency'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface OrdersPanelProps {
  orders: DashboardOrder[]
}

function OrderItemImage({ src, title }: { src?: string; title: string }) {
  const primary = src ? resolveHomeImageSrc(src) : ''
  const fallback = primary.endsWith('.jpg')
    ? primary.replace(/\.jpg$/i, '.svg')
    : primary.replace(/\.svg$/i, '.jpg')

  const [imgSrc, setImgSrc] = useState(primary)
  const [failed, setFailed] = useState(!primary)

  useEffect(() => {
    setImgSrc(primary)
    setFailed(!primary)
  }, [primary])

  if (failed) {
    return <OrdersIcon size={22} className="text-warm" />
  }

  return (
    <img
      src={imgSrc}
      alt={title}
      className="size-full object-cover"
      onError={() => {
        if (imgSrc !== fallback) {
          setImgSrc(fallback)
          return
        }
        setFailed(true)
      }}
    />
  )
}

const statusAccent: Record<string, string> = {
  pending: 'bg-warm',
  processing: 'bg-text-muted',
  shipped: 'bg-accent',
  delivered: 'bg-accent',
  cancelled: 'bg-sale',
}

export function OrdersPanel({ orders }: OrdersPanelProps) {
  const { t } = useTranslation()
  const { locale, currency } = useLocaleSettings()
  const activeCurrency = resolveActiveCurrency(locale, currency)
  const [statusFilter, setStatusFilter] = useState<OrderStatusFilter>('all')
  const [query, setQuery] = useState('')
  const [sort, setSort] = useState<OrderSortOption>('newest')
  const [expandedId, setExpandedId] = useState<string | null>(null)

  const formatAmount = (amount: number, currencyCode = 'IRT') => (
    <PriceDisplay money={{ amount, currencyCode }} currency={activeCurrency} />
  )

  const statusCounts = useMemo(() => countOrdersByStatus(orders), [orders])

  const filteredOrders = useMemo(
    () => filterDashboardOrders(orders, { status: statusFilter, query, sort }),
    [orders, statusFilter, query, sort],
  )

  const toggleDetails = (orderId: string) => {
    setExpandedId((current) => (current === orderId ? null : orderId))
  }

  if (orders.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.orders.title')}
          description={t('dashboard.orders.description')}
          icon={<OrdersIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<OrdersIcon size={28} />}
          title={t('dashboard.orders.emptyTitle')}
          message={t('dashboard.orders.emptyMessage')}
        />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.orders.title')}
        description={t('dashboard.orders.description')}
        icon={<OrdersIcon size={22} />}
      />

      <OrderFiltersBar
        status={statusFilter}
        query={query}
        sort={sort}
        counts={statusCounts}
        resultCount={filteredOrders.length}
        onStatusChange={setStatusFilter}
        onQueryChange={setQuery}
        onSortChange={setSort}
      />

      {filteredOrders.length === 0 ? (
        <DashboardEmptyState
          icon={<OrdersIcon size={28} />}
          title={t('dashboard.orders.noResultsTitle')}
          message={t('dashboard.orders.noResultsMessage')}
          action={
            <Button
              variant="secondary"
              onClick={() => {
                setStatusFilter('all')
                setQuery('')
              }}
            >
              {t('dashboard.orders.clearFilters')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-5">
          {filteredOrders.map((order) => {
            const isExpanded = expandedId === order.id
            const hasTracking = Boolean(order.trackingNumber)

            return (
              <article
                key={order.id}
                className="overflow-hidden rounded-sm border border-border bg-surface shadow-sm transition-shadow duration-200 hover:shadow-md"
              >
                <div
                  className={`h-1 ${statusAccent[order.status] ?? 'bg-border'}`}
                  aria-hidden
                />
                <header className="flex flex-wrap items-center justify-between gap-3 border-b border-border bg-surface-muted/30 px-5 py-4 sm:px-6">
                  <div>
                    <p className="text-sm font-semibold text-text">
                      {t('dashboard.orders.orderId', { id: order.id })}
                    </p>
                    <p className="mt-0.5 text-xs text-text-muted">
                      {formatBlogDate(order.date, locale)}
                      <span className="mx-2 text-border">·</span>
                      {t('dashboard.orders.itemCount', { count: order.items.length })}
                    </p>
                  </div>
                  <div className="flex items-center gap-3">
                    <OrderStatusBadge status={order.status} />
                    <span className="rounded-sm bg-warm-soft px-2.5 py-1 text-sm font-semibold text-warm">
                      {formatAmount(order.total)}
                    </span>
                  </div>
                </header>

                <ul className="divide-y divide-border">
                  {order.items.map((item, index) => (
                    <li
                      key={`${order.id}-${index}`}
                      className="flex items-center gap-4 px-5 py-4 transition-colors hover:bg-surface-muted/30 sm:px-6"
                    >
                      <div className="flex size-16 shrink-0 items-center justify-center overflow-hidden rounded-sm bg-warm-soft ring-1 ring-border">
                        <OrderItemImage src={item.imageUrl} title={item.title} />
                      </div>
                      <div className="min-w-0 flex-1">
                        <p className="truncate text-sm font-medium text-text">{item.title}</p>
                        <p className="mt-0.5 text-xs text-text-muted">
                          {t('dashboard.orders.quantity', { count: item.quantity })}
                        </p>
                      </div>
                      <span className="shrink-0 text-sm font-medium text-text-muted">
                        {formatAmount(item.price)}
                      </span>
                    </li>
                  ))}
                </ul>

                <footer className="flex flex-wrap gap-2 border-t border-border bg-surface-muted/20 px-5 py-4 sm:px-6">
                  {hasTracking && (
                    <Button
                      variant="secondary"
                      className="text-xs"
                      onClick={() => {
                        setExpandedId(order.id)
                        requestAnimationFrame(() => {
                          document
                            .getElementById(`track-${order.id}`)
                            ?.scrollIntoView({ behavior: 'smooth', block: 'nearest' })
                        })
                      }}
                    >
                      {t('dashboard.orders.track')}
                    </Button>
                  )}
                  <Button
                    variant={isExpanded ? 'warm' : 'ghost'}
                    className="text-xs"
                    aria-expanded={isExpanded}
                    onClick={() => toggleDetails(order.id)}
                  >
                    {isExpanded ? t('dashboard.orders.hideDetails') : t('dashboard.orders.details')}
                  </Button>
                </footer>

                {isExpanded && (
                  <OrderDetailsSection
                    order={order}
                    formatAmount={(amount) => formatAmount(amount)}
                  />
                )}
              </article>
            )
          })}
        </div>
      )}
    </div>
  )
}
