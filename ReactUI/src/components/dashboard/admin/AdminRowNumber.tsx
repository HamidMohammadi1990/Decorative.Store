export const ADMIN_ROW_NUMBER_CLASS =
  'w-10 shrink-0 text-center text-xs font-medium tabular-nums text-text-muted'

export function getAdminRowNumber(
  index: number,
  pagination?: { pageNumber: number; pageSize: number },
) {
  const offset = pagination ? (pagination.pageNumber - 1) * pagination.pageSize : 0
  return offset + index + 1
}

export function AdminRowNumber({ value }: { value: number }) {
  return <span className={ADMIN_ROW_NUMBER_CLASS}>{value}</span>
}
