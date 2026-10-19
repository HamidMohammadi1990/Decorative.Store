import type { AboutPageContent } from '@/models/about/aboutPage.model'
import type { Locale } from '@/models/shared/locale.model'
import aboutEn from '@/data/mock/about.en.json'
import aboutFa from '@/data/mock/about.fa.json'
import { isCmsUploadedImage, isStockHomeMediaSrc } from '@/utils/cmsSectionUtils'

const aboutByLocale = {
  en: aboutEn,
  fa: aboutFa,
} as const

export function getAboutPageMock(locale: Locale): AboutPageContent {
  return aboutByLocale[locale] as AboutPageContent
}

function isAboutContentIncomplete(content: AboutPageContent): boolean {
  return !content.hero.title?.trim()
}

export function applyAboutPageFallback(
  content: AboutPageContent,
  locale: Locale,
): AboutPageContent {
  if (!isAboutContentIncomplete(content)) return content

  const fallback = getAboutPageMock(locale)
  return {
    ...fallback,
    slug: content.slug || fallback.slug,
    title: content.title?.trim() || fallback.title,
    metaTitle: content.metaTitle ?? fallback.metaTitle,
    metaDescription: content.metaDescription ?? fallback.metaDescription,
    hero: {
      ...fallback.hero,
      ...content.hero,
      title: content.hero.title?.trim() || fallback.hero.title,
      subtitle: content.hero.subtitle?.trim() || fallback.hero.subtitle,
      image:
        isCmsUploadedImage(content.hero.image.src) ||
        (content.hero.image.src && !isStockHomeMediaSrc(content.hero.image.src))
          ? content.hero.image
          : fallback.hero.image,
    },
    story: {
      ...fallback.story,
      ...content.story,
      image: isCmsUploadedImage(content.story.image.src) ? content.story.image : fallback.story.image,
    },
  }
}
