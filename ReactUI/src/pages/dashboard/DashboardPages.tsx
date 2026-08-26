import { useOutletContext } from 'react-router-dom'
import type { DashboardData } from '@/models/dashboard/dashboard.model'
import { WalletPanel } from '@/components/dashboard/WalletPanel'
import { OrdersPanel } from '@/components/dashboard/OrdersPanel'
import { TransactionsPanel } from '@/components/dashboard/TransactionsPanel'
import { ReviewsPanel } from '@/components/dashboard/ReviewsPanel'
import { AddressesPanel } from '@/components/dashboard/AddressesPanel'
import { CouponsPanel } from '@/components/dashboard/CouponsPanel'
import { WishlistPanel } from '@/components/dashboard/WishlistPanel'
import { StoriesPanel } from '@/components/dashboard/StoriesPanel'
import { ProfileCompletionPanel } from '@/components/dashboard/ProfileCompletionPanel'
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

function useDashboardContext() {
  return useOutletContext<DashboardData>()
}

export function DashboardWalletPage() {
  const data = useDashboardContext()
  return <WalletPanel wallet={data.wallet} recentTransactions={data.transactions} />
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

export function DashboardStoriesPage() {
  return <StoriesPanel />
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
  return <CategoriesPanel />
}

export function DashboardSubCategoriesPage() {
  return <SubCategoriesPanel />
}

export function DashboardProductsPage() {
  return <ProductsPanel />
}

export function DashboardProductCreatePage() {
  return <ProductFormPanel />
}

export function DashboardProductEditPage() {
  return <ProductFormPanel />
}

export function DashboardProductImagesPage() {
  return <ProductImagesPanel />
}

export function DashboardProductDescriptionsPage() {
  return <ProductDescriptionsPanel />
}

export function DashboardProductCommentsPage() {
  return <ProductCommentsPanel />
}

export function DashboardPropertiesPage() {
  return <PropertiesPanel />
}

export function DashboardPropertyCategoriesPage() {
  return <PropertyCategoriesPanel />
}

export function DashboardPropertyItemsPage() {
  return <PropertyItemsPanel />
}

export function DashboardProductPropertiesPage() {
  return <ProductPropertiesPanel />
}

export function DashboardBlogPostCategoriesPage() {
  return <BlogPostCategoriesPanel />
}

export function DashboardBlogTagsPage() {
  return <BlogTagsPanel />
}

export function DashboardBlogPostsPage() {
  return <BlogPostsPanel />
}

export function DashboardBlogPostCreatePage() {
  return <BlogPostFormPanel />
}

export function DashboardBlogPostEditPage() {
  return <BlogPostFormPanel />
}

export function DashboardBlogPostTagsPage() {
  return <BlogPostTagsPanel />
}

export function DashboardBlogPostCommentsPage() {
  return <BlogPostCommentsPanel />
}

export function DashboardCmsPagesPage() {
  return <CmsPagesPanel />
}

export function DashboardCmsSectionTypesPage() {
  return <CmsSectionTypesPanel />
}

export function DashboardCmsSectionsPage() {
  return <CmsSectionsPanel />
}

export function DashboardCmsSectionItemsPage() {
  return <CmsSectionItemsPanel />
}

export function DashboardCmsPageSectionsPage() {
  return <CmsPageSectionsPanel />
}
