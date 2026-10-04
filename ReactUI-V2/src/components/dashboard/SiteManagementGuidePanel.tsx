import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { GuideBookIcon } from '@/components/dashboard/DashboardIcons'
import { GuideDiagram } from '@/components/dashboard/guide/SiteManagementGuideDiagrams'
import { getSiteManagementGuide } from '@/data/siteManagementGuideContent'
import type { GuideBlock, GuideSection } from '@/models/dashboard/siteManagementGuide.model'
import { useSettingsStore } from '@/stores/settingsStore'

function GuideBlockView({ block, locale }: { block: GuideBlock; locale: 'fa' | 'en' }) {
  switch (block.type) {
    case 'paragraph':
      return <p className="text-sm leading-relaxed text-text-muted">{block.text}</p>
    case 'list':
      return (
        <ul className="list-disc space-y-2 ps-5 text-sm leading-relaxed text-text-muted">
          {block.items.map((item) => (
            <li key={item.slice(0, 48)}>{item}</li>
          ))}
        </ul>
      )
    case 'ordered':
      return (
        <ol className="list-decimal space-y-2 ps-5 text-sm leading-relaxed text-text-muted">
          {block.items.map((item, index) => (
            <li key={`${index}-${item.slice(0, 32)}`}>{item}</li>
          ))}
        </ol>
      )
    case 'table':
      return (
        <div className="overflow-x-auto rounded-sm border border-border">
          <table className="min-w-full text-sm">
            <thead className="bg-surface-muted/70">
              <tr>
                {block.headers.map((header) => (
                  <th key={header} className="px-3 py-2 text-start font-semibold text-text">
                    {header}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {block.rows.map((row) => (
                <tr key={row.join('|')} className="border-t border-border/70">
                  {row.map((cell, cellIndex) => (
                    <td key={`${cellIndex}-${cell.slice(0, 24)}`} className="px-3 py-2 text-text-muted">
                      {cell}
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )
    case 'callout': {
      const styles = {
        info: 'border-sky-500/30 bg-sky-500/5 text-sky-900 dark:text-sky-100',
        warning: 'border-amber-500/35 bg-amber-500/8 text-amber-950 dark:text-amber-100',
        tip: 'border-warm/35 bg-warm-soft/50 text-text',
        success: 'border-emerald-500/30 bg-emerald-500/8 text-emerald-950 dark:text-emerald-100',
      }
      return (
        <div className={`rounded-sm border px-4 py-3 ${styles[block.variant]}`}>
          {block.title && <p className="text-sm font-semibold">{block.title}</p>}
          <p className={`text-sm leading-relaxed ${block.title ? 'mt-1 opacity-90' : ''}`}>{block.text}</p>
        </div>
      )
    }
    case 'adminLinks':
      return (
        <div className="flex flex-wrap gap-2">
          {block.links.map((link) => (
            <Link
              key={link.path}
              to={link.path}
              className="inline-flex items-center rounded-sm border border-warm/25 bg-warm-soft/40 px-3 py-1.5 text-xs font-semibold text-warm transition-colors hover:bg-warm-soft"
            >
              {link.label} →
            </Link>
          ))}
        </div>
      )
    case 'storeLinks':
      return (
        <div className="flex flex-wrap gap-2">
          {block.links.map((link) => (
            <a
              key={link.path}
              href={link.path}
              target="_blank"
              rel="noopener noreferrer"
              className="inline-flex items-center rounded-sm border border-border bg-surface px-3 py-1.5 text-xs font-medium text-text transition-colors hover:border-warm/30 hover:text-warm"
            >
              {link.label} ↗
            </a>
          ))}
        </div>
      )
    case 'diagram':
      return <GuideDiagram id={block.id} className="my-2" locale={locale} />
    default:
      return null
  }
}

function GuideSectionCard({
  section,
  expanded,
  onToggle,
  locale,
}: {
  section: GuideSection
  expanded: boolean
  onToggle: () => void
  locale: 'fa' | 'en'
}) {
  return (
    <section id={section.id} className="scroll-mt-24 rounded-sm border border-border bg-surface shadow-sm">
      <button
        type="button"
        onClick={onToggle}
        className="flex w-full items-start justify-between gap-4 px-4 py-4 text-start sm:px-5"
      >
        <div className="min-w-0">
          <div className="flex flex-wrap items-center gap-2">
            <h2 className="text-base font-semibold text-text sm:text-lg">{section.title}</h2>
            {section.incomplete && (
              <span className="rounded-full bg-amber-500/15 px-2 py-0.5 text-[10px] font-bold uppercase tracking-wide text-amber-800">
                WIP
              </span>
            )}
          </div>
          {section.summary && (
            <p className="mt-1 text-sm text-text-muted">{section.summary}</p>
          )}
        </div>
        <span className="mt-1 shrink-0 text-text-muted" aria-hidden>
          {expanded ? '−' : '+'}
        </span>
      </button>
      {expanded && (
        <div className="space-y-4 border-t border-border/70 px-4 py-4 sm:px-5 sm:py-5">
          {section.blocks.map((block, index) => (
            <GuideBlockView key={`${section.id}-${index}`} block={block} locale={locale} />
          ))}
        </div>
      )}
    </section>
  )
}

export function SiteManagementGuidePanel() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const guide = useMemo(() => getSiteManagementGuide(locale), [locale])
  const [expandedIds, setExpandedIds] = useState<Set<string>>(() => new Set(['intro', 'cms-basics']))
  const [activeId, setActiveId] = useState('intro')

  const toggleSection = useCallback((id: string) => {
    setExpandedIds((prev) => {
      const next = new Set(prev)
      if (next.has(id)) next.delete(id)
      else next.add(id)
      return next
    })
  }, [])

  const expandAll = useCallback(() => {
    setExpandedIds(new Set(guide.sections.map((s) => s.id)))
  }, [guide.sections])

  const collapseAll = useCallback(() => {
    setExpandedIds(new Set())
  }, [])

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        const visible = entries
          .filter((e) => e.isIntersecting)
          .sort((a, b) => b.intersectionRatio - a.intersectionRatio)[0]
        if (visible?.target.id) setActiveId(visible.target.id)
      },
      { rootMargin: '-20% 0px -60% 0px', threshold: [0, 0.25, 0.5] },
    )

    guide.sections.forEach((section) => {
      const el = document.getElementById(section.id)
      if (el) observer.observe(el)
    })

    return () => observer.disconnect()
  }, [guide.sections])

  return (
    <div className="pb-10">
      <DashboardPageHeader
        title={t('dashboard.siteGuide.title')}
        description={t('dashboard.siteGuide.description')}
        icon={<GuideBookIcon size={22} />}
        action={
          <div className="flex flex-wrap gap-2">
            <button
              type="button"
              onClick={expandAll}
              className="rounded-sm border border-border px-3 py-1.5 text-xs font-medium text-text-muted hover:text-text"
            >
              {t('dashboard.siteGuide.expandAll')}
            </button>
            <button
              type="button"
              onClick={collapseAll}
              className="rounded-sm border border-border px-3 py-1.5 text-xs font-medium text-text-muted hover:text-text"
            >
              {t('dashboard.siteGuide.collapseAll')}
            </button>
          </div>
        }
      />

      <div className="mb-6 rounded-sm border border-warm/20 bg-gradient-to-br from-warm-soft/40 to-surface p-4 sm:p-5">
        <p className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
          {t('dashboard.siteGuide.quickStart')}
        </p>
        <ol className="mt-3 grid gap-2 sm:grid-cols-3">
          {[1, 2, 3].map((step) => (
            <li
              key={step}
              className="flex items-start gap-3 rounded-sm border border-border/70 bg-surface/80 px-3 py-3"
            >
              <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-warm text-xs font-bold text-warm-text">
                {step}
              </span>
              <span className="text-sm text-text">{t(`dashboard.siteGuide.quickStep${step}`)}</span>
            </li>
          ))}
        </ol>
      </div>

      <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_220px]">
        <div className="space-y-4">
          {guide.sections.map((section) => (
            <GuideSectionCard
              key={section.id}
              section={section}
              expanded={expandedIds.has(section.id)}
              onToggle={() => toggleSection(section.id)}
              locale={locale}
            />
          ))}
        </div>

        <aside className="hidden lg:block">
          <nav
            aria-label={t('dashboard.siteGuide.toc')}
            className="sticky top-24 rounded-sm border border-border bg-surface-muted/30 p-4"
          >
            <p className="text-xs font-semibold uppercase tracking-[0.12em] text-text-muted">
              {t('dashboard.siteGuide.toc')}
            </p>
            <ul className="mt-3 space-y-1">
              {guide.sections.map((section) => (
                <li key={section.id}>
                  <a
                    href={`#${section.id}`}
                    onClick={() => setExpandedIds((prev) => new Set(prev).add(section.id))}
                    className={`block rounded-sm px-2 py-1.5 text-xs leading-snug transition-colors ${
                      activeId === section.id
                        ? 'bg-warm-soft font-semibold text-warm'
                        : 'text-text-muted hover:bg-surface hover:text-text'
                    }`}
                  >
                    {section.title.replace(/^\d+\.\s*/, '')}
                  </a>
                </li>
              ))}
            </ul>
          </nav>
        </aside>
      </div>
    </div>
  )
}
