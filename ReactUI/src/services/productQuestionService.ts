import type {
  CreateProductQuestionInput,
  ProductQuestion,
} from '@/models/catalog/productQuestion.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'

const PRODUCT_QUESTION_SEARCH_PATH = '/api/v1/product-question/search'
const PRODUCT_QUESTION_CREATE_PATH = '/api/v1/product-question/create'

export const productQuestionService = {
  async search(
    productId: string,
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ) {
    return apiPost<PagedResult<ProductQuestion>>(
      PRODUCT_QUESTION_SEARCH_PATH,
      { productId, pagination },
      { locale },
    )
  },

  async create(
    input: CreateProductQuestionInput,
    locale: Locale,
    accessToken: string,
  ) {
    return apiPost<{ id: string }>(PRODUCT_QUESTION_CREATE_PATH, input, {
      locale,
      accessToken,
    })
  },
}
