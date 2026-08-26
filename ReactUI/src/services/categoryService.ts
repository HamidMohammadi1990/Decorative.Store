import type { CategoryTreeItem } from '@/models/catalog/categoryTree.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet } from '@/services/api/apiClient'
import { readRecord } from '@/services/api/apiNormalize'

const CATEGORY_TREE_PATH = '/api/v1/category/tree'

function normalizeCategoryTree(data: unknown): CategoryTreeItem[] {
  if (Array.isArray(data)) return data as CategoryTreeItem[]

  const record = readRecord(data)
  if (!record) return []

  const nested = record.categories ?? record.Categories ?? record.items ?? record.Items
  if (Array.isArray(nested)) return nested as CategoryTreeItem[]

  return []
}

export const categoryService = {
  async getTree(locale: Locale): Promise<CategoryTreeItem[]> {
    const data = await apiGet<unknown>(CATEGORY_TREE_PATH, locale)
    return normalizeCategoryTree(data)
  },
}
