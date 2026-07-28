import type { ReactElement, ReactNode } from 'react'

interface IconProps {
  className?: string
}

const base =
  'shrink-0 text-text-muted'

function IconSvg({
  className = '',
  children,
}: IconProps & { children: ReactNode }) {
  return (
    <svg
      width="20"
      height="20"
      viewBox="0 0 20 20"
      fill="none"
      aria-hidden
      className={`${base} ${className}`}
    >
      {children}
    </svg>
  )
}

function NewIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M10 2.5v3M10 14.5v3M2.5 10h3M14.5 10h3M5.05 5.05l2.12 2.12M12.83 12.83l2.12 2.12M5.05 14.95l2.12-2.12M12.83 7.17l2.12-2.12"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
      <circle cx="10" cy="10" r="2.25" stroke="currentColor" strokeWidth="1.25" />
    </IconSvg>
  )
}

function SofaIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M3.5 11.5V9a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v2.5M3.5 11.5h13M4.5 11.5V14h11v-2.5M6 14v1.5M14 14v1.5"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function GardenIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M10 16.5V9M10 9c-2.5-3-5-3.5-5-6a5 5 0 0 0 5 3.5M10 9c2.5-3 5-3.5 5-6a5 5 0 0 1-5 3.5"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function RugIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <rect
        x="3"
        y="5"
        width="14"
        height="10"
        rx="1"
        stroke="currentColor"
        strokeWidth="1.25"
      />
      <path
        d="M6 8h8M6 11h5"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function BedIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M3 12.5V14h14v-1.5M3 12.5V9a2.5 2.5 0 0 1 2.5-2.5h9A2.5 2.5 0 0 1 17 9v3.5M5.5 9V7M14.5 9V7"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function BathIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M4 11h12v2.5a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V11ZM4 11c0-2.5 1.5-4 3-4h6c1.5 0 3 1.5 3 4"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path
        d="M7 7V5.5a1.5 1.5 0 0 1 3 0V7"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function LampIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M10 3.5v2M7.5 5.5h5l-1.25 4H8.75L7.5 5.5ZM8.75 9.5V11M11.25 9.5V11M7 11h6v1.5H7V11Z"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function VaseIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M8 6c0-1.5 1-2.5 2-2.5s2 1 2 2.5M7 6h6l-1 9.5H8L7 6Z"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path
        d="M9 3.5h2"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function FrameIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <rect
        x="4"
        y="4.5"
        width="12"
        height="11"
        rx="0.75"
        stroke="currentColor"
        strokeWidth="1.25"
      />
      <path
        d="M7 12.5l2-2.5 2 2 3-3.5"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function KitchenIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <ellipse
        cx="10"
        cy="12"
        rx="6.5"
        ry="2.25"
        stroke="currentColor"
        strokeWidth="1.25"
      />
      <path
        d="M6.5 12V8.5a3.5 3.5 0 0 1 7 0V12"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function GiftIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <rect
        x="4"
        y="8"
        width="12"
        height="8"
        rx="0.75"
        stroke="currentColor"
        strokeWidth="1.25"
      />
      <path
        d="M10 8V16M4 11h12M10 8c-1.5 0-2.5-1-2.5-2.25S8.5 4 10 4s2.5 1 2.5 2.75S11.5 8 10 8Z"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

export function JournalIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M5.5 4.5h9a1.5 1.5 0 0 1 1.5 1.5v10a1.5 1.5 0 0 1-1.5 1.5h-9A1.5 1.5 0 0 1 4 16V6a1.5 1.5 0 0 1 1.5-1.5Z"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinejoin="round"
      />
      <path
        d="M8 4.5V16M11.5 7.5h2M11.5 10.5h2"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function SaleIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M5.5 5.5 11 11M11 5.5 5.5 11"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
      <path
        d="M6 4.5h8l-1.5 11H7.5L6 4.5Z"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinejoin="round"
      />
      <circle cx="7.25" cy="7.25" r="0.75" fill="currentColor" />
      <circle cx="12.75" cy="12.75" r="0.75" fill="currentColor" />
    </IconSvg>
  )
}

function DefaultIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <circle cx="10" cy="10" r="6.5" stroke="currentColor" strokeWidth="1.25" />
      <path
        d="M10 7v6M7 10h6"
        stroke="currentColor"
        strokeWidth="1.25"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

const CATEGORY_ICON_MAP: Record<string, (props: IconProps) => ReactElement> = {
  new: NewIcon,
  furniture: SofaIcon,
  sofas: SofaIcon,
  garden: GardenIcon,
  rugs: RugIcon,
  'bed-linen': BedIcon,
  bath: BathIcon,
  lighting: LampIcon,
  decor: VaseIcon,
  art: FrameIcon,
  kitchen: KitchenIcon,
  gifts: GiftIcon,
  sale: SaleIcon,
}

interface NavCategoryIconProps extends IconProps {
  categoryId: string
}

export function NavCategoryIcon({ categoryId, className }: NavCategoryIconProps) {
  const Icon = CATEGORY_ICON_MAP[categoryId] ?? DefaultIcon
  return <Icon className={className} />
}
