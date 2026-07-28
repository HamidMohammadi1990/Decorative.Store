import type { DashboardData } from '@/models/dashboard/dashboard.model'
import type { Locale } from '@/models/shared/locale.model'
import { getDashboardMock } from '@/data/mock'
import { mockFetch } from '@/services/api/mockClient'

export const dashboardService = {
  async getData(locale: Locale): Promise<DashboardData> {
    return mockFetch(async () => getDashboardMock(locale) as DashboardData)
  },
}
