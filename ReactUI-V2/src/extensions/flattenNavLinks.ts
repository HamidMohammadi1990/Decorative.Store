import type { AppLink } from '@/models/shared/link.model'
import type { NavLinkGroup } from '@/models/shared/link.model'

/** All links under a nav group (columns + children) for mobile/search */
export function flattenNavLinks(group: NavLinkGroup): AppLink[] {
  const links: AppLink[] = []

  if (group.columns) {
    for (const col of group.columns) {
      links.push(...col.links)
    }
  }

  if (group.children) {
    links.push(...group.children)
  }

  return links
}

export function navGroupHasPanel(group: NavLinkGroup): boolean {
  return Boolean(
    (group.columns && group.columns.length > 0) ||
      (group.children && group.children.length > 0),
  )
}

export const JOURNAL_NAV_ID = 'blog'

/** Journal lives outside shop category navigation. */
export function splitJournalNav(items: NavLinkGroup[]) {
  const journalIndex = items.findIndex((item) => item.id === JOURNAL_NAV_ID)
  if (journalIndex === -1) {
    return { journal: null as NavLinkGroup | null, categories: items }
  }

  const journal = items[journalIndex]
  const categories = [
    ...items.slice(0, journalIndex),
    ...items.slice(journalIndex + 1),
  ]

  return { journal, categories }
}
