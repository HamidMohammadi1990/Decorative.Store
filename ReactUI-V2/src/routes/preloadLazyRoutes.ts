import { preloadDashboardPages } from '@/routes/lazyPages'

/** Preload dashboard lazy chunk before hydration on admin routes. */
export async function preloadLazyRoutes(_routeIds: string[]) {
  if (typeof window === 'undefined') return
  if (window.location.pathname.startsWith('/account/dashboard')) {
    await preloadDashboardPages()
  }
}

export async function preloadLazyRoutesForPath(pathname: string) {
  if (pathname.startsWith('/account/dashboard')) {
    await preloadDashboardPages()
  }
}
