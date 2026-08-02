import type {
  CreateProductCommentInput,
  ProductComment,
} from '@/models/catalog/productComment.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'

const PRODUCT_COMMENT_SEARCH_PATH = '/api/v1/product-comment/search'
const PRODUCT_COMMENT_CREATE_PATH = '/api/v1/product-comment/create'

export const productCommentService = {
  async search(
    productId: string,
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ) {
    return apiPost<PagedResult<ProductComment>>(
      PRODUCT_COMMENT_SEARCH_PATH,
      { productId, pagination },
      { locale },
    )
  },

  async create(
    input: CreateProductCommentInput,
    locale: Locale,
    accessToken: string,
  ) {
    return apiPost<{ id: string }>(PRODUCT_COMMENT_CREATE_PATH, input, {
      locale,
      accessToken,
    })
  },
}
