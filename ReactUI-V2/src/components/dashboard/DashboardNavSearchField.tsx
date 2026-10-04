import { useTranslation } from 'react-i18next'

interface DashboardNavSearchFieldProps {
  value: string
  onChange: (value: string) => void
  id?: string
  className?: string
}

export function DashboardNavSearchField({
  value,
  onChange,
  id = 'dashboard-nav-search',
  className = '',
}: DashboardNavSearchFieldProps) {
  const { t } = useTranslation()

  return (
    <div className={`relative ${className}`}>
      <SearchIcon className="pointer-events-none absolute start-3 top-1/2 size-4 -translate-y-1/2 text-text-muted" />
      <input
        id={id}
        type="search"
        value={value}
        onChange={(event) => onChange(event.target.value)}
        placeholder={t('dashboard.navSearchPlaceholder')}
        aria-label={t('dashboard.navSearchLabel')}
        autoComplete="off"
        spellCheck={false}
        className="w-full rounded-sm border border-border bg-surface py-2.5 pe-9 ps-9 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm"
      />
      {value ? (
        <button
          type="button"
          onClick={() => onChange('')}
          aria-label={t('dashboard.navSearchClear')}
          className="absolute end-2 top-1/2 flex size-7 -translate-y-1/2 items-center justify-center rounded-sm text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
        >
          <ClearIcon />
        </button>
      ) : null}
    </div>
  )
}

function SearchIcon({ className = '' }: { className?: string }) {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden className={className}>
      <circle cx="7" cy="7" r="4.25" stroke="currentColor" strokeWidth="1.3" />
      <path d="M10.2 10.2 13 13" stroke="currentColor" strokeWidth="1.3" strokeLinecap="round" />
    </svg>
  )
}

function ClearIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 14 14" fill="none" aria-hidden>
      <path
        d="M3.5 3.5l7 7M10.5 3.5l-7 7"
        stroke="currentColor"
        strokeWidth="1.3"
        strokeLinecap="round"
      />
    </svg>
  )
}
