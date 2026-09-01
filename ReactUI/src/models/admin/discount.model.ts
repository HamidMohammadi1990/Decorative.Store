export type AdminDiscountType = 'percent' | 'fixed'

export interface AdminDiscount {
  id: string
  code: string
  percentage: number
  amount: number
  expiryDateOnUtc: string | null
  maxDiscountAmount: number
  usageLimit: number
  remainingUses: number
  minimumAmount: number | null
  isActive: boolean
}

export interface CreateAdminDiscountInput {
  code: string
  percentage: number
  amount: number
  expiryDateOnUtc: string | null
  maxDiscountAmount: number
  usageLimit: number
  remainingUses: number
  minimumAmount: number | null
  isActive: boolean
  userId?: string | null
  productId?: string | null
  subCategoryId?: string | null
  isCooperation?: boolean
}

export interface UpdateAdminDiscountInput extends CreateAdminDiscountInput {
  id: string
}