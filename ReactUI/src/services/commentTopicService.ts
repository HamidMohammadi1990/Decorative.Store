import type { CommentTopic } from '@/models/catalog/commentTopic.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeCommentTopic } from '@/services/mappers/catalogResponseNormalize'

const COMMENT_TOPIC_SEARCH_PATH = '/api/v1/comment-topic/search'

export const commentTopicService = {
  async search(locale: Locale, pagination: PagedRequest = { pageNumber: 1, pageSize: 20 }) {
    const result = await apiPost<{
      items?: unknown[]
      Items?: unknown[]
      pageNumber?: number
      PageNumber?: number
      pageSize?: number
      PageSize?: number
      totalCount?: number
      TotalCount?: number
      totalPages?: number
      TotalPages?: number
    }>(COMMENT_TOPIC_SEARCH_PATH, { pagination, isActive: true }, { locale })

    const items = (result.items ?? result.Items ?? []).map(normalizeCommentTopic)

    return {
      items,
      pageNumber: result.pageNumber ?? result.PageNumber ?? pagination.pageNumber,
      pageSize: result.pageSize ?? result.PageSize ?? pagination.pageSize,
      totalCount: result.totalCount ?? result.TotalCount ?? items.length,
      totalPages: result.totalPages ?? result.TotalPages ?? 1,
    } satisfies PagedResult<CommentTopic>
  },

  async getDefaultTopicId(locale: Locale): Promise<string | null> {
    const result = await this.search(locale, { pageNumber: 1, pageSize: 1 })
    return result.items[0]?.id ?? null
  },
}
