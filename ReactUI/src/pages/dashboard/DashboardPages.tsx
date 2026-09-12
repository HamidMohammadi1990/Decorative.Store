import type { ReactNode } from 'react'
import { useOutletContext } from 'react-router-dom'
import type { DashboardData } from '@/models/dashboard/dashboard.model'
import { DashboardAdminGate } from '@/components/dashboard/DashboardAdminGate'
import { CartPanel } from '@/components/cart/CartPanel'
import { WalletPanel } from '@/components/dashboard/WalletPanel'
import { OrdersPanel } from '@/components/dashboard/OrdersPanel'
import { TransactionsPanel } from '@/components/dashboard/TransactionsPanel'
import { ReviewsPanel } from '@/components/dashboard/ReviewsPanel'
import { AddressesPanel } from '@/components/dashboard/AddressesPanel'
import { CouponsPanel } from '@/components/dashboard/CouponsPanel'
import { WishlistPanel } from '@/components/dashboard/WishlistPanel'
import { StoriesPanel } from '@/components/dashboard/StoriesPanel'
import { UserStoryCommentsPanel } from '@/components/dashboard/UserStoryCommentsPanel'
import { ProfileCompletionPanel } from '@/components/dashboard/ProfileCompletionPanel'
import { ProfileQuestionsAdminPage } from '@/components/dashboard/ProfileQuestionsAdminPage'
import { BlogPostCategoriesPanel } from '@/components/dashboard/BlogPostCategoriesPanel'
import { BlogPostCommentsPanel } from '@/components/dashboard/BlogPostCommentsPanel'
import { BlogPostFormPanel } from '@/components/dashboard/BlogPostFormPanel'
import { BlogPostTagsPanel } from '@/components/dashboard/BlogPostTagsPanel'
import { BlogPostsPanel } from '@/components/dashboard/BlogPostsPanel'
import { BlogTagsPanel } from '@/components/dashboard/BlogTagsPanel'
import { CategoriesPanel } from '@/components/dashboard/CategoriesPanel'
import { SubCategoriesPanel } from '@/components/dashboard/SubCategoriesPanel'
import { ProductsPanel } from '@/components/dashboard/ProductsPanel'
import { ProductCommentsPanel } from '@/components/dashboard/ProductCommentsPanel'
import { ProductDescriptionsPanel } from '@/components/dashboard/ProductDescriptionsPanel'
import { ProductFormPanel } from '@/components/dashboard/ProductFormPanel'
import { ProductImagesPanel } from '@/components/dashboard/ProductImagesPanel'
import { ProductPropertiesPanel } from '@/components/dashboard/ProductPropertiesPanel'
import { PropertiesPanel } from '@/components/dashboard/PropertiesPanel'
import { PropertyCategoriesPanel } from '@/components/dashboard/PropertyCategoriesPanel'
import { PropertyItemsPanel } from '@/components/dashboard/PropertyItemsPanel'
import { CmsPagesPanel } from '@/components/dashboard/CmsPagesPanel'
import { CmsSectionTypesPanel } from '@/components/dashboard/CmsSectionTypesPanel'
import { CmsSectionsPanel } from '@/components/dashboard/CmsSectionsPanel'
import { CmsSectionItemsPanel } from '@/components/dashboard/CmsSectionItemsPanel'
import { CmsPageSectionsPanel } from '@/components/dashboard/CmsPageSectionsPanel'
import { DiscountCodesPanel } from '@/components/dashboard/DiscountCodesPanel'
import { MarketingPromosPanel } from '@/components/dashboard/MarketingPromosPanel'
import { NewsletterSubscribersPanel } from '@/components/dashboard/NewsletterSubscribersPanel'
import { ProfileCompletionAnswersPanel } from '@/components/dashboard/ProfileCompletionAnswersPanel'
import { AssistantFaqPanel } from '@/components/dashboard/AssistantFaqPanel'
import { SiteManagementGuidePanel } from '@/components/dashboard/SiteManagementGuidePanel'
import { RoomTypesPanel } from '@/components/dashboard/RoomTypesPanel'
import { UsersPanel } from '@/components/dashboard/UsersPanel'
import { RolesPanel } from '@/components/dashboard/RolesPanel'

function useDashboardContext() {
  return useOutletContext<DashboardData>()
}

function AdminPage({ permission, children }: { permission: string; children: ReactNode }) {
  return <DashboardAdminGate permission={permission}>{children}</DashboardAdminGate>
}

export function DashboardWalletPage() {
  const data = useDashboardContext()
  return <WalletPanel wallet={data.wallet} recentTransactions={data.transactions} />
}

export function DashboardCartPage() {
  return <CartPanel embedded />
}

export function DashboardOrdersPage() {
  return <OrdersPanel />
}

export function DashboardTransactionsPage() {
  const data = useDashboardContext()
  return <TransactionsPanel transactions={data.transactions} />
}

export function DashboardReviewsPage() {
  return <ReviewsPanel />
}

export function DashboardProfilePage() {
  return <ProfileCompletionPanel />
}

export function DashboardProfileQuestionsPage() {
  return (
    <AdminPage permission="GetProfileCompletionConfig">
      <ProfileQuestionsAdminPage />
    </AdminPage>
  )
}

export function DashboardStoriesPage() {
  return <StoriesPanel />
}

export function DashboardUserStoryCommentsPage() {
  return (
    <AdminPage permission="ListUserStoryComment">
      <UserStoryCommentsPanel />
    </AdminPage>
  )
}

export function DashboardWishlistPage() {
  return <WishlistPanel />
}

