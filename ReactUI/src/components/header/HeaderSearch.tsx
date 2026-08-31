import { useEffect, useRef, useState, type FormEvent, type ReactNode } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import { catalogSearchService } from '@/services/catalogSearchService'
import { useSettingsStore } from '@/stores/settingsStore'
import { InlineLoading } from '@/components/ui/Spinner'

const MIN_QUERY_LENGTH = 2
const SUGGESTION_LIMIT = 6

interface HeaderSearchProps {
  placeholder: string
  className?: string
}

export function HeaderSearch({ placeholder, className }: HeaderSearchProps) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const locale = useSettingsStore((s) => s.locale)
  const containerRef = useRef<HTMLDivElement>(null)
  const [query, setQuery] = useState('')
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)
  const [results, setResults] = useState<CatalogSearchResponse | null>(null)

  useEffect(() => {
    const trimmed = query.trim()
    if (trimmed.length < MIN_QUERY_LENGTH) {
      setResults(null)
      setLoading(false)
      return
    }

    let cancelled = false
    setLoading(true)

    const timer = window.setTimeout(() => {
      catalogSearchService
        .search(trimmed, locale, SUGGESTION_LIMIT)
        .then((data) => {
          if (!cancelled) {
            setResults(data)
            setOpen(true)
          }
        })
        .catch(() => {
          if (!cancelled) setResults(null)
        })
        .finally(() => {
          if (!cancelled) setLoading(false)
        })
    }, 300)

    return () => {
      cancelled = true
      window.clearTimeout(timer)
    }
  }, [query, locale])

  useEffect(() => {
    const onPointerDown = (event: MouseEvent) => {
      if (!containerRef.current?.contains(event.target as Node)) {
        setOpen(false)
      }
    }
    document.addEventListener('mousedown', onPointerDown)
    return () => document.removeEventListener('mousedown', onPointerDown)
  }, [])

  const submit = (event?: FormEvent) => {
    event?.preventDefault()
    const trimmed = query.trim()
    if (trimmed.length < MIN_QUERY_LENGTH) return
    setOpen(false)
    navigate(`/search?q=${encodeURIComponent(trimmed)}`)
  }

  const trimmed = query.trim()
  const hasResults =
    results &&
    (results.categories.length > 0 ||
      results.subCategories.length > 0 ||
      results.products.length > 0)

  const showPanel = open && trimmed.length >= MIN_QUERY_LENGTH

  return (
    <div ref={containerRef} className={`relative min-w-0 flex-1 ${className ?? ''}`}>
      <form className="mx-auto w-full max-w-lg" role="search" onSubmit={submit}>
        <input
          type="search"
          value={query}
          onChange={(event) => setQuery(event.target.value)}
          onFocus={() => {
            if (trimmed.length >= MIN_QUERY_LENGTH) setOpen(true)
          }}
          placeholder={placeholder}
          className="w-full rounded-sm border border-border bg-surface-muted px-3 py-1.5 text-sm outline-none focus:border-accent"
          aria-label={t('common.search')}
          aria-expanded={showPanel}
          aria-controls="header-search-results"
        />
      </form>

      {showPanel && (
        <div
          id="header-search-results"
          className="absolute start-0 end-0 top-full z-50 mt-1 max-h-[min(70vh,24rem)] overflow-y-auto rounded-sm border border-border bg-surface shadow-lg"
        >
          {loading ? (
            <div className="flex justify-center py-6">
              <InlineLoading />
            </div>
          ) : hasResults ? (
            <div className="py-2 text-sm">
              {results.categories.length > 0 && (
                <SearchSection title={t('siteSearch.categories')}>
                  {results.categories.map((item) => (
                    <SearchLink
                      key={`cat-${item.slug}`}
                      href={`/${item.slug}`}
                      label={item.title}
                      onNavigate={() => setOpen(false)}
                    />
                  ))}
                </SearchSection>
              )}

              {results.subCategories.length > 0 && (
                <SearchSection title={t('siteSearch.subCategories')}>
                  {results.subCategories.map((item) => (
                    <SearchLink
                      key={`sub-${item.slug}`}
                      href={`/${item.slug}`}
                      label={item.title}
                      hint={item.categoryTitle}
                      onNavigate={() => setOpen(false)}
                    />
                  ))}
                </SearchSection>
              )}

              {results.products.length > 0 && (
                <SearchSection title={t('siteSearch.products')}>
                  {results.products.map((item) => (
                    <SearchLink
                      key={`product-${item.slug}`}
                      href={`/product/${item.slug}`}
                      label={item.title}
                      onNavigate={() => setOpen(false)}
                    />
                  ))}
                </SearchSection>
              )}

              <div className="border-t border-border px-3 py-2">
                <button
                  type="button"
                  onClick={() => submit()}
                  className="text-sm font-semibold text-warm hover:underline"
                >
                  {t('siteSearch.viewAllResults')}
                </button>
              </div>
            </div>
          ) : (
            <p className="px-3 py-4 text-sm text-text-muted">{t('siteSearch.noResults')}</p>
          )}
        </div>
      )}
    </div>
  )
}

function SearchSection({
  title,
  children,
}: {
  title: string
  children: ReactNode
}) {
  return (
    <div className="border-b border-border last:border-b-0">
      <p className="px-3 py-1.5 text-xs font-semibold uppercase tracking-wide text-text-muted">
        {title}
      </p>
      <ul>{children}</ul>
    </div>
  )
}

function SearchLink({
  href,
  label,
  hint,
  onNavigate,
}: {
  href: string
  label: string
  hint?: string
  onNavigate: () => void
}) {
  return (
    <li>
      <Link
        to={href}
        onClick={onNavigate}
        className="flex flex-col gap-0.5 px-3 py-2 hover:bg-surface-muted"
      >
        <span className="text-text">{label}</span>
        {hint ? <span className="text-xs text-text-muted">{hint}</span> : null}
      </Link>
    </li>
  )
}
