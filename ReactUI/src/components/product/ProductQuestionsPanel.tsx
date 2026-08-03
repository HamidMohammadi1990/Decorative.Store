import { useMemo, useState } from 'react'
import type { TFunction } from 'i18next'
import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { QuestionSubmitModal } from '@/components/product/QuestionSubmitModal'
import type { ProductQuestionItem } from '@/extensions/productQuestions'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import { useProductQuestions } from '@/hooks/useProductQuestions'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { productQuestionService } from '@/services/productQuestionService'
import { openLoginModal } from '@/stores/authModalStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken, useIsAuthenticated } from '@/stores/userStore'

interface ProductQuestionsPanelProps {
  product: ProductDetail
}

type SortId = 'newest' | 'mostAnswers'

const INITIAL_VISIBLE = 5
const MOBILE_ANSWER_LIMIT = 90

export function ProductQuestionsPanel({ product }: ProductQuestionsPanelProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const isAuthenticated = useIsAuthenticated()
  const accessToken = useAccessToken()
  const { questions: allQuestions, loading, reload } = useProductQuestions(product.id)
  const mobileDrag = useHorizontalDragScroll<HTMLDivElement>()

  const [sort, setSort] = useState<SortId>('newest')
  const [expanded, setExpanded] = useState(false)
  const [mobileShowAll, setMobileShowAll] = useState(false)
  const [questionModalOpen, setQuestionModalOpen] = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)

  const sortedQuestions = useMemo(() => {
    const list = [...allQuestions]
    if (sort === 'mostAnswers') {
      return list.sort((a, b) => b.answerCount - a.answerCount)
    }
    return list
  }, [allQuestions, sort])

  const answeredQuestions = sortedQuestions.filter((question) => question.answer)
  const visibleQuestions = expanded ? sortedQuestions : sortedQuestions.slice(0, INITIAL_VISIBLE)
  const hiddenCount = Math.max(0, sortedQuestions.length - INITIAL_VISIBLE)

  const sortOptions: { id: SortId; label: string }[] = [
    { id: 'newest', label: t('product.sortNewest') },
    { id: 'mostAnswers', label: t('product.sortMostAnswers') },
  ]

  const openQuestionModal = () => {
    if (!isAuthenticated) {
      openLoginModal({
        onSuccess: () => {
          setSubmitError(null)
          setQuestionModalOpen(true)
        },
      })
      return
    }
    setSubmitError(null)
    setQuestionModalOpen(true)
  }

  const handleSubmitQuestion = async (question: string) => {
    if (!accessToken) return

    setSubmitting(true)
    setSubmitError(null)

    try {
      await productQuestionService.create(
        { productId: product.id, question },
        locale,
        accessToken,
      )
      setQuestionModalOpen(false)
      await reload()
    } catch {
      setSubmitError(t('product.questionSubmitFailed'))
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div>
      <QuestionSubmitModal
        isOpen={questionModalOpen}
        onClose={() => setQuestionModalOpen(false)}
        onSubmit={handleSubmitQuestion}
        submitting={submitting}
        submitError={submitError}
      />

      {loading && (
        <p className="py-6 text-center text-sm text-text-muted">{t('common.loading')}</p>
      )}

      {!loading && sortedQuestions.length === 0 && (
        <p className="py-6 text-center text-sm text-text-muted">{t('product.noQuestionsYet')}</p>
      )}

      {!loading && sortedQuestions.length > 0 && (
        <>
          <div className="lg:hidden">
            <div className="flex items-start justify-between gap-3">
              <h3 className="text-base font-bold text-text">{t('product.questionsMobileTitle')}</h3>
              <button
                type="button"
                onClick={() => setMobileShowAll((value) => !value)}
                className="inline-flex shrink-0 items-center gap-0.5 text-sm font-medium text-warm"
              >
                {t('product.viewAllQuestions', { count: sortedQuestions.length })}
                <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
              </button>
            </div>

            {!mobileShowAll ? (
              <>
                <div
                  ref={mobileDrag.ref}
                  className={`-mx-4 mt-4 flex gap-3 overflow-x-auto px-4 pb-1 touch-pan-y select-none ${
                    mobileDrag.isGrabbing ? 'cursor-grabbing snap-none' : 'cursor-grab snap-x snap-mandatory'
                  }`}
                  onPointerDown={mobileDrag.onPointerDown}
                  onPointerMove={mobileDrag.onPointerMove}
                  onPointerUp={mobileDrag.onPointerUp}
                  onLostPointerCapture={mobileDrag.onLostPointerCapture}
                  onClickCapture={mobileDrag.onClickCapture}
                  onDragStart={(event) => event.preventDefault()}
                >
                  {answeredQuestions.map((item) => (
                    <MobileQuestionCard key={item.id} item={item} t={t} />
                  ))}
                </div>

                <button
                  type="button"
                  onClick={openQuestionModal}
                  className="mt-4 flex w-full items-center justify-between gap-3 rounded-lg bg-surface-muted px-4 py-3.5 text-sm text-text-muted transition-colors hover:bg-surface-muted/80"
                >
                  <span>{t('product.askQuestionBar')}</span>
                  <span className="flex size-8 shrink-0 items-center justify-center rounded-full border border-border bg-surface text-warm">
                    <QuestionMarkIcon />
                  </span>
                </button>
              </>
            ) : (
              <ul className="mt-4 divide-y divide-border">
                {sortedQuestions.map((item) => (
                  <QuestionRow key={item.id} item={item} t={t} compact />
                ))}
              </ul>
            )}
          </div>

          <div className="hidden lg:block">
            <h3 className="text-base font-bold text-text">
              {t('product.questionsSectionTitle')}
              <span className="mt-2 block h-0.5 w-10 bg-warm" aria-hidden />
            </h3>

            <div className="mt-6 grid grid-cols-[15rem_minmax(0,1fr)] gap-10">
              <aside className="flex flex-col items-center border-s border-border ps-8 text-center">
                <p className="text-sm leading-relaxed text-text-muted">{t('product.questionInvite')}</p>
                <button
                  type="button"
                  onClick={openQuestionModal}
                  className="mt-4 w-full rounded-md border border-warm bg-surface px-4 py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft"
                >
                  {t('product.askQuestion')}
                </button>
              </aside>

              <div className="min-w-0">
                <div className="flex flex-wrap items-center justify-between gap-3 border-b border-border pb-3">
                  <div className="flex flex-wrap items-center gap-1 text-sm">
                    <span className="text-text-muted">{t('product.sortBy')}:</span>
                    {sortOptions.map((option, index) => (
                      <span key={option.id} className="inline-flex items-center">
                        {index > 0 && (
                          <span className="mx-1.5 text-border-strong" aria-hidden>
                            |
                          </span>
                        )}
                        <button
                          type="button"
                          onClick={() => setSort(option.id)}
                          className={
                            sort === option.id
                              ? 'font-semibold text-warm'
                              : 'text-text-muted transition-colors hover:text-text'
                          }
                        >
                          {option.label}
                        </button>
                      </span>
                    ))}
                  </div>
                  <span className="text-sm text-text-muted">
                    {t('product.questionsTotal', { count: sortedQuestions.length })}
                  </span>
                </div>

                <ul>
                  {visibleQuestions.map((item) => (
                    <QuestionRow key={item.id} item={item} t={t} />
                  ))}
                </ul>

                {!expanded && hiddenCount > 0 && (
                  <button
                    type="button"
                    onClick={() => setExpanded(true)}
                    className="mt-2 flex w-full items-center justify-center gap-2 border-t border-border pt-4 text-sm font-medium text-warm hover:underline"
                  >
                    {t('product.moreQuestions', { count: hiddenCount })}
                    <ChevronIcon expanded={false} className="text-warm" />
                  </button>
                )}
              </div>
            </div>
          </div>
        </>
      )}

      {!loading && (
        <button
          type="button"
          onClick={openQuestionModal}
          className="mt-4 w-full rounded-md border border-warm bg-surface py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-warm-soft lg:hidden"
        >
          {t('product.askQuestion')}
        </button>
      )}
    </div>
  )
}

