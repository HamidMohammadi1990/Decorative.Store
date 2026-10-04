import type { CouponStatus, DashboardCoupon } from '@/models/dashboard/dashboard.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'
import { readBooleanField, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'

const DISCOUNT_MY_PATH = '/api/v1/discount/my'

function formatDiscountLabel(percentage: number, amount: number, locale: Locale): string {
  if (percentage > 0) {
    return locale === 'fa' ? `${percentage}٪` : `${percentage}%`
  }

  if (amount > 0) {
    const formatted = amount.toLocaleString(locale === 'fa' ? 'fa-IR' : 'en-US')
    return locale === 'fa' ? `${formatted} تومان` : `${formatted}`
  }

  return '—'
}

function buildDescription(
  percentage: number,
  amount: number,
  minimumAmount: number | undefined,
  isPersonal: boolean,
  locale: Locale,
): string {
  const parts: string[] = []

  if (isPersonal) {
    parts.push(locale === 'fa' ? 'کد اختصاصی شما' : 'Personal code')
  } else {
    parts.push(locale === 'fa' ? 'کد عمومی فروشگاه' : 'Store-wide code')
  }

  if (percentage > 0) {
    parts.push(locale === 'fa' ? `${percentage}٪ تخفیف` : `${percentage}% off`)
  } else if (amount > 0) {
    const formatted = amount.toLocaleString(locale === 'fa' ? 'fa-IR' : 'en-US')
    parts.push(locale === 'fa' ? `${formatted} تومان تخفیف` : `${formatted} off`)
  }

  if (minimumAmount && minimumAmount > 0) {
    const formatted = minimumAmount.toLocaleString(locale === 'fa' ? 'fa-IR' : 'en-US')
    parts.push(
      locale === 'fa'
        ? `حداقل خرید ${formatted} تومان`
        : `Min order ${formatted}`,
    )
  }

  return parts.join(' · ')
}

function resolveStatus(
  isActive: boolean,
  remainingUses: number,
  expiresAt: string,
): CouponStatus {
  if (!isActive || remainingUses <= 0) return 'used'

  if (expiresAt) {
    const expiry = new Date(expiresAt)
    if (!Number.isNaN(expiry.getTime()) && expiry.getTime() < Date.now()) {
      return 'expired'
    }
  }

  return 'active'
}

export function normalizeDiscountCoupon(data: unknown, locale: Locale): DashboardCoupon | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readStringField(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  const percentage = readNumberField(record, 'percentage', 'Percentage')
  const amount = readNumberField(record, 'amount', 'Amount')
  const remainingUses = readNumberField(record, 'remainingUses', 'RemainingUses')
  const isActive = readBooleanField(record, 'isActive', 'IsActive')
  const isPersonal = readBooleanField(record, 'isPersonal', 'IsPersonal')
  const expiresAt = readStringField(record, 'expiryDateOnUtc', 'ExpiryDateOnUtc')
  const minimumRaw = record.minimumAmount ?? record.MinimumAmount
  const minimumAmount =
    typeof minimumRaw === 'number' && Number.isFinite(minimumRaw) ? minimumRaw : undefined

  return {
    id,
    code,
    description: buildDescription(percentage, amount, minimumAmount, isPersonal, locale),
    discount: formatDiscountLabel(percentage, amount, locale),
    expiresAt: expiresAt ? expiresAt.slice(0, 10) : '',
    status: resolveStatus(isActive, remainingUses, expiresAt),
  }
}

export const discountService = {
  async getMyAvailable(accessToken: string, locale: Locale): Promise<DashboardCoupon[]> {
    const result = await apiGet<{ items?: unknown[]; Items?: unknown[] }>(
      DISCOUNT_MY_PATH,
      locale,
      accessToken,
    )

    const items = result.items ?? result.Items ?? []
    return items
      .map((item) => normalizeDiscountCoupon(item, locale))
      .filter((coupon): coupon is DashboardCoupon => Boolean(coupon))
  },
}
