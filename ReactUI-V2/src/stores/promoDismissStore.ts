import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface PromoDismissState {
  dismissedIds: string[]
  dismiss: (id: string) => void
  isDismissed: (id: string) => boolean
}

export const usePromoDismissStore = create<PromoDismissState>()(
  persist(
    (set, get) => ({
      dismissedIds: [],

      dismiss: (id) =>
        set((state) => ({
          dismissedIds: state.dismissedIds.includes(id)
            ? state.dismissedIds
            : [...state.dismissedIds, id],
        })),

      isDismissed: (id) => get().dismissedIds.includes(id),
    }),
    { name: 'westelm-promo-dismissed' },
  ),
)
