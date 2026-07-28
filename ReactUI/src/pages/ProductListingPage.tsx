import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ListingBreadcrumbs } from '@/components/listing/ListingBreadcrumbs'
import { ListingSidebar } from '@/components/listing/ListingSidebar'
import { ListingToolbar } from '@/components/listing/ListingToolbar'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { useListingFilters } from '@/hooks/useListingFilters'
import { useProductListing } from '@/hooks/useProductListing'
import { NotFoundPage } from '@/pages/NotFoundPage'

export function ProductListingPage() {
  const { t } = useTranslation()
  const { data, loading, error } = useProductListing()
  const { toggleFilter, applyPriceRange, clearFilters, setSort, currentSort } = useListingFilters()
  const [mobileFiltersOpen, setMobileFiltersOpen] = useState(false)

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
    <div className="bg-surface py-8 md:py-10">
      <Container>
        <ListingBreadcrumbs items={data.breadcrumbs} />

        <header className="mt-4 mb-8">
          <h1 className="text-2xl font-semibold text-text md:text-3xl">{data.title}</h1>
        </header>

        <div className="flex gap-10 lg:gap-12">
          <ListingSidebar
            facets={data.facets}
            activeFilters={data.activeFilters}
            onToggle={toggleFilter}
            onApplyPriceRange={applyPriceRange}
            onClear={clearFilters}
            className="hidden w-56 shrink-0 lg:block xl:w-64"
          />

          <div className="min-w-0 flex-1">
            <ListingToolbar
              totalCount={data.totalCount}
              sortOptions={data.sortOptions}
              currentSort={currentSort}
              onSortChange={setSort}
              onOpenFilters={() => setMobileFiltersOpen(true)}
            />

            {data.products.length > 0 ? (
              <ProductGrid products={data.products} />
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
                facets={data.facets}
                activeFilters={data.activeFilters}
                onToggle={(facetId, value) => {
                  toggleFilter(facetId, value)
                }}
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
