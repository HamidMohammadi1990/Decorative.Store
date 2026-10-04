import { useTranslation } from 'react-i18next'
import { ProfileCompletionUserSection } from '@/components/dashboard/ProfileCompletionUserSection'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { ProfileCompletionIcon } from '@/components/dashboard/DashboardIcons'
import { useProfileCompletion } from '@/hooks/useProfileCompletion'

export function ProfileCompletionPanel() {
  const { t } = useTranslation()
  const { config } = useProfileCompletion()

  return (
    <div>
      <DashboardPageHeader
        title={config?.campaign.title ?? t('dashboard.profileCompletion.pageTitle')}
        description={config?.campaign.subtitle ?? t('dashboard.profileCompletion.pageDescription')}
        icon={<ProfileCompletionIcon size={22} />}
      />
      <ProfileCompletionUserSection />
    </div>
  )
}
