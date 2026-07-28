export type CompareRowType = 'text' | 'list' | 'price' | 'boolean'

export interface CompareRow {
  id: string
  label: string
  type: CompareRowType
  values: (string | string[] | boolean | number | null)[]
  highlightDiff?: boolean
}

export const MAX_COMPARE_PRODUCTS = 4
