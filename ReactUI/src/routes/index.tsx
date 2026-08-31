import { createBrowserRouter, Navigate } from 'react-router-dom'
import { RequireAuth } from '@/components/auth/RequireAuth'
import { AuthLayout } from '@/layouts/AuthLayout'
import { DashboardLayout } from '@/layouts/DashboardLayout'
import { ShopLayout } from '@/layouts/ShopLayout'
import { AuthPage } from '@/pages/AuthPage'
import {
  DashboardAddressesPage,
  DashboardBlogPostCategoriesPage,
  DashboardBlogPostCommentsPage,
  DashboardBlogPostCreatePage,
  DashboardBlogPostEditPage,
  DashboardBlogPostTagsPage,
  DashboardBlogPostsPage,
  DashboardBlogTagsPage,
  DashboardCmsPageSectionsPage,
  DashboardCmsPagesPage,
  DashboardCmsSectionItemsPage,
  DashboardCmsSectionsPage,
  DashboardCmsSectionTypesPage,
  DashboardCategoriesPage,
  DashboardCouponsPage,
  DashboardCartPage,
  DashboardOrdersPage,
  DashboardProductCommentsPage,
  DashboardProductCreatePage,
  DashboardProductDescriptionsPage,
  DashboardProductEditPage,
  DashboardProductImagesPage,
  DashboardProductPropertiesPage,
  DashboardProductsPage,
  DashboardPropertiesPage,
  DashboardPropertyCategoriesPage,
  DashboardPropertyItemsPage,
  DashboardProfilePage,
  DashboardReviewsPage,
  DashboardStoriesPage,
  DashboardUserStoryCommentsPage,
  DashboardSubCategoriesPage,
  DashboardTransactionsPage,
  DashboardUsersPage,
  DashboardRolesPage,
  DashboardWalletPage,
  DashboardWishlistPage,
} from '@/pages/dashboard/DashboardPages'
import { CheckoutPage } from '@/pages/CheckoutPage'
import { CartPage } from '@/pages/CartPage'
import { HomePage } from '@/pages/HomePage'
import { BlogDetailPage } from '@/pages/BlogDetailPage'
import { BlogListingPage } from '@/pages/BlogListingPage'
import { ComparePage } from '@/pages/ComparePage'
import { ProductDetailPage } from '@/pages/ProductDetailPage'
import { NotFoundPage } from '@/pages/NotFoundPage'
import { ProductListingPage } from '@/pages/ProductListingPage'
import { SearchPage } from '@/pages/SearchPage'
import { RoomLayoutPage } from '@/pages/RoomLayoutPage'

export const router = createBrowserRouter([
  {
    element: <ShopLayout />,
    children: [
      {
        path: '/',
        element: <HomePage />,
      },
      {
        path: '/cart',
        element: (
          <RequireAuth>
            <CartPage />
          </RequireAuth>
        ),
      },
      {
        path: '/checkout',
        element: (
          <RequireAuth>
            <CheckoutPage />
          </RequireAuth>
        ),
      },
      {
        path: '/product/:slug',
        element: <ProductDetailPage />,
      },
      {
        path: '/compare',
        element: <ComparePage />,
      },
      {
        path: '/room-layout',
        element: <RoomLayoutPage />,
      },
      {
        path: '/blog',
        element: <BlogListingPage />,
      },
      {
        path: '/blog/category/:categorySlug',
        element: <BlogListingPage />,
      },
      {
        path: '/blog/:slug',
        element: <BlogDetailPage />,
      },
      {
        path: '/search',
        element: <SearchPage />,
      },
      {
        path: '*',
        element: <ProductListingPage />,
      },
    ],
  },
  {
    element: <AuthLayout />,
    children: [
      {
        path: '/account',
        element: <AuthPage />,
      },
      {
        path: '/account/dashboard',
        element: <DashboardLayout />,
        children: [
          { index: true, element: <Navigate to="wallet" replace /> },
          { path: 'wallet', element: <DashboardWalletPage /> },
          { path: 'cart', element: <DashboardCartPage /> },
          { path: 'profile', element: <DashboardProfilePage /> },
          { path: 'orders', element: <DashboardOrdersPage /> },
          { path: 'transactions', element: <DashboardTransactionsPage /> },
          { path: 'reviews', element: <DashboardReviewsPage /> },
          { path: 'wishlist', element: <DashboardWishlistPage /> },
          { path: 'stories', element: <DashboardStoriesPage /> },
          { path: 'user-story-comments', element: <DashboardUserStoryCommentsPage /> },
          { path: 'addresses', element: <DashboardAddressesPage /> },
          { path: 'coupons', element: <DashboardCouponsPage /> },
          { path: 'categories', element: <DashboardCategoriesPage /> },
          { path: 'sub-categories', element: <DashboardSubCategoriesPage /> },
          { path: 'products', element: <DashboardProductsPage /> },
          { path: 'products/new', element: <DashboardProductCreatePage /> },
          { path: 'products/edit', element: <DashboardProductEditPage /> },
          { path: 'product-images', element: <DashboardProductImagesPage /> },
          { path: 'product-descriptions', element: <DashboardProductDescriptionsPage /> },
          { path: 'product-comments', element: <DashboardProductCommentsPage /> },
          { path: 'properties', element: <DashboardPropertiesPage /> },
          { path: 'property-categories', element: <DashboardPropertyCategoriesPage /> },
          { path: 'property-items', element: <DashboardPropertyItemsPage /> },
          { path: 'product-properties', element: <DashboardProductPropertiesPage /> },
          { path: 'blog-categories', element: <DashboardBlogPostCategoriesPage /> },
          { path: 'blog-tags', element: <DashboardBlogTagsPage /> },
          { path: 'blog-posts', element: <DashboardBlogPostsPage /> },
          { path: 'blog-posts/new', element: <DashboardBlogPostCreatePage /> },
          { path: 'blog-posts/edit', element: <DashboardBlogPostEditPage /> },
          { path: 'blog-post-tags', element: <DashboardBlogPostTagsPage /> },
          { path: 'blog-post-comments', element: <DashboardBlogPostCommentsPage /> },
          { path: 'cms-pages', element: <DashboardCmsPagesPage /> },
          { path: 'cms-section-types', element: <DashboardCmsSectionTypesPage /> },
          { path: 'cms-sections', element: <DashboardCmsSectionsPage /> },
          { path: 'cms-section-items', element: <DashboardCmsSectionItemsPage /> },
          { path: 'cms-page-sections', element: <DashboardCmsPageSectionsPage /> },
          { path: 'users', element: <DashboardUsersPage /> },
          { path: 'roles', element: <DashboardRolesPage /> },
          { path: '*', element: <NotFoundPage /> },
        ],
      },
      { path: '/account/*', element: <NotFoundPage /> },
    ],
  },
])
