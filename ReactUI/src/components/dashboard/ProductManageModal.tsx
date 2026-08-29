import { useTranslation } from 'react-i18next'
import type { AdminProductListItem } from '@/models/admin/catalog.model'
import { AdminLargeModal } from '@/components/dashboard/admin/AdminLargeModal'
import { DashboardProductQuestionsPanel } from '@/components/dashboard/DashboardProductQuestionsPanel'
import { ProductCommentsPanel } from '@/components/dashboard/ProductCommentsPanel'
import { ProductDescriptionsPanel } from '@/components/dashboard/ProductDescriptionsPanel'
import { ProductImagesPanel } from '@/components/dashboard/ProductImagesPanel'
import { ProductPropertiesPanel } from '@/components/dashboard/ProductPropertiesPanel'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'

export type ProductManageTab = 'images' | 'properties' | 'descriptions' | 'comments' | 'questions'

export function ProductManageModal({
  open,
  tab,
  product,
  onClose,
}: {
  open: boolean
  tab: ProductManageTab
  product: AdminProductListItem | null
  onClose: () => void
}) {
  const { t } = useTranslation()

  if (!product) return null

  const productLabel = embeddedProductLabel(product.title, product.productCode)

  const titleKey =
    tab === 'images'
      ? 'dashboard.productImages.title'
      : tab === 'properties'
        ? 'dashboard.productProperties.title'
        : tab === 'descriptions'
          ? 'dashboard.productDescriptions.title'
          : tab === 'questions'
            ? 'dashboard.productQuestions.title'
            : 'dashboard.productComments.title'

  const descriptionKey =
    tab === 'images'
      ? 'dashboard.productImages.description'
      : tab === 'properties'
        ? 'dashboard.productProperties.description'
        : tab === 'descriptions'
          ? 'dashboard.productDescriptions.description'
          : tab === 'questions'
            ? 'dashboard.productQuestions.description'
            : 'dashboard.productComments.description'

  const embedProps = {
    embedded: true,
    productId: product.id,
    productTitle: product.title,
    productCode: product.productCode,
  }

  return (
    <AdminLargeModal
      open={open}
      title={t(titleKey)}
      description={`${t(descriptionKey)} · ${productLabel}`}
      onClose={onClose}
    >
      {tab === 'images' && <ProductImagesPanel {...embedProps} />}
      {tab === 'properties' && <ProductPropertiesPanel {...embedProps} />}
      {tab === 'descriptions' && <ProductDescriptionsPanel {...embedProps} />}
      {tab === 'comments' && <ProductCommentsPanel {...embedProps} />}
      {tab === 'questions' && <DashboardProductQuestionsPanel {...embedProps} />}
    </AdminLargeModal>
  )
}
