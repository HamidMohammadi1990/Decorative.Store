import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminDiscount, AdminDiscountType } from '@/models/admin/discount.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DiscountCodesIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminDiscountService } from '@/services/adminDiscountService'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

function toUtcIsoFromDateInput(value: string): string | null {
  if (!value.trim()) return null
  const date = new Date(`${value}T23:59:59`)
  if (Number.isNaN(date.getTime())) return null
  return date.toISOString()
}

function toDateInputValue(iso: string | null): string {
  if (!iso) return ''
  const date = new Date(iso)
  if (Number.isNaN(date.getTime())) return ''
  return date.toISOString().slice(0, 10)
}

function formatDiscountValue(item: AdminDiscount, locale: string): string {
  if (item.percentage > 0) {
    return `${item.percentage}%`
  }
  return new Intl.NumberFormat(locale === 'fa' ? 'fa-IR' : 'en-US').format(item.amount)
}

export function DiscountCodesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const uiLocale = useSettingsStore((s) => s.locale)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [code, setCode] = useState('')
  const [discountType, setDiscountType] = useState<AdminDiscountType>('percent')
  const [percentage, setPercentage] = useState(10)
  const [amount, setAmount] = useState(0)
  const [maxDiscountAmount, setMaxDiscountAmount] = useState(0)
  const [minimumAmount, setMinimumAmount] = useState('')
  const [usageLimit, setUsageLimit] = useState(1)
  const [remainingUses, setRemainingUses] = useState(1)
  const [expiryDate, setExpiryDate] = useState('')
  const [isActive, setIsActive] = useState(true)

  const fetchPage = useCallback(
    (pageNumber: number, pageSize: number) =>
      adminDiscountService.getAll(accessToken!, locale, { pageNumber, pageSize }),
    [accessToken, locale],
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
    reload,
  } = useAdminPagedList<AdminDiscount>({
    fetchPage,
    initialPageSize: 20,
    enabled: Boolean(accessToken && accessToken !== 'mock-access-token' && !languageLoading),
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.discountCodes.loadFailed'))
    : null

  const resetForm = () => {
    setCode('')
    setDiscountType('percent')
    setPercentage(10)
    setAmount(0)
    setMaxDiscountAmount(0)
    setMinimumAmount('')
    setUsageLimit(1)
    setRemainingUses(1)
    setExpiryDate('')
    setIsActive(true)
    setEditingId(null)
    setFormError(null)
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminDiscount) => {
    setEditingId(item.id)
    setCode(item.code)
    setDiscountType(item.percentage > 0 ? 'percent' : 'fixed')
    setPercentage(item.percentage > 0 ? item.percentage : 10)
    setAmount(item.amount > 0 ? item.amount : 0)
    setMaxDiscountAmount(item.maxDiscountAmount)
    setMinimumAmount(item.minimumAmount != null ? String(item.minimumAmount) : '')
    setUsageLimit(item.usageLimit)
    setRemainingUses(item.remainingUses)
    setExpiryDate(toDateInputValue(item.expiryDateOnUtc))
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const buildPayload = () => {
    const parsedMinimum = minimumAmount.trim() ? Number(minimumAmount) : null
    const limit = Math.max(1, usageLimit)
    return {
      code: code.trim().toUpperCase(),
      percentage: discountType === 'percent' ? Math.max(1, Math.min(100, percentage)) : 0,
      amount: discountType === 'fixed' ? Math.max(0, amount) : 0,
      expiryDateOnUtc: toUtcIsoFromDateInput(expiryDate),
      maxDiscountAmount: discountType === 'percent' ? Math.max(0, maxDiscountAmount) : 0,
      usageLimit: limit,
      remainingUses: mode === 'create' ? limit : Math.max(0, remainingUses),
      minimumAmount: parsedMinimum != null && !Number.isNaN(parsedMinimum) ? parsedMinimum : null,
      isActive,
    }
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.discountCodes.authRequired'))
      return
    }
    if (!code.trim()) {
      setFormError(t('dashboard.discountCodes.validationRequired'))
      return
    }
    if (discountType === 'percent' && percentage <= 0) {
      setFormError(t('dashboard.discountCodes.validationPercent'))
      return
    }
    if (discountType === 'fixed' && amount <= 0) {
      setFormError(t('dashboard.discountCodes.validationAmount'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      const payload = buildPayload()
      if (mode === 'edit' && editingId) {
        await adminDiscountService.update(accessToken, locale, { ...payload, id: editingId })
      } else {
        await adminDiscountService.create(accessToken, locale, payload)
      }
      reload()
      backToList()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.discountCodes.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.discountCodes.deleteConfirm'))) return

    setSaving(true)
    setFormError(null)
    try {
      await adminDiscountService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.discountCodes.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.discountCodes.editTitle')
              : t('dashboard.discountCodes.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.discountCodes.editDescription')
              : t('dashboard.discountCodes.createDescription')
          }
          icon={<DiscountCodesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.discountCodes.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value.toUpperCase())}
              className={adminInputClass}
              dir="ltr"
              maxLength={20}
              placeholder="SUMMER20"
            />
          </AdminField>

          <AdminField label={t('dashboard.discountCodes.fieldType')}>
            <select
              value={discountType}
              onChange={(e) => setDiscountType(e.target.value as AdminDiscountType)}
              className={adminInputClass}
            >
              <option value="percent">{t('dashboard.discountCodes.typePercent')}</option>
              <option value="fixed">{t('dashboard.discountCodes.typeFixed')}</option>
            </select>
          </AdminField>

          {discountType === 'percent' ? (
            <>
              <AdminField label={t('dashboard.discountCodes.fieldPercentage')}>
                <input
                  type="number"
                  min={1}
                  max={100}
                  value={percentage}
                  onChange={(e) => setPercentage(Number(e.target.value) || 0)}
                  className={adminInputClass}
                  dir="ltr"
                />
              </AdminField>
              <AdminField label={t('dashboard.discountCodes.fieldMaxDiscount')}>
                <input
                  type="number"
                  min={0}
                  value={maxDiscountAmount}
                  onChange={(e) => setMaxDiscountAmount(Number(e.target.value) || 0)}
                  className={adminInputClass}
                  dir="ltr"
                />
              </AdminField>
            </>
          ) : (
            <AdminField label={t('dashboard.discountCodes.fieldAmount')}>
              <input
                type="number"
                min={0}
                value={amount}
                onChange={(e) => setAmount(Number(e.target.value) || 0)}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>
          )}

          <div className="grid gap-5 sm:grid-cols-2">
            <AdminField label={t('dashboard.discountCodes.fieldUsageLimit')}>
              <input
                type="number"
                min={1}
                value={usageLimit}
                onChange={(e) => {
                  const next = Math.max(1, Number(e.target.value) || 1)
                  setUsageLimit(next)
                  if (mode === 'create') setRemainingUses(next)
                }}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>

            {mode === 'edit' && (
              <AdminField label={t('dashboard.discountCodes.fieldRemainingUses')}>
                <input
                  type="number"
                  min={0}
                  value={remainingUses}
                  onChange={(e) => setRemainingUses(Math.max(0, Number(e.target.value) || 0))}
                  className={adminInputClass}
                  dir="ltr"
                />
              </AdminField>
            )}

            <AdminField label={t('dashboard.discountCodes.fieldMinimumAmount')}>
              <input
                type="number"
                min={0}
                value={minimumAmount}
                onChange={(e) => setMinimumAmount(e.target.value)}
                className={adminInputClass}
                dir="ltr"
                placeholder="0"
              />
            </AdminField>

            <AdminField label={t('dashboard.discountCodes.fieldExpiry')}>
              <input
                type="date"
                value={expiryDate}
                onChange={(e) => setExpiryDate(e.target.value)}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>
          </div>

          {mode === 'edit' && (
            <label className="flex cursor-pointer items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.discountCodes.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.discountCodes.saving')} />
              ) : (
                t('dashboard.discountCodes.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.discountCodes.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<DiscountCodesIcon size={28} />}
        title={t('dashboard.discountCodes.emptyTitle')}
        message={t('dashboard.discountCodes.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.discountCodes.title')}
        description={t('dashboard.discountCodes.description')}
        icon={<DiscountCodesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.discountCodes.add')}
          </Button>
        }
      />

      {languageLoading || (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.discountCodes.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<DiscountCodesIcon size={28} />}
          title={t('dashboard.discountCodes.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.discountCodes.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<DiscountCodesIcon size={28} />}
          title={t('dashboard.discountCodes.emptyTitle')}
          message={t('dashboard.discountCodes.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.discountCodes.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {formError && <p className="text-sm text-sale">{formError}</p>}

          <AdminDataGrid
            columns={[
              {
                id: 'code',
                header: t('dashboard.discountCodes.colCode'),
                cell: (item) => (
                  <span className="font-mono font-semibold text-text" dir="ltr">
                    {item.code}
                  </span>
                ),
              },
              {
                id: 'value',
                header: t('dashboard.discountCodes.colValue'),
                cell: (item) => formatDiscountValue(item, uiLocale),
              },
              {
                id: 'uses',
                header: t('dashboard.discountCodes.colUses'),
                align: 'center',
                cell: (item) => (
                  <span className="text-xs text-text-muted" dir="ltr">
                    {item.remainingUses}/{item.usageLimit}
                  </span>
                ),
              },
              {
                id: 'expiry',
                header: t('dashboard.discountCodes.colExpiry'),
                cell: (item) =>
                  item.expiryDateOnUtc ? (
                    <span className="text-xs text-text-muted">
                      {formatBlogDate(item.expiryDateOnUtc, uiLocale)}
                    </span>
                  ) : (
                    '—'
                  ),
              },
              {
                id: 'status',
                header: t('dashboard.discountCodes.colStatus'),
                align: 'center',
                cell: (item) => (
                  <span
                    className={`inline-flex rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isActive
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isActive
                      ? t('dashboard.discountCodes.statusActive')
                      : t('dashboard.discountCodes.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: t('dashboard.discountCodes.colActions'),
                align: 'right',
                cell: (item) => (
                  <AdminGridActions>
                    <AdminGridIconButton
                      label={t('dashboard.discountCodes.edit')}
                      icon={<EditIcon size={15} />}
                      onClick={() => openEdit(item)}
                      disabled={saving}
                    />
                    <AdminGridIconButton
                      label={t('dashboard.discountCodes.delete')}
                      icon={<DeleteIcon size={15} />}
                      tone="danger"
                      onClick={() => void handleDelete(item.id)}
                      disabled={saving}
                    />
                  </AdminGridActions>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.discountCodes.loading')}
            pagination={{
              pageNumber,
              pageSize,
              totalCount,
              totalPages,
              onPageChange: goToPage,
            }}
          />
        </div>
      )}
    </div>
  )
}