function QuestionRow({
  item,
  t,
  compact = false,
}: {
  item: ProductQuestionItem
  t: TFunction
  compact?: boolean
}) {
  const hasAnswer = Boolean(item.answer)

  return (
    <li className={`border-b border-border ${compact ? 'py-4' : 'py-5'} last:border-b-0`}>
      <p className="text-sm font-medium leading-relaxed text-text">{item.question}</p>

      {hasAnswer ? (
        <div className="mt-3 rounded-lg bg-surface-muted px-4 py-3">
          <div className="flex flex-wrap items-center gap-2">
            <UserAvatar name={item.author ?? '?'} size="sm" />
            <span className="text-sm font-semibold text-text">{item.author}</span>
            {item.isBuyer && (
              <span className="rounded-md bg-accent/10 px-2 py-0.5 text-[11px] font-medium text-accent">
                {t('product.buyerBadge')}
              </span>
            )}
            {item.isExpert && (
              <span className="inline-flex items-center gap-1 text-[11px] font-medium text-accent">
                <ExpertStarIcon />
                {t('product.expertBadge')}
              </span>
            )}
          </div>
          <p className="mt-2 text-sm leading-relaxed text-text-muted">{item.answer}</p>
          {!compact && <p className="mt-2 text-xs text-text-muted">{item.date}</p>}
        </div>
      ) : (
        <p className="mt-3 text-xs text-text-muted">{t('product.awaitingAnswer')}</p>
      )}
    </li>
  )
}

