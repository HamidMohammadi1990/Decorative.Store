import { createBrowserRouter, Navigate } from 'react-router-dom'
import { AuthLayout } from '@/layouts/AuthLayout'
import { DashboardLayout } from '@/layouts/DashboardLayout'
import { ShopLayout } from '@/layouts/ShopLayout'
import { AuthPage } from '@/pages/AuthPage'
import {
  DashboardAddressesPage,
  DashboardCouponsPage,
  DashboardOrdersPage,
  DashboardProfilePage,
  DashboardReviewsPage,
  DashboardStoriesPage,
  DashboardTransactionsPage,
  DashboardWalletPage,
  DashboardWishlistPage,
} from '@/pages/dashboard/DashboardPages'
import { CheckoutPage } from '@/pages/CheckoutPage'
import { HomePage } from '@/pages/HomePage'
import { BlogDetailPage } from '@/pages/BlogDetailPage'
import { BlogListingPage } from '@/pages/BlogListingPage'
import { ComparePage } from '@/pages/ComparePage'
import { ProductDetailPage } from '@/pages/ProductDetailPage'
import { NotFoundPage } from '@/pages/NotFoundPage'
import { ProductListingPage } from '@/pages/ProductListingPage'
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
        path: '/checkout',
        element: <CheckoutPage />,
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
          { path: 'profile', element: <DashboardProfilePage /> },
          { path: 'orders', element: <DashboardOrdersPage /> },
          { path: 'transactions', element: <DashboardTransactionsPage /> },
          { path: 'reviews', element: <DashboardReviewsPage /> },
          { path: 'wishlist', element: <DashboardWishlistPage /> },
          { path: 'stories', element: <DashboardStoriesPage /> },
          { path: 'addresses', element: <DashboardAddressesPage /> },
          { path: 'coupons', element: <DashboardCouponsPage /> },
          { path: '*', element: <NotFoundPage /> },
        ],
      },
      { path: '/account/*', element: <NotFoundPage /> },
    ],
  },
])
