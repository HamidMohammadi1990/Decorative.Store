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
    <div className="border-b border-border/60 px-4 py-4 last:border-b-0">
      <h3 className="mb-4 text-xs font-semibold uppercase tracking-[0.14em] text-text-muted">
        {facet.label}
      </h3>

      <div className="space-y-4 rounded-lg bg-surface-muted/40 p-3 ring-1 ring-border/50">
        <div className="flex items-center justify-between gap-2 text-sm font-medium text-text">
          <span className="rounded-lg border border-border/60 bg-surface px-2.5 py-1.5 shadow-sm">
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
          <span className="text-[11px] text-text-muted">{t('listing.priceTo')}</span>
          <span className="rounded-lg border border-border/60 bg-surface px-2.5 py-1.5 shadow-sm">
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
            className="w-full text-center text-xs font-medium text-warm hover:underline"
          >
            {t('listing.clearPriceRange')}
          </button>
        )}
      </div>

      {facet.options.length > 0 && (
        <ul className="mt-3 flex flex-wrap gap-2">
          {facet.options.map((option) => {
            const checked = activeBucketValues.includes(option.value)

            return (
              <li key={option.value}>
                <button
                  type="button"
                  onClick={() => onToggleBucket(facet.id, option.value)}
                  aria-pressed={checked}
                  className={`rounded-full border px-3 py-1.5 text-xs font-medium transition-all ${
                    checked
                      ? 'border-warm bg-warm-soft text-text ring-1 ring-warm/20'
                      : 'border-border/70 bg-surface text-text-muted hover:border-warm/30 hover:text-text'
                  }`}
                >
                  {option.label}
                  <span className="ms-1.5 tabular-nums opacity-70">({option.count})</span>
                </button>
              </li>
            )
          })}
        </ul>
      )}
    </div>
  )
}
