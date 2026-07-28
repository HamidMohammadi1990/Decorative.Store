import { useTranslation } from 'react-i18next'

type BadgeVariant = 'success' | 'warning' | 'info' | 'neutral' | 'danger'

interface StatusBadgeProps {
  label: string
  variant?: BadgeVariant
}

const variantClasses: Record<BadgeVariant, { badge: string; dot: string }> = {
  success: { badge: 'bg-accent/10 text-accent', dot: 'bg-accent' },
  warning: { badge: 'bg-warm-soft text-warm', dot: 'bg-warm' },
  info: { badge: 'bg-surface-muted text-text', dot: 'bg-text-muted' },
  neutral: { badge: 'bg-surface-muted text-text-muted', dot: 'bg-border-strong' },
  danger: { badge: 'bg-sale/10 text-sale', dot: 'bg-sale' },
}

export function StatusBadge({ label, variant = 'neutral' }: StatusBadgeProps) {
  const styles = variantClasses[variant]
  return (
    <span
      className={`inline-flex items-center gap-1.5 rounded-full px-2.5 py-1 text-[11px] font-semibold ${styles.badge}`}
    >
      <span className={`size-1.5 shrink-0 rounded-full ${styles.dot}`} aria-hidden />
      {label}
    </span>
  )
}

export function OrderStatusBadge({ status }: { status: string }) {
  const { t } = useTranslation()
  const variantMap: Record<string, BadgeVariant> = {
    pending: 'warning',
    processing: 'info',
    shipped: 'info',
    delivered: 'success',
    cancelled: 'danger',
  }
  return (
    <StatusBadge
      label={t(`dashboard.orderStatus.${status}`, status)}
      variant={variantMap[status] ?? 'neutral'}
    />
  )
}

export function CouponStatusBadge({ status }: { status: string }) {
  const { t } = useTranslation()
  const variantMap: Record<string, BadgeVariant> = {
    active: 'success',
    used: 'neutral',
    expired: 'danger',
  }
  return (
    <StatusBadge
      label={t(`dashboard.couponStatus.${status}`, status)}
      variant={variantMap[status] ?? 'neutral'}
    />
  )
}

export function ReviewStatusBadge({ status }: { status: string }) {
  const { t } = useTranslation()
  const variantMap: Record<string, BadgeVariant> = {
    published: 'success',
    pending: 'warning',
  }
  return (
    <StatusBadge
      label={t(`dashboard.reviewStatus.${status}`, status)}
      variant={variantMap[status] ?? 'neutral'}
    />
  )
}
