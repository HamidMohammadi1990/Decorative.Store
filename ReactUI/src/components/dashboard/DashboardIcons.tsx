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

export function AccountIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="12" cy="9" r="3.5" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M6 19c0-3.3 2.7-6 6-6s6 2.7 6 6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function UsersIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="9" cy="9" r="2.5" stroke="currentColor" strokeWidth="1.5" />
      <circle cx="16.5" cy="10" r="2" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M5.5 18c0-2.2 1.6-4 3.5-4s3.5 1.8 3.5 4M14 18c0-1.7 1.1-3 2.5-3"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function RolesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M12 3.5 4.5 7v5c0 4.2 3.2 7.5 7.5 8.5 4.3-1 7.5-4.3 7.5-8.5V7L12 3.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M9.5 12.5 11 14l3.5-3.5"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function AssistantFaqIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 5.5h12v13H6V5.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M9 9.5h6M9 13h4"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <circle cx="12" cy="17" r="0.75" fill="currentColor" />
    </IconBase>
  )
}

export function MarketingPromoIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 8.5h14v9H5v-9Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M8.5 6.5h7l1.5 2h-10l1.5-2Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M9 12h6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function NewsletterSubscriberIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 7.5 12 13l8-5.5V17a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V7.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M20 7.5 12 13 4 7.5"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconBase>
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

export function CartIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M8 8V7a4 4 0 0 1 8 0v1"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <path
        d="M6 8h12l-1.25 12H7.25L6 8Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
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

export function ProductCommentsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M12 3.5 14.2 8.2l5.3.5-4 3.5 1.2 5.2L12 15.2 7.3 17.4l1.2-5.2-4-3.5 5.3-.5L12 3.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M16 5.5h4M18 3.5v4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function ProductQuestionsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="12" cy="12" r="8.5" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M9.2 9.4a2.8 2.8 0 0 1 4.9 2c0 1.5-1.6 1.9-1.6 3.1"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <circle cx="12" cy="16.2" r="0.8" fill="currentColor" />
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

export function DiscountCodesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 7.5h14v9H5v-9Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M8 7.5V6.5a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1v1M8 16.5v1a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-1"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <path
        d="M10.5 12h3"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
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

export function PropertiesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 6h16M4 12h10M4 18h16"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <circle cx="17" cy="12" r="2" stroke="currentColor" strokeWidth="1.5" />
    </IconBase>
  )
}

export function PropertyItemsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 5h12v4H6V5Zm0 7h8v4H6v-4Zm10 0h4v4h-4v-4Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function ProductPropertiesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 8.5 12 4l7 4.5v7L12 20l-7-4.5v-7Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M15.5 3.5 18 2M18.5 6H21M15.5 8.5 18 10"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function PropertyCategoriesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 5.5h7v7H4v-7Zm9 0h7v7h-7v-7ZM4 14.5h7v7H4v-7Zm9 0h7v4h-7v-4Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M16 18.5h4M18 16.5v4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function ProductDescriptionsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M7 4h10a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2Z"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <path
        d="M8 9h8M8 12.5h8M8 16h5"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function BlogCategoriesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 5.5h7v7H4v-7Zm9 0h7v7h-7v-7ZM4 14.5h7v7H4v-7Zm9 3.5h7M16.5 14.5v7"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function BlogTagsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 5.5h5.5L12 4l2.5 1.5H18v5.5L12 20l-6-4.5V5.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function BlogPostsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 4h12v16H6V4Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M9 8h6M9 12h6M9 16h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function BlogPostTagsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 5.5h5.5L12 4l2.5 1.5H18v5.5L12 20l-6-4.5V5.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M14 8h4M14 12h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function BlogCommentsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 6.5h14v9H9l-4 3.5V6.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M8 10h8M8 13.5h5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
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

export function RoomTypeIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <rect x="4" y="8" width="16" height="12" rx="1.5" stroke="currentColor" strokeWidth="1.5" />
      <path d="M4 12h16M10 8V5.5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1V8" stroke="currentColor" strokeWidth="1.5" />
      <path d="M8 16h8" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function GuideBookIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 5.5h9.5a2 2 0 0 1 2 2V19.5H8a2 2 0 0 1-2-2V5.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path
        d="M15.5 7.5H17a2 2 0 0 1 2 2V19.5H15.5V7.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M8 9h5M8 12.5h5M8 16h3.5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function CmsPagesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <rect x="5" y="4" width="14" height="16" rx="1.5" stroke="currentColor" strokeWidth="1.5" />
      <path d="M8 8h8M8 12h8M8 16h5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function CmsSectionTypesIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path d="M6 7h12M6 12h12M6 17h8" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function CmsSectionsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <rect x="4" y="5" width="16" height="5" rx="1" stroke="currentColor" strokeWidth="1.5" />
      <rect x="4" y="13" width="16" height="5" rx="1" stroke="currentColor" strokeWidth="1.5" />
    </IconBase>
  )
}

export function CmsSectionItemsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <circle cx="8" cy="8" r="2" stroke="currentColor" strokeWidth="1.5" />
      <circle cx="16" cy="8" r="2" stroke="currentColor" strokeWidth="1.5" />
      <circle cx="12" cy="16" r="2" stroke="currentColor" strokeWidth="1.5" />
    </IconBase>
  )
}

export function CmsPageSectionsIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path d="M5 6h14v12H5z" stroke="currentColor" strokeWidth="1.5" />
      <path d="M9 10h6M9 14h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function EditIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 20h4l10.5-10.5a1.8 1.8 0 0 0 0-2.5l-1.5-1.5a1.8 1.8 0 0 0-2.5 0L4 16v4Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <path d="M13.5 6.5 17.5 10.5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function DeleteIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4.5 7h15M9 7V5.5A1 1 0 0 1 10 4.5h4a1 1 0 0 1 1 1V7M8 7v11.5h8V7"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path d="M10 10.5v6M14 10.5v6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </IconBase>
  )
}

export function CheckIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M6 12.5 10 16.5 18 7.5"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function CommentIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M5 6.5A2.5 2.5 0 0 1 7.5 4h9A2.5 2.5 0 0 1 19 6.5v7A2.5 2.5 0 0 1 16.5 16H10l-4 3.5V6.5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </IconBase>
  )
}

export function EyeIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M3.5 12s3-5.5 8.5-5.5S20.5 12 20.5 12s-3 5.5-8.5 5.5S3.5 12 3.5 12Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
      <circle cx="12" cy="12" r="2.5" stroke="currentColor" strokeWidth="1.5" />
    </IconBase>
  )
}

export function EyeOffIcon(props: DashboardIconProps) {
  return (
    <IconBase {...props}>
      <path
        d="M4 4l16 16M9.5 9.7A2.5 2.5 0 0 0 12 14.5c.7 0 1.3-.3 1.8-.7M7.2 7.6C5.6 8.6 4.3 10 3.5 12c0 0 3 5.5 8.5 5.5 1.4 0 2.7-.4 3.8-1"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path
        d="M14.8 10.3C15.2 10.8 15.5 11.4 15.5 12c0 1.9-1.6 3.5-3.5 3.5-.6 0-1.2-.3-1.7-.7"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </IconBase>
  )
}

export function StarIcon({ filled = false, ...props }: DashboardIconProps & { filled?: boolean }) {
  return (
    <IconBase {...props}>
      <path
        d="M12 4.2 13.8 8.8 18.7 9.3 15 12.4 16 17.2 12 14.8 8 17.2 9 12.4 5.3 9.3 10.2 8.8 12 4.2Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
        fill={filled ? 'currentColor' : 'none'}
      />
    </IconBase>
  )
}
