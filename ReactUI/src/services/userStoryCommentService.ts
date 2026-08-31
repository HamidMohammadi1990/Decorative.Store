import type { StoryComment } from '@/models/stories/story.model'
import type { Locale } from '@/models/shared/locale.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import { apiPost } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import { readEncryptedId } from '@/services/admin/adminCatalogNormalize'

const SEARCH_PATH = '/api/v1/user-story-comment/search'
const CREATE_PATH = '/api/v1/user-story-comment/create'

function normalizeStoryComment(data: unknown): StoryComment | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const content = readStringField(record, 'content', 'Content')
  if (!id || !content) return null

  const first = readStringField(record, 'userFirstName', 'UserFirstName', 'createdByUserFirstName', 'CreatedByUserFirstName')
  const last = readStringField(record, 'userLastName', 'UserLastName', 'createdByUserLastName', 'CreatedByUserLastName')
  const createdOnUtc = readStringField(record, 'createdOnUtc', 'CreatedOnUtc')

  return {
    id,
    authorName: [first, last].filter(Boolean).join(' ').trim() || 'User',
    date: createdOnUtc || new Date().toISOString(),
    text: content,
    likes: 0,
  }
}

export const userStoryCommentService = {
  async search(
    locale: Locale,
    userStoryId: string,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ): Promise<PagedResult<StoryComment>> {
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
    }>(
      SEARCH_PATH,
      { userStoryId, pagination },
      { locale },
    )

    const items = (result.items ?? result.Items ?? [])
      .map(normalizeStoryComment)
      .filter((item): item is StoryComment => item !== null)

    return {
      items,
      pageNumber: result.pageNumber ?? result.PageNumber ?? pagination.pageNumber,
      pageSize: result.pageSize ?? result.PageSize ?? pagination.pageSize,
      totalCount: result.totalCount ?? result.TotalCount ?? items.length,
      totalPages: result.totalPages ?? result.TotalPages ?? 1,
    }
  },

  async create(
    userStoryId: string,
    content: string,
    locale: Locale,
    accessToken: string,
  ): Promise<void> {
    await apiPost(
      CREATE_PATH,
      { userStoryId, content },
      { locale, accessToken },
    )
  },
}
