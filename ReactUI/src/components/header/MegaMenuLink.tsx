import { Link } from 'react-router-dom'
import type { AppLink } from '@/models/shared/link.model'
import { NavLinkIcon } from '@/components/ui/NavTreeIcons'

export const navSubmenuLinkClass =
  'group flex items-center gap-2.5 rounded-md border-s-2 border-transparent py-2 pe-3 ps-2.5 text-sm font-normal leading-snug text-text-muted transition-all duration-200 hover:border-warm hover:bg-warm-soft hover:ps-3.5 hover:text-warm focus-visible:outline-2 focus-visible:outline-offset-1 focus-visible:outline-warm'

interface MegaMenuLinkProps {
  link: AppLink
  className?: string
  onNavigate?: () => void
}

function isInternalHref(href: string) {
  return href.startsWith('/') && !href.startsWith('//')
}

export function MegaMenuLink({ link, className = '', onNavigate }: MegaMenuLinkProps) {
  const classes = `${navSubmenuLinkClass} ${className}`
  const icon = (
    <NavLinkIcon className="shrink-0 text-border-strong transition-all duration-200 group-hover:translate-x-0.5 group-hover:text-warm rtl:group-hover:-translate-x-0.5" />
  )

  if (link.external || !isInternalHref(link.href)) {
    return (
      <a
        href={link.href}
        className={classes}
        onClick={onNavigate}
        {...(link.external ? { target: '_blank', rel: 'noreferrer' } : {})}
      >
        {icon}
        <span>{link.label}</span>
      </a>
    )
  }

  return (
    <Link to={link.href} className={classes} onClick={onNavigate}>
      {icon}
      <span>{link.label}</span>
    </Link>
  )
}

export const navShopAllLinkClass =
  'inline-flex items-center gap-2 rounded-md px-3 py-2 text-sm font-semibold text-warm transition-all duration-200 hover:bg-warm hover:text-warm-text focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-warm'
