import type { AdminPermission } from '@/models/admin/permission.model'

export interface PermissionTreeNode extends AdminPermission {
  children: PermissionTreeNode[]
}

function sortPermissionNodes(nodes: PermissionTreeNode[]) {
  nodes.sort((a, b) => a.priority - b.priority || a.title.localeCompare(b.title))
  for (const node of nodes) {
    sortPermissionNodes(node.children)
  }
}

export function buildPermissionTree(items: AdminPermission[]): PermissionTreeNode[] {
  const nodes = new Map<string, PermissionTreeNode>()

  for (const item of items) {
    nodes.set(item.id, { ...item, children: [] })
  }

  const roots: PermissionTreeNode[] = []

  for (const item of items) {
    const node = nodes.get(item.id)
    if (!node) continue

    if (item.parentId && nodes.has(item.parentId)) {
      nodes.get(item.parentId)!.children.push(node)
    } else {
      roots.push(node)
    }
  }

  sortPermissionNodes(roots)
  return roots
}

function permissionMatchesQuery(permission: AdminPermission, query: string) {
  return (
    permission.title.toLowerCase().includes(query) ||
    permission.url.toLowerCase().includes(query) ||
    (permission.nameSpace?.toLowerCase().includes(query) ?? false)
  )
}

export function filterPermissionTree(
  roots: PermissionTreeNode[],
  rawQuery: string,
): PermissionTreeNode[] {
  const query = rawQuery.trim().toLowerCase()
  if (!query) return roots

  const filterNode = (node: PermissionTreeNode): PermissionTreeNode | null => {
    const filteredChildren = node.children
      .map(filterNode)
      .filter((child): child is PermissionTreeNode => child !== null)

    if (permissionMatchesQuery(node, query) || filteredChildren.length > 0) {
      return { ...node, children: filteredChildren }
    }

    return null
  }

  return roots.map(filterNode).filter((node): node is PermissionTreeNode => node !== null)
}

export function collectDescendantIds(node: PermissionTreeNode): string[] {
  const ids: string[] = []
  for (const child of node.children) {
    ids.push(child.id, ...collectDescendantIds(child))
  }
  return ids
}

export function findPermissionNode(
  roots: PermissionTreeNode[],
  id: string,
): PermissionTreeNode | null {
  for (const root of roots) {
    if (root.id === id) return root
    const found = findPermissionNode(root.children, id)
    if (found) return found
  }
  return null
}
