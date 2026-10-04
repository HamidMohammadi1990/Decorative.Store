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
    <div className="min-w-0 space-y-4">
      <div className="space-y-3 rounded-lg bg-surface-muted/25 p-3 ring-1 ring-border/40">
        <div className="grid grid-cols-1 gap-2 text-sm font-medium text-text">
          <div className="flex min-w-0 items-center justify-between gap-2 rounded-lg border border-border/50 bg-surface px-2.5 py-1.5">
            <span className="shrink-0 text-[11px] font-normal text-text-muted">
              {t('listing.priceMin')}
            </span>
            <span className="min-w-0 truncate text-end">
              {currency ? (
                <PriceDisplay
                  money={{ amount: localMin, currencyCode: currency.code }}
                  currency={currency}
                  iconSize={11}
                  className="text-xs"
                />
              ) : (
                <span className="text-xs tabular-nums">{localMin.toLocaleString()}</span>
              )}
            </span>
          </div>
          <div className="flex min-w-0 items-center justify-between gap-2 rounded-lg border border-border/50 bg-surface px-2.5 py-1.5">
            <span className="shrink-0 text-[11px] font-normal text-text-muted">
              {t('listing.priceMax')}
            </span>
            <span className="min-w-0 truncate text-end">
              {currency ? (
                <PriceDisplay
                  money={{ amount: localMax, currencyCode: currency.code }}
                  currency={currency}
                  iconSize={11}
                  className="text-xs"
                />
              ) : (
                <span className="text-xs tabular-nums">{localMax.toLocaleString()}</span>
              )}
            </span>
          </div>
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
        <ul className="flex flex-wrap gap-2">
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
                      : 'border-border/70 bg-surface-muted/30 text-text-muted hover:border-warm/30 hover:text-text'
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

export function getPriceFacetActiveCount(
  facet: FilterFacet,
  activeBucketValues: string[],
): number {
  if (!facet.range) return activeBucketValues.length

  const hasCustomRange =
    facet.range.selectedMin != null &&
    facet.range.selectedMax != null &&
    (facet.range.selectedMin > facet.range.min || facet.range.selectedMax < facet.range.max)

  return activeBucketValues.length + (hasCustomRange ? 1 : 0)
}
