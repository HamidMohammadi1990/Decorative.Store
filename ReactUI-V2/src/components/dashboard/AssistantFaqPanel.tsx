import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminAssistantFaq } from '@/models/admin/assistantFaq.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { AssistantFaqIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
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
import { adminAssistantFaqService } from '@/services/assistantFaqService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function AssistantFaqPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [question, setQuestion] = useState('')
  const [answer, setAnswer] = useState('')
  const [priority, setPriority] = useState(0)
  const [isActive, setIsActive] = useState(true)

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
        throw new Error(t('dashboard.assistantFaq.authRequired'))
      }
      return adminAssistantFaqService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId,
      })
    },
    [accessToken, contentLanguageId, locale, t],
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
  } = useAdminPagedList<AdminAssistantFaq>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.assistantFaq.loadFailed'))
    : null

  const resetForm = () => {
    setQuestion('')
    setAnswer('')
    setPriority(0)
    setIsActive(true)
    setEditingId(null)
    setFormError(null)
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminAssistantFaq) => {
    setEditingId(item.id)
    setQuestion(item.question)
    setAnswer(item.answer)
    setPriority(item.priority)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setFormError(t('dashboard.assistantFaq.authRequired'))
      return
    }
    if (!question.trim() || !answer.trim()) {
      setFormError(t('dashboard.assistantFaq.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      const payload = {
        languageId: contentLanguageId,
        question: question.trim(),
        answer: answer.trim(),
        priority,
      }
      if (mode === 'edit' && editingId) {
        await adminAssistantFaqService.update(accessToken, locale, {
          ...payload,
          id: editingId,
          isActive,
        })
      } else {
        await adminAssistantFaqService.create(accessToken, locale, payload)
      }
      reload()
      backToList()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.assistantFaq.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.assistantFaq.deleteConfirm'))) return

    setSaving(true)
    setFormError(null)
    try {
      await adminAssistantFaqService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.assistantFaq.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.assistantFaq.editTitle')
              : t('dashboard.assistantFaq.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.assistantFaq.editDescription')
              : t('dashboard.assistantFaq.createDescription')
          }
          icon={<AssistantFaqIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
            disabled={contentLanguageLoading || mode === 'edit'}
          />

          <AdminField label={t('dashboard.assistantFaq.fieldQuestion')}>
            <input
              value={question}
              onChange={(e) => setQuestion(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.assistantFaq.questionPlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.assistantFaq.fieldAnswer')}>
            <textarea
              value={answer}
              onChange={(e) => setAnswer(e.target.value)}
              rows={5}
              className={`${adminInputClass} min-h-[7rem] resize-y`}
              placeholder={t('dashboard.assistantFaq.answerPlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.assistantFaq.fieldPriority')}>
            <input
              type="number"
              min={0}
              value={priority}
              onChange={(e) => setPriority(Number(e.target.value) || 0)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          {mode === 'edit' && (
            <label className="flex cursor-pointer items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.assistantFaq.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.assistantFaq.saving')} />
              ) : (
                t('dashboard.assistantFaq.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.assistantFaq.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<AssistantFaqIcon size={28} />}
        title={t('dashboard.assistantFaq.emptyTitle')}
        message={t('dashboard.assistantFaq.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.assistantFaq.title')}
        description={t('dashboard.assistantFaq.description')}
        icon={<AssistantFaqIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate} disabled={!canLoad}>
            {t('dashboard.assistantFaq.add')}
          </Button>
        }
      />

      {!canLoad ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.assistantFaq.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<AssistantFaqIcon size={28} />}
          title={t('dashboard.assistantFaq.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.assistantFaq.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<AssistantFaqIcon size={28} />}
          title={t('dashboard.assistantFaq.emptyTitle')}
          message={t('dashboard.assistantFaq.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.assistantFaq.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {formError && <p className="text-sm text-sale">{formError}</p>}

          <AdminDataGrid
            columns={[
              {
                id: 'question',
                header: t('dashboard.assistantFaq.colQuestion'),
                cell: (item) => (
                  <span className="font-medium text-text">{item.question}</span>
                ),
              },
              {
                id: 'answer',
                header: t('dashboard.assistantFaq.colAnswer'),
                cell: (item) => (
                  <p className="line-clamp-2 max-w-md text-xs text-text-muted">{item.answer}</p>
                ),
              },
              {
                id: 'priority',
                header: t('dashboard.assistantFaq.colPriority'),
                align: 'center',
                cell: (item) => (
                  <span className="font-mono text-xs text-text-muted">{item.priority}</span>
                ),
              },
              {
                id: 'status',
                header: t('dashboard.assistantFaq.colStatus'),
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
                      ? t('dashboard.assistantFaq.statusActive')
                      : t('dashboard.assistantFaq.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: t('dashboard.assistantFaq.colActions'),
                align: 'right',
                cell: (item) => (
                  <AdminGridActions>
                    <AdminGridIconButton
                      label={t('dashboard.assistantFaq.edit')}
                      icon={<EditIcon size={15} />}
                      onClick={() => openEdit(item)}
                      disabled={saving}
                    />
                    <AdminGridIconButton
                      label={t('dashboard.assistantFaq.delete')}
                      icon={<DeleteIcon size={15} />}
                      tone="danger"
                      onClick={() => void handleDelete(item.id)}
                      disabled={saving}
                    />
                  </AdminGridActions>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.assistantFaq.loading')}
            pagination={{
              pageNumber,
              pageSize,
              totalCount,
              totalPages,
              onPageChange: goToPage,
            }}
          />
        </div>
      )}
    </div>
  )
}
