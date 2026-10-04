import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readRecord } from '@/services/api/apiNormalize'

const TOGGLE_PATH = '/api/v1/user-story-like/toggle'

export const userStoryLikeService = {
  async toggle(
    userStoryId: string,
    locale: Locale,
    accessToken: string,
  ): Promise<{ liked: boolean; likeCount: number }> {
    const data = await apiPost<{ liked?: boolean; Liked?: boolean; likeCount?: number; LikeCount?: number }>(
      TOGGLE_PATH,
      { userStoryId },
      { locale, accessToken },
    )

    const record = readRecord(data) ?? data
    return {
      liked: Boolean(record.liked ?? record.Liked),
      likeCount: Number(record.likeCount ?? record.LikeCount ?? 0),
    }
  },
}
