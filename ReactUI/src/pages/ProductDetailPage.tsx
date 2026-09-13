import { useTranslation } from 'react-i18next'
import { PageBreadcrumbs } from '@/components/ui/PageBreadcrumbs'
import { ShareBar } from '@/components/ui/ShareBar'
import { ProductBuyBox } from '@/components/product/ProductBuyBox'
import { ProductDetailSections } from '@/components/product/ProductDetailSections'
import { ProductGallery } from '@/components/product/ProductGallery'
import { ProductInfoPanel } from '@/components/product/ProductInfoPanel'
import { RelatedProductsSlider } from '@/components/product/RelatedProductsSlider'
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
import { buildProductBreadcrumbJsonLdItems, buildProductPageBreadcrumbs } from '@/extensions/productPageBreadcrumbs'
import { NotFoundPage } from '@/pages/NotFoundPage'

import { useSettingsStore } from '@/stores/settingsStore'

export function ProductDetailPage() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const currencyCode = useSettingsStore((s) => s.currency?.code ?? 'IRR')
  const { product, related, loading, error } = useProductDetail()
  const {
    reviews,
    loading: reviewsLoading,
    reload: reloadReviews,
  } = useProductReviews(product?.id, product?.slug)

  const primaryCategory = product?.categorySlugs[0] ?? 'shop'
  const breadcrumbItems = product ? buildProductPageBreadcrumbs(product, t) : []

  const productImage = product?.images[0] ?? product?.image

  useShopPageMeta({
    title: product?.title,
    description: product?.description?.slice(0, 160),
    image: product ? resolveOgImageSrc(productImage?.src) : undefined,
    imageAlt: productImage?.alt ?? product?.title,
    type: 'product',
    path: product ? `/product/${product.slug}` : undefined,
    productPrice: product?.price.amount,
    productCurrency: currencyCode,
    keywords: product?.title,
    active: Boolean(product),
    jsonLd: product
      ? [
          buildBreadcrumbJsonLd(buildProductBreadcrumbJsonLdItems(product, t)),
          buildProductJsonLd({
            name: product.title,
            description: product.description,
            image: resolveOgImageSrc(productImage?.src) ?? '',
            url: absoluteUrl(`/product/${product.slug}`),
            sku: product.id,
            price: product.price.amount,
            currency: currencyCode,
            inStock: product.inStock,
            rating: product.averageRating,
            reviewCount: product.reviewCount,
            brand: t('product.brandName'),
            category: t(`product.categories.${primaryCategory}`, {
              defaultValue: primaryCategory.replace(/-/g, ' '),
            }),
            locale,
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

  const shareUrl =
    typeof window !== 'undefined'
      ? window.location.href
      : absoluteUrl(`/product/${product.slug}`)

  return (
    <div className="bg-surface">
      <Container className="py-6 md:py-8">
        <PageBreadcrumbs items={breadcrumbItems} className="mb-6" />

        <div className="grid gap-6 lg:grid-cols-12 lg:gap-5 xl:gap-8">
          <div className="lg:col-span-5">
            <ProductGallery
              images={product.images}
              title={product.title}
              slug={product.slug}
              onSale={product.onSale}
            />
            <ShareBar
              className="mt-4"
              heading={t('product.shareTitle')}
              title={product.title}
              url={shareUrl}
              shareActionLabel={t('product.share')}
            />
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

        <RelatedProductsSlider products={related} />
      </Container>
    </div>
  )
}
