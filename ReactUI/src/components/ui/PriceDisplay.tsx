import type { Money } from '@/models/shared/money.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import { formatMoney } from '@/extensions/formatMoney'
import { TomanIcon } from '@/components/ui/TomanIcon'

interface PriceDisplayProps {
  money: Money
  currency: CurrencyConfig
  className?: string
  iconClassName?: string
  iconSize?: number
  strike?: boolean
}

export function PriceDisplay({
  money,
  currency,
  className = '',
  iconClassName = '',
  iconSize = 14,
  strike = false,
}: PriceDisplayProps) {
  return (
    <span
      className={`inline-flex items-baseline gap-1 tabular-nums ${strike ? 'line-through' : ''} ${className}`.trim()}
    >
      <span>{formatMoney(money, currency)}</span>
      <TomanIcon
        size={iconSize}
        className={`shrink-0 text-current ${iconClassName}`.trim()}
      />
    </span>
  )
}
