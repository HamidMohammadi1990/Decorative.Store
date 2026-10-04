import { useCallback, useEffect, useState } from 'react'
import type { AdminPagedResult } from '@/services/admin/adminCatalogNormalize'

interface UseAdminPagedListOptions<T> {
  /** Fetch a page; should call API with pagination body */
  fetchPage: (pageNumber: number, pageSize: number) => Promise<AdminPagedResult<T>>
  initialPageSize?: number
  enabled?: boolean
}

export function useAdminPagedList<T>({
  fetchPage,
  initialPageSize = 20,
  enabled = true,
}: UseAdminPagedListOptions<T>) {
  const [items, setItems] = useState<T[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<unknown>(null)
  const [pageNumber, setPageNumber] = useState(1)
  const [pageSize, setPageSize] = useState(initialPageSize)
  const [totalCount, setTotalCount] = useState(0)
  const [totalPages, setTotalPages] = useState(1)

  const load = useCallback(
    async (page: number, size: number) => {
      setLoading(true)
      setError(null)
      try {
        const result = await fetchPage(page, size)
        setItems(result.items)
        setTotalCount(result.totalCount)
        setPageNumber(result.pageNumber)
        setPageSize(result.pageSize)
        setTotalPages(result.totalPages)
      } catch (err) {
        setError(err)
        setItems([])
        setTotalCount(0)
        setTotalPages(1)
      } finally {
        setLoading(false)
      }
    },
    [fetchPage],
  )

  useEffect(() => {
    if (!enabled) {
      setLoading(false)
      return
    }
    void load(pageNumber, pageSize)
  }, [enabled, pageNumber, pageSize, load])

  const goToPage = useCallback(
    (page: number) => {
      const next = Math.max(1, page)
      setPageNumber((prev) => {
        if (next === prev) {
          void load(next, pageSize)
        }
        return next
      })
    },
    [load, pageSize],
  )

  const changePageSize = useCallback(
    (size: number) => {
      setPageSize(size)
      setPageNumber((prev) => {
        if (prev === 1) {
          void load(1, size)
        }
        return 1
      })
    },
    [load],
  )

  const reload = useCallback(() => {
    void load(pageNumber, pageSize)
  }, [load, pageNumber, pageSize])

  return {
    items,
    loading,
    error,
    pageNumber,
    pageSize,
    totalCount,
    totalPages,
    goToPage,
    changePageSize,
    reload,
    setError,
  }
}
