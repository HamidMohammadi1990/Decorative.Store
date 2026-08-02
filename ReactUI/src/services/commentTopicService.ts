import type { CommentTopic } from '@/models/catalog/commentTopic.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'

const COMMENT_TOPIC_SEARCH_PATH = '/api/v1/comment-topic/search'

export const commentTopicService = {
  async search(locale: Locale, pagination: PagedRequest = { pageNumber: 1, pageSize: 20 }) {
    return apiPost<PagedResult<CommentTopic>>(
      COMMENT_TOPIC_SEARCH_PATH,
      { pagination, isActive: true },
      { locale },
    )
  },

  async getDefaultTopicId(locale: Locale): Promise<string | null> {
    const result = await this.search(locale, { pageNumber: 1, pageSize: 1 })
    return result.items[0]?.id ?? null
  },
}
