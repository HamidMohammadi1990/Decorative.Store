import type { DashboardSection } from '@/models/dashboard/dashboard.model'
import type { ComponentType } from 'react'
import {
  AccountIcon,
  AddressesIcon,
  BlogCommentsIcon,
  BlogCategoriesIcon,
  BlogPostTagsIcon,
  BlogPostsIcon,
  BlogTagsIcon,
  CategoriesIcon,
  CmsPageSectionsIcon,
  CmsPagesIcon,
  CmsSectionItemsIcon,
  CmsSectionsIcon,
  CmsSectionTypesIcon,
  CouponsIcon,
  OrdersIcon,
  ProductsIcon,
  PropertyCategoriesIcon,
  PropertiesIcon,
  PropertyItemsIcon,
  ProfileCompletionIcon,
  ReviewsIcon,
  RolesIcon,
  StoriesIcon,
  SubCategoriesIcon,
  TransactionsIcon,
  UsersIcon,
  WalletIcon,
} from '@/components/dashboard/DashboardIcons'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'

export type DashboardNavIcon = ComponentType<{ size?: number; className?: string }>

export interface DashboardNavItem {
  section: DashboardSection | 'productImages'
  path: string
  icon: DashboardNavIcon
  /** Match nested routes (e.g. products/new) */
  matchPrefix?: boolean
}

export interface DashboardNavGroup {
  id: 'account' | 'users' | 'catalog' | 'properties' | 'blog' | 'cms'
  icon: DashboardNavIcon
  items: DashboardNavItem[]
}

export const DASHBOARD_PROFILE_NAV_ITEM: DashboardNavItem = {
  section: 'profile',
  path: '/account/dashboard/profile',
  icon: ProfileCompletionIcon,
}

export const DASHBOARD_NAV_GROUPS: DashboardNavGroup[] = [
  {
    id: 'account',
    icon: AccountIcon,
    items: [
      { section: 'wallet', path: '/account/dashboard/wallet', icon: WalletIcon },
      { section: 'orders', path: '/account/dashboard/orders', icon: OrdersIcon },
      { section: 'transactions', path: '/account/dashboard/transactions', icon: TransactionsIcon },
      { section: 'reviews', path: '/account/dashboard/reviews', icon: ReviewsIcon },
      { section: 'wishlist', path: '/account/dashboard/wishlist', icon: WishlistIcon },
      { section: 'stories', path: '/account/dashboard/stories', icon: StoriesIcon },
      { section: 'addresses', path: '/account/dashboard/addresses', icon: AddressesIcon },
      { section: 'coupons', path: '/account/dashboard/coupons', icon: CouponsIcon },
    ],
  },
  {
    id: 'users',
    icon: UsersIcon,
    items: [
      { section: 'users', path: '/account/dashboard/users', icon: UsersIcon },
      { section: 'roles', path: '/account/dashboard/roles', icon: RolesIcon },
    ],
  },
  {
    id: 'catalog',
    icon: ProductsIcon,
    items: [
      { section: 'categories', path: '/account/dashboard/categories', icon: CategoriesIcon },
      { section: 'subCategories', path: '/account/dashboard/sub-categories', icon: SubCategoriesIcon },
      {
        section: 'products',
        path: '/account/dashboard/products',
        icon: ProductsIcon,
        matchPrefix: true,
      },
    ],
  },
  {
    id: 'properties',
    icon: PropertiesIcon,
    items: [
      {
        section: 'propertyCategories',
        path: '/account/dashboard/property-categories',
        icon: PropertyCategoriesIcon,
      },
      { section: 'properties', path: '/account/dashboard/properties', icon: PropertiesIcon },
      { section: 'propertyItems', path: '/account/dashboard/property-items', icon: PropertyItemsIcon },
    ],
  },
  {
    id: 'blog',
    icon: BlogPostsIcon,
    items: [
      { section: 'blogCategories', path: '/account/dashboard/blog-categories', icon: BlogCategoriesIcon },
      { section: 'blogTags', path: '/account/dashboard/blog-tags', icon: BlogTagsIcon },
      {
        section: 'blogPosts',
        path: '/account/dashboard/blog-posts',
        icon: BlogPostsIcon,
        matchPrefix: true,
      },
      { section: 'blogPostTags', path: '/account/dashboard/blog-post-tags', icon: BlogPostTagsIcon },
      {
        section: 'blogPostComments',
        path: '/account/dashboard/blog-post-comments',
        icon: BlogCommentsIcon,
      },
    ],
  },
  {
    id: 'cms',
    icon: CmsPagesIcon,
    items: [
      { section: 'cmsPages', path: '/account/dashboard/cms-pages', icon: CmsPagesIcon },
      {
        section: 'cmsSectionTypes',
        path: '/account/dashboard/cms-section-types',
        icon: CmsSectionTypesIcon,
      },
      { section: 'cmsSections', path: '/account/dashboard/cms-sections', icon: CmsSectionsIcon },
      {
        section: 'cmsSectionItems',
        path: '/account/dashboard/cms-section-items',
        icon: CmsSectionItemsIcon,
      },
      {
        section: 'cmsPageSections',
        path: '/account/dashboard/cms-page-sections',
        icon: CmsPageSectionsIcon,
      },
    ],
  },
]

export const DASHBOARD_FLAT_NAV = [
  DASHBOARD_PROFILE_NAV_ITEM,
  ...DASHBOARD_NAV_GROUPS.flatMap((group) => group.items),
]

export function isNavItemActive(pathname: string, item: DashboardNavItem): boolean {
  if (item.matchPrefix) {
    return pathname === item.path || pathname.startsWith(`${item.path}/`)
  }
  return pathname === item.path
}

export function findActiveNavGroup(pathname: string): DashboardNavGroup['id'] | null {
  for (const group of DASHBOARD_NAV_GROUPS) {
    if (group.items.some((item) => isNavItemActive(pathname, item))) {
      return group.id
    }
  }
  return null
}
