export type OrderStatus = 'pending' | 'processing' | 'shipped' | 'delivered' | 'cancelled'

export type TransactionType = 'credit' | 'debit'

export type CouponStatus = 'active' | 'used' | 'expired'

export type ReviewStatus = 'published' | 'pending'

export interface DashboardUser {
  id: string
  firstName: string
  lastName: string
  email: string
  memberSince: string
}

export interface WalletInfo {
  balance: number
  currencyCode: string
}

export interface DashboardOrderItem {
  title: string
  imageUrl?: string
  quantity: number
  price: number
}

export type OrderPaymentMethod = 'card' | 'wallet' | 'cod'

export interface DashboardOrderAddress {
  label: string
  recipient: string
  lines: string[]
}

export interface DashboardOrderTimelineStep {
  key: 'placed' | 'confirmed' | 'processing' | 'shipped' | 'delivered' | 'cancelled'
  date?: string
  done: boolean
}

export interface DashboardOrder {
  id: string
  date: string
  status: OrderStatus
  total: number
  subtotal: number
  shippingCost: number
  paymentMethod: OrderPaymentMethod
  items: DashboardOrderItem[]
  shippingAddress: DashboardOrderAddress
  timeline: DashboardOrderTimelineStep[]
  trackingNumber?: string
  carrier?: string
  estimatedDelivery?: string
}

export type OrderStatusFilter = OrderStatus | 'all'

export type OrderSortOption = 'newest' | 'oldest' | 'amountHigh' | 'amountLow'

export interface DashboardTransaction {
  id: string
  date: string
  description: string
  amount: number
  type: TransactionType
}

export interface DashboardReview {
  id: string
  productTitle: string
  productSlug: string
  rating: number
  comment: string
  date: string
  status: ReviewStatus
}

export interface DashboardCoupon {
  id: string
  code: string
  description: string
  discount: string
  expiresAt: string
  status: CouponStatus
}

export interface DashboardData {
  wallet: WalletInfo
  orders: DashboardOrder[]
  transactions: DashboardTransaction[]
  reviews: DashboardReview[]
  coupons: DashboardCoupon[]
}

export type DashboardSection =
  | 'wallet'
  | 'profile'
  | 'orders'
  | 'transactions'
  | 'reviews'
  | 'wishlist'
  | 'stories'
  | 'addresses'
  | 'coupons'
