import { lazy, type ComponentType } from 'react'

function lazyNamed<T extends Record<string, ComponentType>>(
  factory: () => Promise<T>,
  name: keyof T,
) {
  return lazy(() => factory().then((module) => ({ default: module[name] as ComponentType })))
}

const loadDashboardPages = () => import('@/pages/dashboard/DashboardPages')

// Dashboard pages (single async chunk)
export const DashboardWalletPage = lazyNamed(loadDashboardPages, 'DashboardWalletPage')
export const DashboardCartPage = lazyNamed(loadDashboardPages, 'DashboardCartPage')
export const DashboardOrdersPage = lazyNamed(loadDashboardPages, 'DashboardOrdersPage')
export const DashboardTransactionsPage = lazyNamed(loadDashboardPages, 'DashboardTransactionsPage')
export const DashboardReviewsPage = lazyNamed(loadDashboardPages, 'DashboardReviewsPage')
export const DashboardProfilePage = lazyNamed(loadDashboardPages, 'DashboardProfilePage')
export const DashboardProfileQuestionsPage = lazyNamed(loadDashboardPages, 'DashboardProfileQuestionsPage')
export const DashboardWishlistPage = lazyNamed(loadDashboardPages, 'DashboardWishlistPage')
export const DashboardStoriesPage = lazyNamed(loadDashboardPages, 'DashboardStoriesPage')
export const DashboardUserStoryCommentsPage = lazyNamed(loadDashboardPages, 'DashboardUserStoryCommentsPage')
export const DashboardAddressesPage = lazyNamed(loadDashboardPages, 'DashboardAddressesPage')
export const DashboardCouponsPage = lazyNamed(loadDashboardPages, 'DashboardCouponsPage')
export const DashboardCategoriesPage = lazyNamed(loadDashboardPages, 'DashboardCategoriesPage')
export const DashboardSubCategoriesPage = lazyNamed(loadDashboardPages, 'DashboardSubCategoriesPage')
export const DashboardProductsPage = lazyNamed(loadDashboardPages, 'DashboardProductsPage')
export const DashboardProductCreatePage = lazyNamed(loadDashboardPages, 'DashboardProductCreatePage')
export const DashboardProductEditPage = lazyNamed(loadDashboardPages, 'DashboardProductEditPage')
export const DashboardDiscountCodesPage = lazyNamed(loadDashboardPages, 'DashboardDiscountCodesPage')
export const DashboardRoomTypesPage = lazyNamed(loadDashboardPages, 'DashboardRoomTypesPage')
export const DashboardProductImagesPage = lazyNamed(loadDashboardPages, 'DashboardProductImagesPage')
export const DashboardProductDescriptionsPage = lazyNamed(loadDashboardPages, 'DashboardProductDescriptionsPage')
export const DashboardProductCommentsPage = lazyNamed(loadDashboardPages, 'DashboardProductCommentsPage')
export const DashboardPropertiesPage = lazyNamed(loadDashboardPages, 'DashboardPropertiesPage')
export const DashboardPropertyCategoriesPage = lazyNamed(loadDashboardPages, 'DashboardPropertyCategoriesPage')
export const DashboardPropertyItemsPage = lazyNamed(loadDashboardPages, 'DashboardPropertyItemsPage')
export const DashboardProductPropertiesPage = lazyNamed(loadDashboardPages, 'DashboardProductPropertiesPage')
export const DashboardBlogPostCategoriesPage = lazyNamed(loadDashboardPages, 'DashboardBlogPostCategoriesPage')
export const DashboardBlogTagsPage = lazyNamed(loadDashboardPages, 'DashboardBlogTagsPage')
export const DashboardBlogPostsPage = lazyNamed(loadDashboardPages, 'DashboardBlogPostsPage')
export const DashboardBlogPostCreatePage = lazyNamed(loadDashboardPages, 'DashboardBlogPostCreatePage')
export const DashboardBlogPostEditPage = lazyNamed(loadDashboardPages, 'DashboardBlogPostEditPage')
export const DashboardBlogPostTagsPage = lazyNamed(loadDashboardPages, 'DashboardBlogPostTagsPage')
export const DashboardBlogPostCommentsPage = lazyNamed(loadDashboardPages, 'DashboardBlogPostCommentsPage')
export const DashboardCmsPagesPage = lazyNamed(loadDashboardPages, 'DashboardCmsPagesPage')
export const DashboardCmsSectionTypesPage = lazyNamed(loadDashboardPages, 'DashboardCmsSectionTypesPage')
export const DashboardCmsSectionsPage = lazyNamed(loadDashboardPages, 'DashboardCmsSectionsPage')
export const DashboardCmsSectionItemsPage = lazyNamed(loadDashboardPages, 'DashboardCmsSectionItemsPage')
export const DashboardCmsPageSectionsPage = lazyNamed(loadDashboardPages, 'DashboardCmsPageSectionsPage')
export const DashboardSiteGuidePage = lazyNamed(loadDashboardPages, 'DashboardSiteGuidePage')
export const DashboardMarketingPromosPage = lazyNamed(loadDashboardPages, 'DashboardMarketingPromosPage')
export const DashboardNewsletterSubscribersPage = lazyNamed(
  loadDashboardPages,
  'DashboardNewsletterSubscribersPage',
)
export const DashboardAssistantFaqPage = lazyNamed(loadDashboardPages, 'DashboardAssistantFaqPage')
export const DashboardProfileCompletionAnswersPage = lazyNamed(
  loadDashboardPages,
  'DashboardProfileCompletionAnswersPage',
)
export const DashboardUsersPage = lazyNamed(loadDashboardPages, 'DashboardUsersPage')
export const DashboardRolesPage = lazyNamed(loadDashboardPages, 'DashboardRolesPage')

export async function preloadDashboardPages() {
  await loadDashboardPages()
}
