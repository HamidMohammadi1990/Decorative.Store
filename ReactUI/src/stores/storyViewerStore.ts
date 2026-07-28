import { create } from 'zustand'
import type { StoryGroup } from '@/models/stories/story.model'

interface StoryViewerState {
  isOpen: boolean
  groups: StoryGroup[]
  groupIndex: number
  slideIndex: number
  commentsOpen: boolean
  open: (groups: StoryGroup[], startGroupIndex?: number) => void
  close: () => void
  setGroupIndex: (index: number) => void
  setSlideIndex: (index: number) => void
  openComments: () => void
  closeComments: () => void
  nextSlide: () => void
  prevSlide: () => void
  nextGroup: () => void
  prevGroup: () => void
}

export const useStoryViewerStore = create<StoryViewerState>((set, get) => ({
  isOpen: false,
  groups: [],
  groupIndex: 0,
  slideIndex: 0,
  commentsOpen: false,

  open: (groups, startGroupIndex = 0) =>
    set({
      isOpen: true,
      groups,
      groupIndex: Math.max(0, Math.min(startGroupIndex, groups.length - 1)),
      slideIndex: 0,
      commentsOpen: false,
    }),

  close: () =>
    set({
      isOpen: false,
      commentsOpen: false,
      slideIndex: 0,
    }),

  setGroupIndex: (index) => set({ groupIndex: index, slideIndex: 0, commentsOpen: false }),

  setSlideIndex: (index) => set({ slideIndex: index }),

  openComments: () => set({ commentsOpen: true }),

  closeComments: () => set({ commentsOpen: false }),

  nextSlide: () => {
    const { groups, groupIndex, slideIndex } = get()
    const group = groups[groupIndex]
    if (!group) return

    if (slideIndex < group.slides.length - 1) {
      set({ slideIndex: slideIndex + 1 })
      return
    }

    if (groupIndex < groups.length - 1) {
      set({ groupIndex: groupIndex + 1, slideIndex: 0 })
      return
    }

    set({ isOpen: false, commentsOpen: false })
  },

  prevSlide: () => {
    const { groups, groupIndex, slideIndex } = get()

    if (slideIndex > 0) {
      set({ slideIndex: slideIndex - 1 })
      return
    }

    if (groupIndex > 0) {
      const prevGroup = groups[groupIndex - 1]
      set({
        groupIndex: groupIndex - 1,
        slideIndex: Math.max(0, (prevGroup?.slides.length ?? 1) - 1),
      })
    }
  },

  nextGroup: () => {
    const { groups, groupIndex } = get()
    if (groupIndex < groups.length - 1) {
      set({ groupIndex: groupIndex + 1, slideIndex: 0, commentsOpen: false })
    } else {
      set({ isOpen: false, commentsOpen: false })
    }
  },

  prevGroup: () => {
    const { groupIndex } = get()
    if (groupIndex > 0) {
      set({ groupIndex: groupIndex - 1, slideIndex: 0, commentsOpen: false })
    }
  },
}))
