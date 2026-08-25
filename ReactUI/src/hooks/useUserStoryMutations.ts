import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { UserStoryDraft, UserStoryInput } from '@/models/stories/story.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { ApiError } from '@/services/api/apiTypes'
import { userStoryService } from '@/services/userStoryService'
import { useUserStoryStore } from '@/stores/userStoryStore'
import { useUserStore } from '@/stores/userStore'

function resolveMutationError(error: unknown, fallback: string) {
  if (error instanceof ApiError && error.messages.length > 0) {
    return error.messages[0].message
  }

  return fallback
}

export function useUserStoryMutations() {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const accessToken = useUserStore((state) => state.accessToken)
  const loadStories = useUserStoryStore((state) => state.loadStories)
  const [isSaving, setIsSaving] = useState(false)
  const [mutationError, setMutationError] = useState<string | null>(null)

  const publishStory = useCallback(
    async (input: UserStoryInput) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('dashboard.stories.saveFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        await userStoryService.create(accessToken, locale, input)
        await loadStories(accessToken, locale)
        return true
      } catch (error) {
        setMutationError(resolveMutationError(error, t('dashboard.stories.saveFailed')))
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadStories, t],
  )

  const updateStory = useCallback(
    async (id: string, input: UserStoryInput) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('dashboard.stories.saveFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        await userStoryService.update(accessToken, locale, id, input)
        await loadStories(accessToken, locale)
        return true
      } catch (error) {
        setMutationError(resolveMutationError(error, t('dashboard.stories.saveFailed')))
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadStories, t],
  )

  const toggleStoryActive = useCallback(
    async (story: UserStoryDraft) => {
      return updateStory(story.id, {
        title: story.title,
        caption: story.caption,
        mediaType: story.mediaType,
        mediaPath: story.mediaPath,
        mediaAlt: story.mediaAlt,
        productSlug: story.productSlugs[0],
        isActive: !story.isActive,
      })
    },
    [updateStory],
  )

  const deleteStory = useCallback(
    async (id: string) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('dashboard.stories.deleteFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        await userStoryService.delete(accessToken, id)
        await loadStories(accessToken, locale)
        return true
      } catch (error) {
        setMutationError(resolveMutationError(error, t('dashboard.stories.deleteFailed')))
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadStories, t],
  )

  const uploadMedia = useCallback(
    async (file: File) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        throw new Error('auth_required')
      }

      return userStoryService.uploadMedia(accessToken, locale, file)
    },
    [accessToken, locale],
  )

  const clearMutationError = useCallback(() => setMutationError(null), [])

  return {
    isSaving,
    mutationError,
    publishStory,
    updateStory,
    toggleStoryActive,
    deleteStory,
    uploadMedia,
    clearMutationError,
  }
}
