import {
  DASHBOARD_FLAT_NAV,
  DASHBOARD_NAV_GROUPS,
  DASHBOARD_PROFILE_NAV_ITEM,
  isNavItemActive,
  type DashboardNavGroup,
  type DashboardNavItem,
} from '@/config/dashboardNav'

/** Admin routes that are not listed in the sidebar but still require a view permission. */
export const DASHBOARD_EXTRA_ADMIN_ROUTES: Array<{
  path: string
  permission: string
  matchPrefix?: boolean
}> = [
  { path: '/account/dashboard/products/new', permission: 'ListProduct' },
  { path: '/account/dashboard/products/edit', permission: 'ListProduct' },
  { path: '/account/dashboard/product-descriptions', permission: 'ListProductDescription' },
  { path: '/account/dashboard/product-comments', permission: 'ListProductComment' },
  { path: '/account/dashboard/product-properties', permission: 'ListProductProperty' },
  { path: '/account/dashboard/blog-posts/new', permission: 'ListBlogPost' },
  { path: '/account/dashboard/blog-posts/edit', permission: 'ListBlogPost' },
  { path: '/account/dashboard/user-story-comments', permission: 'ListUserStoryComment' },
]

export function isAdminNavGroup(groupId: DashboardNavGroup['id']): boolean {
  return groupId !== 'account'
}

export function resolveDashboardRoutePermission(pathname: string): string | null {
  for (const route of DASHBOARD_EXTRA_ADMIN_ROUTES) {
    if (route.matchPrefix) {
      if (pathname === route.path || pathname.startsWith(`${route.path}/`)) {
        return route.permission
      }
    } else if (pathname === route.path) {
      return route.permission
    }
  }

  for (const item of DASHBOARD_FLAT_NAV) {
    if (!item.permission) continue
    if (isNavItemActive(pathname, item)) {
      return item.permission
    }
  }

  return null
}

export function filterDashboardNavGroups(
  groups: DashboardNavGroup[],
  hasPermission: (code: string | undefined) => boolean,
): DashboardNavGroup[] {
  return groups
    .map((group) => {
      if (!isAdminNavGroup(group.id)) return group

      return {
        ...group,
        items: group.items.filter((item) => hasPermission(item.permission)),
      }
    })
    .filter((group) => !isAdminNavGroup(group.id) || group.items.length > 0)
}

export function filterDashboardFlatNav(
  items: DashboardNavItem[],
  hasPermission: (code: string | undefined) => boolean,
): DashboardNavItem[] {
  return items.filter((item) => {
    if (!item.permission) return true
    return hasPermission(item.permission)
  })
}

export function filterDashboardNavSearchResults<T extends { item: DashboardNavItem }>(
  results: T[],
  hasPermission: (code: string | undefined) => boolean,
): T[] {
  return results.filter(({ item }) => hasPermission(item.permission))
}

export const DASHBOARD_FILTERABLE_FLAT_NAV = [
  DASHBOARD_PROFILE_NAV_ITEM,
  ...DASHBOARD_NAV_GROUPS.flatMap((group) => group.items),
]
