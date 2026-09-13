import { useCallback, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminOrderListItem, AdminOrderStatus } from '@/models/admin/order.model'
import type { DashboardOrder } from '@/models/dashboard/dashboard.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { OrderDetailsSection } from '@/components/dashboard/OrderDetailsSection'
import { OrdersIcon } from '@/components/dashboard/DashboardIcons'
import { OrderStatusBadge } from '@/components/dashboard/StatusBadge'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import { AdminGridActions, AdminGridIconButton } from '@/components/dashboard/admin/AdminGridActions'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { resolveActiveCurrency } from '@/extensions/resolveActiveCurrency'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import {
  adminOrderService,
  mapAdminOrderToDashboardOrder,
} from '@/services/adminOrderService'
import { useUserStore } from '@/stores/userStore'

const STATUS_FILTER_OPTIONS: Array<{ value: string; status?: AdminOrderStatus }> = [
  { value: '' },
  { value: '1', status: 1 },
  { value: '2', status: 2 },
  { value: '3', status: 3 },
  { value: '4', status: 4 },
]

function mapBackendStatusKey(status: AdminOrderStatus): string {
  switch (status) {
    case 4:
      return 'delivered'
    case 2:
      return 'cancelled'
    case 3:
      return 'processing'
    case 1:
    default:
      return 'pending'
  }
}

