import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import type { Locale } from '@/models/shared/locale.model'
import privacyEn from '@/data/mock/content/privacy.en.json'
import privacyFa from '@/data/mock/content/privacy.fa.json'
import designServicesEn from '@/data/mock/content/design-services.en.json'
import designServicesFa from '@/data/mock/content/design-services.fa.json'

const contentBySlug: Record<string, Record<Locale, CmsContentPageContent>> = {
  privacy: { en: privacyEn as CmsContentPageContent, fa: privacyFa as CmsContentPageContent },
  'design-services': {
    en: designServicesEn as CmsContentPageContent,
    fa: designServicesFa as CmsContentPageContent,
  },
}

export function getContentPageMock(slug: string, locale: Locale): CmsContentPageContent | null {
  return contentBySlug[slug]?.[locale] ?? null
}

function isContentIncomplete(content: CmsContentPageContent): boolean {
  return !content.hero.title?.trim()
}

export function applyContentPageFallback(
  content: CmsContentPageContent,
  slug: string,
  locale: Locale,
): CmsContentPageContent {
  if (!isContentIncomplete(content)) return content

  const fallback = getContentPageMock(slug, locale)
  if (!fallback) return content

  return {
    ...fallback,
    slug: content.slug || fallback.slug,
    title: content.title?.trim() || fallback.title,
    metaTitle: content.metaTitle ?? fallback.metaTitle,
    metaDescription: content.metaDescription ?? fallback.metaDescription,
  }
}
