import { useMemo, useState } from 'react'
import type { TFunction } from 'i18next'
import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { QuestionSubmitModal } from '@/components/product/QuestionSubmitModal'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

interface ProductQuestionsPanelProps {
  product: ProductDetail
}

type SortId = 'newest' | 'mostAnswers'

interface QuestionItem {
  id: string
  question: string
  answer?: string
  author?: string
  date?: string
  isBuyer?: boolean
  isExpert?: boolean
  answerCount: number
  helpful: number
  notHelpful: number
}

const INITIAL_VISIBLE = 5
const MOBILE_ANSWER_LIMIT = 90

const MOCK_QUESTIONS: Record<'en' | 'fa', QuestionItem[]> = {
  en: [
    {
      id: 'q1',
      question: 'What grind size works best for espresso with this machine?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q2',
      question: 'Does the espresso come out hot enough, or should I preheat the cup?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q3',
      question: 'How can I tell if this is an original Diba Gallery piece?',
      answer:
        'All Diba Gallery furniture includes a sewn-in label and order documentation. You can also verify via our support team with your order number.',
      author: 'Maede Hasani',
      date: '12 May 2026',
      isBuyer: true,
      answerCount: 2,
      helpful: 17,
      notHelpful: 1,
    },
    {
      id: 'q4',
      question: 'Is white-glove delivery available for this sofa?',
      answer:
        'Yes — white-glove delivery is available in most metro areas. Measure doorways before ordering.',
      author: 'Diba Gallery Support',
      date: '3 days ago',
      answerCount: 1,
      helpful: 8,
      notHelpful: 0,
    },
    {
      id: 'q5',
      question: 'Can the velvet covers be removed for washing?',
      answer:
        'The covers are removable for spot-cleaning. Professional cleaning is recommended once a year.',
      author: 'Alireza Ghasemi',
      date: '20 Apr 2026',
      isBuyer: true,
      isExpert: true,
      answerCount: 3,
      helpful: 24,
      notHelpful: 2,
    },
    {
      id: 'q6',
      question: 'What is the seat depth for everyday lounging?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q7',
      question: 'Are replacement legs sold separately?',
      answer: 'Replacement hardware kits are available through customer care.',
      author: 'Diba Gallery Support',
      date: '2 weeks ago',
      answerCount: 1,
      helpful: 5,
      notHelpful: 0,
    },
    {
      id: 'q8',
      question: 'Does the green velvet match the website photos?',
      answer:
        'Colour can vary slightly by screen. We recommend ordering a free swatch first.',
      author: 'Sara Mohammadi',
      date: '1 month ago',
      isBuyer: true,
      answerCount: 2,
      helpful: 11,
      notHelpful: 0,
    },
    {
      id: 'q9',
      question: 'How long is the warranty on the frame?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q10',
      question: 'Can this sofa fit through a standard apartment doorway?',
      answer:
        'The widest point is 95 cm. Most standard doorways accommodate it when carried upright.',
      author: 'Diba Gallery Support',
      date: '6 weeks ago',
      answerCount: 1,
      helpful: 9,
      notHelpful: 1,
    },
    {
      id: 'q11',
      question: 'Is assembly required on delivery?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q12',
      question: 'Do you offer fabric swatches for this collection?',
      answer: 'Complimentary swatches ship within 3–5 business days.',
      author: 'Diba Gallery Support',
      date: '2 months ago',
      answerCount: 1,
      helpful: 6,
      notHelpful: 0,
    },
    {
      id: 'q13',
      question: 'What is the return window if the colour does not match?',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q14',
      question: 'Is this suitable for a small living room?',
      answer:
        'At 220 cm wide it suits medium spaces. We suggest measuring your layout before purchase.',
      author: 'Hamid M.',
      date: '3 months ago',
      isBuyer: true,
      answerCount: 1,
      helpful: 4,
      notHelpful: 0,
    },
    {
      id: 'q15',
      question: 'Can I pair this with the Harmony armchair?',
      answer: 'Yes — both pieces share complementary linen and velvet textures.',
      author: 'Diba Gallery Support',
      date: '4 months ago',
      answerCount: 1,
      helpful: 7,
      notHelpful: 0,
    },
  ],
  fa: [
    {
      id: 'q1',
      question: 'برای اسپرسو با این دستگاه چه درجه آسیابی مناسب‌تر است؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q2',
      question: 'دمای اسپرسو چقدر است؟ آیا لازم است فنجان را از قبل گرم کنم؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q3',
      question: 'چطور می‌توان اصل بودن محصول را تشخیص داد؟',
      answer:
        'همه محصولات دیبا گالری برچسب دوخته‌شده و مدارک سفارش دارند. با پشتیبانی و شماره سفارش هم قابل استعلام است.',
      author: 'مائده حسنی قاین',
      date: '۲۰ اردیبهشت ۱۴۰۵',
      isBuyer: true,
      answerCount: 2,
      helpful: 17,
      notHelpful: 1,
    },
    {
      id: 'q4',
      question: 'آیا ارسال ویژه برای این مبل موجود است؟',
      answer:
        'بله — در بیشتر شهرها ارسال ویژه فعال است. قبل از سفارش ابعاد درب‌ها را اندازه بگیرید.',
      author: 'پشتیبانی دیبا گالری',
      date: '۳ روز پیش',
      answerCount: 1,
      helpful: 8,
      notHelpful: 0,
    },
    {
      id: 'q5',
      question: 'آیا روکش مخمل قابل جداسازی و شستشو است؟',
      answer:
        'روکش قابل جداسازی است و برای لکه‌گیری مناسب است. شستشوی تخصصی سالانه توصیه می‌شود.',
      author: 'علیرضا قاسمی',
      date: '۲۰ اردیبهشت ۱۴۰۵',
      isBuyer: true,
      isExpert: true,
      answerCount: 3,
      helpful: 24,
      notHelpful: 2,
    },
    {
      id: 'q6',
      question: 'عمق نشیمن برای استراحت روزمره چقدر است؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q7',
      question: 'آیا پایه‌های یدکی جداگانه فروخته می‌شود؟',
      answer: 'کیت یراق‌آلات یدکی از پشتیبانی قابل سفارش است.',
      author: 'پشتیبانی دیبا گالری',
      date: '۲ هفته پیش',
      answerCount: 1,
      helpful: 5,
      notHelpful: 0,
    },
    {
      id: 'q8',
      question: 'آیا رنگ مخمل سبز با عکس سایت یکی است؟',
      answer: 'رنگ ممکن است کمی با نمایشگر متفاوت باشد. پیشنهاد می‌کنیم نمونه پارچه بگیرید.',
      author: 'سارا محمدی',
      date: '۱ ماه پیش',
      isBuyer: true,
      answerCount: 2,
      helpful: 11,
      notHelpful: 0,
    },
    {
      id: 'q9',
      question: 'گارانتی قاب چند سال است؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q10',
      question: 'آیا از درب آپارتمان استاندارد رد می‌شود؟',
      answer: 'عرضی‌ترین بخش ۹۵ سانتی‌متر است. در حالت عمودی معمولاً از درب عبور می‌کند.',
      author: 'پشتیبانی دیبا گالری',
      date: '۶ هفته پیش',
      answerCount: 1,
      helpful: 9,
      notHelpful: 1,
    },
    {
      id: 'q11',
      question: 'آیا مونتاژ هنگام تحویل لازم است؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q12',
      question: 'نمونه پارچه برای این مجموعه دارید؟',
      answer: 'نمونه پارچه رایگان در ۳ تا ۵ روز کاری ارسال می‌شود.',
      author: 'پشتیبانی دیبا گالری',
      date: '۲ ماه پیش',
      answerCount: 1,
      helpful: 6,
      notHelpful: 0,
    },
    {
      id: 'q13',
      question: 'اگر رنگ مطابق نبود مهلت مرجوعی چقدر است؟',
      answerCount: 0,
      helpful: 0,
      notHelpful: 0,
    },
    {
      id: 'q14',
      question: 'برای نشیمن کوچک مناسب است؟',
      answer: 'با عرض ۲۲۰ سانتی‌متر برای فضاهای متوسط مناسب است. قبل از خرید اندازه بگیرید.',
      author: 'حمید م.',
      date: '۳ ماه پیش',
      isBuyer: true,
      answerCount: 1,
      helpful: 4,
      notHelpful: 0,
    },
    {
      id: 'q15',
      question: 'آیا با صندلی هارمونی ست می‌شود؟',
      answer: 'بله — بافت پارچه و مخمل این دو محصول با هم هماهنگ است.',
      author: 'پشتیبانی دیبا گالری',
      date: '۴ ماه پیش',
      answerCount: 1,
      helpful: 7,
      notHelpful: 0,
    },
  ],
}

