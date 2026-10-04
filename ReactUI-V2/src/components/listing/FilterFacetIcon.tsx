import type { ReactElement, ReactNode } from 'react'
import type { FilterFacetType } from '@/models/catalog/listing.model'

interface IconProps {
  className?: string
}

function IconSvg({ className = '', children }: IconProps & { children: ReactNode }) {
  return (
    <svg
      width="16"
      height="16"
      viewBox="0 0 24 24"
      fill="none"
      aria-hidden
      className={`shrink-0 ${className}`}
    >
      {children}
    </svg>
  )
}

function PriceIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7H14a3.5 3.5 0 0 1 0 7H6"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function ColorIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 3c-4 0-7 2.5-7 6.5 0 2.2 1.2 4.1 3 5.2V19a2 2 0 0 0 2 2h4a2 2 0 0 0 2-2v-4.3c1.8-1.1 3-3 3-5.2C19 5.5 16 3 12 3Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
      <circle cx="9" cy="10" r="1" fill="currentColor" />
      <circle cx="12" cy="8" r="1" fill="currentColor" />
      <circle cx="15" cy="10" r="1" fill="currentColor" />
    </IconSvg>
  )
}

function SizeIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M4 8V4h4M20 8V4h-4M4 16v4h4M20 16v4h-4"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path d="M9 12h6M12 9v6" stroke="currentColor" strokeWidth="1.75" strokeLinecap="round" />
    </IconSvg>
  )
}

function MaterialIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 3 4 7.5v9L12 21l8-4.5v-9L12 3Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
      <path d="M12 12 4 7.5M12 12l8-4.5M12 12v9" stroke="currentColor" strokeWidth="1.75" strokeLinejoin="round" />
    </IconSvg>
  )
}

function RoomIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M4 10.5 12 5l8 5.5V20a1 1 0 0 1-1 1h-5v-6H10v6H5a1 1 0 0 1-1-1v-9.5Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function InStockIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M21 8.5 12 3 3 8.5V20a1 1 0 0 0 1 1h16a1 1 0 0 0 1-1V8.5Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
      <path d="M9 14l2 2 4-4" stroke="currentColor" strokeWidth="1.75" strokeLinecap="round" strokeLinejoin="round" />
    </IconSvg>
  )
}

function OnSaleIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L3 13.41a2 2 0 0 1 0-2.82l7.17-7.17a2 2 0 0 1 2.83 0L20.59 10.6a2 2 0 0 1 0 2.81Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
      <path d="M9.5 9.5h.01M14.5 14.5h.01" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" />
    </IconSvg>
  )
}

function IsNewIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 3v3M12 18v3M3 12h3M18 12h3M5.6 5.6l2.1 2.1M16.3 16.3l2.1 2.1M5.6 18.4l2.1-2.1M16.3 7.7l2.1-2.1"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
      <circle cx="12" cy="12" r="3" stroke="currentColor" strokeWidth="1.75" />
    </IconSvg>
  )
}

function RatingIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 3.5 14.4 9H20l-4.6 3.4 1.8 5.6L12 15.8 6.8 18l1.8-5.6L4 9h5.6L12 3.5Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
    </IconSvg>
  )
}

function BrandIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <circle cx="12" cy="8" r="4" stroke="currentColor" strokeWidth="1.75" />
      <path
        d="M6 21v-1a6 6 0 0 1 12 0v1"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
    </IconSvg>
  )
}

function StyleIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M12 3l1.8 4.2L18 9l-4.2 1.8L12 15l-1.8-4.2L6 9l4.2-1.8L12 3Z"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinejoin="round"
      />
      <path d="M19 17l.8 2 2 .8-2 .8-.8 2-.8-2-2-.8 2-.8.8-2Z" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round" />
    </IconSvg>
  )
}

function PatternIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <rect x="4" y="4" width="7" height="7" rx="1.5" stroke="currentColor" strokeWidth="1.75" />
      <rect x="13" y="4" width="7" height="7" rx="1.5" stroke="currentColor" strokeWidth="1.75" />
      <rect x="4" y="13" width="7" height="7" rx="1.5" stroke="currentColor" strokeWidth="1.75" />
      <rect x="13" y="13" width="7" height="7" rx="1.5" stroke="currentColor" strokeWidth="1.75" />
    </IconSvg>
  )
}

function AttributeIcon({ className }: IconProps) {
  return (
    <IconSvg className={className}>
      <path
        d="M4 7h16M4 12h10M4 17h14"
        stroke="currentColor"
        strokeWidth="1.75"
        strokeLinecap="round"
      />
      <circle cx="18" cy="12" r="2" stroke="currentColor" strokeWidth="1.75" />
    </IconSvg>
  )
}

const FACET_ICON_MAP: Record<string, (props: IconProps) => ReactElement> = {
  price: PriceIcon,
  color: ColorIcon,
  size: SizeIcon,
  material: MaterialIcon,
  room: RoomIcon,
  inStock: InStockIcon,
  onSale: OnSaleIcon,
  isNew: IsNewIcon,
  minRating: RatingIcon,
  brand: BrandIcon,
  style: StyleIcon,
  pattern: PatternIcon,
  finish: StyleIcon,
  texture: MaterialIcon,
  shape: SizeIcon,
  weight: SizeIcon,
  collection: PatternIcon,
  category: PatternIcon,
  default: AttributeIcon,
}

function normalizeFacetKey(facetId: string) {
  return facetId.trim().toLowerCase().replace(/[_\s]+/g, '-')
}

export function resolveFilterFacetIconKey(facetId: string, facetType: FilterFacetType): string {
  const key = normalizeFacetKey(facetId)

  if (facetType === 'range' || key === 'price') return 'price'
  if (facetType === 'color' || key.includes('color') || key.includes('colour') || key === 'rang') {
    return 'color'
  }

  const aliases: Record<string, string> = {
    instock: 'inStock',
    'in-stock': 'inStock',
    availability: 'inStock',
    onsale: 'onSale',
    'on-sale': 'onSale',
    offers: 'onSale',
    isnew: 'isNew',
    'is-new': 'isNew',
    newness: 'isNew',
    minrating: 'minRating',
    rating: 'minRating',
    stars: 'minRating',
  }

  if (aliases[key]) return aliases[key]
  if (FACET_ICON_MAP[key]) return key

  if (key.includes('size') || key.includes('dimension') || key.includes('width') || key.includes('height')) {
    return 'size'
  }
  if (
    key.includes('material') ||
    key.includes('fabric') ||
    key.includes('wood') ||
    key.includes('metal') ||
    key.includes('texture')
  ) {
    return 'material'
  }
  if (key.includes('room') || key.includes('space') || key.includes('otagh')) return 'room'
  if (key.includes('brand') || key.includes('manufacturer')) return 'brand'
  if (key.includes('style') || key.includes('finish')) return 'style'
  if (key.includes('pattern') || key.includes('motif')) return 'pattern'
  if (key.includes('rating') || key.includes('star')) return 'minRating'
  if (key.includes('stock') || key.includes('avail')) return 'inStock'
  if (key.includes('sale') || key.includes('offer') || key.includes('discount')) return 'onSale'
  if (key.includes('new') || key.includes('arrival')) return 'isNew'
  if (key.includes('price') || key.includes('cost')) return 'price'

  return 'default'
}

interface FilterFacetIconProps extends IconProps {
  facetId: string
  facetType: FilterFacetType
}

export function FilterFacetIcon({ facetId, facetType, className }: FilterFacetIconProps) {
  const iconKey = resolveFilterFacetIconKey(facetId, facetType)
  const Icon = FACET_ICON_MAP[iconKey] ?? FACET_ICON_MAP.default
  return <Icon className={className} />
}
