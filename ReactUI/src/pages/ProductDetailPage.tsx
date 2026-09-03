import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { CopyableShortLink } from '@/components/ui/CopyableShortLink'
import { ProductBuyBox } from '@/components/product/ProductBuyBox'
import { ProductDetailSections } from '@/components/product/ProductDetailSections'
import { ProductGallery } from '@/components/product/ProductGallery'
import { ProductInfoPanel } from '@/components/product/ProductInfoPanel'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { useProductDetail } from '@/hooks/useProductDetail'
import { useProductReviews } from '@/hooks/useProductReviews'
import { useShopPageMeta, resolveOgImageSrc } from '@/hooks/useShopPageMeta'
import {
  buildBreadcrumbJsonLd,
  buildProductJsonLd,
} from '@/components/seo/jsonLdBuilders'
import { absoluteUrl } from '@/config/site'
import { NotFoundPage } from '@/pages/NotFoundPage'

import { useSettingsStore } from '@/stores/settingsStore'

export function ProductDetailPage() {
  const { t } = useTranslation()
  const currencyCode = useSettingsStore((s) => s.currency?.code ?? 'IRR')
  const { product, related, loading, error } = useProductDetail()
  const {
    reviews,
    loading: reviewsLoading,
    reload: reloadReviews,
  } = useProductReviews(product?.id, product?.slug)

  const primaryCategory = product?.categorySlugs[0] ?? 'shop'

  useShopPageMeta({
    title: product?.title,
    description: product?.description?.slice(0, 160),
    image: product ? resolveOgImageSrc(product.images[0]?.src ?? product.image.src) : undefined,
    type: 'product',
    path: product ? `/product/${product.slug}` : undefined,
    active: Boolean(product),
    jsonLd: product
      ? [
          buildBreadcrumbJsonLd([
            { name: t('product.breadcrumbHome'), path: '/' },
            {
              name: t(`product.categories.${primaryCategory}`, {
                defaultValue: primaryCategory.replace(/-/g, ' '),
              }),
              path: `/${primaryCategory}`,
            },
            { name: product.title },
          ]),
          buildProductJsonLd({
            name: product.title,
            description: product.description,
            image: resolveOgImageSrc(product.images[0]?.src ?? product.image.src) ?? '',
            url: absoluteUrl(`/product/${product.slug}`),
            sku: product.id,
            price: product.price.amount,
            currency: currencyCode,
            inStock: product.inStock,
            rating: product.averageRating,
            reviewCount: product.reviewCount,
          }),
        ]
      : null,
  })

  if (loading && !product) {
    return <PageLoading />
  }

  if (error || !product) {
    return <NotFoundPage />
  }

  return (
    <div className="bg-surface">
      <Container className="py-6 md:py-8">
        <nav
          aria-label="Breadcrumb"
          className="mb-6 flex flex-wrap items-center gap-1.5 text-xs text-text-muted"
        >
          <Link to="/" className="transition-colors hover:text-accent">
            {t('product.breadcrumbHome')}
          </Link>
          <span aria-hidden>/</span>
          <Link to={`/${primaryCategory}`} className="transition-colors hover:text-accent">
            {t(`product.categories.${primaryCategory}`, {
              defaultValue: primaryCategory.replace(/-/g, ' '),
            })}
          </Link>
          <span aria-hidden>/</span>
          <span className="text-text">{product.title}</span>
        </nav>

        <div className="grid gap-6 lg:grid-cols-12 lg:gap-5 xl:gap-8">
          <div className="lg:col-span-5">
            <ProductGallery
              images={product.images}
              title={product.title}
              slug={product.slug}
              onSale={product.onSale}
            />
            <CopyableShortLink path={`/product/${product.slug}`} className="mt-4" />
          </div>

          <div className="lg:col-span-4">
            <ProductInfoPanel
              product={product}
              primaryCategory={primaryCategory}
              reviews={reviews}
              reviewsLoading={reviewsLoading}
            />
          </div>

          <div className="lg:col-span-3">
            <ProductBuyBox
              product={product}
              reviews={reviews}
              reviewsLoading={reviewsLoading}
            />
          </div>
        </div>

        <ProductDetailSections
          product={product}
          reviews={reviews}
          reviewsLoading={reviewsLoading}
          reloadReviews={reloadReviews}
        />

        {related.length > 0 && (
          <section className="mt-14 border-t border-border pt-10">
            <h2 className="mb-6 text-base font-semibold text-text">
              {t('product.relatedTitle')}
            </h2>
            <ProductGrid products={related} />
          </section>
        )}
      </Container>
    </div>
  )
}
