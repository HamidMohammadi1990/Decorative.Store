import { Outlet } from 'react-router-dom'
import { CartDrawer } from '@/components/cart/CartDrawer'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

export function AppLayout() {
  useLocaleSettings()

  return (
    <>
      <Outlet />
      <CartDrawer />
    </>
  )
}