export function ProductQuestionsPanel({ product: _product }: ProductQuestionsPanelProps) {
  const { t, i18n } = useTranslation()
  const locale = i18n.language === 'fa' ? 'fa' : 'en'
  const allQuestions = MOCK_QUESTIONS[locale]
  const mobileDrag = useHorizontalDragScroll<HTMLDivElement>()

  const [sort, setSort] = useState<SortId>('newest')
  const [expanded, setExpanded] = useState(false)
  const [mobileShowAll, setMobileShowAll] = useState(false)
  const [questionModalOpen, setQuestionModalOpen] = useState(false)

  const sortedQuestions = useMemo(() => {
    const list = [...allQuestions]
    if (sort === 'mostAnswers') {
      return list.sort((a, b) => b.answerCount - a.answerCount)
    }
    return list
  }, [allQuestions, sort])

  const answeredQuestions = sortedQuestions.filter((q) => q.answer)
  const visibleQuestions = expanded ? sortedQuestions : sortedQuestions.slice(0, INITIAL_VISIBLE)
  const hiddenCount = Math.max(0, sortedQuestions.length - INITIAL_VISIBLE)

  const sortOptions: { id: SortId; label: string }[] = [
    { id: 'newest', label: t('product.sortNewest') },
    { id: 'mostAnswers', label: t('product.sortMostAnswers') },
  ]

  const openQuestionModal = () => setQuestionModalOpen(true)

  return (
    <div>
      <QuestionSubmitModal
        isOpen={questionModalOpen}
        onClose={() => setQuestionModalOpen(false)}
      />

      {/* ── Mobile ── */}
      <div className="lg:hidden">
        <div className="flex items-start justify-between gap-3">
          <h3 className="text-base font-bold text-text">{t('product.questionsMobileTitle')}</h3>
          <button
            type="button"
            onClick={() => setMobileShowAll((v) => !v)}
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
              onDragStart={(e) => e.preventDefault()}
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

      {/* ── Desktop ── */}
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
    </div>
  )
}

function QuestionRow({
  item,
  t,
  compact = false,
}: {
  item: QuestionItem
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
          {!compact && (
            <p className="mt-2 text-xs text-text-muted">{item.date}</p>
          )}
        </div>
      ) : (
        <button
          type="button"
          className="mt-3 inline-flex items-center gap-1.5 text-sm font-medium text-accent hover:underline"
        >
          <PencilIcon />
          {t('product.submitAnswer')}
        </button>
      )}
    </li>
  )
}

function MobileQuestionCard({ item, t }: { item: QuestionItem; t: TFunction }) {
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

function PencilIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 12 12" fill="none" aria-hidden>
      <path
        d="M8.5 1.5l2 2L4 10H2v-2L8.5 1.5z"
        stroke="currentColor"
        strokeWidth="1"
        strokeLinejoin="round"
      />
    </svg>
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
