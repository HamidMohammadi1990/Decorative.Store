import { useTranslation } from 'react-i18next'
import { ProfileQuestionsAdminPanel } from '@/components/dashboard/admin/ProfileQuestionsAdminPanel'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { ProfileCompletionIcon } from '@/components/dashboard/DashboardIcons'

export function ProfileQuestionsAdminPage() {
  const { t } = useTranslation()

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.adminProfile.title')}
        description={t('dashboard.adminProfile.description')}
        icon={<ProfileCompletionIcon size={22} />}
      />
      <ProfileQuestionsAdminPanel />
    </div>
  )
}
