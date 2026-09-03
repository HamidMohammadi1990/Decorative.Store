import type { GuideDiagramId } from '@/models/dashboard/siteManagementGuide.model'

interface DiagramProps {
  className?: string
  locale?: 'fa' | 'en'
}

const CAPTIONS = {
  hierarchy: {
    fa: 'از بالا به پایین: صفحه ← اتصال به صفحه ← بخش ← آیتم‌های محتوا',
    en: 'Top to bottom: Page → Page link → Section → Section items',
  },
  homepage: {
    fa: 'چیدمان عمومی صفحه اصلی از بالا به پایین',
    en: 'Typical homepage layout from top to bottom',
  },
  siteMap: {
    fa: 'نقشه ساده مسیرهای مهم فروشگاه',
    en: 'Simplified map of key storefront routes',
  },
} as const

export function CmsHierarchyDiagram({ className = '', locale = 'fa' }: DiagramProps) {
  const isFa = locale === 'fa'
  return (
    <figure className={`overflow-hidden rounded-sm border border-border bg-surface-muted/30 p-4 ${className}`}>
      <svg viewBox="0 0 720 280" className="mx-auto w-full max-w-3xl" role="img" aria-hidden>
        <rect x="250" y="16" width="220" height="44" rx="6" fill="#faf6f1" stroke="#9a7448" strokeWidth="1.5" />
        <text x="360" y="44" textAnchor="middle" fill="#1a1a1a" fontSize="13" fontWeight="600">
          {isFa ? 'صفحات CMS (CmsPages)' : 'CMS Pages'}
        </text>

        <path d="M360 60v24" stroke="#e8e6e3" strokeWidth="1.5" />
        <rect x="230" y="84" width="260" height="44" rx="6" fill="#ffffff" stroke="#e8e6e3" strokeWidth="1.5" />
        <text x="360" y="112" textAnchor="middle" fill="#1a1a1a" fontSize="12" fontWeight="500">
          {isFa ? 'بخش‌های صفحه (PageSections)' : 'Page Sections + priority'}
        </text>

        <path d="M360 128v24" stroke="#e8e6e3" strokeWidth="1.5" />
        <rect x="210" y="152" width="300" height="44" rx="6" fill="#ffffff" stroke="#e8e6e3" strokeWidth="1.5" />
        <text x="360" y="180" textAnchor="middle" fill="#1a1a1a" fontSize="12" fontWeight="500">
          {isFa ? 'بخش ← نوع بخش' : 'Sections ← Section Type'}
        </text>

        <path d="M360 196v24" stroke="#e8e6e3" strokeWidth="1.5" />
        <rect x="190" y="220" width="340" height="44" rx="6" fill="#ffffff" stroke="#e8e6e3" strokeWidth="1.5" />
        <text x="360" y="248" textAnchor="middle" fill="#1a1a1a" fontSize="12" fontWeight="500">
          {isFa ? 'آیتم‌های بخش (SectionItems)' : 'Section Items (title, URL, text)'}
        </text>
      </svg>
      <figcaption className="mt-3 text-center text-xs text-text-muted">{CAPTIONS.hierarchy[locale]}</figcaption>
    </figure>
  )
}

export function HomepageLayoutDiagram({ className = '', locale = 'fa' }: DiagramProps) {
  const isFa = locale === 'fa'
  const blocks = isFa
    ? [
        { y: 8, h: 28, label: 'نوار پرومو', fill: '#faf6f1' },
        { y: 42, h: 36, label: 'هدر + منو', fill: '#ffffff' },
        { y: 84, h: 24, label: 'استوری‌ها', fill: '#f7f6f4' },
        { y: 114, h: 52, label: 'اسلایدر Hero', fill: '#faf6f1' },
        { y: 172, h: 32, label: 'کارت‌های حراج', fill: '#ffffff' },
        { y: 210, h: 28, label: 'نوار دسته‌ها', fill: '#f7f6f4' },
        { y: 244, h: 40, label: 'ویترین محصولات', fill: '#ffffff' },
        { y: 290, h: 28, label: 'خدمات طراحی', fill: '#faf6f1' },
        { y: 324, h: 36, label: 'فوتر', fill: '#ffffff' },
      ]
    : [
        { y: 8, h: 28, label: 'Promo bar', fill: '#faf6f1' },
        { y: 42, h: 36, label: 'Header + nav', fill: '#ffffff' },
        { y: 84, h: 24, label: 'Stories strip', fill: '#f7f6f4' },
        { y: 114, h: 52, label: 'Hero carousel', fill: '#faf6f1' },
        { y: 172, h: 32, label: 'Sale / new tiles', fill: '#ffffff' },
        { y: 210, h: 28, label: 'Category nav', fill: '#f7f6f4' },
        { y: 244, h: 40, label: 'Featured products', fill: '#ffffff' },
        { y: 290, h: 28, label: 'Design services', fill: '#faf6f1' },
        { y: 324, h: 36, label: 'Footer', fill: '#ffffff' },
      ]

  return (
    <figure className={`overflow-hidden rounded-sm border border-border bg-surface-muted/30 p-4 ${className}`}>
      <svg viewBox="0 0 420 380" className="mx-auto w-full max-w-md" role="img" aria-hidden>
        {blocks.map((block) => (
          <g key={block.label}>
            <rect x="24" y={block.y} width="372" height={block.h} rx="5" fill={block.fill} stroke="#e8e6e3" strokeWidth="1.2" />
            <text x="210" y={block.y + block.h / 2 + 4} textAnchor="middle" fill="#1a1a1a" fontSize="10" fontWeight="500">
              {block.label}
            </text>
          </g>
        ))}
      </svg>
      <figcaption className="mt-3 text-center text-xs text-text-muted">{CAPTIONS.homepage[locale]}</figcaption>
    </figure>
  )
}

