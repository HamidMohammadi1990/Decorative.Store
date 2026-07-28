import { useCallback } from 'react'
import { useSearchParams } from 'react-router-dom'
import { CHECKBOX_FILTER_KEYS } from '@/extensions/listingFilters'

export function useListingFilters() {
  const [searchParams, setSearchParams] = useSearchParams()

  const toggleFilter = useCallback(
    (facetId: string, value: string) => {
      setSearchParams((prev) => {
        const next = new URLSearchParams(prev)
        const current = next.getAll(facetId)
        next.delete(facetId)

        if (facetId === 'price') {
          next.delete('minPrice')
          next.delete('maxPrice')
        }

        if (current.includes(value)) {
          current.filter((v) => v !== value).forEach((v) => next.append(facetId, v))
        } else {
          ;[...current, value].forEach((v) => next.append(facetId, v))
        }
        next.delete('page')
        return next
      })
    },
    [setSearchParams],
  )

  const applyPriceRange = useCallback(
    (min?: number, max?: number) => {
      setSearchParams((prev) => {
        const next = new URLSearchParams(prev)
        next.delete('price')

        if (min == null || max == null) {
          next.delete('minPrice')
          next.delete('maxPrice')
        } else {
          next.set('minPrice', String(min))
          next.set('maxPrice', String(max))
        }
        next.delete('page')
        return next
      })
    },
    [setSearchParams],
  )

  const clearFilters = useCallback(() => {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev)
      for (const key of CHECKBOX_FILTER_KEYS) {
        next.delete(key)
      }
      next.delete('minPrice')
      next.delete('maxPrice')
      next.delete('page')
      return next
    })
  }, [setSearchParams])

  const setSort = useCallback(
    (sort: string) => {
      setSearchParams((prev) => {
        const next = new URLSearchParams(prev)
        if (sort === 'featured') next.delete('sort')
        else next.set('sort', sort)
        next.delete('page')
        return next
      })
    },
    [setSearchParams],
  )

  const currentSort = searchParams.get('sort') || 'featured'

  return {
    toggleFilter,
    applyPriceRange,
    clearFilters,
    setSort,
    currentSort,
    searchParams,
  }
}
