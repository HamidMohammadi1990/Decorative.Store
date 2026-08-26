import type { ReactNode } from 'react'
import { AdminPagination, type AdminPaginationProps } from '@/components/dashboard/admin/AdminPagination'
import { InlineLoading } from '@/components/ui/Spinner'

export interface AdminDataGridColumn<T> {
  id: string
  header: string
  cell: (row: T) => ReactNode
  className?: string
  headerClassName?: string
  align?: 'left' | 'center' | 'right'
}

export interface AdminDataGridProps<T> {
  columns: AdminDataGridColumn<T>[]
  rows: T[]
  rowKey: (row: T) => string
  loading?: boolean
  loadingLabel?: string
  pagination?: Omit<AdminPaginationProps, 'disabled'>
  emptyState?: ReactNode
  className?: string
}

function alignClass(align: AdminDataGridColumn<unknown>['align']) {
  if (align === 'center') return 'text-center'
  if (align === 'right') return 'text-end'
  return 'text-start'
}

export function AdminDataGrid<T>({
  columns,
  rows,
  rowKey,
  loading = false,
  loadingLabel,
  pagination,
  emptyState,
  className = '',
}: AdminDataGridProps<T>) {
  const showPagination = pagination && pagination.totalCount > 0

  return (
    <div
      className={`overflow-hidden rounded-sm border border-border bg-surface shadow-sm ring-1 ring-black/[0.02] ${className}`}
    >
      <div className="overflow-x-auto">
        <table className="w-full min-w-[32rem] border-collapse text-sm">
          <thead>
            <tr className="border-b border-border bg-surface-muted/50">
              {columns.map((column) => (
                <th
                  key={column.id}
                  scope="col"
                  className={`px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-text-muted sm:px-5 ${alignClass(column.align)} ${column.headerClassName ?? ''}`}
                >
                  {column.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-border/80">
            {loading && rows.length === 0 ? (
              <tr>
                <td colSpan={columns.length} className="px-4 py-16 text-center sm:px-5">
                  <InlineLoading label={loadingLabel} />
                </td>
              </tr>
            ) : rows.length === 0 ? (
              <tr>
                <td colSpan={columns.length} className="px-4 py-12 text-center sm:px-5">
                  {emptyState ?? (
                    <p className="text-sm text-text-muted">—</p>
                  )}
                </td>
              </tr>
            ) : (
              rows.map((row) => (
                <tr
                  key={rowKey(row)}
                  className="transition-colors hover:bg-surface-muted/40"
                >
                  {columns.map((column) => (
                    <td
                      key={column.id}
                      className={`px-4 py-3.5 align-middle sm:px-5 ${alignClass(column.align)} ${column.className ?? ''}`}
                    >
                      {column.cell(row)}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {loading && rows.length > 0 && (
        <div className="flex justify-center border-t border-border py-3">
          <InlineLoading label={loadingLabel} />
        </div>
      )}

      {showPagination && (
        <AdminPagination
          {...pagination}
          disabled={loading}
        />
      )}
    </div>
  )
}
