import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { ProfileAnswerValue } from '@/models/profile/profileCompletion.model'

interface ProfileCompletionState {
  answers: Record<string, ProfileAnswerValue>
  rewardClaimedAt: string | null
  setAnswer: (questionId: string, value: ProfileAnswerValue) => void
  claimReward: () => void
  reset: () => void
}

export const useProfileCompletionStore = create<ProfileCompletionState>()(
  persist(
    (set) => ({
      answers: {},
      rewardClaimedAt: null,

      setAnswer: (questionId, value) =>
        set((state) => ({
          answers: { ...state.answers, [questionId]: value },
        })),

      claimReward: () => set({ rewardClaimedAt: new Date().toISOString() }),

      reset: () => set({ answers: {}, rewardClaimedAt: null }),
    }),
    {
      name: 'diba-profile-completion',
    },
  ),
)
