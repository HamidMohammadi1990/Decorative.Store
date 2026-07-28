import { Link } from 'react-router-dom'
import type { AppLink } from '@/models/shared/link.model'

interface TextLinkProps {
  link: AppLink
  className?: string
}

function isInternalHref(href: string) {
  return href.startsWith('/') && !href.startsWith('//')
}

export function TextLink({ link, className = '' }: TextLinkProps) {
  const classes = `transition-colors duration-200 hover:text-accent ${className}`

  if (link.external || !isInternalHref(link.href)) {
    return (
      <a
        href={link.href}
        className={classes}
        {...(link.external ? { target: '_blank', rel: 'noreferrer' } : {})}
      >
        {link.label}
      </a>
    )
  }

  return (
    <Link to={link.href} className={classes}>
      {link.label}
    </Link>
  )
}
