import { useEffect, useId, useMemo, useRef, useState } from 'react'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { InlineLoading } from '@/components/ui/Spinner'

export interface AdminSearchableSelectOption {
  value: string
  label: string
  hint?: string
}

export function AdminSearchableSelect({
  value,
  onChange,
  options,
  loading = false,
  disabled = false,
  placeholder,
  searchPlaceholder,
  emptyMessage,
  searchValue,
  onSearchChange,
  clearLabel,
  loadingLabel,
}: {
  value: string
  onChange: (value: string) => void
  options: AdminSearchableSelectOption[]
  loading?: boolean
  disabled?: boolean
  placeholder: string
  searchPlaceholder: string
  emptyMessage: string
  searchValue: string
  onSearchChange: (value: string) => void
  clearLabel: string
  loadingLabel?: string
}) {
  const listboxId = useId()
  const containerRef = useRef<HTMLDivElement>(null)
  const searchInputRef = useRef<HTMLInputElement>(null)
  const [open, setOpen] = useState(false)

  const selectedOption = useMemo(
    () => options.find((option) => option.value === value) ?? null,
    [options, value],
  )

  useEffect(() => {
    if (!open) return

    const handlePointerDown = (event: MouseEvent) => {
      if (!containerRef.current?.contains(event.target as Node)) {
        setOpen(false)
      }
    }

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setOpen(false)
    }

    document.addEventListener('mousedown', handlePointerDown)
    document.addEventListener('keydown', handleKeyDown)
    return () => {
      document.removeEventListener('mousedown', handlePointerDown)
      document.removeEventListener('keydown', handleKeyDown)
    }
  }, [open])

  useEffect(() => {
    if (open) {
      searchInputRef.current?.focus()
    }
  }, [open])

  const toggleOpen = () => {
    if (disabled) return
    setOpen((current) => !current)
  }

  const handleSelect = (nextValue: string) => {
    onChange(nextValue)
    onSearchChange('')
    setOpen(false)
  }

  return (
    <div ref={containerRef} className="relative">
      <button
        type="button"
        disabled={disabled}
        aria-haspopup="listbox"
        aria-expanded={open}
        aria-controls={listboxId}
        onClick={toggleOpen}
        className="flex w-full items-center justify-between gap-3 rounded-lg border border-border bg-surface px-3 py-2.5 text-start text-sm text-text outline-none transition-colors hover:border-warm/40 focus:border-warm disabled:cursor-not-allowed disabled:opacity-50"
      >
        <span className={selectedOption ? 'truncate' : 'truncate text-text-muted'}>
          {selectedOption
            ? formatOptionLabel(selectedOption)
            : placeholder}
        </span>
        <ChevronIcon expanded={open} className="text-text-muted" />
      </button>

      {open && (
        <div className="absolute z-30 mt-1 w-full overflow-hidden rounded-sm border border-border bg-surface shadow-lg">
          <div className="border-b border-border p-2">
            <input
              ref={searchInputRef}
              value={searchValue}
              onChange={(e) => onSearchChange(e.target.value)}
              placeholder={searchPlaceholder}
              className="w-full rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm"
            />
          </div>

          <div id={listboxId} role="listbox" className="max-h-56 overflow-y-auto p-1">
            {loading ? (
              <div className="flex justify-center px-3 py-6">
                <InlineLoading label={loadingLabel} />
              </div>
            ) : options.length === 0 ? (
              <p className="px-3 py-4 text-center text-xs text-text-muted">{emptyMessage}</p>
            ) : (
              <>
                <OptionRow
                  selected={value === ''}
                  label={clearLabel}
                  onSelect={() => handleSelect('')}
                />
                {options.map((option) => (
                  <OptionRow
                    key={option.value}
                    selected={option.value === value}
                    label={option.label}
                    hint={option.hint}
                    onSelect={() => handleSelect(option.value)}
                  />
                ))}
              </>
            )}
          </div>
        </div>
      )}
    </div>
  )
}

function formatOptionLabel(option: AdminSearchableSelectOption) {
  return option.hint ? `${option.label} · ${option.hint}` : option.label
}

function OptionRow({
  selected,
  label,
  hint,
  onSelect,
}: {
  selected: boolean
  label: string
  hint?: string
  onSelect: () => void
}) {
  return (
    <button
      type="button"
      role="option"
      aria-selected={selected}
      onClick={onSelect}
      className={`flex w-full items-start rounded-sm px-3 py-2 text-start text-sm transition-colors hover:bg-surface-muted ${
        selected ? 'bg-warm-soft/70 text-warm' : 'text-text'
      }`}
    >
      <span className="min-w-0">
        <span className="block truncate font-medium">{label}</span>
        {hint ? <span className="mt-0.5 block truncate text-xs text-text-muted">{hint}</span> : null}
      </span>
    </button>
  )
}
