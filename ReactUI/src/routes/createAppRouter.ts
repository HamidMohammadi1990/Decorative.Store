import { createBrowserRouter, type HydrationState } from 'react-router-dom'
import { createAppRoutes } from '@/routes/routeConfig'

export function createAppRouter(hydrationData?: HydrationState) {
  return createBrowserRouter(createAppRoutes(), hydrationData ? { hydrationData } : undefined)
}

/** @deprecated Prefer createAppRouter() — kept for modules that import a singleton during CSR-only dev. */
export const router = createAppRouter()
