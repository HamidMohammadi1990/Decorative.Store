import { useTranslation } from 'react-i18next'
import type {
  CheckoutProperty,
  CheckoutPropertyGroup,
  PropertyTypeValue,
} from '@/models/checkout/checkout.model'
import { PropertyType } from '@/models/checkout/checkout.model'
import { AuthField } from '@/components/auth/AuthField'

export type CheckoutPropertyValues = Record<string, Record<string, unknown>>

interface CheckoutProductOptionsSectionProps {
  productTitle: string
  propertyGroups: CheckoutPropertyGroup[]
  values: Record<string, unknown>
  errors: Record<string, string>
  onChange: (propertyId: string, value: unknown) => void
}

export function CheckoutProductOptionsSection({
  productTitle,
  propertyGroups,
  values,
  errors,
  onChange,
}: CheckoutProductOptionsSectionProps) {
  const { t } = useTranslation()

  if (propertyGroups.length === 0) {
    return null
  }

  return (
    <div className="space-y-5">
      <div>
        <h3 className="text-sm font-semibold text-text">{productTitle}</h3>
        <p className="mt-1 text-xs text-text-muted">{t('checkout.productOptionsHint')}</p>
      </div>

      {propertyGroups.map((group) => (
        <div key={group.categoryTitle} className="rounded-sm border border-border bg-surface-muted/30 p-4">
          <p className="mb-4 text-xs font-semibold uppercase tracking-wider text-text-muted">
            {group.categoryTitle}
          </p>
          <div className="space-y-5">
            {group.properties.map((property) => (
              <CheckoutPropertyField
                key={property.id}
                property={property}
                value={values[property.id]}
                error={errors[property.id]}
                onChange={(value) => onChange(property.id, value)}
              />
            ))}
          </div>
        </div>
      ))}
    </div>
  )
}

function CheckoutPropertyField({
  property,
  value,
  error,
  onChange,
}: {
  property: CheckoutProperty
  value: unknown
  error?: string
  onChange: (value: unknown) => void
}) {
  const { t } = useTranslation()
  const mandatory = property.rule?.isMandatory ?? false

  return (
    <div>
      <div className="mb-2 flex flex-wrap items-center gap-2">
        <label className="text-sm font-medium text-text">{property.title}</label>
        {mandatory && (
          <span className="text-xs font-medium text-warm">{t('checkout.required')}</span>
        )}
      </div>
      {property.rule?.description && (
        <p className="mb-2 text-xs text-text-muted">{property.rule.description}</p>
      )}

      <PropertyInput property={property} value={value} onChange={onChange} />
      {error && <p className="mt-1.5 text-xs text-sale">{error}</p>}
    </div>
  )
}

function PropertyInput({
  property,
  value,
  onChange,
}: {
  property: CheckoutProperty
  value: unknown
  onChange: (value: unknown) => void
}) {
  switch (property.propertyType as PropertyTypeValue) {
    case PropertyType.Boolean:
      return (
        <label className="inline-flex items-center gap-2 text-sm text-text">
          <input
            type="checkbox"
            checked={Boolean(value)}
            onChange={(e) => onChange(e.target.checked)}
            className="size-4 accent-[#9a7448]"
          />
          <span>{property.title}</span>
        </label>
      )

    case PropertyType.Select:
      return (
        <div className="flex flex-wrap gap-2">
          {property.items.map((item) => (
            <label
              key={item.id}
              className={`cursor-pointer rounded-sm border px-3 py-2 text-sm transition-colors ${
                value === item.id
                  ? 'border-warm bg-warm-soft text-warm'
                  : 'border-border bg-surface text-text hover:border-warm-muted'
              }`}
            >
              <input
                type="radio"
                name={`property-${property.id}`}
                value={item.id}
                checked={value === item.id}
                onChange={() => onChange(item.id)}
                className="sr-only"
              />
              {item.title}
            </label>
          ))}
        </div>
      )

    case PropertyType.Text:
      return (
        <AuthField
          name={`property-${property.id}`}
          label={property.title}
          value={typeof value === 'string' ? value : ''}
          onChange={(e) => onChange(e.target.value)}
          placeholder={property.title}
        />
      )

    case PropertyType.Numeric:
      return (
        <input
          type="number"
          min={property.rule?.minQuantity}
          max={property.rule?.maxQuantity}
          value={typeof value === 'number' ? value : ''}
          onChange={(e) => onChange(Number(e.target.value))}
          className="w-full max-w-[10rem] rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text"
        />
      )

    case PropertyType.NumericWithItem: {
      const current =
        typeof value === 'object' && value !== null
          ? (value as { itemId?: string; quantity?: number })
          : {}
      return (
        <div className="flex flex-wrap items-center gap-3">
          <select
            value={current.itemId ?? ''}
            onChange={(e) =>
              onChange({ ...current, itemId: e.target.value, quantity: current.quantity ?? 1 })
            }
            className="rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text"
          >
            <option value="">{property.title}</option>
            {property.items.map((item) => (
              <option key={item.id} value={item.id}>
                {item.title}
              </option>
            ))}
          </select>
          <input
            type="number"
            min={property.rule?.minQuantity ?? 1}
            max={property.rule?.maxQuantity}
            value={current.quantity ?? 1}
            onChange={(e) =>
              onChange({ ...current, quantity: Number(e.target.value), itemId: current.itemId })
            }
            className="w-20 rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text"
          />
        </div>
      )
    }

    case PropertyType.Dimensions: {
      const current =
        typeof value === 'object' && value !== null
          ? (value as { width?: number; height?: number })
          : {}
      return (
        <div className="grid max-w-md grid-cols-2 gap-3">
          <AuthField
            name={`${property.id}-width`}
            type="number"
            label="Width"
            value={current.width?.toString() ?? ''}
            onChange={(e) =>
              onChange({ ...current, width: Number(e.target.value), height: current.height })
            }
          />
          <AuthField
            name={`${property.id}-height`}
            type="number"
            label="Height"
            value={current.height?.toString() ?? ''}
            onChange={(e) =>
              onChange({ ...current, height: Number(e.target.value), width: current.width })
            }
          />
        </div>
      )
    }

    case PropertyType.HasParents:
      return (
        <div className="space-y-4">
          <div className="flex flex-wrap gap-2">
            {property.items.map((item) => (
              <label
                key={item.id}
                className={`cursor-pointer rounded-sm border px-3 py-2 text-sm transition-colors ${
                  (value as { parentItemId?: string })?.parentItemId === item.id
                    ? 'border-warm bg-warm-soft text-warm'
                    : 'border-border bg-surface text-text hover:border-warm-muted'
                }`}
              >
                <input
                  type="radio"
                  name={`property-parent-${property.id}`}
                  checked={(value as { parentItemId?: string })?.parentItemId === item.id}
                  onChange={() => onChange({ parentItemId: item.id, nested: {} })}
                  className="sr-only"
                />
                {item.title}
              </label>
            ))}
          </div>
          {property.parents.map((parent) => {
            const nestedValues =
              typeof value === 'object' && value !== null
                ? ((value as { nested?: Record<string, unknown> }).nested ?? {})
                : {}
            return (
              <div key={parent.id} className="border-s-2 border-border ps-4">
                <CheckoutPropertyField
                  property={parent}
                  value={nestedValues[parent.id]}
                  onChange={(nestedValue) =>
                    onChange({
                      ...(typeof value === 'object' && value !== null ? value : {}),
                      nested: { ...nestedValues, [parent.id]: nestedValue },
                    })
                  }
                />
              </div>
            )
          })}
        </div>
      )

    default:
      return null
  }
}
