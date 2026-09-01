import { useCallback, useEffect, useState } from 'react'
import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { DeleteIcon } from '@/components/dashboard/DashboardIcons'
import { adminProfileCompletionService } from '@/services/profileCompletionApiService'
import { useUserStore } from '@/stores/userStore'
import {
  isSelectType,
  patchLocalized,
  useProfileCompletionAdminStore,
} from '@/stores/profileCompletionAdminStore'
import type { AdminProfileQuestion, AdminQuestionOption, AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'
import type { ProfileQuestionType } from '@/models/profile/profileCompletion.model'

type AdminTab = 'campaign' | 'questions'
type ContentLocale = 'fa' | 'en'

const QUESTION_TYPES: ProfileQuestionType[] = [
  'text',
  'textarea',
  'tel',
  'date',
  'single_select',
  'multi_select',
]

const TYPE_CHIP_CLASS: Record<ProfileQuestionType, string> = {
  text: 'bg-sky-500/10 text-sky-700 ring-sky-500/20',
  textarea: 'bg-violet-500/10 text-violet-700 ring-violet-500/20',
  tel: 'bg-emerald-500/10 text-emerald-700 ring-emerald-500/20',
  date: 'bg-amber-500/10 text-amber-800 ring-amber-500/20',
  single_select: 'bg-warm-soft text-warm ring-warm/25',
  multi_select: 'bg-orange-500/10 text-orange-800 ring-orange-500/20',
}

export function ProfileQuestionsAdminPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const config = useProfileCompletionAdminStore((s) => s.config)
  const setConfigFromServer = useProfileCompletionAdminStore((s) => s.setConfigFromServer)
  const updateCampaign = useProfileCompletionAdminStore((s) => s.updateCampaign)
  const addQuestion = useProfileCompletionAdminStore((s) => s.addQuestion)
  const updateQuestion = useProfileCompletionAdminStore((s) => s.updateQuestion)
  const removeQuestion = useProfileCompletionAdminStore((s) => s.removeQuestion)
  const moveQuestion = useProfileCompletionAdminStore((s) => s.moveQuestion)
  const addOption = useProfileCompletionAdminStore((s) => s.addOption)
  const updateOption = useProfileCompletionAdminStore((s) => s.updateOption)
  const removeOption = useProfileCompletionAdminStore((s) => s.removeOption)

  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [savedHint, setSavedHint] = useState(false)
  const [activeTab, setActiveTab] = useState<AdminTab>('questions')
  const [contentLocale, setContentLocale] = useState<ContentLocale>('fa')
  const [expandedQuestionId, setExpandedQuestionId] = useState<string | null>(null)

  const loadConfig = useCallback(async () => {
    if (!accessToken) {
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const remote = await adminProfileCompletionService.getConfig(accessToken)
      if (!remote) {
        setError(t('dashboard.adminProfile.loadFailed'))
        return
      }
      setConfigFromServer(remote)
      const firstQuestion = [...remote.questions].sort((a, b) => a.order - b.order)[0]
      setExpandedQuestionId(firstQuestion?.id ?? null)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.adminProfile.loadFailed')))
    } finally {
      setLoading(false)
    }
  }, [accessToken, setConfigFromServer, t])

  useEffect(() => {
    void loadConfig()
  }, [loadConfig])

  const handleReload = () => {
    setSavedHint(false)
    void loadConfig()
  }

  const handleSave = async () => {
    if (!accessToken || !config) return

    setSaving(true)
    setError(null)
    setSavedHint(false)
    try {
      await adminProfileCompletionService.saveConfig(accessToken, config)
      setSavedHint(true)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.adminProfile.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleAddQuestion = () => {
    addQuestion()
    const sorted = [...useProfileCompletionAdminStore.getState().config.questions].sort(
      (a, b) => a.order - b.order,
    )
    const newest = sorted[sorted.length - 1]
    if (newest) {
      setExpandedQuestionId(newest.id)
      setActiveTab('questions')
    }
  }

  const sortedQuestions = [...(config?.questions ?? [])].sort((a, b) => a.order - b.order)

  if (loading) {
    return (
      <div className="flex justify-center py-16">
        <InlineLoading label={t('dashboard.adminProfile.loading')} />
      </div>
    )
  }

  if (!accessToken) {
    return (
      <div className="rounded-sm border border-border bg-surface-muted/30 px-5 py-10 text-center text-sm text-text-muted">
        {t('dashboard.adminProfile.authRequired')}
      </div>
    )
  }

  if (!config) {
    return (
      <div className="rounded-sm border border-sale/30 bg-sale/5 px-5 py-10 text-center text-sm text-sale">
        {error ?? t('dashboard.adminProfile.loadFailed')}
      </div>
    )
  }

  return (
    <div className="pb-24">
      <WorkflowGuide activeTab={activeTab} onGoToTab={setActiveTab} />

      {error && (
        <div className="mb-4 rounded-sm border border-sale/30 bg-sale/5 px-4 py-3 text-sm text-sale">
          {error}
        </div>
      )}

      <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <AdminTabBar activeTab={activeTab} onChange={setActiveTab} questionCount={sortedQuestions.length} />
        <div className="flex flex-wrap items-center gap-2">
          <Button variant="warm" className="text-xs" onClick={handleAddQuestion}>
            + {t('dashboard.adminProfile.addQuestion')}
          </Button>
          <Link
            to="/account/dashboard/profile"
            className="inline-flex shrink-0 items-center justify-center gap-2 rounded-sm border border-border bg-surface px-4 py-2 text-xs font-semibold text-text transition-colors hover:border-warm/40 hover:text-warm"
          >
            {t('dashboard.adminProfile.previewPage')}
            <span aria-hidden>↗</span>
          </Link>
        </div>
      </div>

      {activeTab === 'campaign' ? (
        <>
          <CampaignSection
            config={config}
            contentLocale={contentLocale}
            onContentLocaleChange={setContentLocale}
            onUpdateCampaign={updateCampaign}
          />
          <div className="mt-6 rounded-sm border border-dashed border-warm/30 bg-warm-soft/20 px-5 py-4">
            <p className="text-sm font-medium text-text">{t('dashboard.adminProfile.questionsTitle')}</p>
            <p className="mt-1 text-sm text-text-muted">
              {t('dashboard.adminProfile.questionsCount', { count: sortedQuestions.length })}
            </p>
            <div className="mt-3 flex flex-wrap gap-2">
              <Button variant="warm" className="text-xs" onClick={() => { setActiveTab('questions'); handleAddQuestion() }}>
                + {t('dashboard.adminProfile.addQuestion')}
              </Button>
              <Button variant="secondary" className="text-xs" onClick={() => setActiveTab('questions')}>
                {t('dashboard.adminProfile.tabQuestions')}
              </Button>
            </div>
          </div>
        </>
      ) : (
        <QuestionsSection
          questions={sortedQuestions}
          contentLocale={contentLocale}
          onContentLocaleChange={setContentLocale}
          expandedQuestionId={expandedQuestionId}
          onExpand={setExpandedQuestionId}
          onAddQuestion={handleAddQuestion}
          onUpdateQuestion={updateQuestion}
          onRemoveQuestion={removeQuestion}
          onMoveQuestion={moveQuestion}
          onAddOption={addOption}
          onUpdateOption={updateOption}
          onRemoveOption={removeOption}
        />
      )}

      <StickySaveBar
        recentlySaved={savedHint}
        isSaving={saving}
        onSave={() => void handleSave()}
        onReload={handleReload}
      />
    </div>
  )
}

function WorkflowGuide({
  activeTab,
  onGoToTab,
}: {
  activeTab: AdminTab
  onGoToTab: (tab: AdminTab) => void
}) {
  const { t } = useTranslation()

  const steps: { id: AdminTab | 'save'; label: string; action?: () => void }[] = [
    { id: 'campaign', label: t('dashboard.adminProfile.workflowStep1'), action: () => onGoToTab('campaign') },
    { id: 'questions', label: t('dashboard.adminProfile.workflowStep2'), action: () => onGoToTab('questions') },
    { id: 'save', label: t('dashboard.adminProfile.workflowStep3') },
  ]

  return (
    <section className="mb-6 rounded-sm border border-border bg-gradient-to-br from-surface-muted/40 to-surface p-4 sm:p-5">
      <p className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
        {t('dashboard.adminProfile.workflowTitle')}
      </p>
      <ol className="mt-4 grid gap-3 sm:grid-cols-3">
        {steps.map((step, index) => {
          const isActive = step.id === activeTab
          const isDone = step.id === 'campaign' && activeTab === 'questions'

          return (
            <li key={step.id}>
              <button
                type="button"
                onClick={step.action}
                disabled={!step.action}
                className={`flex w-full items-start gap-3 rounded-sm border px-3 py-3 text-start transition-colors ${
                  isActive
                    ? 'border-warm/40 bg-warm-soft/60'
                    : isDone
                      ? 'border-warm/20 bg-surface'
                      : 'border-border bg-surface hover:border-warm/25'
                } ${!step.action ? 'cursor-default' : ''}`}
              >
                <span
                  className={`flex size-7 shrink-0 items-center justify-center rounded-full text-xs font-bold ${
                    isActive ? 'bg-warm text-warm-text' : 'bg-surface-muted text-text-muted'
                  }`}
                >
                  {index + 1}
                </span>
                <span className="text-sm font-medium text-text">{step.label}</span>
              </button>
            </li>
          )
        })}
      </ol>
    </section>
  )
}

function AdminTabBar({
  activeTab,
  onChange,
  questionCount,
}: {
  activeTab: AdminTab
  onChange: (tab: AdminTab) => void
  questionCount: number
}) {
  const { t } = useTranslation()

  const tabs: { id: AdminTab; label: string; hint: string; badge?: string }[] = [
    {
      id: 'campaign',
      label: t('dashboard.adminProfile.tabCampaign'),
      hint: t('dashboard.adminProfile.tabCampaignHint'),
    },
    {
      id: 'questions',
      label: t('dashboard.adminProfile.tabQuestions'),
      hint: t('dashboard.adminProfile.tabQuestionsHint'),
      badge: String(questionCount),
    },
  ]

  return (
    <div
      role="tablist"
      aria-label={t('dashboard.adminProfile.tabsLabel')}
      className="inline-flex flex-wrap gap-1 rounded-sm border border-border bg-surface-muted/40 p-1"
    >
      {tabs.map((tab) => {
        const isActive = activeTab === tab.id
        return (
          <button
            key={tab.id}
            type="button"
            role="tab"
            aria-selected={isActive}
            onClick={() => onChange(tab.id)}
            className={`rounded-sm px-4 py-2.5 text-start transition-colors ${
              isActive ? 'bg-surface shadow-sm ring-1 ring-warm/20' : 'hover:bg-surface/70'
            }`}
          >
            <span className="flex items-center gap-2">
              <span className={`text-sm font-semibold ${isActive ? 'text-warm' : 'text-text'}`}>
                {tab.label}
              </span>
              {tab.badge !== undefined && (
                <span className="rounded-full bg-surface-muted px-2 py-0.5 text-[10px] font-bold text-text-muted">
                  {tab.badge}
                </span>
              )}
            </span>
            <span className="mt-0.5 block text-[11px] text-text-muted">{tab.hint}</span>
          </button>
        )
      })}
    </div>
  )
}

function LocaleToggle({
  value,
  onChange,
}: {
  value: ContentLocale
  onChange: (locale: ContentLocale) => void
}) {
  const { t } = useTranslation()

  return (
    <div className="flex items-center gap-2">
      <span className="text-xs font-medium text-text-muted">{t('dashboard.adminProfile.editLanguage')}</span>
      <div className="inline-flex rounded-sm border border-border p-0.5">
        {(['fa', 'en'] as const).map((locale) => (
          <button
            key={locale}
            type="button"
            onClick={() => onChange(locale)}
            className={`rounded-sm px-3 py-1 text-xs font-semibold transition-colors ${
              value === locale ? 'bg-warm text-warm-text' : 'text-text-muted hover:text-text'
            }`}
          >
            {locale === 'fa' ? 'فارسی' : 'English'}
          </button>
        ))}
      </div>
    </div>
  )
}

function CampaignSection({
  config,
  contentLocale,
  onContentLocaleChange,
  onUpdateCampaign,
}: {
  config: AdminProfileCompletionConfig
  contentLocale: ContentLocale
  onContentLocaleChange: (locale: ContentLocale) => void
  onUpdateCampaign: (
    patch: Partial<Pick<AdminProfileCompletionConfig, 'campaignId' | 'titles' | 'subtitles' | 'reward'>>,
  ) => void
}) {
  const { t } = useTranslation()

  return (
    <div className="space-y-6">
      <SectionCard
        title={t('dashboard.adminProfile.campaignPageText')}
        description={t('dashboard.adminProfile.campaignPageTextHint')}
        action={<LocaleToggle value={contentLocale} onChange={onContentLocaleChange} />}
      >
        <div className="grid gap-4 lg:grid-cols-2">
          <Field label={t('dashboard.adminProfile.fieldTitle')}>
            <input
              type="text"
              value={config.titles[contentLocale]}
              onChange={(e) =>
                onUpdateCampaign({ titles: patchLocalized(config.titles, contentLocale, e.target.value) })
              }
              className={inputClass}
              dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
            />
          </Field>
          <Field label={t('dashboard.adminProfile.fieldSubtitle')}>
            <input
              type="text"
              value={config.subtitles[contentLocale]}
              onChange={(e) =>
                onUpdateCampaign({
                  subtitles: patchLocalized(config.subtitles, contentLocale, e.target.value),
                })
              }
              className={inputClass}
              dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
            />
          </Field>
        </div>
      </SectionCard>

      <SectionCard
        title={t('dashboard.adminProfile.rewardSettings')}
        description={t('dashboard.adminProfile.rewardSettingsHint')}
      >
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <Field label={t('dashboard.adminProfile.rewardType')}>
            <select
              value={config.reward.type}
              onChange={(e) =>
                onUpdateCampaign({
                  reward: {
                    ...config.reward,
                    type: e.target.value as 'percent_discount' | 'fixed_discount',
                  },
                })
              }
              className={inputClass}
            >
              <option value="percent_discount">{t('dashboard.adminProfile.rewardPercentType')}</option>
              <option value="fixed_discount">{t('dashboard.adminProfile.rewardFixedType')}</option>
            </select>
          </Field>
          <Field label={t('dashboard.adminProfile.rewardValue')}>
            <input
              type="number"
              min={1}
              value={config.reward.value}
              onChange={(e) =>
                onUpdateCampaign({
                  reward: { ...config.reward, value: Number(e.target.value) || 0 },
                })
              }
              className={inputClass}
            />
          </Field>
          <Field label={t('dashboard.adminProfile.rewardCode')}>
            <input
              type="text"
              value={config.reward.code}
              onChange={(e) =>
                onUpdateCampaign({ reward: { ...config.reward, code: e.target.value.toUpperCase() } })
              }
              className={`${inputClass} font-mono uppercase`}
            />
          </Field>
          <Field label={t('dashboard.adminProfile.rewardValidDays')}>
            <input
              type="number"
              min={1}
              value={config.reward.validDays}
              onChange={(e) =>
                onUpdateCampaign({
                  reward: { ...config.reward, validDays: Number(e.target.value) || 1 },
                })
              }
              className={inputClass}
            />
          </Field>
        </div>

        <div className="mt-4 flex flex-wrap items-center justify-between gap-3">
          <LocaleToggle value={contentLocale} onChange={onContentLocaleChange} />
        </div>
        <div className="mt-3">
          <Field label={t('dashboard.adminProfile.rewardDescription')}>
            <input
              type="text"
              value={config.reward.descriptions[contentLocale]}
              onChange={(e) =>
                onUpdateCampaign({
                  reward: {
                    ...config.reward,
                    descriptions: patchLocalized(config.reward.descriptions, contentLocale, e.target.value),
                  },
                })
              }
              className={inputClass}
              dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
            />
          </Field>
        </div>

        <div className="mt-5 rounded-sm border border-warm/25 bg-warm-soft/40 p-4">
          <p className="text-xs font-semibold uppercase tracking-wide text-warm">
            {t('dashboard.adminProfile.rewardPreview')}
          </p>
          <p className="mt-2 text-2xl font-bold text-warm">
            {config.reward.type === 'percent_discount'
              ? t('dashboard.profileCompletion.rewardPercent', { value: config.reward.value })
              : config.reward.descriptions[contentLocale]}
          </p>
          <p className="mt-1 font-mono text-sm tracking-wider text-text">{config.reward.code}</p>
        </div>
      </SectionCard>
    </div>
  )
}

function QuestionsSection({
  questions,
  contentLocale,
  onContentLocaleChange,
  expandedQuestionId,
  onExpand,
  onAddQuestion,
  onUpdateQuestion,
  onRemoveQuestion,
  onMoveQuestion,
  onAddOption,
  onUpdateOption,
  onRemoveOption,
}: {
  questions: AdminProfileQuestion[]
  contentLocale: ContentLocale
  onContentLocaleChange: (locale: ContentLocale) => void
  expandedQuestionId: string | null
  onExpand: (id: string | null) => void
  onAddQuestion: () => void
  onUpdateQuestion: (id: string, patch: Partial<AdminProfileQuestion>) => void
  onRemoveQuestion: (id: string) => void
  onMoveQuestion: (id: string, direction: 'up' | 'down') => void
  onAddOption: (id: string) => void
  onUpdateOption: (id: string, optionIndex: number, patch: Partial<AdminQuestionOption>) => void
  onRemoveOption: (id: string, optionIndex: number) => void
}) {
  const { t } = useTranslation()

  const collapseAll = () => onExpand(null)

  return (
    <div>
      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div>
          <h2 className="text-base font-semibold text-text">{t('dashboard.adminProfile.questionsTitle')}</h2>
          <p className="mt-1 text-sm text-text-muted">
            {t('dashboard.adminProfile.questionsCount', { count: questions.length })}
          </p>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <LocaleToggle value={contentLocale} onChange={onContentLocaleChange} />
          {questions.length > 1 && expandedQuestionId && (
            <button
              type="button"
              onClick={collapseAll}
              className="rounded-sm border border-border px-3 py-1.5 text-xs text-text-muted hover:bg-surface-muted"
            >
              {t('dashboard.adminProfile.collapseAll')}
            </button>
          )}
          <Button variant="warm" className="text-xs" onClick={onAddQuestion}>
            + {t('dashboard.adminProfile.addQuestion')}
          </Button>
        </div>
      </div>

      <div className="space-y-3">
        {questions.length === 0 ? (
          <div className="rounded-sm border border-dashed border-border bg-surface-muted/20 px-6 py-14 text-center">
            <p className="text-base font-medium text-text">{t('dashboard.adminProfile.emptyQuestionsTitle')}</p>
            <p className="mx-auto mt-2 max-w-md text-sm text-text-muted">
              {t('dashboard.adminProfile.emptyQuestions')}
            </p>
            <Button variant="warm" className="mt-6" onClick={onAddQuestion}>
              {t('dashboard.adminProfile.addFirstQuestion')}
            </Button>
          </div>
        ) : (
          questions.map((question, index) => (
            <QuestionAccordion
              key={question.id}
              question={question}
              index={index}
              total={questions.length}
              contentLocale={contentLocale}
              expanded={expandedQuestionId === question.id}
              onToggle={() =>
                onExpand(expandedQuestionId === question.id ? null : question.id)
              }
              onUpdate={(patch) => onUpdateQuestion(question.id, patch)}
              onRemove={() => onRemoveQuestion(question.id)}
              onMove={(direction) => onMoveQuestion(question.id, direction)}
              onAddOption={() => onAddOption(question.id)}
              onUpdateOption={(optionIndex, patch) =>
                onUpdateOption(question.id, optionIndex, patch)
              }
              onRemoveOption={(optionIndex) => onRemoveOption(question.id, optionIndex)}
            />
          ))
        )}
      </div>
    </div>
  )
}

function QuestionAccordion({
  question,
  index,
  total,
  contentLocale,
  expanded,
  onToggle,
  onUpdate,
  onRemove,
  onMove,
  onAddOption,
  onUpdateOption,
  onRemoveOption,
}: {
  question: AdminProfileQuestion
  index: number
  total: number
  contentLocale: ContentLocale
  expanded: boolean
  onToggle: () => void
  onUpdate: (patch: Partial<AdminProfileQuestion>) => void
  onRemove: () => void
  onMove: (direction: 'up' | 'down') => void
  onAddOption: () => void
  onUpdateOption: (optionIndex: number, patch: Partial<AdminQuestionOption>) => void
  onRemoveOption: (optionIndex: number) => void
}) {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const showOptions = isSelectType(question.type)
  const summary =
    question.labels[contentLocale]?.trim() ||
    question.labels.fa?.trim() ||
    question.labels.en?.trim() ||
    t('dashboard.adminProfile.questionSummaryEmpty')

  const handleRemoveQuestion = async () => {
    if (!(await confirm({ message: t('dashboard.adminProfile.deleteQuestionConfirm') }))) return
    onRemove()
  }

  const handleRemoveOption = async (optionIndex: number) => {
    if (!(await confirm({ message: t('dashboard.adminProfile.deleteOptionConfirm') }))) return
    onRemoveOption(optionIndex)
  }

  return (
    <article
      className={`overflow-hidden rounded-sm border transition-shadow ${
        expanded
          ? 'border-warm/35 bg-surface shadow-md ring-1 ring-warm/10'
          : 'border-border bg-surface shadow-sm hover:border-warm/20'
      }`}
    >
      <div className="flex items-stretch gap-1 px-2 py-2 sm:px-3">
        <button
          type="button"
          onClick={onToggle}
          className="flex min-w-0 flex-1 items-center gap-3 rounded-sm px-2 py-2 text-start sm:px-3"
        >
          <span
            className={`flex size-8 shrink-0 items-center justify-center rounded-full text-sm font-bold ${
              expanded ? 'bg-warm text-warm-text' : 'bg-surface-muted text-text-muted'
            }`}
          >
            {index + 1}
          </span>
          <div className="min-w-0 flex-1">
            <div className="flex flex-wrap items-center gap-2">
              <span className="truncate text-sm font-semibold text-text sm:text-base">{summary}</span>
              {question.required && (
                <span className="rounded-full bg-sale/10 px-2 py-0.5 text-[10px] font-semibold text-sale">
                  {t('dashboard.adminProfile.required')}
                </span>
              )}
            </div>
            <div className="mt-1.5 flex flex-wrap items-center gap-2">
              <TypeChip type={question.type} />
              {showOptions && (
                <span className="text-[11px] text-text-muted">
                  {t('dashboard.adminProfile.optionsCount', { count: question.options.length })}
                </span>
              )}
            </div>
          </div>
          <span
            className={`shrink-0 text-lg text-text-muted transition-transform ${expanded ? 'rotate-180' : ''}`}
            aria-hidden
          >
            ▾
          </span>
        </button>
        <button
          type="button"
          onClick={() => void handleRemoveQuestion()}
          aria-label={t('dashboard.adminProfile.deleteQuestion')}
          title={t('dashboard.adminProfile.deleteQuestion')}
          className="flex size-10 shrink-0 items-center justify-center rounded-sm border border-sale/25 text-sale transition-colors hover:bg-sale/5"
        >
          <DeleteIcon size={16} />
        </button>
      </div>

      {expanded && (
        <div className="border-t border-border/70 px-4 pb-5 pt-4 sm:px-5">
          <div className="mb-4 flex flex-wrap items-center justify-end gap-2">
            <IconButton
              label={t('dashboard.adminProfile.moveUp')}
              disabled={index === 0}
              onClick={() => onMove('up')}
            >
              ↑
            </IconButton>
            <IconButton
              label={t('dashboard.adminProfile.moveDown')}
              disabled={index === total - 1}
              onClick={() => onMove('down')}
            >
              ↓
            </IconButton>
            <button
              type="button"
              onClick={() => void handleRemoveQuestion()}
              className="rounded-sm border border-sale/30 px-3 py-1.5 text-xs font-medium text-sale hover:bg-sale/5"
            >
              {t('dashboard.adminProfile.deleteQuestion')}
            </button>
          </div>

          <div className="grid gap-4 sm:grid-cols-2">
            <Field label={t('dashboard.adminProfile.questionType')}>
              <select
                value={question.type}
                onChange={(e) => {
                  const type = e.target.value as ProfileQuestionType
                  onUpdate({
                    type,
                    options:
                      isSelectType(type) && question.options.length === 0
                        ? [{ value: 'option-1', labels: { en: '', fa: '' } }]
                        : question.options,
                  })
                }}
                className={inputClass}
              >
                {QUESTION_TYPES.map((type) => (
                  <option key={type} value={type}>
                    {t(`dashboard.adminProfile.types.${type}`)}
                  </option>
                ))}
              </select>
            </Field>
            <Field label={t('dashboard.adminProfile.required')}>
              <label className="inline-flex min-h-[42px] items-center gap-2 text-sm text-text">
                <input
                  type="checkbox"
                  checked={question.required}
                  onChange={(e) => onUpdate({ required: e.target.checked })}
                  className="size-4 rounded border-border text-warm focus:ring-warm/30"
                />
                {t('dashboard.adminProfile.requiredHint')}
              </label>
            </Field>
          </div>

          <div className="mt-4 space-y-4">
            <Field label={t('dashboard.adminProfile.questionLabel')}>
              <input
                type="text"
                value={question.labels[contentLocale]}
                onChange={(e) =>
                  onUpdate({ labels: patchLocalized(question.labels, contentLocale, e.target.value) })
                }
                className={inputClass}
                dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
              />
            </Field>
            <div className="grid gap-4 lg:grid-cols-2">
              <Field label={t('dashboard.adminProfile.questionHint')}>
                <input
                  type="text"
                  value={question.hints[contentLocale]}
                  onChange={(e) =>
                    onUpdate({ hints: patchLocalized(question.hints, contentLocale, e.target.value) })
                  }
                  className={inputClass}
                  dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
                />
              </Field>
              <Field label={t('dashboard.adminProfile.questionPlaceholder')}>
                <input
                  type="text"
                  value={question.placeholders[contentLocale]}
                  onChange={(e) =>
                    onUpdate({
                      placeholders: patchLocalized(question.placeholders, contentLocale, e.target.value),
                    })
                  }
                  className={inputClass}
                  dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
                />
              </Field>
            </div>
          </div>

          <p className="mt-4 text-[11px] text-text-muted">
            {t('dashboard.adminProfile.advancedId')}:{' '}
            <code className="rounded bg-surface-muted px-1.5 py-0.5 font-mono">{question.id}</code>
          </p>

          {showOptions && (
            <div className="mt-5 rounded-sm border border-border/70 bg-surface-muted/20 p-4">
              <div className="mb-3 flex items-center justify-between gap-3">
                <p className="text-sm font-semibold text-text">{t('dashboard.adminProfile.optionsTitle')}</p>
                <button
                  type="button"
                  onClick={onAddOption}
                  className="text-xs font-semibold text-warm hover:underline"
                >
                  + {t('dashboard.adminProfile.addOption')}
                </button>
              </div>
              <div className="space-y-3">
                {question.options.map((option, optionIndex) => (
                  <div
                    key={`${question.id}-${optionIndex}`}
                    className="rounded-sm border border-border bg-surface p-3"
                  >
                    <div className="mb-2 flex items-center justify-between gap-2">
                      <span className="text-xs font-semibold text-text-muted">
                        {t('dashboard.adminProfile.optionNumber', { number: optionIndex + 1 })}
                      </span>
                      <button
                        type="button"
                        onClick={() => void handleRemoveOption(optionIndex)}
                        className="text-xs text-sale hover:underline"
                      >
                        {t('dashboard.adminProfile.deleteOption')}
                      </button>
                    </div>
                    <div className="grid gap-3 sm:grid-cols-2">
                      <Field label={t('dashboard.adminProfile.optionValue')}>
                        <input
                          type="text"
                          value={option.value}
                          onChange={(e) => onUpdateOption(optionIndex, { value: e.target.value })}
                          className={`${inputClass} font-mono text-xs`}
                        />
                      </Field>
                      <Field label={contentLocale === 'fa' ? 'برچسب فارسی' : 'English label'}>
                        <input
                          type="text"
                          value={option.labels[contentLocale]}
                          onChange={(e) =>
                            onUpdateOption(optionIndex, {
                              labels: patchLocalized(option.labels, contentLocale, e.target.value),
                            })
                          }
                          className={inputClass}
                          dir={contentLocale === 'fa' ? 'rtl' : 'ltr'}
                        />
                      </Field>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      )}
    </article>
  )
}

function TypeChip({ type }: { type: ProfileQuestionType }) {
  const { t } = useTranslation()
  return (
    <span
      className={`inline-flex rounded-full px-2 py-0.5 text-[10px] font-semibold ring-1 ring-inset ${TYPE_CHIP_CLASS[type]}`}
    >
      {t(`dashboard.adminProfile.types.${type}`)}
    </span>
  )
}

function SectionCard({
  title,
  description,
  action,
  children,
}: {
  title: string
  description?: string
  action?: ReactNode
  children: ReactNode
}) {
  return (
    <section className="rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6">
      <div className="mb-5 flex flex-wrap items-start justify-between gap-3">
        <div>
          <h2 className="text-base font-semibold text-text">{title}</h2>
          {description && <p className="mt-1 text-sm text-text-muted">{description}</p>}
        </div>
        {action}
      </div>
      {children}
    </section>
  )
}

function StickySaveBar({
  recentlySaved,
  isSaving,
  onSave,
  onReload,
}: {
  recentlySaved: boolean
  isSaving: boolean
  onSave: () => void
  onReload: () => void
}) {
  const { t } = useTranslation()

  return (
    <div className="pointer-events-none fixed inset-x-0 bottom-0 z-40 px-4 pb-4 sm:px-6 lg:px-8">
      <div className="pointer-events-auto mx-auto max-w-5xl rounded-sm border border-border bg-surface/95 p-3 shadow-lg backdrop-blur-md">
        <div className="flex flex-wrap items-center justify-between gap-3">
          <p className={`text-sm ${recentlySaved ? 'text-warm' : 'text-text-muted'}`}>
            {recentlySaved
              ? t('dashboard.adminProfile.savedHint')
              : t('dashboard.adminProfile.saveReminder')}
          </p>
          <div className="flex flex-wrap items-center gap-2">
            <Button variant="secondary" className="text-xs" onClick={onReload}>
              {t('dashboard.adminProfile.reloadFromServer')}
            </Button>
            <Button variant="warm" className="min-w-[9rem] text-xs" onClick={onSave} disabled={isSaving}>
              {isSaving ? (
                <InlineLoading label={t('dashboard.adminProfile.saving')} />
              ) : (
                t('dashboard.adminProfile.save')
              )}
            </Button>
          </div>
        </div>
      </div>
    </div>
  )
}

function IconButton({
  label,
  disabled,
  onClick,
  children,
}: {
  label: string
  disabled?: boolean
  onClick: () => void
  children: ReactNode
}) {
  return (
    <button
      type="button"
      aria-label={label}
      title={label}
      disabled={disabled}
      onClick={onClick}
      className="flex size-8 items-center justify-center rounded-sm border border-border text-sm text-text-muted hover:bg-surface-muted disabled:opacity-40"
    >
      {children}
    </button>
  )
}

const inputClass =
  'w-full rounded-sm border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm focus:ring-2 focus:ring-warm/15'

function Field({ label, children }: { label: string; children: ReactNode }) {
  return (
    <label className="block">
      <span className="mb-1.5 block text-xs font-medium text-text-muted">{label}</span>
      {children}
    </label>
  )
}
