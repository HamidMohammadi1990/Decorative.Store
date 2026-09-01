import type { ContactPageContent } from '@/models/contact/contactPage.model'
import type { Locale } from '@/models/shared/locale.model'
import contactEn from '@/data/mock/contact.en.json'
import contactFa from '@/data/mock/contact.fa.json'

const contactByLocale = {
  en: contactEn,
  fa: contactFa,
} as const

export function getContactPageMock(locale: Locale): ContactPageContent {
  return contactByLocale[locale] as ContactPageContent
}

function isContactContentIncomplete(content: ContactPageContent): boolean {
  return !content.hero.title?.trim()
}

export function applyContactPageFallback(
  content: ContactPageContent,
  locale: Locale,
): ContactPageContent {
  if (!isContactContentIncomplete(content)) return content

  const fallback = getContactPageMock(locale)
  return {
    ...fallback,
    slug: content.slug || fallback.slug,
    title: content.title?.trim() || fallback.title,
    metaTitle: content.metaTitle ?? fallback.metaTitle,
    metaDescription: content.metaDescription ?? fallback.metaDescription,
  }
}
