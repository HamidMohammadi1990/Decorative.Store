import type { ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'
import {
  isSelectType,
  patchLocalized,
  useProfileCompletionAdminStore,
} from '@/stores/profileCompletionAdminStore'
import type { AdminProfileQuestion, AdminQuestionOption } from '@/models/profile/profileCompletionAdmin.model'
import type { ProfileQuestionType } from '@/models/profile/profileCompletion.model'

const QUESTION_TYPES: ProfileQuestionType[] = [
  'text',
  'textarea',
  'tel',
  'date',
  'single_select',
  'multi_select',
]

export function ProfileQuestionsAdminPanel() {
  const { t } = useTranslation()
  const config = useProfileCompletionAdminStore((s) => s.config)
  const updateCampaign = useProfileCompletionAdminStore((s) => s.updateCampaign)
  const addQuestion = useProfileCompletionAdminStore((s) => s.addQuestion)
  const updateQuestion = useProfileCompletionAdminStore((s) => s.updateQuestion)
  const removeQuestion = useProfileCompletionAdminStore((s) => s.removeQuestion)
  const moveQuestion = useProfileCompletionAdminStore((s) => s.moveQuestion)
  const addOption = useProfileCompletionAdminStore((s) => s.addOption)
  const updateOption = useProfileCompletionAdminStore((s) => s.updateOption)
  const removeOption = useProfileCompletionAdminStore((s) => s.removeOption)
  const resetToDefaults = useProfileCompletionAdminStore((s) => s.resetToDefaults)

  const sortedQuestions = [...config.questions].sort((a, b) => a.order - b.order)

  return (
    <div>
      <div className="mb-6 flex flex-wrap items-start justify-between gap-3 border-b border-border/70 pb-5">
        <div>
          <h2 className="text-lg font-semibold text-text">{t('dashboard.adminProfile.title')}</h2>
          <p className="mt-1 max-w-2xl text-sm text-text-muted">
            {t('dashboard.adminProfile.description')}
          </p>
        </div>
        <Button variant="secondary" className="text-xs" onClick={resetToDefaults}>
          {t('dashboard.adminProfile.resetDefaults')}
        </Button>
      </div>

      <section className="mb-8 rounded-sm border border-border bg-gradient-to-br from-warm-soft/40 to-surface p-5 shadow-sm sm:p-6">
        <h2 className="text-sm font-semibold uppercase tracking-[0.12em] text-text-muted">
          {t('dashboard.adminProfile.campaignTitle')}
        </h2>

        <div className="mt-4 grid gap-4 lg:grid-cols-2">
          <LocalizedField
            label={t('dashboard.adminProfile.fieldTitle')}
            en={config.titles.en}
            fa={config.titles.fa}
            onEnChange={(value) => updateCampaign({ titles: patchLocalized(config.titles, 'en', value) })}
            onFaChange={(value) => updateCampaign({ titles: patchLocalized(config.titles, 'fa', value) })}
          />
          <LocalizedField
            label={t('dashboard.adminProfile.fieldSubtitle')}
            en={config.subtitles.en}
            fa={config.subtitles.fa}
            onEnChange={(value) =>
              updateCampaign({ subtitles: patchLocalized(config.subtitles, 'en', value) })
            }
            onFaChange={(value) =>
              updateCampaign({ subtitles: patchLocalized(config.subtitles, 'fa', value) })
            }
          />
        </div>

        <div className="mt-5 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <Field label={t('dashboard.adminProfile.rewardType')}>
            <select
              value={config.reward.type}
              onChange={(e) =>
                updateCampaign({
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
                updateCampaign({
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
                updateCampaign({ reward: { ...config.reward, code: e.target.value.toUpperCase() } })
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
                updateCampaign({
                  reward: { ...config.reward, validDays: Number(e.target.value) || 1 },
                })
              }
              className={inputClass}
            />
          </Field>
        </div>

        <div className="mt-4">
          <LocalizedField
            label={t('dashboard.adminProfile.rewardDescription')}
            en={config.reward.descriptions.en}
            fa={config.reward.descriptions.fa}
            onEnChange={(value) =>
              updateCampaign({
                reward: {
                  ...config.reward,
                  descriptions: patchLocalized(config.reward.descriptions, 'en', value),
                },
              })
            }
            onFaChange={(value) =>
              updateCampaign({
                reward: {
                  ...config.reward,
                  descriptions: patchLocalized(config.reward.descriptions, 'fa', value),
                },
              })
            }
          />
        </div>
      </section>

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div>
          <h2 className="text-sm font-semibold uppercase tracking-[0.12em] text-text-muted">
            {t('dashboard.adminProfile.questionsTitle')}
          </h2>
          <p className="mt-1 text-xs text-text-muted">
            {t('dashboard.adminProfile.questionsCount', { count: sortedQuestions.length })}
          </p>
        </div>
        <Button variant="warm" className="text-xs" onClick={addQuestion}>
          {t('dashboard.adminProfile.addQuestion')}
        </Button>
      </div>

      <div className="space-y-4">
        {sortedQuestions.length === 0 ? (
          <div className="rounded-sm border border-dashed border-border bg-surface-muted/30 px-5 py-10 text-center">
            <p className="text-sm text-text-muted">{t('dashboard.adminProfile.emptyQuestions')}</p>
            <Button variant="warm" className="mt-4 text-xs" onClick={addQuestion}>
              {t('dashboard.adminProfile.addFirstQuestion')}
            </Button>
          </div>
        ) : (
          sortedQuestions.map((question, index) => (
            <QuestionEditor
              key={question.id}
              question={question}
              index={index}
              total={sortedQuestions.length}
              onUpdate={(patch) => updateQuestion(question.id, patch)}
              onRemove={() => removeQuestion(question.id)}
              onMove={(direction) => moveQuestion(question.id, direction)}
              onAddOption={() => addOption(question.id)}
              onUpdateOption={(optionIndex, patch) =>
                updateOption(question.id, optionIndex, patch)
              }
              onRemoveOption={(optionIndex) => removeOption(question.id, optionIndex)}
            />
          ))
        )}
      </div>
    </div>
  )
}

function QuestionEditor({
  question,
  index,
  total,
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
  onUpdate: (patch: Partial<AdminProfileQuestion>) => void
  onRemove: () => void
  onMove: (direction: 'up' | 'down') => void
  onAddOption: () => void
  onUpdateOption: (optionIndex: number, patch: Partial<AdminQuestionOption>) => void
  onRemoveOption: (optionIndex: number) => void
}) {
  const { t } = useTranslation()
  const showOptions = isSelectType(question.type)

  return (
    <article className="rounded-sm border border-border bg-surface p-4 shadow-sm sm:p-5">
      <div className="flex flex-wrap items-start justify-between gap-3 border-b border-border/70 pb-4">
        <div>
          <p className="text-xs font-semibold uppercase tracking-wide text-warm">
            {t('dashboard.adminProfile.questionNumber', { number: index + 1 })}
          </p>
          <p className="mt-1 font-mono text-[11px] text-text-muted">{question.id}</p>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <button
            type="button"
            disabled={index === 0}
            onClick={() => onMove('up')}
            className="rounded-sm border border-border px-2 py-1 text-xs text-text-muted hover:bg-surface-muted disabled:opacity-40"
          >
            {t('dashboard.adminProfile.moveUp')}
          </button>
          <button
            type="button"
            disabled={index === total - 1}
            onClick={() => onMove('down')}
            className="rounded-sm border border-border px-2 py-1 text-xs text-text-muted hover:bg-surface-muted disabled:opacity-40"
          >
            {t('dashboard.adminProfile.moveDown')}
          </button>
          <button
            type="button"
            onClick={onRemove}
            className="rounded-sm border border-sale/30 px-2 py-1 text-xs text-sale hover:bg-sale/5"
          >
            {t('dashboard.adminProfile.deleteQuestion')}
          </button>
        </div>
      </div>

      <div className="mt-4 grid gap-4 sm:grid-cols-2">
        <Field label={t('dashboard.adminProfile.questionType')}>
          <select
            value={question.type}
            onChange={(e) => {
              const type = e.target.value as ProfileQuestionType
              onUpdate({
                type,
                options: isSelectType(type) && question.options.length === 0
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
          <label className="inline-flex items-center gap-2 text-sm text-text">
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

      <div className="mt-4">
        <LocalizedField
          label={t('dashboard.adminProfile.questionLabel')}
          en={question.labels.en}
          fa={question.labels.fa}
          onEnChange={(value) =>
            onUpdate({ labels: patchLocalized(question.labels, 'en', value) })
          }
          onFaChange={(value) =>
            onUpdate({ labels: patchLocalized(question.labels, 'fa', value) })
          }
        />
      </div>

      <div className="mt-4 grid gap-4 lg:grid-cols-2">
        <LocalizedField
          label={t('dashboard.adminProfile.questionHint')}
          en={question.hints.en}
          fa={question.hints.fa}
          onEnChange={(value) => onUpdate({ hints: patchLocalized(question.hints, 'en', value) })}
          onFaChange={(value) => onUpdate({ hints: patchLocalized(question.hints, 'fa', value) })}
        />
        <LocalizedField
          label={t('dashboard.adminProfile.questionPlaceholder')}
          en={question.placeholders.en}
          fa={question.placeholders.fa}
          onEnChange={(value) =>
            onUpdate({ placeholders: patchLocalized(question.placeholders, 'en', value) })
          }
          onFaChange={(value) =>
            onUpdate({ placeholders: patchLocalized(question.placeholders, 'fa', value) })
          }
        />
      </div>

      {showOptions && (
        <div className="mt-5 rounded-sm border border-border/70 bg-surface-muted/25 p-4">
          <div className="mb-3 flex items-center justify-between gap-3">
            <p className="text-xs font-semibold uppercase tracking-wide text-text-muted">
              {t('dashboard.adminProfile.optionsTitle')}
            </p>
            <button
              type="button"
              onClick={onAddOption}
              className="text-xs font-semibold text-warm hover:underline"
            >
              {t('dashboard.adminProfile.addOption')}
            </button>
          </div>
          <div className="space-y-3">
            {question.options.map((option, optionIndex) => (
              <div
                key={`${question.id}-${optionIndex}`}
                className="grid gap-3 rounded-sm border border-border bg-surface p-3 lg:grid-cols-[minmax(0,1fr)_auto]"
              >
                <div className="grid gap-3 sm:grid-cols-3">
                  <Field label={t('dashboard.adminProfile.optionValue')}>
                    <input
                      type="text"
                      value={option.value}
                      onChange={(e) =>
                        onUpdateOption(optionIndex, { value: e.target.value })
                      }
                      className={`${inputClass} font-mono text-xs`}
                    />
                  </Field>
                  <Field label="English">
                    <input
                      type="text"
                      value={option.labels.en}
                      onChange={(e) =>
                        onUpdateOption(optionIndex, {
                          labels: patchLocalized(option.labels, 'en', e.target.value),
                        })
                      }
                      className={inputClass}
                    />
                  </Field>
                  <Field label="فارسی">
                    <input
                      type="text"
                      value={option.labels.fa}
                      onChange={(e) =>
                        onUpdateOption(optionIndex, {
                          labels: patchLocalized(option.labels, 'fa', e.target.value),
                        })
                      }
                      className={inputClass}
                      dir="rtl"
                    />
                  </Field>
                </div>
                <button
                  type="button"
                  onClick={() => onRemoveOption(optionIndex)}
                  className="self-end rounded-sm px-2 py-1 text-xs text-sale hover:bg-sale/5 lg:self-center"
                >
                  {t('dashboard.adminProfile.deleteOption')}
                </button>
              </div>
            ))}
          </div>
        </div>
      )}
    </article>
  )
}

const inputClass =
  'w-full rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text outline-none transition-colors focus:border-warm focus:ring-2 focus:ring-warm/15'

function Field({ label, children }: { label: string; children: ReactNode }) {
  return (
    <label className="block">
      <span className="mb-1.5 block text-xs font-medium text-text-muted">{label}</span>
      {children}
    </label>
  )
}

function LocalizedField({
  label,
  en,
  fa,
  onEnChange,
  onFaChange,
}: {
  label: string
  en: string
  fa: string
  onEnChange: (value: string) => void
  onFaChange: (value: string) => void
}) {
  return (
    <div>
      <p className="mb-2 text-xs font-medium text-text-muted">{label}</p>
      <div className="grid gap-3 sm:grid-cols-2">
        <Field label="English">
          <input type="text" value={en} onChange={(e) => onEnChange(e.target.value)} className={inputClass} />
        </Field>
        <Field label="فارسی">
          <input
            type="text"
            value={fa}
            onChange={(e) => onFaChange(e.target.value)}
            className={inputClass}
            dir="rtl"
          />
        </Field>
      </div>
    </div>
  )
}
