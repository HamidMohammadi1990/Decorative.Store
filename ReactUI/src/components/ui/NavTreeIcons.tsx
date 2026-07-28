/** Folder-style icon for nav group (column) headers */
export function NavGroupIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      width="16"
      height="16"
      viewBox="0 0 16 16"
      aria-hidden
      className={`shrink-0 text-text-muted ${className}`}
    >
      <path
        d="M2 4.5A1.5 1.5 0 0 1 3.5 3h2.2l1.3 1.5H12.5A1.5 1.5 0 0 1 14 6v6.5a1.5 1.5 0 0 1-1.5 1.5h-9A1.5 1.5 0 0 1 2 12.5v-8Z"
        stroke="currentColor"
        strokeWidth="1.2"
        fill="none"
        strokeLinejoin="round"
      />
    </svg>
  )
}

/** Small arrow for leaf links (respects RTL via CSS) */
export function NavLinkIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      width="14"
      height="14"
      viewBox="0 0 14 14"
      aria-hidden
      className={`shrink-0 text-text-muted rtl:rotate-180 ${className}`}
    >
      <path
        d="M5 3.5 9.5 7 5 10.5"
        stroke="currentColor"
        strokeWidth="1.2"
        fill="none"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}
