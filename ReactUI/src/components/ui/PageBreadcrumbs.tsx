import { Link } from 'react-router-dom'

export interface PageBreadcrumbItem {
  label: string
  href?: string
  home?: boolean
}

interface PageBreadcrumbsProps {
  items: PageBreadcrumbItem[]
  className?: string
}

function HomeIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 10.5 12 5l8 5.5V20a1 1 0 0 1-1 1h-5v-6H10v6H5a1 1 0 0 1-1-1v-9.5Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function SeparatorIcon() {
  return (
    <svg
      width="12"
      height="12"
      viewBox="0 0 24 24"
      fill="none"
      aria-hidden
      className="shrink-0 text-text-muted/45 rtl:rotate-180"
    >
      <path
        d="M10 6l6 6-6 6"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

export function PageBreadcrumbs({ items, className = '' }: PageBreadcrumbsProps) {
  if (items.length === 0) return null

  return (
    <nav
      aria-label="Breadcrumb"
      className={`rounded-xl border border-border/50 bg-surface-muted/25 px-3 py-2.5 ring-1 ring-black/[0.02] sm:px-4 sm:py-3 ${className}`}
    >
      <ol className="flex flex-wrap items-center gap-1 sm:gap-1.5">
        {items.map((item, index) => {
          const isLast = index === items.length - 1

          return (
            <li key={`${item.label}-${index}`} className="flex min-w-0 items-center gap-1 sm:gap-1.5">
              {index > 0 && (
                <SeparatorIcon />
              )}

              {isLast || !item.href ? (
                <span
                  className="max-w-[10rem] truncate rounded-full bg-warm-soft/55 px-2.5 py-1 text-[11px] font-semibold text-text sm:max-w-md sm:text-xs"
                  title={item.label}
                >
                  {item.label}
                </span>
              ) : item.home ? (
                <Link
                  to={item.href}
                  className="inline-flex items-center gap-1.5 rounded-full px-1.5 py-1 text-[11px] font-medium text-text-muted transition-colors hover:bg-surface/80 hover:text-warm sm:px-2 sm:text-xs"
                  title={item.label}
                >
                  <span className="flex size-6 shrink-0 items-center justify-center rounded-full bg-surface text-text-muted ring-1 ring-border/50">
                    <HomeIcon />
                  </span>
                  <span className="hidden sm:inline">{item.label}</span>
                </Link>
              ) : (
                <Link
                  to={item.href}
                  className="max-w-[8rem] truncate rounded-full px-2.5 py-1 text-[11px] font-medium text-text-muted transition-colors hover:bg-surface/80 hover:text-warm sm:max-w-[10rem] sm:text-xs"
                  title={item.label}
                >
                  {item.label}
                </Link>
              )}
            </li>
          )
        })}
      </ol>
    </nav>
  )
}
