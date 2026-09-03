import {
  DASHBOARD_NAV_GROUPS,
  DASHBOARD_PROFILE_NAV_ITEM,
  type DashboardNavGroup,
  type DashboardNavItem,
} from '@/config/dashboardNav'
import en from '@/i18n/locales/en.json'
import fa from '@/i18n/locales/fa.json'

export interface DashboardNavSearchResult {
  item: DashboardNavItem
  group: DashboardNavGroup | null
}

/** Normalize query for Persian/English menu search. */
export function normalizeNavSearchText(text: string): string {
  return text
    .trim()
    .toLowerCase()
    .replace(/[\u064A\u0649]/g, '\u06CC')
    .replace(/\u0643/g, '\u06A9')
    .replace(/\u200c/g, ' ')
    .replace(/\s+/g, ' ')
}

function navItemLabel(locale: 'en' | 'fa', section: DashboardNavItem['section']): string {
  const labels = locale === 'en' ? en.dashboard.nav : fa.dashboard.nav
  return (labels as Record<string, string | undefined>)[section] ?? ''
}

function navGroupLabel(locale: 'en' | 'fa', groupId: DashboardNavGroup['id']): string {
  const labels = locale === 'en' ? en.dashboard.navGroups : fa.dashboard.navGroups
  return (labels as Record<string, string | undefined>)[groupId] ?? ''
}

function itemSearchTerms(item: DashboardNavItem): string[] {
  const pathTail = item.path.split('/').filter(Boolean).pop() ?? ''
  return [
    item.section,
    navItemLabel('en', item.section),
    navItemLabel('fa', item.section),
    pathTail,
    pathTail.replace(/-/g, ' '),
  ].filter(Boolean)
}

function groupSearchTerms(groupId: DashboardNavGroup['id']): string[] {
  return [groupId, navGroupLabel('en', groupId), navGroupLabel('fa', groupId)].filter(Boolean)
}

function termMatches(query: string, terms: string[]): boolean {
  return terms.some((term) => normalizeNavSearchText(term).includes(query))
}

export function searchDashboardNav(rawQuery: string): DashboardNavSearchResult[] {
  const query = normalizeNavSearchText(rawQuery)
  if (!query) return []

  const results: DashboardNavSearchResult[] = []
  const seen = new Set<string>()

  const push = (item: DashboardNavItem, group: DashboardNavGroup | null) => {
    if (seen.has(item.path)) return
    seen.add(item.path)
    results.push({ item, group })
  }

  if (termMatches(query, itemSearchTerms(DASHBOARD_PROFILE_NAV_ITEM))) {
    push(DASHBOARD_PROFILE_NAV_ITEM, null)
  }

  for (const group of DASHBOARD_NAV_GROUPS) {
    const groupMatches = termMatches(query, groupSearchTerms(group.id))

    for (const item of group.items) {
      if (groupMatches || termMatches(query, itemSearchTerms(item))) {
        push(item, group)
      }
    }
  }

  return results
}
