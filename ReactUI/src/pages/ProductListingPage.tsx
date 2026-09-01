import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ListingBreadcrumbs } from '@/components/listing/ListingBreadcrumbs'
import { ListingSidebar } from '@/components/listing/ListingSidebar'
import { ListingToolbar } from '@/components/listing/ListingToolbar'
import { ListingPagination } from '@/components/listing/ListingPagination'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { parseListingFilters, toActiveFiltersRecord } from '@/extensions/listingFilters'
import { useListingFilters } from '@/hooks/useListingFilters'
import { useProductListing } from '@/hooks/useProductListing'
import { NotFoundPage } from '@/pages/NotFoundPage'

export function ProductListingPage() {
  const { t } = useTranslation()
  const { data, loading, error } = useProductListing()
  const { toggleFilter, applyPriceRange, clearFilters, setSort, setPage, currentSort, searchParams } =
    useListingFilters()
  const [mobileFiltersOpen, setMobileFiltersOpen] = useState(false)
  const activeFilters = useMemo(
    () => toActiveFiltersRecord(parseListingFilters(searchParams)),
    [searchParams],
  )
  const activeFilterCount = useMemo(
    () => Object.values(activeFilters).reduce((sum, values) => sum + values.length, 0),
    [activeFilters],
  )

  if (loading) {
    return <PageLoading />
  }

  if (error || !data) {
    return (
      <Container className="py-20 text-center text-sm text-sale">
        {t('common.error')}
      </Container>
    )
  }

  if (data.pathNotFound) {
    return <NotFoundPage />
  }

  return (
    <div className="bg-gradient-to-b from-surface-muted/40 via-surface to-surface py-8 md:py-10">
      <Container>
        <ListingBreadcrumbs items={data.breadcrumbs} />

        <header className="mt-4 mb-8 md:mb-10">
          <h1 className="font-display text-2xl font-semibold tracking-tight text-text md:text-3xl lg:text-4xl">
            {data.title}
          </h1>
          <p className="mt-2 text-sm text-text-muted">
            {t('listing.resultCount', { count: data.totalCount })}
          </p>
        </header>

        <div className="flex gap-6 lg:gap-8 xl:gap-10">
          <ListingSidebar
            facets={data.facets}
            activeFilters={activeFilters}
            onToggle={toggleFilter}
            onApplyPriceRange={applyPriceRange}
            onClear={clearFilters}
            className="hidden w-60 shrink-0 lg:block xl:w-72"
          />

          <div className="min-w-0 flex-1">
            <ListingToolbar
              totalCount={data.totalCount}
              sortOptions={data.sortOptions}
              currentSort={currentSort}
              onSortChange={setSort}
              onOpenFilters={() => setMobileFiltersOpen(true)}
              activeFilterCount={activeFilterCount}
            />

            {data.products.length > 0 ? (
              <>
                <ProductGrid products={data.products} />
                <ListingPagination
                  page={data.page}
                  pageSize={data.pageSize}
                  totalCount={data.totalCount}
                  onPageChange={setPage}
                />
              </>
            ) : (
              <div className="rounded-xl border border-dashed border-border bg-surface/80 px-6 py-16 text-center shadow-sm">
                <p className="text-base font-semibold text-text">{t('listing.emptyTitle')}</p>
                <p className="mt-2 text-sm text-text-muted">{t('listing.emptyMessage')}</p>
                <button
                  type="button"
                  onClick={clearFilters}
                  className="mt-5 rounded-lg bg-warm px-4 py-2 text-sm font-semibold text-warm-text transition-opacity hover:opacity-90"
                >
                  {t('listing.clearFilters')}
                </button>
              </div>
            )}
          </div>
        </div>
      </Container>

      {mobileFiltersOpen && (
        <Portal>
          <button
            type="button"
            aria-label={t('common.close')}
            className="fixed inset-0 z-[100] bg-black/45 backdrop-blur-[2px] lg:hidden"
            onClick={() => setMobileFiltersOpen(false)}
          />
          <div className="fixed inset-y-0 start-0 z-[110] flex w-[min(100%,22rem)] flex-col overflow-hidden rounded-e-2xl bg-surface shadow-2xl lg:hidden">
            <div className="flex items-center justify-between border-b border-border/60 px-4 py-3">
              <span className="text-base font-semibold text-text">{t('listing.filters')}</span>
              <button
                type="button"
                onClick={() => setMobileFiltersOpen(false)}
                className="flex size-9 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
              >
                <CloseIcon />
              </button>
            </div>
            <div className="min-h-0 flex-1 overflow-hidden">
              <ListingSidebar
                embedded
                className="h-full"
                facets={data.facets}
                activeFilters={activeFilters}
                onToggle={(facetId, value) => {
                  toggleFilter(facetId, value)
                }}
                onApplyPriceRange={applyPriceRange}
                onClear={clearFilters}
              />
            </div>
            <div className="border-t border-border/60 p-4">
              <button
                type="button"
                onClick={() => setMobileFiltersOpen(false)}
                className="w-full rounded-lg bg-warm py-3 text-sm font-semibold text-warm-text"
              >
                {t('listing.showResults', { count: data.totalCount })}
              </button>
            </div>
          </div>
        </Portal>
      )}
    </div>
  )
}
