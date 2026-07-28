import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ProfileQuestionsAdminPanel } from '@/components/dashboard/admin/ProfileQuestionsAdminPanel'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { ProfileCompletionIcon } from '@/components/dashboard/DashboardIcons'
import { ProfileCompletionUserSection } from '@/components/dashboard/ProfileCompletionUserSection'
import { useProfileCompletion } from '@/hooks/useProfileCompletion'

type ProfileTab = 'complete' | 'manage'

export function ProfileCompletionPanel() {
  const { t } = useTranslation()
  const { config } = useProfileCompletion()
  const [tab, setTab] = useState<ProfileTab>('complete')

  return (
    <div>
      <DashboardPageHeader
        title={config.campaign.title}
        description={config.campaign.subtitle}
        icon={<ProfileCompletionIcon size={22} />}
      />

      <div
        role="tablist"
        aria-label={t('dashboard.profileCompletion.tabsLabel')}
        className="mb-6 flex gap-1 rounded-sm border border-border bg-surface-muted/40 p-1"
      >
        <TabButton
          active={tab === 'complete'}
          onClick={() => setTab('complete')}
          label={t('dashboard.profileCompletion.tabComplete')}
        />
        <TabButton
          active={tab === 'manage'}
          onClick={() => setTab('manage')}
          label={t('dashboard.profileCompletion.tabManage')}
        />
      </div>

      {tab === 'complete' ? <ProfileCompletionUserSection /> : <ProfileQuestionsAdminPanel />}
    </div>
  )
}

function TabButton({
  active,
  onClick,
  label,
}: {
  active: boolean
  onClick: () => void
  label: string
}) {
  return (
    <button
      type="button"
      role="tab"
      aria-selected={active}
      onClick={onClick}
      className={`flex-1 rounded-sm px-4 py-2.5 text-sm font-medium transition-colors ${
        active
          ? 'bg-surface text-text shadow-sm'
          : 'text-text-muted hover:text-text'
      }`}
    >
      {label}
    </button>
  )
}
