import type { DashboardSection } from '@/models/dashboard/dashboard.model'
import type { ComponentType } from 'react'
import {
  AccountIcon,
  AddressesIcon,
  AssistantFaqIcon,
  MarketingPromoIcon,
  NewsletterSubscriberIcon,
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
  GuideBookIcon,
  RoomTypeIcon,
  CouponsIcon,
  DiscountCodesIcon,
  CartIcon,
  OrdersIcon,
  ProductsIcon,
  PropertyCategoriesIcon,
  PropertiesIcon,
  PropertyItemsIcon,
  ProfileCompletionIcon,
  AdminProfileIcon,
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
  /** Admin view permission code (PermissionType enum name). */
  permission?: string
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
      { section: 'cart', path: '/account/dashboard/cart', icon: CartIcon },
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
      { section: 'users', path: '/account/dashboard/users', icon: UsersIcon, permission: 'ListUser' },
      { section: 'roles', path: '/account/dashboard/roles', icon: RolesIcon, permission: 'ListRole' },
    ],
  },
  {
    id: 'catalog',
    icon: ProductsIcon,
    items: [
      {
        section: 'categories',
        path: '/account/dashboard/categories',
        icon: CategoriesIcon,
        permission: 'ListCategory',
      },
      {
        section: 'subCategories',
        path: '/account/dashboard/sub-categories',
        icon: SubCategoriesIcon,
        permission: 'ListSubCategory',
      },
      {
        section: 'products',
        path: '/account/dashboard/products',
        icon: ProductsIcon,
        matchPrefix: true,
        permission: 'ListProduct',
      },
      {
        section: 'productImages',
        path: '/account/dashboard/product-images',
        icon: ProductsIcon,
        permission: 'ListProductFile',
      },
      {
        section: 'discountCodes',
        path: '/account/dashboard/discount-codes',
        icon: DiscountCodesIcon,
        permission: 'ListDiscount',
      },
      {
        section: 'roomTypes',
        path: '/account/dashboard/room-types',
        icon: RoomTypeIcon,
        permission: 'ListRoomType',
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
        permission: 'ListPropertyCategory',
      },
      {
        section: 'properties',
        path: '/account/dashboard/properties',
        icon: PropertiesIcon,
        permission: 'ListProperty',
      },
      {
        section: 'propertyItems',
        path: '/account/dashboard/property-items',
        icon: PropertyItemsIcon,
        permission: 'ListPropertyItem',
      },
    ],
  },
  {
    id: 'blog',
    icon: BlogPostsIcon,
    items: [
      {
        section: 'blogCategories',
        path: '/account/dashboard/blog-categories',
        icon: BlogCategoriesIcon,
        permission: 'ListBlogPostCategory',
      },
      {
        section: 'blogTags',
        path: '/account/dashboard/blog-tags',
        icon: BlogTagsIcon,
        permission: 'ListTag',
      },
      {
        section: 'blogPosts',
        path: '/account/dashboard/blog-posts',
        icon: BlogPostsIcon,
        matchPrefix: true,
        permission: 'ListBlogPost',
      },
      {
        section: 'blogPostTags',
        path: '/account/dashboard/blog-post-tags',
        icon: BlogPostTagsIcon,
        permission: 'ListBlogPostTag',
      },
      {
        section: 'blogPostComments',
        path: '/account/dashboard/blog-post-comments',
        icon: BlogCommentsIcon,
        permission: 'ListBlogPostComment',
      },
    ],
  },
  {
    id: 'cms',
    icon: CmsPagesIcon,
    items: [
      {
        section: 'siteGuide',
        path: '/account/dashboard/site-guide',
        icon: GuideBookIcon,
        permission: 'ListPage',
      },
      {
        section: 'cmsPages',
        path: '/account/dashboard/cms-pages',
        icon: CmsPagesIcon,
        permission: 'ListPage',
      },
      {
        section: 'cmsSectionTypes',
        path: '/account/dashboard/cms-section-types',
        icon: CmsSectionTypesIcon,
        permission: 'ListSectionType',
      },
      {
        section: 'cmsSections',
        path: '/account/dashboard/cms-sections',
        icon: CmsSectionsIcon,
        permission: 'ListSection',
      },
      {
        section: 'cmsSectionItems',
        path: '/account/dashboard/cms-section-items',
        icon: CmsSectionItemsIcon,
        permission: 'ListSectionItem',
      },
      {
        section: 'cmsPageSections',
        path: '/account/dashboard/cms-page-sections',
        icon: CmsPageSectionsIcon,
        permission: 'ListPageSection',
      },
      {
        section: 'marketingPromos',
        path: '/account/dashboard/marketing-promos',
        icon: MarketingPromoIcon,
        permission: 'ListMarketingPromo',
      },
      {
        section: 'newsletterSubscribers',
        path: '/account/dashboard/newsletter-subscribers',
        icon: NewsletterSubscriberIcon,
        permission: 'ListMarketingPromo',
      },
      {
        section: 'assistantFaq',
        path: '/account/dashboard/assistant-faq',
        icon: AssistantFaqIcon,
        permission: 'ListAssistantFaq',
      },
      {
        section: 'profileQuestions',
        path: '/account/dashboard/profile-questions',
        icon: AdminProfileIcon,
        permission: 'GetProfileCompletionConfig',
      },
      {
        section: 'profileCompletionAnswers',
        path: '/account/dashboard/profile-completion-answers',
        icon: ProfileCompletionIcon,
        permission: 'ListProfileCompletionUserState',
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
