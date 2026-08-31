import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Link, useSearchParams } from 'react-router-dom'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import { catalogSearchService } from '@/services/catalogSearchService'
import { mapCatalogSearchProduct } from '@/services/mappers/catalogSearchMapper'
import { useSettingsStore } from '@/stores/settingsStore'

const MIN_QUERY_LENGTH = 2
const RESULT_LIMIT = 24

export function SearchPage() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const [searchParams] = useSearchParams()
  const query = (searchParams.get('q') ?? '').trim()
  const [data, setData] = useState<CatalogSearchResponse | null>(null)
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    if (query.length < MIN_QUERY_LENGTH) {
      setData(null)
      setLoading(false)
      return
    }

    let cancelled = false
    setLoading(true)

    catalogSearchService
      .search(query, locale, RESULT_LIMIT)
      .then((response) => {
        if (!cancelled) setData(response)
      })
      .catch(() => {
        if (!cancelled) setData(null)
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })

    return () => {
      cancelled = true
    }
  }, [query, locale])

  if (query.length < MIN_QUERY_LENGTH) {
    return (
      <Container className="py-16 text-center text-sm text-text-muted">
        {t('siteSearch.minChars')}
      </Container>
    )
  }

  if (loading) {
    return <PageLoading />
  }

  const products = data?.products.map(mapCatalogSearchProduct) ?? []
  const hasCategories = (data?.categories.length ?? 0) > 0
  const hasSubCategories = (data?.subCategories.length ?? 0) > 0
  const hasProducts = products.length > 0
  const hasAny = hasCategories || hasSubCategories || hasProducts

  return (
    <div className="bg-surface py-8 md:py-10">
      <Container>
        <header className="mb-8">
          <h1 className="text-2xl font-semibold text-text md:text-3xl">
            {t('siteSearch.resultsTitle', { query })}
          </h1>
        </header>

        {!hasAny ? (
          <p className="text-sm text-text-muted">{t('siteSearch.noResults')}</p>
        ) : (
          <div className="space-y-10">
            {hasCategories && (
              <section>
                <h2 className="mb-3 text-lg font-semibold text-text">
                  {t('siteSearch.categories')}
                </h2>
                <ul className="divide-y divide-border rounded-sm border border-border">
                  {data!.categories.map((item) => (
                    <li key={item.slug}>
                      <Link
                        to={`/${item.slug}`}
                        className="block px-4 py-3 text-sm hover:bg-surface-muted"
                      >
                        {item.title}
                      </Link>
                    </li>
                  ))}
                </ul>
              </section>
            )}

            {hasSubCategories && (
              <section>
                <h2 className="mb-3 text-lg font-semibold text-text">
                  {t('siteSearch.subCategories')}
                </h2>
                <ul className="divide-y divide-border rounded-sm border border-border">
                  {data!.subCategories.map((item) => (
                    <li key={item.slug}>
                      <Link
                        to={`/${item.slug}`}
                        className="block px-4 py-3 text-sm hover:bg-surface-muted"
                      >
                        <span>{item.title}</span>
                        {item.categoryTitle ? (
                          <span className="mt-0.5 block text-xs text-text-muted">
                            {item.categoryTitle}
                          </span>
                        ) : null}
                      </Link>
                    </li>
                  ))}
                </ul>
              </section>
            )}

            {hasProducts && (
              <section>
                <h2 className="mb-4 text-lg font-semibold text-text">
                  {t('siteSearch.products')}
                </h2>
                <ProductGrid products={products} />
              </section>
            )}
          </div>
        )}
      </Container>
    </div>
  )
}
