import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Link, useSearchParams } from 'react-router-dom'
import { ListingSidebar } from '@/components/listing/ListingSidebar'
import { ListingToolbar } from '@/components/listing/ListingToolbar'
import { ListingPagination } from '@/components/listing/ListingPagination'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import {
  parseListingFilters,
  productMatchesFilters,
  toActiveFiltersRecord,
} from '@/extensions/listingFilters'
import { useListingFilters } from '@/hooks/useListingFilters'
import { useSearchResults } from '@/hooks/useSearchResults'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'
import {
  buildListingFacets,
  getListingSortOptions,
} from '@/services/catalogService'
import { mapCatalogSearchProduct } from '@/services/mappers/catalogSearchMapper'
import { useSettingsStore } from '@/stores/settingsStore'
import type { SortOptionId } from '@/models/catalog/listing.model'

const MIN_QUERY_LENGTH = 2
const PAGE_SIZE = 12

function sortSearchProducts<T extends { price: { amount: number }; isNew: boolean; averageRating?: number; reviewCount: number; purchaseCount: number }>(
  products: T[],
  sort: SortOptionId,
) {
  const copy = [...products]
  switch (sort) {
    case 'price-asc':
      return copy.sort((a, b) => a.price.amount - b.price.amount)
    case 'price-desc':
      return copy.sort((a, b) => b.price.amount - a.price.amount)
    case 'newest':
      return copy.sort((a, b) => Number(b.isNew) - Number(a.isNew))
    case 'best-selling':
      return copy.sort((a, b) => b.purchaseCount - a.purchaseCount)
    case 'rating':
      return copy.sort(
        (a, b) =>
          (b.averageRating ?? 0) - (a.averageRating ?? 0) ||
          b.reviewCount - a.reviewCount,
      )
    default:
      return copy
  }
}

export function SearchPage() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const [searchParams] = useSearchParams()
  const { query, data, loading } = useSearchResults()
  const [mobileFiltersOpen, setMobileFiltersOpen] = useState(false)
  const {
    toggleFilter,
    applyPriceRange,
    clearFilters,
    setSort,
    setPage,
    currentSort,
  } = useListingFilters()

  useShopPageMeta({
    title: query ? `${t('common.search')}: ${query}` : t('common.search'),
    description: t('seo.searchDescription'),
    noindex: query.length > 0,
    path: query ? `/search?q=${encodeURIComponent(query)}` : '/search',
  })

  const allProducts = useMemo(
    () => data?.products.map(mapCatalogSearchProduct) ?? [],
    [data?.products],
  )

  const parsedFilters = useMemo(() => parseListingFilters(searchParams), [searchParams])

  const filteredProducts = useMemo(() => {
    const filtered = allProducts.filter((product) => productMatchesFilters(product, parsedFilters))
    return sortSearchProducts(filtered, currentSort as SortOptionId)
  }, [allProducts, currentSort, parsedFilters])

  const page = Math.max(1, Number(searchParams.get('page')) || 1)
  const pageProducts = filteredProducts.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE)
  const facets = useMemo(
    () => buildListingFacets(locale, allProducts, parsedFilters),
    [allProducts, locale, parsedFilters],
  )

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

  const hasCategories = (data?.categories.length ?? 0) > 0
  const hasSubCategories = (data?.subCategories.length ?? 0) > 0
  const hasProducts = allProducts.length > 0
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

                <div className="flex gap-10 lg:gap-12">
                  <ListingSidebar
                    facets={facets}
                    activeFilters={toActiveFiltersRecord(parsedFilters)}
                    onToggle={toggleFilter}
                    onApplyPriceRange={applyPriceRange}
                    onClear={clearFilters}
                    className="hidden w-56 shrink-0 lg:block xl:w-64"
                  />

                  <div className="min-w-0 flex-1">
                    <ListingToolbar
                      totalCount={filteredProducts.length}
                      sortOptions={getListingSortOptions(locale)}
                      currentSort={currentSort}
                      onSortChange={setSort}
                      onOpenFilters={() => setMobileFiltersOpen(true)}
                    />

                    {pageProducts.length > 0 ? (
                      <>
                        <ProductGrid products={pageProducts} />
                        <ListingPagination
                          page={page}
                          pageSize={PAGE_SIZE}
                          totalCount={filteredProducts.length}
                          onPageChange={setPage}
                        />
                      </>
                    ) : (
                      <div className="rounded-sm border border-border bg-surface-muted px-6 py-16 text-center">
                        <p className="text-base font-medium text-text">{t('listing.emptyTitle')}</p>
                        <p className="mt-2 text-sm text-text-muted">{t('listing.emptyMessage')}</p>
                        <button
                          type="button"
                          onClick={clearFilters}
                          className="mt-4 text-sm font-semibold text-warm hover:underline"
                        >
                          {t('listing.clearFilters')}
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </section>
            )}
          </div>
        )}
      </Container>

      {mobileFiltersOpen && (
        <Portal>
          <button
            type="button"
            aria-label={t('common.close')}
            className="fixed inset-0 z-[100] bg-black/40 lg:hidden"
            onClick={() => setMobileFiltersOpen(false)}
          />
          <div className="fixed inset-y-0 start-0 z-[110] flex w-[min(100%,20rem)] flex-col bg-surface shadow-2xl lg:hidden">
            <div className="flex items-center justify-between border-b border-border px-4 py-4">
              <span className="text-base font-semibold">{t('listing.filters')}</span>
              <button
                type="button"
                onClick={() => setMobileFiltersOpen(false)}
                className="flex size-9 items-center justify-center rounded-full text-text-muted hover:bg-surface-muted"
              >
                <CloseIcon />
              </button>
            </div>
            <div className="overflow-y-auto px-4">
              <ListingSidebar
                facets={facets}
                activeFilters={toActiveFiltersRecord(parsedFilters)}
                onToggle={toggleFilter}
                onApplyPriceRange={applyPriceRange}
                onClear={clearFilters}
              />
            </div>
          </div>
        </Portal>
      )}
    </div>
  )
}
