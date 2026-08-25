import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { DashboardCoupon } from '@/models/dashboard/dashboard.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CouponStatusBadge } from '@/components/dashboard/StatusBadge'
import { CouponsIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { useMyDiscounts } from '@/hooks/useMyDiscounts'
import { useSettingsStore } from '@/stores/settingsStore'
import { useEarnedCouponsStore } from '@/stores/earnedCouponsStore'

function mergeCoupons(
  apiCoupons: DashboardCoupon[],
  earnedCoupons: DashboardCoupon[],
): DashboardCoupon[] {
  const seenCodes = new Set<string>()
  const merged: DashboardCoupon[] = []

  for (const coupon of [...earnedCoupons, ...apiCoupons]) {
    const key = coupon.code.trim().toUpperCase()
    if (!key || seenCodes.has(key)) continue
    seenCodes.add(key)
    merged.push(coupon)
  }

  return merged.sort((a, b) => {
    if (a.status === 'active' && b.status !== 'active') return -1
    if (a.status !== 'active' && b.status === 'active') return 1
    return a.expiresAt.localeCompare(b.expiresAt)
  })
}

export function CouponsPanel() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const earnedCoupons = useEarnedCouponsStore((s) => s.coupons)
  const { coupons: apiCoupons, loading, error, reload } = useMyDiscounts()
  const [copiedId, setCopiedId] = useState<string | null>(null)

  const allCoupons = useMemo(
    () => mergeCoupons(apiCoupons, earnedCoupons),
    [apiCoupons, earnedCoupons],
  )

  const handleCopy = async (coupon: DashboardCoupon) => {
    try {
      await navigator.clipboard.writeText(coupon.code)
      setCopiedId(coupon.id)
      setTimeout(() => setCopiedId(null), 2000)
    } catch {
      /* clipboard unavailable */
    }
  }

  if (loading && allCoupons.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.coupons.title')}
          description={t('dashboard.coupons.description')}
          icon={<CouponsIcon size={22} />}
        />
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.coupons.loading')} />
        </div>
      </div>
    )
  }

  if (error && allCoupons.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.coupons.title')}
          description={t('dashboard.coupons.description')}
          icon={<CouponsIcon size={22} />}
        />
        <div className="rounded-sm border border-dashed border-border bg-surface-muted/30 px-4 py-12 text-center">
          <p className="text-sm text-text-muted">{t('dashboard.coupons.loadFailed')}</p>
          <Button variant="secondary" className="mt-4" onClick={() => void reload()}>
            {t('dashboard.coupons.retry')}
          </Button>
        </div>
      </div>
    )
  }

  if (allCoupons.length === 0) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.coupons.title')}
          description={t('dashboard.coupons.description')}
          icon={<CouponsIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<CouponsIcon size={28} />}
          title={t('dashboard.coupons.emptyTitle')}
          message={t('dashboard.coupons.emptyMessage')}
        />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.coupons.title')}
        description={t('dashboard.coupons.description')}
        icon={<CouponsIcon size={22} />}
        action={
          <span className="rounded-sm bg-warm-soft px-2.5 py-1 text-xs font-semibold text-warm">
            {t('dashboard.coupons.itemCount', { count: allCoupons.length })}
          </span>
        }
      />

      {error && (
        <p className="mb-4 text-sm text-sale">
          {t('dashboard.coupons.loadFailed')}{' '}
          <button type="button" className="underline" onClick={() => void reload()}>
            {t('dashboard.coupons.retry')}
          </button>
        </p>
      )}

      <div className="grid gap-4 sm:grid-cols-2">
        {allCoupons.map((coupon) => {
          const isActive = coupon.status === 'active'
          return (
            <article
              key={coupon.id}
              className={`relative overflow-hidden rounded-sm border shadow-sm transition-all duration-200 hover:shadow-md ${
                isActive
                  ? 'border-warm/30 bg-gradient-to-br from-warm-soft/50 to-surface'
                  : 'border-border bg-surface-muted/30 opacity-85'
              }`}
            >
              <div className="absolute inset-y-3 start-0 w-1 rounded-e-full bg-warm" aria-hidden />
              <div
                aria-hidden
                className="pointer-events-none absolute -end-4 -top-4 size-20 rounded-full bg-warm/5"
              />
              <div className="relative p-5 ps-6 sm:p-6">
                <div className="flex items-start justify-between gap-3">
                  <div>
                    <p className="font-mono text-lg font-bold tracking-wider text-warm">
                      {coupon.code}
                    </p>
                    <p className="mt-1 text-sm text-text">{coupon.description}</p>
                  </div>
                  <CouponStatusBadge status={coupon.status} />
                </div>

                <div className="mt-5 grid grid-cols-2 gap-3 rounded-sm border border-border/70 bg-surface/60 p-3">
                  <div>
                    <p className="text-[10px] font-semibold uppercase tracking-wider text-text-muted">
                      {t('dashboard.coupons.discount')}
                    </p>
                    <p className="mt-1 text-sm font-semibold text-text">{coupon.discount}</p>
                  </div>
                  <div className="text-end">
                    <p className="text-[10px] font-semibold uppercase tracking-wider text-text-muted">
                      {t('dashboard.coupons.expires')}
                    </p>
                    <p className="mt-1 text-sm text-text-muted">
                      {coupon.expiresAt
                        ? formatBlogDate(coupon.expiresAt, locale)
                        : t('dashboard.coupons.noExpiry')}
                    </p>
                  </div>
                </div>

                {isActive && (
                  <Button
                    variant="secondary"
                    className="mt-4 w-full text-xs"
                    onClick={() => void handleCopy(coupon)}
                  >
                    {copiedId === coupon.id
                      ? t('dashboard.coupons.copied')
                      : t('dashboard.coupons.copy')}
                  </Button>
                )}
              </div>
            </article>
          )
        })}
      </div>
    </div>
  )
}
