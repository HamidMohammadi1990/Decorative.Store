import { useTranslation } from 'react-i18next'
import type { CheckoutDeliveryType } from '@/models/checkout/checkout.model'
import type { ReactNode } from 'react'

interface CheckoutDeliveryTypeOptionsProps {
  deliveryTypes: CheckoutDeliveryType[]
  selectedId: string | null
  onSelect: (id: string) => void
  renderPrice?: (deliveryTypeId: string) => ReactNode
}

export function CheckoutDeliveryTypeOptions({
  deliveryTypes,
  selectedId,
  onSelect,
  renderPrice,
}: CheckoutDeliveryTypeOptionsProps) {
  const { t } = useTranslation()

  if (deliveryTypes.length === 0) {
    return (
      <p className="text-sm text-text-muted">{t('checkout.deliveryTypesUnavailable')}</p>
    )
  }

  return (
    <div className="grid gap-3 sm:grid-cols-2">
      {deliveryTypes.map((deliveryType) => (
        <label
          key={deliveryType.id}
          htmlFor={`delivery-${deliveryType.id}`}
          className={`flex cursor-pointer flex-col rounded-sm border p-4 transition-colors ${
            selectedId === deliveryType.id
              ? 'border-warm bg-warm-soft ring-1 ring-warm/30'
              : 'border-border bg-surface hover:border-warm-muted'
          }`}
        >
          <div className="flex items-start gap-3">
            <input
              id={`delivery-${deliveryType.id}`}
              type="radio"
              name="delivery-type"
              checked={selectedId === deliveryType.id}
              onChange={() => onSelect(deliveryType.id)}
              className="mt-0.5 size-4 accent-[#9a7448]"
            />
            <div>
              <p className="text-sm font-semibold text-text">{deliveryType.title}</p>
              {renderPrice && (
                <p className="mt-1 text-sm text-warm">{renderPrice(deliveryType.id)}</p>
              )}
            </div>
          </div>
        </label>
      ))}
    </div>
  )
}
