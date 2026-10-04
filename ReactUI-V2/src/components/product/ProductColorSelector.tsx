import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { getColorSwatch } from '@/extensions/colorSwatches'

interface ProductColorSelectorProps {
  colors: string[]
}

function isLightSwatch(hex: string) {
  const value = hex.replace('#', '')
  if (value.length !== 6) return true

  const r = Number.parseInt(value.slice(0, 2), 16)
  const g = Number.parseInt(value.slice(2, 4), 16)
  const b = Number.parseInt(value.slice(4, 6), 16)
  return (r * 299 + g * 587 + b * 114) / 1000 > 160
}

export function ProductColorSelector({ colors }: ProductColorSelectorProps) {
  const { t } = useTranslation()
  const [selectedColor, setSelectedColor] = useState(colors[0] ?? '')

  if (colors.length === 0) return null

  const selectedLabel = t(`product.values.${selectedColor}`, { defaultValue: selectedColor })
  const selectedSwatch = getColorSwatch(selectedColor)

  return (
    <div className="mt-6">
      <div className="mb-3 flex items-center gap-2">
        <p className="text-sm font-semibold text-text">
          {t('product.color')}: {selectedLabel}
        </p>
        <span
          className="size-4 shrink-0 rounded-full border border-border-strong/40"
          style={{ backgroundColor: selectedSwatch }}
          aria-hidden
        />
      </div>

      <div className="flex flex-wrap gap-3">
        {colors.map((value) => {
          const swatch = getColorSwatch(value)
          const selected = value === selectedColor
          const label = t(`product.values.${value}`, { defaultValue: value })
          const checkColor = isLightSwatch(swatch) ? '#111827' : '#ffffff'

          return (
            <button
              key={value}
              type="button"
              onClick={() => setSelectedColor(value)}
              aria-label={label}
              aria-pressed={selected}
              className={`inline-flex items-center gap-3 rounded-xl border bg-surface px-4 py-2.5 text-sm font-medium text-text transition-colors ${
                selected
                  ? 'border-text'
                  : 'border-border hover:border-border-strong'
              }`}
            >
              <span
                className="flex size-5 shrink-0 items-center justify-center rounded-full border border-border-strong/30"
                style={{ backgroundColor: swatch }}
                aria-hidden
              >
                {selected && (
                  <svg width="10" height="10" viewBox="0 0 10 10" aria-hidden>
                    <path
                      d="M2 5l2 2 4-4.5"
                      stroke={checkColor}
                      strokeWidth="1.4"
                      fill="none"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                )}
              </span>
              <span>{label}</span>
            </button>
          )
        })}
      </div>
    </div>
  )
}
