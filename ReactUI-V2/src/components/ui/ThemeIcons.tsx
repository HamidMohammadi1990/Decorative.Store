interface ThemeIconProps {
  size?: number
  className?: string
}

export function SunIcon({ size = 16, className = '' }: ThemeIconProps) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 16 16"
      fill="none"
      aria-hidden
      className={className}
    >
      <circle cx="8" cy="8" r="3" stroke="currentColor" strokeWidth="1.3" />
      <path
        d="M8 1.5v1.5M8 13v1.5M1.5 8H3M13 8h1.5M3.05 3.05l1.06 1.06M11.89 11.89l1.06 1.06M3.05 12.95l1.06-1.06M11.89 4.11l1.06-1.06"
        stroke="currentColor"
        strokeWidth="1.3"
        strokeLinecap="round"
      />
    </svg>
  )
}

export function MoonIcon({ size = 16, className = '' }: ThemeIconProps) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 16 16"
      fill="none"
      aria-hidden
      className={className}
    >
      <path
        d="M11.2 10.4A4.6 4.6 0 0 1 5.6 4.8 4.6 4.6 0 1 0 11.2 10.4Z"
        stroke="currentColor"
        strokeWidth="1.3"
        strokeLinejoin="round"
      />
    </svg>
  )
}

export function SystemThemeIcon({ size = 16, className = '' }: ThemeIconProps) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 16 16"
      fill="none"
      aria-hidden
      className={className}
    >
      <rect
        x="2"
        y="3"
        width="12"
        height="8.5"
        rx="1.2"
        stroke="currentColor"
        strokeWidth="1.3"
      />
      <path d="M5.5 13h5" stroke="currentColor" strokeWidth="1.3" strokeLinecap="round" />
    </svg>
  )
}
