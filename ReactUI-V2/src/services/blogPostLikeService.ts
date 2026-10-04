import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'

const BLOG_POST_LIKE_CREATE_PATH = '/api/v1/blog-post-like/create'

export const blogPostLikeService = {
  async create(
    blogPostId: string,
    locale: Locale,
    accessToken?: string | null,
  ): Promise<{ id: string }> {
    const result = await apiPost<unknown>(
      BLOG_POST_LIKE_CREATE_PATH,
      { blogPostId },
      { locale, accessToken },
    )

    const record = readRecord(result) ?? {}
    return { id: readStringField(record, 'id', 'Id') }
  },
}
