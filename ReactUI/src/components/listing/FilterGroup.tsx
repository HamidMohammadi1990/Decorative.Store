import type { FilterFacet } from '@/models/catalog/listing.model'
import { getColorSwatch } from '@/extensions/colorSwatches'

interface FilterGroupProps {
  facet: FilterFacet
  activeValues: string[]
  onToggle: (facetId: string, value: string) => void
}

export function FilterGroup({ facet, activeValues, onToggle }: FilterGroupProps) {
  const isColor = facet.type === 'color'

  return (
    <div className="border-b border-border py-5 last:border-b-0">
      <h3 className="mb-3 text-sm font-semibold text-text">{facet.label}</h3>

      {isColor ? (
        <ul className="grid grid-cols-2 gap-2">
          {facet.options.map((option) => {
            const checked = activeValues.includes(option.value)
            const id = `${facet.id}-${option.value}`
            const swatch = option.swatch ?? getColorSwatch(option.value)

            return (
              <li key={option.value}>
                <label
                  htmlFor={id}
                  className={`flex cursor-pointer items-center gap-2 rounded-sm border px-2.5 py-2 text-sm transition-colors ${
                    checked
                      ? 'border-warm bg-warm-soft text-text'
                      : 'border-border text-text-muted hover:border-warm-muted hover:text-text'
                  }`}
                >
                  <input
                    id={id}
                    type="checkbox"
                    checked={checked}
                    onChange={() => onToggle(facet.id, option.value)}
                    className="sr-only"
                  />
                  <span
                    aria-hidden
                    className="size-5 shrink-0 rounded-full border border-border-strong shadow-inner"
                    style={{ backgroundColor: swatch }}
                  />
                  <span className="min-w-0 flex-1 truncate text-xs">{option.label}</span>
                  <span className="text-[10px] tabular-nums text-text-muted/80">
                    {option.count}
                  </span>
                </label>
              </li>
            )
          })}
        </ul>
      ) : (
        <ul className="space-y-2.5">
          {facet.options.map((option) => {
            const checked = activeValues.includes(option.value)
            const id = `${facet.id}-${option.value}`

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
                      onChange={() => onToggle(facet.id, option.value)}
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
