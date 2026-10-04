import { create } from 'zustand'
import type { ProfileAnswerValue } from '@/models/profile/profileCompletion.model'

interface ProfileCompletionState {
  answers: Record<string, ProfileAnswerValue>
  rewardClaimedAt: string | null
  setAnswer: (questionId: string, value: ProfileAnswerValue) => void
  setAnswers: (answers: Record<string, ProfileAnswerValue>) => void
  setRewardClaimedAt: (value: string | null) => void
  reset: () => void
}

/** In-memory session state only — synced from API on load, saved via API on demand. */
export const useProfileCompletionStore = create<ProfileCompletionState>()((set) => ({
  answers: {},
  rewardClaimedAt: null,

  setAnswer: (questionId, value) =>
    set((state) => ({
      answers: { ...state.answers, [questionId]: value },
    })),

  setAnswers: (answers) => set({ answers }),

  setRewardClaimedAt: (value) => set({ rewardClaimedAt: value }),

  reset: () => set({ answers: {}, rewardClaimedAt: null }),
}))
