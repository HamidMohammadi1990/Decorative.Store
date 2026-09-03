import { Navigate } from 'react-router-dom'
import type { RouteObject } from 'react-router-dom'
import { RequireAuth } from '@/components/auth/RequireAuth'
import { AuthLayout } from '@/layouts/AuthLayout'
import { DashboardLayout } from '@/layouts/DashboardLayout'
import { ShopLayout } from '@/layouts/ShopLayout'
import { CMS_CONTENT_PATH_TO_SLUG } from '@/extensions/cmsContentRoute'
import { aboutPageLoader } from '@/routes/loaders/aboutPageLoader'
import { blogDetailLoader } from '@/routes/loaders/blogDetailLoader'
import { blogListingLoader } from '@/routes/loaders/blogListingLoader'
import { cmsContentPageLoader } from '@/routes/loaders/cmsContentPageLoader'
import { comparePageLoader } from '@/routes/loaders/comparePageLoader'
import { contactPageLoader } from '@/routes/loaders/contactPageLoader'
import { homePageLoader } from '@/routes/loaders/homePageLoader'
import { productDetailLoader } from '@/routes/loaders/productDetailLoader'
import { productListingLoader } from '@/routes/loaders/productListingLoader'
import { roomLayoutLoader } from '@/routes/loaders/roomLayoutLoader'
import { searchPageLoader } from '@/routes/loaders/searchPageLoader'
import { shopLayoutLoader } from '@/routes/loaders/shopLayoutLoader'
import {
  AboutPage,
  AuthPage,
  BlogDetailPage,
  BlogListingPage,
  CartPage,
  CheckoutPage,
  CmsContentPage,
  ComparePage,
  ContactPage,
  HomePage,
  NotFoundPage,
  ProductDetailPage,
  ProductListingPage,
  RoomLayoutPage,
  SearchPage,
} from '@/routes/shopPages'
import {
  DashboardAddressesPage,
  DashboardAssistantFaqPage,
  DashboardBlogPostCategoriesPage,
  DashboardBlogPostCommentsPage,
  DashboardBlogPostCreatePage,
  DashboardBlogPostEditPage,
  DashboardBlogPostTagsPage,
  DashboardBlogPostsPage,
  DashboardBlogTagsPage,
  DashboardCartPage,
  DashboardCategoriesPage,
  DashboardCmsPageSectionsPage,
  DashboardCmsPagesPage,
  DashboardCmsSectionItemsPage,
  DashboardCmsSectionsPage,
  DashboardCmsSectionTypesPage,
  DashboardCouponsPage,
  DashboardDiscountCodesPage,
  DashboardMarketingPromosPage,
  DashboardNewsletterSubscribersPage,
  DashboardOrdersPage,
  DashboardProductCommentsPage,
  DashboardProductCreatePage,
  DashboardProductDescriptionsPage,
  DashboardProductEditPage,
  DashboardProductImagesPage,
  DashboardProductPropertiesPage,
  DashboardProductsPage,
  DashboardProfileCompletionAnswersPage,
  DashboardProfilePage,
  DashboardProfileQuestionsPage,
  DashboardPropertiesPage,
  DashboardPropertyCategoriesPage,
  DashboardPropertyItemsPage,
  DashboardReviewsPage,
  DashboardRolesPage,
  DashboardRoomTypesPage,
  DashboardSiteGuidePage,
  DashboardStoriesPage,
  DashboardSubCategoriesPage,
  DashboardTransactionsPage,
  DashboardUserStoryCommentsPage,
  DashboardUsersPage,
  DashboardWalletPage,
  DashboardWishlistPage,
} from '@/routes/lazyPages'

/** Shared route tree for client router and SSR static handler. */
export function createAppRoutes(): RouteObject[] {
  return [
    {
      id: 'shop',
      element: <ShopLayout />,
      loader: shopLayoutLoader,
      children: [
        { id: 'home', path: '/', element: <HomePage />, loader: homePageLoader },
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
          id: 'product-detail',
          path: '/product/:slug',
          element: <ProductDetailPage />,
          loader: productDetailLoader,
        },
        {
          id: 'compare',
          path: '/compare',
          element: <ComparePage />,
          loader: comparePageLoader,
        },
        {
          id: 'room-layout',
          path: '/room-layout',
          element: <RoomLayoutPage />,
          loader: roomLayoutLoader,
        },
        {
          id: 'blog-index',
          path: '/blog',
          element: <BlogListingPage />,
          loader: blogListingLoader,
        },
        {
          id: 'blog-category',
          path: '/blog/category/:categorySlug',
          element: <BlogListingPage />,
          loader: blogListingLoader,
        },
        {
          id: 'blog-detail',
          path: '/blog/:slug',
          element: <BlogDetailPage />,
          loader: blogDetailLoader,
        },
        {
          id: 'search',
          path: '/search',
          element: <SearchPage />,
          loader: searchPageLoader,
        },
        {
          id: 'about',
          path: '/about',
          element: <AboutPage />,
          loader: aboutPageLoader,
        },
        {
          id: 'contact',
          path: '/contact',
          element: <ContactPage />,
          loader: contactPageLoader,
        },
        ...Object.entries(CMS_CONTENT_PATH_TO_SLUG).map(([path, slug]) => ({
          id: `cms-${slug}`,
          path,
          element: <CmsContentPage slug={slug} />,
          loader: cmsContentPageLoader,
        })),
        {
          id: 'cms-dynamic',
          path: '/p/:slug',
          element: <CmsContentPage />,
          loader: cmsContentPageLoader,
        },
        {
          id: 'product-listing',
          path: '*',
          element: <ProductListingPage />,
          loader: productListingLoader,
        },
      ],
    },
    {
      element: <AuthLayout />,
      children: [
        { path: '/account', element: <AuthPage /> },
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
            { path: 'discount-codes', element: <DashboardDiscountCodesPage /> },
            { path: 'room-types', element: <DashboardRoomTypesPage /> },
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
            { path: 'site-guide', element: <DashboardSiteGuidePage /> },
            { path: 'marketing-promos', element: <DashboardMarketingPromosPage /> },
            { path: 'newsletter-subscribers', element: <DashboardNewsletterSubscribersPage /> },
            { path: 'assistant-faq', element: <DashboardAssistantFaqPage /> },
            { path: 'profile-questions', element: <DashboardProfileQuestionsPage /> },
            { path: 'profile-completion-answers', element: <DashboardProfileCompletionAnswersPage /> },
            { path: 'users', element: <DashboardUsersPage /> },
            { path: 'roles', element: <DashboardRolesPage /> },
            { path: '*', element: <NotFoundPage /> },
          ],
        },
        { path: '/account/*', element: <NotFoundPage /> },
      ],
    },
  ]
}
