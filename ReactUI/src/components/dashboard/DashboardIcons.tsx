import type { ReactNode } from 'react'

interface DashboardIconProps {
  size?: number
  className?: string
}

function IconBase({ size = 20, className = '', children }: DashboardIconProps & { children: ReactNode }) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      aria-hidden
      className={className}
    >
      {children}
    </svg>
  )
}

export function WalletIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M3 7.5A2.5 2.5 0 0 1 5.5 5h13A2.5 2.5 0 0 1 21 7.5v9A2.5 2.5 0 0 1 18.5 19h-13A2.5 2.5 0 0 1 3 16.5v-9Z"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <path d="M17 12h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
      <circle cx="17.5" cy="12" r="1" fill="currentColor" />
    </IconBase>
  )
}

export function OrdersIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function TransactionsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 8h16M4 16h16M8 4v16M16 4v16"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function ReviewsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M12 3.5 14.2 8.2l5.3.5-4 3.5 1.2 5.2L12 15.2 7.3 17.4l1.2-5.2-4-3.5 5.3-.5L12 3.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function AddressesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M12 21s6-5.2 6-10a6 6 0 1 0-12 0c0 4.8 6 10 6 10Z"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <circle cx="12" cy="11" r="2" stroke="currentColor" strokeWidth="1.5" />
    </IconBase>
  )
}

export function CouponsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 8.5V6a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v2.5M4 15.5V18a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-2.5"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <path
        d="M9 6v12M15 6v12"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeDasharray="2 3"
      />
    </IconBase>
  )
}

export function CategoriesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 5.5h7v7H4v-7ZM13 5.5h7v7h-7v-7ZM4 13.5h7v7H4v-7ZM13 13.5h7v7h-7v-7Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function SubCategoriesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 6h6v6H5V6Zm8 0h6M13 10h6M13 14h6M5 16h6v4H5v-4Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function ProductsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 8.5 12 4l7 4.5v7L12 20l-7-4.5v-7Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M12 12v8M5 8.5l7 3.5 7-3.5" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round" />
    </IconBase>
  )
}

export function ProfileCompletionIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="12" cy="9" r="3.5" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M6 19c0-3.3 2.7-6 6-6s6 2.7 6 6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <path
        d="M17.5 6.5 19 5M19.5 8.5 21 8.5M17.5 10.5 19 12"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function AdminProfileIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <rect x="4" y="5" width="16" height="14" rx="2" stroke="currentColor" strokeWidth="1.5" />
      <path d="M8 9h8M8 12.5h5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
      <circle cx="17" cy="7" r="2.5" fill="currentColor" />
    </IconBase>
  )
}

export function StoriesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="12" cy="12" r="8.5" stroke="currentColor" strokeWidth="1.5" />
      <circle cx="12" cy="12" r="3" stroke="currentColor" strokeWidth="1.5" />
      <path d="M19.5 4.5 22 2M22 8h2.5M19.5 19.5 22 22" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function LogoutIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M10 7V6a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2h-6a2 2 0 0 1-2-2v-1"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <path
        d="M14 12H4m0 0 3-3M4 12l3 3"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}
