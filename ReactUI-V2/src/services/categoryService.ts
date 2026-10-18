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

/** Dedupe concurrent requests only — do not cache the tree (admin may toggle IsActive). */
const treeRequests = new Map<Locale, Promise<CategoryTreeItem[]>>()

export const categoryService = {
  async getTree(locale: Locale, force = false): Promise<CategoryTreeItem[]> {
    if (force) {
      treeRequests.delete(locale)
    } else {
      const inFlight = treeRequests.get(locale)
      if (inFlight) return inFlight
    }

    const request = apiGet<unknown>(CATEGORY_TREE_PATH, locale).then(normalizeCategoryTree)
    treeRequests.set(locale, request)

    try {
      return await request
    } catch (error) {
      treeRequests.delete(locale)
      throw error
    } finally {
      treeRequests.delete(locale)
    }
  },
}
