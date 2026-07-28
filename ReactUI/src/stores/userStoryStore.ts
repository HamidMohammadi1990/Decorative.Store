import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { StoryMediaType, UserStoryDraft } from '@/models/stories/story.model'

interface UserStoryState {
  stories: UserStoryDraft[]
  addStory: (draft: Omit<UserStoryDraft, 'id' | 'createdAt' | 'isActive'>) => string
  updateStory: (id: string, patch: Partial<Omit<UserStoryDraft, 'id' | 'createdAt'>>) => void
  removeStory: (id: string) => void
  toggleActive: (id: string) => void
}

function createStoryId() {
  return `user-story-${Date.now()}-${Math.random().toString(36).slice(2, 7)}`
}

export const useUserStoryStore = create<UserStoryState>()(
  persist(
    (set) => ({
      stories: [],

      addStory: (draft) => {
        const id = createStoryId()
        const story: UserStoryDraft = {
          ...draft,
          id,
          createdAt: new Date().toISOString(),
          isActive: true,
        }
        set((state) => ({ stories: [story, ...state.stories] }))
        return id
      },

      updateStory: (id, patch) =>
        set((state) => ({
          stories: state.stories.map((s) => (s.id === id ? { ...s, ...patch } : s)),
        })),

      removeStory: (id) =>
        set((state) => ({
          stories: state.stories.filter((s) => s.id !== id),
        })),

      toggleActive: (id) =>
        set((state) => ({
          stories: state.stories.map((s) =>
            s.id === id ? { ...s, isActive: !s.isActive } : s,
          ),
        })),
    }),
    {
      name: 'diba-user-stories',
      partialize: (state) => ({ stories: state.stories }),
    },
  ),
)

export function readMediaFile(
  file: File,
): Promise<{ mediaType: StoryMediaType; mediaSrc: string; posterSrc?: string }> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = () => {
      const result = reader.result
      if (typeof result !== 'string') {
        reject(new Error('read failed'))
        return
      }
      const isVideo = file.type.startsWith('video/')
      resolve({
        mediaType: isVideo ? 'video' : 'image',
        mediaSrc: result,
        posterSrc: isVideo ? undefined : result,
      })
    }
    reader.onerror = () => reject(reader.error)
    reader.readAsDataURL(file)
  })
}
