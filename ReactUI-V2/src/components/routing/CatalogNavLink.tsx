import type { AnchorHTMLAttributes, ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { isSsrFullPageHref } from '@/extensions/catalogNavLink'

interface CatalogNavLinkProps
  extends Pick<
    AnchorHTMLAttributes<HTMLAnchorElement>,
    'className' | 'onClick' | 'draggable' | 'title'
  > {
  href: string
  children: ReactNode
}

/** Storefront content routes use `<a href>` for SSR (home, CMS, blog, catalog, product, search). */
export function CatalogNavLink({
  href,
  className,
  children,
  onClick,
  draggable,
  title,
}: CatalogNavLinkProps) {
  if (isSsrFullPageHref(href)) {
    return (
      <a href={href} className={className} onClick={onClick} draggable={draggable} title={title}>
        {children}
      </a>
    )
  }

  return (
    <Link
      to={href}
      className={className}
      prefetch="none"
      onClick={onClick}
      draggable={draggable}
      title={title}
    >
      {children}
    </Link>
  )
}
