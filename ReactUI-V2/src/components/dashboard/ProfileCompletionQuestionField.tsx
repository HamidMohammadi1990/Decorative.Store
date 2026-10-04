import { useTranslation } from 'react-i18next'
import type {
  ProfileAnswerValue,
  ProfileCompletionQuestion,
} from '@/models/profile/profileCompletion.model'
import { PersianDatePicker } from '@/components/ui/PersianDatePicker'
import { useSettingsStore } from '@/stores/settingsStore'

interface ProfileCompletionQuestionFieldProps {
  question: ProfileCompletionQuestion
  value: ProfileAnswerValue | undefined
  onChange: (value: ProfileAnswerValue) => void
  answered: boolean
}

const inputClassName =
  'w-full rounded-sm border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm focus:ring-2 focus:ring-warm/15'

export function ProfileCompletionQuestionField({
  question,
  value,
  onChange,
  answered,
}: ProfileCompletionQuestionFieldProps) {
  const { t } = useTranslation()

  return (
    <article
      className={`rounded-sm border p-4 transition-all duration-200 sm:p-5 ${
        answered
          ? 'border-warm/35 bg-gradient-to-br from-warm-soft/40 to-surface shadow-sm'
          : 'border-border bg-surface'
      }`}
    >
      <div className="flex items-start gap-3">
        <span
          className={`mt-0.5 flex size-7 shrink-0 items-center justify-center rounded-full text-xs font-bold ${
            answered
              ? 'bg-warm text-warm-text shadow-sm'
              : 'bg-surface-muted text-text-muted ring-1 ring-border'
          }`}
          aria-hidden
        >
          {answered ? '✓' : question.order}
        </span>
        <div className="min-w-0 flex-1">
          <h3 className="text-sm font-semibold text-text sm:text-base">
            {question.label}
            {question.required && (
              <span className="ms-1 text-sale" aria-hidden>
                *
              </span>
            )}
          </h3>
          {question.hint && (
            <p className="mt-1 text-xs leading-relaxed text-text-muted">{question.hint}</p>
          )}

          <div className="mt-3">
            <QuestionInput question={question} value={value} onChange={onChange} />
          </div>
        </div>
      </div>
      {question.required && !answered && (
        <p className="mt-3 text-[11px] text-text-muted">{t('dashboard.profileCompletion.requiredHint')}</p>
      )}
    </article>
  )
}

function QuestionInput({
  question,
  value,
  onChange,
}: {
  question: ProfileCompletionQuestion
  value: ProfileAnswerValue | undefined
  onChange: (value: ProfileAnswerValue) => void
}) {
  const locale = useSettingsStore((s) => s.locale)

  switch (question.type) {
    case 'textarea':
      return (
        <textarea
          rows={3}
          value={typeof value === 'string' ? value : ''}
          placeholder={question.placeholder}
          className={`${inputClassName} resize-y`}
          onChange={(e) => onChange(e.target.value)}
        />
      )

    case 'date':
      if (locale === 'fa') {
        return (
          <PersianDatePicker
            value={typeof value === 'string' ? value : ''}
            placeholder={question.placeholder}
            onChange={onChange}
          />
        )
      }
      return (
        <input
          type="date"
          value={typeof value === 'string' ? value : ''}
          className={inputClassName}
          onChange={(e) => onChange(e.target.value)}
        />
      )

    case 'tel':
      return (
        <input
          type="tel"
          inputMode="tel"
          value={typeof value === 'string' ? value : ''}
          placeholder={question.placeholder}
          className={inputClassName}
          onChange={(e) => onChange(e.target.value)}
        />
      )

    case 'single_select':
      return (
        <div className="flex flex-wrap gap-2">
          {(question.options ?? []).map((option) => {
            const selected = value === option.value
            return (
              <button
                key={option.value}
                type="button"
                onClick={() => onChange(option.value)}
                className={`rounded-full border px-3.5 py-2 text-xs font-medium transition-colors sm:text-sm ${
                  selected
                    ? 'border-warm bg-warm text-warm-text shadow-sm'
                    : 'border-border bg-surface text-text hover:border-warm/40 hover:bg-warm-soft hover:text-warm'
                }`}
              >
                {option.label}
              </button>
            )
          })}
        </div>
      )

    case 'multi_select': {
      const selected = Array.isArray(value) ? value : []
      const toggle = (optionValue: string) => {
        if (selected.includes(optionValue)) {
          onChange(selected.filter((v) => v !== optionValue))
          return
        }
        onChange([...selected, optionValue])
      }

      return (
        <div className="flex flex-wrap gap-2">
          {(question.options ?? []).map((option) => {
            const isSelected = selected.includes(option.value)
            return (
              <button
                key={option.value}
                type="button"
                onClick={() => toggle(option.value)}
                className={`rounded-full border px-3.5 py-2 text-xs font-medium transition-colors sm:text-sm ${
                  isSelected
                    ? 'border-warm bg-warm text-warm-text shadow-sm'
                    : 'border-border bg-surface text-text hover:border-warm/40 hover:bg-warm-soft hover:text-warm'
                }`}
              >
                {option.label}
              </button>
            )
          })}
        </div>
      )
    }

    default:
      return (
        <input
          type="text"
          value={typeof value === 'string' ? value : ''}
          placeholder={question.placeholder}
          className={inputClassName}
          onChange={(e) => onChange(e.target.value)}
        />
      )
  }
}