function MobileQuestionCard({ item, t }: { item: ProductQuestionItem; t: TFunction }) {
  const answerText = item.answer ?? ''
  const displayAnswer =
    answerText.length > MOBILE_ANSWER_LIMIT
      ? `${answerText.slice(0, MOBILE_ANSWER_LIMIT)}…`
      : answerText

  return (
    <article className="w-[calc(50%-0.375rem)] min-w-[calc(50%-0.375rem)] shrink-0 snap-start rounded-lg border border-border bg-surface p-3.5">
      <p className="text-sm font-semibold leading-relaxed text-text">{item.question}</p>

      {item.answer && (
        <div className="mt-3 rounded-lg bg-surface-muted p-3">
          <div className="flex items-start gap-2">
            <UserAvatar name={item.author ?? '?'} />
            <div className="min-w-0 flex-1">
              <div className="flex flex-wrap items-center gap-1.5">
                <span className="truncate text-xs font-semibold text-text">{item.author}</span>
                {item.isBuyer && (
                  <span className="shrink-0 rounded bg-accent/10 px-1.5 py-0.5 text-[10px] font-medium text-accent">
                    {t('product.buyerBadge')}
                  </span>
                )}
              </div>
              {item.isExpert && (
                <span className="mt-1 inline-flex items-center gap-1 text-[10px] font-medium text-accent">
                  <ExpertStarIcon />
                  {t('product.expertBadge')}
                </span>
              )}
            </div>
          </div>
          <p className="mt-2 text-xs leading-relaxed text-text-muted">{displayAnswer}</p>
        </div>
      )}

      <div className="mt-3 flex items-center justify-between gap-2 text-[11px] text-text-muted">
        <span>{item.date}</span>
        <div className="flex items-center gap-3">
          <span className="inline-flex items-center gap-1">
            <ThumbUpIcon />
            {item.helpful}
          </span>
          <span className="inline-flex items-center gap-1">
            <ThumbDownIcon />
            {item.notHelpful}
          </span>
        </div>
      </div>
    </article>
  )
}

function UserAvatar({ name, size = 'md' }: { name: string; size?: 'sm' | 'md' }) {
  const initial = name.trim().charAt(0) || '?'
  const dim = size === 'sm' ? 'size-7 text-xs' : 'size-8 text-xs'

  return (
    <span
      className={`inline-flex shrink-0 items-center justify-center rounded-full bg-amber-100 font-semibold text-amber-700 ${dim}`}
      aria-hidden
    >
      {initial}
    </span>
  )
}

function QuestionMarkIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
      <circle cx="8" cy="8" r="6.5" stroke="currentColor" strokeWidth="1.2" />
      <path
        d="M8 11.5v.5M6.2 6.2a1.8 1.8 0 013.1 1.3c0 1.2-1.3 1.5-1.3 2.5"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinecap="round"
      />
    </svg>
  )
}

function ExpertStarIcon() {
  return (
    <svg width="12" height="12" viewBox="0 0 12 12" fill="currentColor" aria-hidden>
      <path d="M6 1.2l1.4 2.9 3.1.5-2.2 2.2.5 3.1L6 8.4 3.2 10l.5-3.1-2.2-2.2 3.1-.5L6 1.2z" />
    </svg>
  )
}

function ThumbUpIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M4 10V5.5L5.5 3h2.8l.7 2.5H10v3.5H4zM2 5.5h1.5V10H2V5.5z"
        stroke="currentColor"
        strokeWidth="0.9"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function ThumbDownIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M4 2v4.5L5.5 9h2.8l.7-2.5H10V3H4zM2 6.5h1.5V2H2v4.5z"
        stroke="currentColor"
        strokeWidth="0.9"
        strokeLinejoin="round"
      />
    </svg>
  )
}
