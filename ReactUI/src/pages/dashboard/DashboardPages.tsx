import { useOutletContext } from 'react-router-dom'
import type { DashboardData } from '@/models/dashboard/dashboard.model'
import { WalletPanel } from '@/components/dashboard/WalletPanel'
import { OrdersPanel } from '@/components/dashboard/OrdersPanel'
import { TransactionsPanel } from '@/components/dashboard/TransactionsPanel'
import { ReviewsPanel } from '@/components/dashboard/ReviewsPanel'
import { AddressesPanel } from '@/components/dashboard/AddressesPanel'
import { CouponsPanel } from '@/components/dashboard/CouponsPanel'
import { WishlistPanel } from '@/components/dashboard/WishlistPanel'
import { StoriesPanel } from '@/components/dashboard/StoriesPanel'
import { ProfileCompletionPanel } from '@/components/dashboard/ProfileCompletionPanel'

function useDashboardContext() {
  return useOutletContext<DashboardData>()
}

export function DashboardWalletPage() {
  const data = useDashboardContext()
  return <WalletPanel wallet={data.wallet} recentTransactions={data.transactions} />
}

export function DashboardOrdersPage() {
  return <OrdersPanel />
}

export function DashboardTransactionsPage() {
  const data = useDashboardContext()
  return <TransactionsPanel transactions={data.transactions} />
}

export function DashboardReviewsPage() {
  return <ReviewsPanel />
}

export function DashboardProfilePage() {
  return <ProfileCompletionPanel />
}

export function DashboardStoriesPage() {
  return <StoriesPanel />
}

export function DashboardWishlistPage() {
  return <WishlistPanel />
}

export function DashboardAddressesPage() {
  return <AddressesPanel />
}

export function DashboardCouponsPage() {
  const data = useDashboardContext()
  return <CouponsPanel coupons={data.coupons} />
}
