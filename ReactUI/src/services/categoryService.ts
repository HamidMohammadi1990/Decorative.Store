import type { CategoryTreeItem } from '@/models/catalog/categoryTree.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'

const CATEGORY_TREE_PATH = '/api/v1/category/tree'

export const categoryService = {
  async getTree(locale: Locale): Promise<CategoryTreeItem[]> {
    return apiGet<CategoryTreeItem[]>(CATEGORY_TREE_PATH, locale)
  },
}
