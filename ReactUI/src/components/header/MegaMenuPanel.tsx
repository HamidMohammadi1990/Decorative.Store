import type { NavLinkGroup } from '@/models/shared/link.model'
import { MegaMenuLink } from '@/components/header/MegaMenuLink'

interface MegaMenuPanelProps {
  group: NavLinkGroup
}

export function MegaMenuPanel({ group }: MegaMenuPanelProps) {
  if (group.columns?.length) {
    const columnCount = group.columns.length

    return (
      <div
        className="grid gap-x-10 gap-y-2 px-8 py-6"
        style={{
          gridTemplateColumns: `repeat(${columnCount}, minmax(12.5rem, max-content))`,
        }}
      >
        {group.columns.map((col) => (
          <div key={col.title} className="min-w-0">
            <p className="mb-2.5 text-sm font-semibold text-text">
              {col.title}
            </p>
            <ul className="flex flex-col gap-1">
              {col.links.map((link) => (
                <li key={link.href}>
                  <MegaMenuLink link={link} />
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    )
  }

  if (group.children?.length) {
    return (
      <ul className="flex flex-col gap-1 px-8 py-5">
        {group.children.map((link) => (
          <li key={link.href}>
            <MegaMenuLink link={link} />
          </li>
        ))}
      </ul>
    )
  }

  return null
}
