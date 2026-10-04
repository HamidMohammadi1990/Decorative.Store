import { useTranslation } from 'react-i18next'
import { CartPanel } from '@/components/cart/CartPanel'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'

export function CartPage() {
  const { t } = useTranslation()

  useShopPageMeta({
    title: t('cartPage.title'),
    noindex: true,
    path: '/cart',
  })

  return <CartPanel />
}
