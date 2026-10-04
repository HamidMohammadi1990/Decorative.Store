import { useEffect, useRef, useState, type FormEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { navigateSsr } from '@/extensions/catalogNavLink'
import { catalogSearchService } from '@/services/catalogSearchService'
import { useSettingsStore } from '@/stores/settingsStore'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { InlineLoading } from '@/components/ui/Spinner'

const MIN_QUERY_LENGTH = 2
const SUGGESTION_LIMIT = 6

interface ZaraHeaderSearchProps {
  placeholder: string
  tone: 'light' | 'dark'
  className?: string
}

export function ZaraHeaderSearch({ placeholder, tone, className }: ZaraHeaderSearchProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const containerRef = useRef<HTMLDivElement>(null)
  const [query, setQuery] = useState('')
  const [focused, setFocused] = useState(false)
  const [loading, setLoading] = useState(false)
  const [results, setResults] = useState<CatalogSearchResponse | null>(null)

  const light = tone === 'light'
  const label = placeholder.trim() || t('common.search')
  const showQuery = focused || query.length > 0
  const trimmed = query.trim()
  const showPanel = focused && trimmed.length >= MIN_QUERY_LENGTH

  useEffect(() => {
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
          if (!cancelled) setResults(data)
        })
        .catch(() => {
          if (!cancelled) setResults(null)
        })
        .finally(() => {
          if (!cancelled) setLoading(false)
        })
    }, 280)

    return () => {
      cancelled = true
      window.clearTimeout(timer)
    }
  }, [trimmed, locale])

  useEffect(() => {
    const onPointerDown = (event: MouseEvent) => {
      if (!containerRef.current?.contains(event.target as Node)) {
        setFocused(false)
      }
    }
    document.addEventListener('mousedown', onPointerDown)
    return () => document.removeEventListener('mousedown', onPointerDown)
  }, [])

  const submit = (event?: FormEvent) => {
    event?.preventDefault()
    if (trimmed.length < MIN_QUERY_LENGTH) return
    setFocused(false)
    navigateSsr(`/search?q=${encodeURIComponent(trimmed)}`)
  }

  const lineClass = light ? 'border-white/70' : 'border-text/80'
  const textClass = light ? 'text-white placeholder:text-white/50' : 'text-text placeholder:text-text-muted'
  const labelClass = light ? 'text-white' : 'text-text'

  const hasResults =
    results &&
    (results.products.length > 0 ||
      results.categories.length > 0 ||
      results.subCategories.length > 0)

  return (
    <div ref={containerRef} className={`relative w-full max-w-[22rem] ${className ?? ''}`}>
      <form role="search" onSubmit={submit} className="flex flex-col items-center">
        {!showQuery && (
          <span
            className={`pointer-events-none mb-1 text-[10px] uppercase tracking-[0.18em] ${labelClass}`}
            aria-hidden
          >
            {label}
          </span>
        )}
        <input
          type="search"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          onFocus={() => setFocused(true)}
          placeholder={showQuery ? label : ''}
          className={`zara-search-input w-full border-0 border-b bg-transparent pb-1.5 text-center text-[11px] uppercase tracking-[0.14em] outline-none ${lineClass} ${textClass}`}
          aria-label={t('common.search')}
        />
      </form>

      {showPanel && (
        <div
          className={`absolute start-1/2 top-full z-50 mt-2 w-[min(100vw-2rem,20rem)] -translate-x-1/2 overflow-hidden border shadow-lg ${
            light ? 'border-white/20 bg-neutral-900 text-white' : 'border-border bg-surface text-text'
          }`}
        >
          {loading ? (
            <div className="flex justify-center py-6">
              <InlineLoading />
            </div>
          ) : hasResults ? (
            <ul className="max-h-64 overflow-y-auto py-2 text-xs">
              {results!.products.map((p) => (
                <li key={p.slug}>
                  <CatalogNavLink
                    href={`/product/${p.slug}`}
                    className="block px-4 py-2 uppercase tracking-wide hover:opacity-70"
                    onClick={() => setFocused(false)}
                  >
                    {p.title}
                  </CatalogNavLink>
                </li>
              ))}
            </ul>
          ) : (
            <p className="px-4 py-3 text-xs opacity-70">{t('siteSearch.noResults')}</p>
          )}
        </div>
      )}
    </div>
  )
}