export function SiteMapDiagram({ className = '', locale = 'fa' }: DiagramProps) {
  const isFa = locale === 'fa'
  const nodes = isFa
    ? [
        { x: 180, y: 20, w: 120, label: 'صفحه اصلی /' },
        { x: 20, y: 90, w: 110, label: 'دسته' },
        { x: 150, y: 90, w: 110, label: 'محصول' },
        { x: 280, y: 90, w: 110, label: 'جستجو' },
        { x: 20, y: 160, w: 110, label: 'بلاگ' },
        { x: 150, y: 160, w: 110, label: 'درباره' },
        { x: 280, y: 160, w: 110, label: 'تماس' },
        { x: 20, y: 230, w: 110, label: 'قوانین' },
        { x: 150, y: 230, w: 110, label: 'سبد' },
        { x: 280, y: 230, w: 110, label: 'حساب' },
      ]
    : [
        { x: 180, y: 20, w: 120, label: 'Home /' },
        { x: 20, y: 90, w: 110, label: 'Category' },
        { x: 150, y: 90, w: 110, label: 'Product' },
        { x: 280, y: 90, w: 110, label: 'Search' },
        { x: 20, y: 160, w: 110, label: 'Blog' },
        { x: 150, y: 160, w: 110, label: 'About' },
        { x: 280, y: 160, w: 110, label: 'Contact' },
        { x: 20, y: 230, w: 110, label: 'Legal' },
        { x: 150, y: 230, w: 110, label: 'Cart' },
        { x: 280, y: 230, w: 110, label: 'Account' },
      ]

  return (
    <figure className={`overflow-hidden rounded-sm border border-border bg-surface-muted/30 p-4 ${className}`}>
      <svg viewBox="0 0 420 300" className="mx-auto w-full max-w-md" role="img" aria-hidden>
        <path d="M240 52 L75 90 M240 52 L205 90 M240 52 L335 90" stroke="#e8e6e3" strokeWidth="1.2" fill="none" />
        <path d="M240 52 L75 160 M240 52 L205 160 M240 52 L335 160" stroke="#e8e6e3" strokeWidth="1.2" fill="none" />
        {nodes.map((node) => (
          <g key={node.label}>
            <rect x={node.x} y={node.y} width={node.w} height="34" rx="5" fill="#ffffff" stroke="#e8e6e3" strokeWidth="1.2" />
            <text x={node.x + node.w / 2} y={node.y + 21} textAnchor="middle" fill="#1a1a1a" fontSize="9" fontWeight="500">
              {node.label}
            </text>
          </g>
        ))}
      </svg>
      <figcaption className="mt-3 text-center text-xs text-text-muted">{CAPTIONS.siteMap[locale]}</figcaption>
    </figure>
  )
}

export function GuideDiagram({
  id,
  className,
  locale = 'fa',
}: {
  id: GuideDiagramId
  className?: string
  locale?: 'fa' | 'en'
}) {
  if (id === 'cms-hierarchy') return <CmsHierarchyDiagram className={className} locale={locale} />
  if (id === 'homepage-layout') return <HomepageLayoutDiagram className={className} locale={locale} />
  return <SiteMapDiagram className={className} locale={locale} />
}
