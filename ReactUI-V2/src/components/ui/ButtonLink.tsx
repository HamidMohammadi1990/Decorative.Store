import type { AppLink } from '@/models/shared/link.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'

type ButtonLinkVariant = 'primary' | 'secondary' | 'ghost' | 'warm'
type ButtonLinkSize = 'sm' | 'md'

interface ButtonLinkProps {
  link: AppLink
  variant?: ButtonLinkVariant
  size?: ButtonLinkSize
  className?: string
}

const variantClasses: Record<ButtonLinkVariant, string> = {
  primary: 'bg-accent text-text-inverse hover:bg-accent-hover',
  secondary: 'border border-border-strong text-text hover:bg-surface-muted',
  ghost: 'text-text hover:bg-surface-muted',
  warm: 'bg-warm text-warm-text shadow-sm hover:bg-warm-hover',
}

const sizeClasses: Record<ButtonLinkSize, string> = {
  sm: 'px-3 py-1.5 text-xs',
  md: 'px-4 py-2 text-sm',
}

function isInternalHref(href: string) {
  return href.startsWith('/') && !href.startsWith('//')
}

function hasValidHref(href: string) {
  const trimmed = href.trim()
  return trimmed.length > 0 && trimmed !== '#'
}

export function ButtonLink({
  link,
  variant = 'primary',
  size = 'md',
  className = '',
}: ButtonLinkProps) {
  if (!hasValidHref(link.href) || !link.label.trim()) return null

  const classes = `inline-flex items-center justify-center rounded-sm font-medium transition-colors ${sizeClasses[size]} ${variantClasses[variant]} ${className}`

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
    <CatalogNavLink href={link.href} className={classes}>
      {link.label}
    </CatalogNavLink>
  )
}
