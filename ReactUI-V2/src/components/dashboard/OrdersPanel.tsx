import { useEffect, useMemo, useState } from 'react'

import { useTranslation } from 'react-i18next'

import type { DashboardOrder, OrderSortOption } from '@/models/dashboard/dashboard.model'

import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'

import { filterDashboardOrders } from '@/extensions/filterDashboardOrders'

import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'

import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'

import { OrderDetailsSection } from '@/components/dashboard/OrderDetailsSection'

import { OrderFiltersBar } from '@/components/dashboard/OrderFiltersBar'

import { OrderStatusBadge } from '@/components/dashboard/StatusBadge'

import { OrdersIcon } from '@/components/dashboard/DashboardIcons'

import { Button } from '@/components/ui/Button'

import { InlineLoading } from '@/components/ui/Spinner'

import { PriceDisplay } from '@/components/ui/PriceDisplay'

import { formatBlogDate } from '@/extensions/formatBlogDate'

import { resolveActiveCurrency } from '@/extensions/resolveActiveCurrency'

import { useLocaleSettings } from '@/hooks/useLocaleSettings'

import { useOrders } from '@/hooks/useOrders'



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



function getOrderLabel(order: DashboardOrder) {

  return order.displayId ?? order.id

}



export function OrdersPanel() {

  const { t } = useTranslation()

  const { locale, currency } = useLocaleSettings()

  const activeCurrency = resolveActiveCurrency(locale, currency)

  const {

    statuses,

    selectedStatusId,

    orders,

    statusesLoading,

    ordersLoading,

    error,

    detailLoadingId,

    setSelectedStatusId,

    reload,

    loadOrderDetail,

  } = useOrders()

  const [query, setQuery] = useState('')

  const [sort, setSort] = useState<OrderSortOption>('newest')

  const [expandedId, setExpandedId] = useState<string | null>(null)



  const formatAmount = (amount: number, currencyCode = 'IRT') => (

    <PriceDisplay money={{ amount, currencyCode }} currency={activeCurrency} />

  )



  const filteredOrders = useMemo(

    () => filterDashboardOrders(orders, { query, sort }),

    [orders, query, sort],

  )



  const toggleDetails = async (order: DashboardOrder) => {

    const nextExpanded = expandedId === order.id ? null : order.id

    setExpandedId(nextExpanded)



    if (nextExpanded && !order.detailsLoaded) {

      await loadOrderDetail(order)

    }

  }



  if (statusesLoading) {

    return (

      <div>

        <DashboardPageHeader

          title={t('dashboard.orders.title')}

          description={t('dashboard.orders.description')}

          icon={<OrdersIcon size={22} />}

        />

        <InlineLoading label={t('common.loading')} />

      </div>

    )

  }



  if (error) {

    return (

      <div>

        <DashboardPageHeader

          title={t('dashboard.orders.title')}

          description={t('dashboard.orders.description')}

          icon={<OrdersIcon size={22} />}

        />

        <DashboardEmptyState

          icon={<OrdersIcon size={28} />}

          title={t('dashboard.orders.loadFailedTitle')}

          message={t('dashboard.orders.loadFailedMessage')}

          action={

            <Button variant="warm" onClick={() => void reload()}>

              {t('common.retry')}

            </Button>

          }

        />

      </div>

    )

  }



  if (statuses.length === 0) {

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

        statuses={statuses}

        selectedStatusId={selectedStatusId}

        query={query}

        sort={sort}

        resultCount={filteredOrders.length}

        onStatusChange={setSelectedStatusId}

        onQueryChange={setQuery}

        onSortChange={setSort}

      />



      {ordersLoading ? (

        <InlineLoading label={t('common.loading')} />

      ) : filteredOrders.length === 0 ? (

        <DashboardEmptyState

          icon={<OrdersIcon size={28} />}

          title={t('dashboard.orders.noResultsTitle')}

          message={t('dashboard.orders.noResultsMessage')}

          action={

            query ? (

              <Button variant="secondary" onClick={() => setQuery('')}>

                {t('dashboard.orders.clearFilters')}

              </Button>

            ) : undefined

          }

        />

      ) : (

        <div className="space-y-5">

          {filteredOrders.map((order) => {

            const isExpanded = expandedId === order.id

            const hasTracking = Boolean(order.trackingNumber)

            const orderLabel = getOrderLabel(order)



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

                      {t('dashboard.orders.orderId', { id: orderLabel })}

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

                        void toggleDetails(order)

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

                    disabled={detailLoadingId === order.id}

                    onClick={() => void toggleDetails(order)}

                  >

                    {detailLoadingId === order.id ? (

                      <InlineLoading label={t('dashboard.orders.details')} />

                    ) : isExpanded ? (

                      t('dashboard.orders.hideDetails')

                    ) : (

                      t('dashboard.orders.details')

                    )}

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