export function DashboardAddressesPage() {
  return <AddressesPanel />
}

export function DashboardCouponsPage() {
  return <CouponsPanel />
}

export function DashboardCategoriesPage() {
  return (
    <AdminPage permission="ListCategory">
      <CategoriesPanel />
    </AdminPage>
  )
}

export function DashboardSubCategoriesPage() {
  return (
    <AdminPage permission="ListSubCategory">
      <SubCategoriesPanel />
    </AdminPage>
  )
}

export function DashboardProductsPage() {
  return (
    <AdminPage permission="ListProduct">
      <ProductsPanel />
    </AdminPage>
  )
}

export function DashboardDiscountCodesPage() {
  return (
    <AdminPage permission="ListDiscount">
      <DiscountCodesPanel />
    </AdminPage>
  )
}

export function DashboardProductCreatePage() {
  return (
    <AdminPage permission="ListProduct">
      <ProductFormPanel />
    </AdminPage>
  )
}

export function DashboardProductEditPage() {
  return (
    <AdminPage permission="ListProduct">
      <ProductFormPanel />
    </AdminPage>
  )
}

export function DashboardProductImagesPage() {
  return (
    <AdminPage permission="ListProductFile">
      <ProductImagesPanel />
    </AdminPage>
  )
}

export function DashboardProductDescriptionsPage() {
  return (
    <AdminPage permission="ListProductDescription">
      <ProductDescriptionsPanel />
    </AdminPage>
  )
}

export function DashboardProductCommentsPage() {
  return (
    <AdminPage permission="ListProductComment">
      <ProductCommentsPanel />
    </AdminPage>
  )
}

export function DashboardPropertiesPage() {
  return (
    <AdminPage permission="ListProperty">
      <PropertiesPanel />
    </AdminPage>
  )
}

export function DashboardPropertyCategoriesPage() {
  return (
    <AdminPage permission="ListPropertyCategory">
      <PropertyCategoriesPanel />
    </AdminPage>
  )
}

export function DashboardPropertyItemsPage() {
  return (
    <AdminPage permission="ListPropertyItem">
      <PropertyItemsPanel />
    </AdminPage>
  )
}

export function DashboardProductPropertiesPage() {
  return (
    <AdminPage permission="ListProductProperty">
      <ProductPropertiesPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostCategoriesPage() {
  return (
    <AdminPage permission="ListBlogPostCategory">
      <BlogPostCategoriesPanel />
    </AdminPage>
  )
}

export function DashboardBlogTagsPage() {
  return (
    <AdminPage permission="ListTag">
      <BlogTagsPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostsPage() {
  return (
    <AdminPage permission="ListBlogPost">
      <BlogPostsPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostCreatePage() {
  return (
    <AdminPage permission="ListBlogPost">
      <BlogPostFormPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostEditPage() {
  return (
    <AdminPage permission="ListBlogPost">
      <BlogPostFormPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostTagsPage() {
  return (
    <AdminPage permission="ListBlogPostTag">
      <BlogPostTagsPanel />
    </AdminPage>
  )
}

export function DashboardBlogPostCommentsPage() {
  return (
    <AdminPage permission="ListBlogPostComment">
      <BlogPostCommentsPanel />
    </AdminPage>
  )
}

export function DashboardCmsPagesPage() {
  return (
    <AdminPage permission="ListPage">
      <CmsPagesPanel />
    </AdminPage>
  )
}

export function DashboardCmsSectionTypesPage() {
  return (
    <AdminPage permission="ListSectionType">
      <CmsSectionTypesPanel />
    </AdminPage>
  )
}

export function DashboardCmsSectionsPage() {
  return (
    <AdminPage permission="ListSection">
      <CmsSectionsPanel />
    </AdminPage>
  )
}

export function DashboardCmsSectionItemsPage() {
  return (
    <AdminPage permission="ListSectionItem">
      <CmsSectionItemsPanel />
    </AdminPage>
  )
}

export function DashboardCmsPageSectionsPage() {
  return (
    <AdminPage permission="ListPageSection">
      <CmsPageSectionsPanel />
    </AdminPage>
  )
}

export function DashboardMarketingPromosPage() {
  return (
    <AdminPage permission="ListMarketingPromo">
      <MarketingPromosPanel />
    </AdminPage>
  )
}

export function DashboardNewsletterSubscribersPage() {
  return (
    <AdminPage permission="ListMarketingPromo">
      <NewsletterSubscribersPanel />
    </AdminPage>
  )
}

export function DashboardProfileCompletionAnswersPage() {
  return (
    <AdminPage permission="ListProfileCompletionUserState">
      <ProfileCompletionAnswersPanel />
    </AdminPage>
  )
}

export function DashboardAssistantFaqPage() {
  return (
    <AdminPage permission="ListAssistantFaq">
      <AssistantFaqPanel />
    </AdminPage>
  )
}

export function DashboardSiteGuidePage() {
  return (
    <AdminPage permission="ListPage">
      <SiteManagementGuidePanel />
    </AdminPage>
  )
}

export function DashboardRoomTypesPage() {
  return (
    <AdminPage permission="ListRoomType">
      <RoomTypesPanel />
    </AdminPage>
  )
}

export function DashboardUsersPage() {
  return (
    <AdminPage permission="ListUser">
      <UsersPanel />
    </AdminPage>
  )
}

export function DashboardRolesPage() {
  return (
    <AdminPage permission="ListRole">
      <RolesPanel />
    </AdminPage>
  )
}