export function AdminOrdersPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, currency } = useLocaleSettings()
  const activeCurrency = resolveActiveCurrency(locale, currency)
  const { locale: apiLocale, loading: languageLoading } = useCurrentLanguageId()

  const [statusFilter, setStatusFilter] = useState('')
  const [titleFilter, setTitleFilter] = useState('')
  const [trackingFilter, setTrackingFilter] = useState('')
  const [appliedFilters, setAppliedFilters] = useState({
    status: '' as string,
    title: '',
    tracking: '',
  })

  const [selectedOrder, setSelectedOrder] = useState<AdminOrderListItem | null>(null)
  const [detailOrder, setDetailOrder] = useState<DashboardOrder | null>(null)
  const [detailLoading, setDetailLoading] = useState(false)
  const [detailError, setDetailError] = useState<string | null>(null)

  const resolvedStatus = useMemo(() => {
    const match = STATUS_FILTER_OPTIONS.find((option) => option.value === appliedFilters.status)
    return match?.status ?? null
  }, [appliedFilters.status])

  const trackingCode = useMemo(() => {
    const trimmed = appliedFilters.tracking.trim()
    if (!trimmed) return null
    const parsed = Number(trimmed)
    return Number.isFinite(parsed) ? parsed : null
  }, [appliedFilters.tracking])

  const fetchPage = useCallback(
    (pageNumber: number, pageSize: number) =>
      adminOrderService.getAll(accessToken!, apiLocale, {
        pageNumber,
        pageSize,
        status: resolvedStatus,
        title: appliedFilters.title.trim() || null,
        trackingCode,
      }),
    [accessToken, apiLocale, appliedFilters.title, resolvedStatus, trackingCode],
  )

  const {
    items,
    loading: listLoading,
    error: listError,
    pageNumber,
    pageSize,
    totalCount,
    totalPages,
    goToPage,
  } = useAdminPagedList<AdminOrderListItem>({
    fetchPage,
    initialPageSize: 20,
    enabled: Boolean(accessToken && accessToken !== 'mock-access-token' && !languageLoading),
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.adminOrders.loadFailed'))
    : null

  const applyFilters = () => {
    setAppliedFilters({
      status: statusFilter,
      title: titleFilter,
      tracking: trackingFilter,
    })
    goToPage(1)
  }

  const formatAmount = (amount: number) => (
    <PriceDisplay money={{ amount, currencyCode: 'IRT' }} currency={activeCurrency} />
  )

  const openDetail = async (item: AdminOrderListItem) => {
    if (selectedOrder?.id === item.id) {
      setSelectedOrder(null)
      setDetailOrder(null)
      setDetailError(null)
      return
    }

    setSelectedOrder(item)
    setDetailOrder(mapAdminOrderToDashboardOrder(item))
    setDetailError(null)

    if (!accessToken || accessToken === 'mock-access-token') return

    setDetailLoading(true)
    try {
      const detailed = await adminOrderService.getDetail(accessToken, apiLocale, item)
      setDetailOrder(detailed)
    } catch (err) {
      setDetailError(resolveAdminMutationError(err, t('dashboard.adminOrders.detailFailed')))
    } finally {
      setDetailLoading(false)
    }
  }

  const customerName = (item: AdminOrderListItem) =>
    [item.userFirstName, item.userLastName].filter(Boolean).join(' ') || '—'

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.adminOrders.title')}
        description={t('dashboard.adminOrders.description')}
        icon={<OrdersIcon size={22} />}
      />

      <div className="mb-6 rounded-sm border border-border bg-surface p-4">
        <div className="flex flex-col gap-4 lg:flex-row lg:items-end">
          <div className="grid flex-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            <AdminField label={t('dashboard.adminOrders.filterStatus')}>
              <select
                value={statusFilter}
                onChange={(event) => setStatusFilter(event.target.value)}
                className={adminInputClass}
              >
                {STATUS_FILTER_OPTIONS.map((option) => (
                  <option key={option.value || 'all'} value={option.value}>
                    {option.value
                      ? t(`dashboard.adminOrders.status.${option.value}`)
                      : t('dashboard.adminOrders.statusAll')}
                  </option>
                ))}
              </select>
            </AdminField>

            <AdminField label={t('dashboard.adminOrders.filterTitle')}>
              <input
                type="search"
                value={titleFilter}
                onChange={(event) => setTitleFilter(event.target.value)}
                placeholder={t('dashboard.adminOrders.filterTitlePlaceholder')}
                className={adminInputClass}
                onKeyDown={(event) => {
                  if (event.key === 'Enter') applyFilters()
                }}
              />
            </AdminField>

            <AdminField label={t('dashboard.adminOrders.filterTracking')}>
              <input
                type="search"
                inputMode="numeric"
                value={trackingFilter}
                onChange={(event) => setTrackingFilter(event.target.value)}
                placeholder={t('dashboard.adminOrders.filterTrackingPlaceholder')}
                className={adminInputClass}
                onKeyDown={(event) => {
                  if (event.key === 'Enter') applyFilters()
                }}
              />
            </AdminField>
          </div>

          <div className="shrink-0 lg:min-w-[9.5rem]">
            <Button
              type="button"
              variant="primary"
              className="w-full py-2.5 lg:w-auto"
              onClick={applyFilters}
            >
              {t('dashboard.adminOrders.applyFilters')}
            </Button>
          </div>
        </div>
      </div>

      {listErrorMessage && (
        <p className="mb-4 text-sm text-sale" role="alert">
          {listErrorMessage}
        </p>
      )}

      <AdminDataGrid
        rows={items}
        rowKey={(row) => row.id}
        loading={listLoading}
        loadingLabel={t('common.loading')}
        emptyState={
          <DashboardEmptyState
            icon={<OrdersIcon size={28} />}
            title={t('dashboard.adminOrders.emptyTitle')}
            message={t('dashboard.adminOrders.emptyMessage')}
          />
        }
        pagination={{
          pageNumber,
          pageSize,
          totalCount,
          totalPages,
          onPageChange: goToPage,
        }}
        columns={[
          {
            id: 'tracking',
            header: t('dashboard.adminOrders.colTracking'),
            cell: (row) => (
              <span className="font-mono text-xs text-text">
                {row.trackingCode > 0 ? row.trackingCode : '—'}
              </span>
            ),
          },
          {
            id: 'title',
            header: t('dashboard.adminOrders.colTitle'),
            cell: (row) => <span className="font-medium text-text">{row.title || '—'}</span>,
          },
          {
            id: 'customer',
            header: t('dashboard.adminOrders.colCustomer'),
            cell: (row) => <span className="text-text-muted">{customerName(row)}</span>,
          },
          {
            id: 'status',
            header: t('dashboard.adminOrders.colStatus'),
            cell: (row) => <OrderStatusBadge status={mapBackendStatusKey(row.status)} />,
          },
          {
            id: 'total',
            header: t('dashboard.adminOrders.colTotal'),
            align: 'right',
            cell: (row) => (
              <span className="font-medium tabular-nums text-text">
                {formatAmount(row.finalPrice || row.totalPrice)}
              </span>
            ),
          },
          {
            id: 'date',
            header: t('dashboard.adminOrders.colDate'),
            cell: (row) => (
              <span className="text-text-muted">
                {formatBlogDate(row.createdOnUtc, locale)}
              </span>
            ),
          },
          {
            id: 'actions',
            header: t('common.grid.actions'),
            align: 'right',
            cell: (row) => (
              <AdminGridActions>
                <AdminGridIconButton
                  onClick={() => void openDetail(row)}
                  label={
                    selectedOrder?.id === row.id
                      ? t('dashboard.adminOrders.hideDetail')
                      : t('dashboard.adminOrders.viewDetail')
                  }
                  icon={<OrdersIcon size={15} />}
                />
              </AdminGridActions>
            ),
          },
        ]}
      />

      {selectedOrder && detailOrder && (
        <div className="mt-6 overflow-hidden rounded-sm border border-border bg-surface shadow-sm">
          <div className="border-b border-border bg-surface-muted/40 px-5 py-4 sm:px-6">
            <div className="flex flex-wrap items-start justify-between gap-3">
              <div>
                <p className="text-[10px] font-semibold uppercase tracking-[0.16em] text-warm">
                  {t('dashboard.adminOrders.detailEyebrow')}
                </p>
                <h2 className="mt-1 text-lg font-semibold text-text">
                  {selectedOrder.title}
                  {selectedOrder.trackingCode > 0 ? ` · #${selectedOrder.trackingCode}` : ''}
                </h2>
                <p className="mt-1 text-sm text-text-muted">
                  {t('dashboard.adminOrders.customerLine', { name: customerName(selectedOrder) })}
                </p>
              </div>
              <OrderStatusBadge status={mapBackendStatusKey(selectedOrder.status)} />
            </div>
          </div>

          {detailLoading ? (
            <div className="px-5 py-10 sm:px-6">
              <InlineLoading label={t('dashboard.adminOrders.loadingDetail')} />
            </div>
          ) : detailError ? (
            <p className="px-5 py-6 text-sm text-sale sm:px-6" role="alert">
              {detailError}
            </p>
          ) : (
            <OrderDetailsSection order={detailOrder} formatAmount={formatAmount} />
          )}
        </div>
      )}
    </div>
  )
}
