import type { NavLinkGroup } from '@/models/shared/link.model'
import homeEn from './home.en.json'
import homeFa from './home.fa.json'
import navigationEn from './navigation.en.json'
import navigationFa from './navigation.fa.json'
import productsEn from './products.en.json'
import productsFa from './products.fa.json'
import blogEn from './blog.en.json'
import blogFa from './blog.fa.json'
import dashboardEn from './dashboard.en.json'
import dashboardFa from './dashboard.fa.json'
import storiesEn from './stories.en.json'
import storiesFa from './stories.fa.json'
import type { Locale } from '@/models/shared/locale.model'

interface NavigationMock {
  primaryNav: NavLinkGroup[]
}

const homeByLocale = {
  en: homeEn,
  fa: homeFa,
} as const

const navigationByLocale: Record<Locale, NavigationMock> = {
  en: navigationEn as NavigationMock,
  fa: navigationFa as NavigationMock,
}

export function getHomeMock(locale: Locale) {
  return homeByLocale[locale]
}

export function getNavigationMock(locale: Locale) {
  return navigationByLocale[locale]
}

const productsByLocale = {
  en: productsEn,
  fa: productsFa,
} as const

export function getProductsMock(locale: Locale) {
  return productsByLocale[locale]
}

const blogByLocale = {
  en: blogEn,
  fa: blogFa,
} as const

export function getBlogMock(locale: Locale) {
  return blogByLocale[locale]
}

const dashboardByLocale = {
  en: dashboardEn,
  fa: dashboardFa,
} as const

export function getDashboardMock(locale: Locale) {
  return dashboardByLocale[locale]
}

const storiesByLocale = {
  en: storiesEn,
  fa: storiesFa,
} as const

export function getStoriesMock(locale: Locale) {
  return storiesByLocale[locale]
}
