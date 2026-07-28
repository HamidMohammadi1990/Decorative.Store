interface ChevronIconProps {
  /** true = expanded (chevron points up) */
  expanded: boolean
  className?: string
}

export function ChevronIcon({ expanded, className = '' }: ChevronIconProps) {
  return (
    <svg
      width="14"
      height="14"
      viewBox="0 0 12 12"
      aria-hidden
      className={`transition-transform duration-200 ${expanded ? 'rotate-180' : ''} ${className}`}
    >
      <path
        d="M3 5l3 3 3-3"
        stroke="currentColor"
        strokeWidth="1.2"
        fill="none"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}
