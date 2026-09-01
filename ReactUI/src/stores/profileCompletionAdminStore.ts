import { create } from 'zustand'
import { createEmptyAdminQuestion } from '@/extensions/profileCompletionAdminDefaults'
import type {
  AdminProfileCompletionConfig,
  AdminProfileQuestion,
  AdminQuestionOption,
  LocalizedText,
} from '@/models/profile/profileCompletionAdmin.model'
import type { ProfileQuestionType } from '@/models/profile/profileCompletion.model'

interface ProfileCompletionAdminState {
  config: AdminProfileCompletionConfig | null
  setConfigFromServer: (config: AdminProfileCompletionConfig) => void
  clearConfig: () => void
  updateCampaign: (
    patch: Partial<
      Pick<AdminProfileCompletionConfig, 'campaignId' | 'titles' | 'subtitles' | 'reward'>
    >,
  ) => void
  addQuestion: () => void
  updateQuestion: (id: string, patch: Partial<AdminProfileQuestion>) => void
  removeQuestion: (id: string) => void
  moveQuestion: (id: string, direction: 'up' | 'down') => void
  addOption: (questionId: string) => void
  updateOption: (
    questionId: string,
    optionIndex: number,
    patch: Partial<AdminQuestionOption>,
  ) => void
  removeOption: (questionId: string, optionIndex: number) => void
}

function reindexQuestions(questions: AdminProfileQuestion[]) {
  return questions.map((q, index) => ({ ...q, order: index + 1 }))
}

function emptyOption(index: number): AdminQuestionOption {
  return {
    value: `option-${index + 1}`,
    labels: { en: '', fa: '' },
  }
}

export const useProfileCompletionAdminStore = create<ProfileCompletionAdminState>()((set) => ({
  config: null,

  setConfigFromServer: (config) => set({ config }),

  clearConfig: () => set({ config: null }),

  updateCampaign: (patch) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          ...patch,
          titles: patch.titles ? { ...state.config.titles, ...patch.titles } : state.config.titles,
          subtitles: patch.subtitles
            ? { ...state.config.subtitles, ...patch.subtitles }
            : state.config.subtitles,
          reward: patch.reward
            ? {
                ...state.config.reward,
                ...patch.reward,
                descriptions: patch.reward.descriptions
                  ? { ...state.config.reward.descriptions, ...patch.reward.descriptions }
                  : state.config.reward.descriptions,
              }
            : state.config.reward,
        },
      }
    }),

  addQuestion: () =>
    set((state) => {
      if (!state.config) return state
      const nextOrder = state.config.questions.length + 1
      return {
        config: {
          ...state.config,
          questions: [...state.config.questions, createEmptyAdminQuestion(nextOrder)],
        },
      }
    }),

  updateQuestion: (id, patch) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          questions: state.config.questions.map((q) =>
            q.id === id
              ? {
                  ...q,
                  ...patch,
                  labels: patch.labels ? { ...q.labels, ...patch.labels } : q.labels,
                  hints: patch.hints ? { ...q.hints, ...patch.hints } : q.hints,
                  placeholders: patch.placeholders
                    ? { ...q.placeholders, ...patch.placeholders }
                    : q.placeholders,
                  options: patch.options ?? q.options,
                }
              : q,
          ),
        },
      }
    }),

  removeQuestion: (id) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          questions: reindexQuestions(state.config.questions.filter((q) => q.id !== id)),
        },
      }
    }),

  moveQuestion: (id, direction) =>
    set((state) => {
      if (!state.config) return state
      const list = [...state.config.questions].sort((a, b) => a.order - b.order)
      const index = list.findIndex((q) => q.id === id)
      if (index < 0) return state

      const target = direction === 'up' ? index - 1 : index + 1
      if (target < 0 || target >= list.length) return state

      const next = [...list]
      ;[next[index], next[target]] = [next[target], next[index]]

      return {
        config: {
          ...state.config,
          questions: reindexQuestions(next),
        },
      }
    }),

  addOption: (questionId) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          questions: state.config.questions.map((q) => {
            if (q.id !== questionId) return q
            return {
              ...q,
              options: [...q.options, emptyOption(q.options.length)],
            }
          }),
        },
      }
    }),

  updateOption: (questionId, optionIndex, patch) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          questions: state.config.questions.map((q) => {
            if (q.id !== questionId) return q
            return {
              ...q,
              options: q.options.map((opt, index) =>
                index === optionIndex
                  ? {
                      ...opt,
                      ...patch,
                      labels: patch.labels ? { ...opt.labels, ...patch.labels } : opt.labels,
                    }
                  : opt,
              ),
            }
          }),
        },
      }
    }),

  removeOption: (questionId, optionIndex) =>
    set((state) => {
      if (!state.config) return state
      return {
        config: {
          ...state.config,
          questions: state.config.questions.map((q) => {
            if (q.id !== questionId) return q
            return {
              ...q,
              options: q.options.filter((_, index) => index !== optionIndex),
            }
          }),
        },
      }
    }),
}))

export function patchLocalized(
  current: LocalizedText,
  locale: keyof LocalizedText,
  value: string,
): LocalizedText {
  return { ...current, [locale]: value }
}

export function isSelectType(type: ProfileQuestionType) {
  return type === 'single_select' || type === 'multi_select'
}
