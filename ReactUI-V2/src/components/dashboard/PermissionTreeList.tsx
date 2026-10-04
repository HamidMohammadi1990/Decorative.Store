import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminPermission } from '@/models/admin/permission.model'
import type { PermissionTreeNode } from '@/extensions/buildPermissionTree'
import {
  buildPermissionTree,
  collectDescendantIds,
  filterPermissionTree,
  findPermissionNode,
} from '@/extensions/buildPermissionTree'
import {
  getPermissionLevelPersianLabel,
  getPermissionPersianLabel,
} from '@/extensions/permissionPersianLabel'

function PermissionTitleDisplay({
  title,
  levelTypeTitle,
}: {
  title: string
  levelTypeTitle: string
}) {
  const { i18n } = useTranslation()
  const persianLabel = useMemo(() => getPermissionPersianLabel(title), [title])
  const persianLevel = useMemo(
    () => getPermissionLevelPersianLabel(levelTypeTitle),
    [levelTypeTitle],
  )
  const isFa = i18n.language === 'fa'
  const showPersianLabel = persianLabel !== title

  return (
    <span className="flex min-w-0 flex-wrap items-center gap-x-1.5 gap-y-0.5">
      <span className="min-w-0 truncate text-sm">
        <span className="font-medium text-text-muted" dir="ltr">
          {title}
        </span>
        {showPersianLabel ? (
          <>
            <span className="mx-1.5 text-text-muted/50" aria-hidden>
              ·
            </span>
            <span className="font-medium text-text">{persianLabel}</span>
          </>
        ) : null}
      </span>
      {levelTypeTitle ? (
        <span className="inline-flex shrink-0 rounded-sm bg-surface-muted px-1.5 py-0.5 text-[10px] font-medium text-text-muted">
          {isFa ? persianLevel : levelTypeTitle}
          {!isFa && persianLevel !== levelTypeTitle ? (
            <span className="ms-1 text-text-muted/80">· {persianLevel}</span>
          ) : null}
        </span>
      ) : null}
    </span>
  )
}

function PermissionTreeNodeRow({
  node,
  depth,
  selectedIds,
  disabled,
  collapsedIds,
  onToggleCollapse,
  onTogglePermission,
}: {
  node: PermissionTreeNode
  depth: number
  selectedIds: Set<string>
  disabled: boolean
  collapsedIds: Set<string>
  onToggleCollapse: (id: string) => void
  onTogglePermission: (id: string) => void
}) {
  const { t } = useTranslation()
  const hasChildren = node.children.length > 0
  const isCollapsed = collapsedIds.has(node.id)

  return (
    <li>
      <div
        className="flex items-start gap-2 rounded-sm border border-border bg-surface px-3 py-2.5 transition-colors hover:bg-surface-muted/30"
        style={{ marginInlineStart: `${depth * 1.25}rem` }}
      >
        {hasChildren ? (
          <button
            type="button"
            onClick={() => onToggleCollapse(node.id)}
            className="mt-0.5 flex size-5 shrink-0 items-center justify-center rounded-sm text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
            aria-expanded={!isCollapsed}
            aria-label={
              isCollapsed ? t('dashboard.roles.expandPermissionGroup') : t('dashboard.roles.collapsePermissionGroup')
            }
          >
            <span className={`text-xs transition-transform ${isCollapsed ? '' : 'rotate-90'}`}>
              ▶
            </span>
          </button>
        ) : (
          <span className="mt-0.5 size-5 shrink-0" aria-hidden />
        )}

        <label className="flex min-w-0 flex-1 cursor-pointer items-start gap-3">
          <input
            type="checkbox"
            checked={selectedIds.has(node.id)}
            onChange={() => onTogglePermission(node.id)}
            disabled={disabled}
            className="mt-0.5 size-4 shrink-0 rounded border-border text-warm focus:ring-warm"
          />
          <span className="min-w-0">
            <PermissionTitleDisplay title={node.title} levelTypeTitle={node.levelTypeTitle} />
            {node.url && (
              <span className="mt-0.5 block truncate text-xs text-text-muted/80" dir="ltr">
                {node.url}
              </span>
            )}
          </span>
        </label>
      </div>

      {hasChildren && !isCollapsed && (
        <ul className="mt-2 space-y-2">
          {node.children.map((child) => (
            <PermissionTreeNodeRow
              key={child.id}
              node={child}
              depth={depth + 1}
              selectedIds={selectedIds}
              disabled={disabled}
              collapsedIds={collapsedIds}
              onToggleCollapse={onToggleCollapse}
              onTogglePermission={onTogglePermission}
            />
          ))}
        </ul>
      )}
    </li>
  )
}

export function PermissionTreeList({
  permissions,
  searchQuery,
  selectedIds,
  disabled = false,
  onSelectedIdsChange,
}: {
  permissions: AdminPermission[]
  searchQuery: string
  selectedIds: Set<string>
  disabled?: boolean
  onSelectedIdsChange: (ids: Set<string>) => void
}) {
  const [collapsedIds, setCollapsedIds] = useState<Set<string>>(() => new Set())

  const fullTree = useMemo(() => buildPermissionTree(permissions), [permissions])
  const tree = useMemo(
    () => filterPermissionTree(fullTree, searchQuery),
    [fullTree, searchQuery],
  )

  const togglePermission = (id: string) => {
    const node = findPermissionNode(fullTree, id)
    if (!node) return

    const affectedIds = [node.id, ...collectDescendantIds(node)]
    const shouldSelect = !selectedIds.has(id)
    const next = new Set(selectedIds)

    for (const affectedId of affectedIds) {
      if (shouldSelect) {
        next.add(affectedId)
      } else {
        next.delete(affectedId)
      }
    }

    onSelectedIdsChange(next)
  }

  const toggleCollapse = (id: string) => {
    setCollapsedIds((prev) => {
      const next = new Set(prev)
      if (next.has(id)) {
        next.delete(id)
      } else {
        next.add(id)
      }
      return next
    })
  }

  if (tree.length === 0) return null

  return (
    <ul className="space-y-2">
      {tree.map((node) => (
        <PermissionTreeNodeRow
          key={node.id}
          node={node}
          depth={0}
          selectedIds={selectedIds}
          disabled={disabled}
          collapsedIds={collapsedIds}
          onToggleCollapse={toggleCollapse}
          onTogglePermission={togglePermission}
        />
      ))}
    </ul>
  )
}
