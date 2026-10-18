import { useEffect, useRef, useState, type FormEvent, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { navigateSsr } from '@/extensions/catalogNavLink'
import { getDirection } from '@/extensions/getDirection'
import { SF_SEARCH_HIT_CLASS } from '@/extensions/localeText'
import { catalogSearchService } from '@/services/catalogSearchService'
import { useSettingsStore } from '@/stores/settingsStore'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { InlineLoading } from '@/components/ui/Spinner'

const MIN_QUERY_LENGTH = 2
const SUGGESTION_LIMIT = 6
/** Wider than the search field; centered under the input (physical centering for RTL). */
const RESULTS_PANEL_CLASS = 'w-[min(calc(100vw-2rem),32rem)]'

interface ZaraHeaderSearchProps {
  placeholder: string
  tone: 'light' | 'dark'
  className?: string
}

export function ZaraHeaderSearch({ placeholder, tone, className }: ZaraHeaderSearchProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const direction = getDirection(locale)
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
      <form
        role="search"
        onSubmit={submit}
        dir={direction}
        className="flex w-full flex-col items-center gap-0.5"
      >
        <span
          className={`zara-search-label pointer-events-none text-[10px] leading-none uppercase tracking-[0.18em] transition-opacity ${
            showQuery ? 'h-0 overflow-hidden opacity-0' : labelClass
          }`}
          aria-hidden={showQuery}
        >
          {label}
        </span>
        <input
          type="search"
          value={query}
          dir={direction}
          onChange={(e) => setQuery(e.target.value)}
          onFocus={() => setFocused(true)}
          placeholder={showQuery ? label : ''}
          className={`zara-search-input w-full border-0 border-b bg-transparent py-0 pb-1 text-center text-[11px] uppercase leading-tight tracking-[0.14em] outline-none rtl:text-start ${lineClass} ${textClass}`}
          aria-label={t('common.search')}
        />
      </form>

      {showPanel && (
        <div
          dir={direction}
          className={`absolute top-full left-1/2 z-50 mt-1.5 ${RESULTS_PANEL_CLASS} -translate-x-1/2 overflow-hidden border shadow-lg ${
            light ? 'border-white/20 bg-neutral-900 text-white' : 'border-border bg-surface text-text'
          }`}
        >
          {loading ? (
            <div className="flex justify-center py-6">
              <InlineLoading />
            </div>
          ) : hasResults ? (
            <div className="max-h-[min(70vh,20rem)] overflow-y-auto py-1 text-xs">
              {results!.categories.length > 0 && (
                <ZaraSearchSection title={t('siteSearch.categories')} light={light}>
                  {results!.categories.map((item) => (
                    <ZaraSearchLink
                      key={`cat-${item.slug}`}
                      href={`/${item.slug}`}
                      label={item.title}
                      light={light}
                      onNavigate={() => setFocused(false)}
                    />
                  ))}
                </ZaraSearchSection>
              )}

              {results!.subCategories.length > 0 && (
                <ZaraSearchSection title={t('siteSearch.subCategories')} light={light}>
                  {results!.subCategories.map((item) => (
                    <ZaraSearchLink
                      key={`sub-${item.slug}`}
                      href={`/${item.slug}`}
                      label={item.title}
                      hint={item.categoryTitle}
                      light={light}
                      onNavigate={() => setFocused(false)}
                    />
                  ))}
                </ZaraSearchSection>
              )}

              {results!.products.length > 0 && (
                <ZaraSearchSection title={t('siteSearch.products')} light={light}>
                  {results!.products.map((item) => (
                    <ZaraSearchLink
                      key={`product-${item.slug}`}
                      href={`/product/${item.slug}`}
                      label={item.title}
                      light={light}
                      onNavigate={() => setFocused(false)}
                    />
                  ))}
                </ZaraSearchSection>
              )}

              <div
                className={`border-t px-4 py-2.5 ${light ? 'border-white/15' : 'border-border'}`}
              >
                <button
                  type="button"
                  onClick={() => submit()}
                  className={`text-[11px] font-medium tracking-wide hover:opacity-70 ${
                    light ? 'text-white' : 'text-text'
                  }`}
                >
                  {t('siteSearch.viewAllResults')}
                </button>
              </div>
            </div>
          ) : (
            <p className="px-4 py-3 text-xs opacity-70">{t('siteSearch.noResults')}</p>
          )}
        </div>
      )}
    </div>
  )
}

function ZaraSearchSection({
  title,
  light,
  children,
}: {
  title: string
  light: boolean
  children: ReactNode
}) {
  return (
    <div className={`border-b last:border-b-0 ${light ? 'border-white/10' : 'border-border'}`}>
      <p
        className={`px-4 py-2 text-[10px] font-semibold uppercase tracking-[0.14em] ${
          light ? 'text-white/55' : 'text-text-muted'
        }`}
      >
        {title}
      </p>
      <ul>{children}</ul>
    </div>
  )
}

function ZaraSearchLink({
  href,
  label,
  hint,
  light,
  onNavigate,
}: {
  href: string
  label: string
  hint?: string
  light: boolean
  onNavigate: () => void
}) {
  const muted = light ? 'text-white/50' : 'text-text-muted'

  return (
    <li>
      <CatalogNavLink
        href={href}
        onClick={onNavigate}
        className={`flex flex-col gap-0.5 px-4 py-2 transition-opacity hover:opacity-80 ${
          light ? 'hover:bg-white/5' : 'hover:bg-surface-muted'
        }`}
      >
        <span className={`${SF_SEARCH_HIT_CLASS} text-[11px] leading-snug`}>{label}</span>
        {hint ? <span className={`text-[10px] ${muted}`}>{hint}</span> : null}
      </CatalogNavLink>
    </li>
  )
}
