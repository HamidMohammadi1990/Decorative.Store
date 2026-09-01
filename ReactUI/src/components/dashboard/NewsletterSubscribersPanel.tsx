import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { NewsletterSubscriberIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminNewsletterService, type AdminNewsletterSubscriber } from '@/services/newsletterService'
import { useUserStore } from '@/stores/userStore'

export function NewsletterSubscribersPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [emailFilter, setEmailFilter] = useState('')
  const [appliedEmailFilter, setAppliedEmailFilter] = useState<string | null>(null)

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
        throw new Error(t('dashboard.newsletterSubscribers.authRequired'))
      }
      return adminNewsletterService.getSubscribers(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId,
        email: appliedEmailFilter,
      })
    },
    [accessToken, appliedEmailFilter, contentLanguageId, locale, t],
  )

  const {
    items,
    loading: listLoading,
    error: listError,
    pageNumber,
    pageSize,
    totalCount,
    totalPages,
    goToPage,
    reload,
  } = useAdminPagedList<AdminNewsletterSubscriber>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad,
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.newsletterSubscribers.loadFailed'))
    : null

  const applyEmailFilter = () => {
    setAppliedEmailFilter(emailFilter.trim() || null)
  }

  const clearEmailFilter = () => {
    setEmailFilter('')
    setAppliedEmailFilter(null)
  }

  if (!canLoad) {
    return (
      <DashboardEmptyState
        icon={<NewsletterSubscriberIcon size={28} />}
        title={t('dashboard.newsletterSubscribers.emptyTitle')}
        message={t('dashboard.newsletterSubscribers.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.newsletterSubscribers.title')}
        description={t('dashboard.newsletterSubscribers.description')}
        icon={<NewsletterSubscriberIcon size={22} />}
      />

      <AdminContentLanguageField
        className="mb-5"
        languages={formLanguages}
        value={contentLanguageId}
        onChange={setContentLanguageId}
      />

      <div className="mb-5 flex flex-col gap-3 sm:flex-row sm:items-end">
        <div className="flex-1">
          <AdminField label={t('dashboard.newsletterSubscribers.fieldEmail')}>
            <input
              type="search"
              value={emailFilter}
              onChange={(event) => setEmailFilter(event.target.value)}
              placeholder={t('dashboard.newsletterSubscribers.fieldEmailPlaceholder')}
              className={adminInputClass}
              dir="ltr"
              onKeyDown={(event) => {
                if (event.key === 'Enter') applyEmailFilter()
              }}
            />
          </AdminField>
        </div>
        <div className="flex flex-wrap gap-2">
          <Button variant="secondary" onClick={applyEmailFilter}>
            {t('dashboard.newsletterSubscribers.search')}
          </Button>
          {appliedEmailFilter && (
            <Button variant="secondary" onClick={clearEmailFilter}>
              {t('dashboard.newsletterSubscribers.clearSearch')}
            </Button>
          )}
        </div>
      </div>

      {listLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.newsletterSubscribers.loading')} />
        </div>
      ) : listErrorMessage ? (
        <DashboardEmptyState
          icon={<NewsletterSubscriberIcon size={28} />}
          title={t('dashboard.newsletterSubscribers.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.newsletterSubscribers.retry')}
            </Button>
          }
        />
      ) : (items ?? []).length === 0 ? (
        <DashboardEmptyState
          icon={<NewsletterSubscriberIcon size={28} />}
          title={t('dashboard.newsletterSubscribers.emptyTitle')}
          message={t('dashboard.newsletterSubscribers.emptyMessage')}
        />
      ) : (
        <AdminDataGrid
          rows={items ?? []}
          rowKey={(item) => String(item.id)}
          loading={listLoading}
          loadingLabel={t('dashboard.newsletterSubscribers.loading')}
          pagination={{
            pageNumber,
            pageSize,
            totalCount,
            totalPages,
            onPageChange: goToPage,
          }}
          columns={[
            {
              id: 'email',
              header: t('dashboard.newsletterSubscribers.colEmail'),
              cell: (item) => <span dir="ltr">{item.email}</span>,
            },
            {
              id: 'date',
              header: t('dashboard.newsletterSubscribers.colDate'),
              cell: (item) =>
                item.subscribedAtUtc
                  ? new Date(item.subscribedAtUtc).toLocaleString(locale === 'fa' ? 'fa-IR' : 'en-US')
                  : '—',
            },
            {
              id: 'status',
              header: t('dashboard.newsletterSubscribers.colStatus'),
              align: 'center',
              cell: (item) => (
                <span
                  className={`inline-flex rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                    item.isActive
                      ? 'bg-warm-soft text-warm'
                      : 'bg-surface-muted text-text-muted'
                  }`}
                >
                  {item.isActive
                    ? t('dashboard.newsletterSubscribers.statusActive')
                    : t('dashboard.newsletterSubscribers.statusInactive')}
                </span>
              ),
            },
          ]}
        />
      )}
    </div>
  )
}
