import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { DualRangeSlider } from '@/components/ui/DualRangeSlider'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import type { FilterFacet } from '@/models/catalog/listing.model'

interface PriceRangeGroupProps {
  facet: FilterFacet
  activeBucketValues: string[]
  onToggleBucket: (facetId: string, value: string) => void
  onApplyRange: (min?: number, max?: number) => void
}

export function PriceRangeGroup({
  facet,
  activeBucketValues,
  onToggleBucket,
  onApplyRange,
}: PriceRangeGroupProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const bounds = facet.range

  const initialMin = bounds?.selectedMin ?? bounds?.min ?? 0
  const initialMax = bounds?.selectedMax ?? bounds?.max ?? 0

  const [localMin, setLocalMin] = useState(initialMin)
  const [localMax, setLocalMax] = useState(initialMax)

  useEffect(() => {
    if (!bounds) return
    setLocalMin(bounds.selectedMin ?? bounds.min)
    setLocalMax(bounds.selectedMax ?? bounds.max)
  }, [bounds?.min, bounds?.max, bounds?.selectedMin, bounds?.selectedMax])

  const step = useMemo(() => {
    if (!bounds) return 1
    const span = bounds.max - bounds.min
    if (span <= 100_000) return 10_000
    if (span <= 1_000_000) return 50_000
    if (span <= 10_000_000) return 100_000
    return 500_000
  }, [bounds])

  const commitRange = useCallback(
    (min: number, max: number) => {
      if (!bounds) return

      const clampedMin = Math.max(bounds.min, Math.min(min, max))
      const clampedMax = Math.min(bounds.max, Math.max(min, max))

      if (clampedMin <= bounds.min && clampedMax >= bounds.max) {
        onApplyRange(undefined, undefined)
        return
      }

      onApplyRange(clampedMin, clampedMax)
    },
    [bounds, onApplyRange],
  )

  if (!bounds || bounds.min >= bounds.max) return null

  const hasCustomRange =
    bounds.selectedMin != null &&
    bounds.selectedMax != null &&
    (bounds.selectedMin > bounds.min || bounds.selectedMax < bounds.max)

  return (
    <div className="border-b border-border py-5 last:border-b-0">
      <h3 className="mb-4 text-sm font-semibold text-text">{facet.label}</h3>

      <div className="space-y-3">
        <div className="flex items-center justify-between gap-3 text-sm font-medium text-text">
          <span className="rounded-lg bg-surface-muted px-2.5 py-1.5">
            {currency ? (
              <PriceDisplay
                money={{ amount: localMin, currencyCode: currency.code }}
                currency={currency}
                iconSize={12}
              />
            ) : (
              localMin.toLocaleString()
            )}
          </span>
          <span className="text-xs text-text-muted">{t('listing.priceTo')}</span>
          <span className="rounded-lg bg-surface-muted px-2.5 py-1.5">
            {currency ? (
              <PriceDisplay
                money={{ amount: localMax, currencyCode: currency.code }}
                currency={currency}
                iconSize={12}
              />
            ) : (
              localMax.toLocaleString()
            )}
          </span>
        </div>

        <DualRangeSlider
          min={bounds.min}
          max={bounds.max}
          step={step}
          valueMin={localMin}
          valueMax={localMax}
          onChange={(min, max) => {
            setLocalMin(min)
            setLocalMax(max)
          }}
          onCommit={commitRange}
          ariaLabelMin={t('listing.priceMin')}
          ariaLabelMax={t('listing.priceMax')}
        />

        {hasCustomRange && (
          <button
            type="button"
            onClick={() => {
              setLocalMin(bounds.min)
              setLocalMax(bounds.max)
              onApplyRange(undefined, undefined)
            }}
            className="text-xs font-medium text-warm hover:underline"
          >
            {t('listing.clearPriceRange')}
          </button>
        )}
      </div>

      {facet.options.length > 0 && (
        <ul className="mt-4 space-y-2.5 border-t border-border pt-4">
          {facet.options.map((option) => {
            const checked = activeBucketValues.includes(option.value)
            const id = `price-${option.value}`

            return (
              <li key={option.value}>
                <label
                  htmlFor={id}
                  className="flex cursor-pointer items-center justify-between gap-3 text-sm text-text-muted transition-colors hover:text-text"
                >
                  <span className="flex items-center gap-2.5">
                    <input
                      id={id}
                      type="checkbox"
                      checked={checked}
                      onChange={() => onToggleBucket(facet.id, option.value)}
                      className="size-4 rounded-sm border-border text-warm accent-[#9a7448]"
                    />
                    <span>{option.label}</span>
                  </span>
                  <span className="text-xs tabular-nums text-text-muted/80">
                    {option.count}
                  </span>
                </label>
              </li>
            )
          })}
        </ul>
      )}
    </div>
  )
}
