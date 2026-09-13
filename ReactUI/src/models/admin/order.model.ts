export type AdminOrderStatus = 1 | 2 | 3 | 4

export interface AdminOrderListItem {
  id: string
  trackingCode: number
  title: string
  userId: string
  userFirstName: string
  userLastName: string
  status: AdminOrderStatus
  isFinaly: boolean
  createdOnUtc: string
  totalPrice: number
  finalPrice: number
  vatPrice: number
}
