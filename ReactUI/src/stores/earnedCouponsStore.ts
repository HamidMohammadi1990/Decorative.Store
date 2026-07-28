import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { DashboardCoupon } from '@/models/dashboard/dashboard.model'

interface EarnedCouponsState {
  coupons: DashboardCoupon[]
  addCoupon: (coupon: DashboardCoupon) => void
  hasCoupon: (id: string) => boolean
}

export const useEarnedCouponsStore = create<EarnedCouponsState>()(
  persist(
    (set, get) => ({
      coupons: [],

      addCoupon: (coupon) =>
        set((state) => {
          if (state.coupons.some((c) => c.id === coupon.id)) {
            return state
          }
          return { coupons: [coupon, ...state.coupons] }
        }),

      hasCoupon: (id) => get().coupons.some((c) => c.id === id),
    }),
    {
      name: 'diba-earned-coupons',
    },
  ),
)
