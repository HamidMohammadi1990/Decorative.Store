import { useEffect, useMemo, useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { FilterFacet, FilterOption } from '@/models/catalog/listing.model'
import { getColorSwatch } from '@/extensions/colorSwatches'

interface FilterGroupProps {
  facet: FilterFacet
  activeValues: string[]
  onToggle: (facetId: string, value: string) => void
}

const COLLAPSED_LIMIT = 6
const SEARCH_THRESHOLD = 6

function isActiveValue(activeValues: string[], value: string) {
  const normalized = value.toLowerCase()
  return activeValues.some((active) => active.toLowerCase() === normalized)
}

function sortOptions(options: FilterOption[], activeValues: string[]) {
  return [...options].sort((a, b) => {
    const aActive = isActiveValue(activeValues, a.value)
    const bActive = isActiveValue(activeValues, b.value)
    if (aActive !== bActive) return aActive ? -1 : 1
    return a.label.localeCompare(b.label)
  })
}

function useVisibleFilterOptions(
  options: FilterOption[],
  activeValues: string[],
  searchQuery: string,
  expanded: boolean,
) {
  const filtered = useMemo(() => {
    const query = searchQuery.trim().toLowerCase()
    const base = query
      ? options.filter(
          (option) =>
            option.label.toLowerCase().includes(query) ||
            option.value.toLowerCase().includes(query),
        )
      : options

    return sortOptions(base, activeValues)
  }, [options, activeValues, searchQuery])

  const visible = useMemo(() => {
    if (expanded || searchQuery.trim()) return filtered

    const activeOptions = filtered.filter((option) => isActiveValue(activeValues, option.value))
    const inactiveOptions = filtered.filter((option) => !isActiveValue(activeValues, option.value))
    const remainingSlots = Math.max(0, COLLAPSED_LIMIT - activeOptions.length)

    return [...activeOptions, ...inactiveOptions.slice(0, remainingSlots)]
  }, [filtered, expanded, searchQuery, activeValues])

  const hiddenCount = Math.max(0, filtered.length - visible.length)

  return {
    filtered,
    visible,
    hiddenCount,
    canCollapse: filtered.length > COLLAPSED_LIMIT,
  }
}

function FilterOptionButton({
  checked,
  onClick,
  children,
  className = '',
}: {
  checked: boolean
  onClick: () => void
  children: ReactNode
  className?: string
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      aria-pressed={checked}
      className={`flex w-full items-center justify-between gap-2 rounded-lg px-2.5 py-2 text-start text-sm transition-all duration-200 ${
        checked
          ? 'bg-warm-soft/90 text-text ring-1 ring-warm/25'
          : 'bg-surface-muted/30 text-text hover:bg-surface-muted/70'
      } ${className}`}
    >
      {children}
    </button>
  )
}

function CountBadge({ count }: { count: number }) {
  return (
    <span className="shrink-0 rounded-md bg-surface px-1.5 py-0.5 text-[10px] font-medium tabular-nums text-text-muted ring-1 ring-border/60">
      {count}
    </span>
  )
}

function FilterSearchField({
  value,
  onChange,
  placeholder,
  onClear,
}: {
  value: string
  onChange: (value: string) => void
  placeholder: string
  onClear: () => void
}) {
  const { t } = useTranslation()

  return (
    <div className="relative">
      <SearchIcon className="pointer-events-none absolute start-2.5 top-1/2 size-3.5 -translate-y-1/2 text-text-muted" />
      <input
        type="search"
        value={value}
        onChange={(event) => onChange(event.target.value)}
        placeholder={placeholder}
        className="w-full rounded-lg border border-border/60 bg-surface-muted/25 py-2 pe-8 ps-8 text-xs text-text outline-none transition-colors placeholder:text-text-muted/80 focus:border-warm/40 focus:bg-surface focus:ring-2 focus:ring-warm/15"
      />
      {value && (
        <button
          type="button"
          onClick={onClear}
          className="absolute end-2 top-1/2 flex size-5 -translate-y-1/2 items-center justify-center rounded text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
          aria-label={t('listing.filterSearchClear')}
        >
          <CloseIcon />
        </button>
      )}
    </div>
  )
}

function FilterExpandControls({
  hiddenCount,
  expanded,
  onToggle,
}: {
  hiddenCount: number
  expanded: boolean
  onToggle: () => void
}) {
  const { t } = useTranslation()

  if (hiddenCount > 0) {
    return (
      <button
        type="button"
        onClick={onToggle}
        className="w-full rounded-lg py-1.5 text-xs font-semibold text-warm transition-colors hover:bg-warm-soft/40"
      >
        {t('listing.showMoreOptions', { count: hiddenCount })}
      </button>
    )
  }

  if (expanded) {
    return (
      <button
        type="button"
        onClick={onToggle}
        className="w-full rounded-lg py-1.5 text-xs font-semibold text-text-muted transition-colors hover:bg-surface-muted/50 hover:text-text"
      >
        {t('listing.showLessOptions')}
      </button>
    )
  }

  return null
}

function SearchIcon({ className = '' }: { className?: string }) {
  return (
    <svg className={className} viewBox="0 0 24 24" fill="none" aria-hidden>
      <circle cx="11" cy="11" r="7" stroke="currentColor" strokeWidth="1.75" />
      <path d="M20 20l-3-3" stroke="currentColor" strokeWidth="1.75" strokeLinecap="round" />
    </svg>
  )
}

function CloseIcon() {
  return (
    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path d="M6 6l12 12M18 6L6 18" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
    </svg>
  )
}

export function FilterGroup({ facet, activeValues, onToggle }: FilterGroupProps) {
  const { t } = useTranslation()
  const isColor = facet.type === 'color'
  const showSearch = facet.options.length >= SEARCH_THRESHOLD

  const [searchQuery, setSearchQuery] = useState('')
  const [expanded, setExpanded] = useState(false)

  useEffect(() => {
    setSearchQuery('')
    setExpanded(false)
  }, [facet.id])

  const { filtered, visible, hiddenCount, canCollapse } = useVisibleFilterOptions(
    facet.options,
    activeValues,
    searchQuery,
    expanded,
  )

  const showExpandControls = canCollapse && !searchQuery.trim()

  const controls = (
    <>
      {showSearch && (
        <FilterSearchField
          value={searchQuery}
          onChange={setSearchQuery}
          placeholder={t('listing.filterSearchPlaceholder', { facet: facet.label })}
          onClear={() => setSearchQuery('')}
        />
      )}

      {filtered.length === 0 ? (
        <p className="py-2 text-center text-xs text-text-muted">{t('listing.filterNoResults')}</p>
      ) : isColor ? (
        <ul className="grid grid-cols-2 gap-2">
          {visible.map((option) => {
            const checked = isActiveValue(activeValues, option.value)
            const swatch = option.swatch ?? getColorSwatch(option.value)

            return (
              <li key={option.value}>
                <FilterOptionButton
                  checked={checked}
                  onClick={() => onToggle(facet.id, option.value)}
                  className="!px-2 !py-2"
                >
                  <span className="flex min-w-0 flex-1 items-center gap-2">
                    <span
                      aria-hidden
                      className={`size-5 shrink-0 rounded-full border-2 shadow-inner transition-transform ${
                        checked ? 'border-warm scale-105' : 'border-border-strong'
                      }`}
                      style={{ backgroundColor: swatch }}
                    />
                    <span className="truncate text-xs font-medium">{option.label}</span>
                  </span>
                  <CountBadge count={option.count} />
                </FilterOptionButton>
              </li>
            )
          })}
        </ul>
      ) : (
        <ul className="space-y-1.5">
          {visible.map((option) => {
            const checked = isActiveValue(activeValues, option.value)

            return (
              <li key={option.value}>
                <FilterOptionButton
                  checked={checked}
                  onClick={() => onToggle(facet.id, option.value)}
                >
                  <span className="flex min-w-0 items-center gap-2.5">
                    <span
                      aria-hidden
                      className={`flex size-4 shrink-0 items-center justify-center rounded border transition-colors ${
                        checked
                          ? 'border-warm bg-warm text-[10px] text-warm-text'
                          : 'border-border bg-surface'
                      }`}
                    >
                      {checked ? '✓' : null}
                    </span>
                    <span className="truncate font-medium">{option.label}</span>
                  </span>
                  <CountBadge count={option.count} />
                </FilterOptionButton>
              </li>
            )
          })}
        </ul>
      )}

      {showExpandControls && (
        <FilterExpandControls
          hiddenCount={hiddenCount}
          expanded={expanded}
          onToggle={() => setExpanded((value) => !value)}
        />
      )}
    </>
  )

  return <div className="space-y-3">{controls}</div>
}
