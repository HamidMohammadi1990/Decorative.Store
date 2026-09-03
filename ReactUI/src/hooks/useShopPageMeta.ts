import { useEffect, useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { useLocation } from 'react-router-dom'
import {
  absoluteUrl,
  buildPageTitle,
  DEFAULT_DESCRIPTION,
  DEFAULT_OG_IMAGE,
  getSiteOrigin,
} from '@/config/site'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { usePageMeta } from '@/hooks/usePageMeta'
import type { PageMetaInput, PageMetaType } from '@/seo/pageMetaManager'
import { setPageMetaDefaults } from '@/seo/pageMetaManager'
import { buildHreflangAlternates } from '@/seo/hreflang'

interface UseShopPageMetaOptions {
  title?: string
  description?: string
  image?: string
  type?: PageMetaType
  noindex?: boolean
  jsonLd?: object | object[] | null
  /** Override canonical path (defaults to current location). */
  path?: string
  active?: boolean
}

/** Resolves absolute image URL for OG tags. */
export function resolveOgImageSrc(src?: string | null): string | undefined {
  if (!src?.trim()) return getSiteOrigin() ? absoluteUrl(DEFAULT_OG_IMAGE) : DEFAULT_OG_IMAGE
  if (src.startsWith('http://') || src.startsWith('https://')) return src
  if (src.startsWith('/')) return absoluteUrl(src)
  return src
}

export function useShopPageMeta(options: UseShopPageMetaOptions = {}) {
  const { locale } = useLocaleSettings()
  const { t } = useTranslation()
  const location = useLocation()
  const {
    title,
    description,
    image,
    type = 'website',
    noindex = false,
    jsonLd = null,
    path,
    active = true,
  } = options

  const meta = useMemo((): PageMetaInput | null => {
    if (!active) return null

    const resolvedTitle = buildPageTitle(
      title ?? t('common.brandName', { defaultValue: 'Diba Gallery' }),
      locale,
    )
    const resolvedDescription =
      description?.trim() || DEFAULT_DESCRIPTION[locale]
    const canonicalPath = path ?? `${location.pathname}${location.search}`
    const canonical = absoluteUrl(canonicalPath)

    return {
      title: resolvedTitle,
      description: resolvedDescription,
      canonical,
      image: resolveOgImageSrc(image),
      type,
      locale,
      noindex,
      jsonLd,
      alternates: noindex ? undefined : buildHreflangAlternates(canonicalPath),
    }
  }, [
    active,
    description,
    image,
    jsonLd,
    locale,
    location.pathname,
    location.search,
    noindex,
    path,
    t,
    title,
    type,
  ])

  usePageMeta(meta)
}

/** Call once in shop layout to set fallback title/description when pages unmount. */
export function useShopPageMetaDefaults() {
  const { locale } = useLocaleSettings()

  useEffect(() => {
    setPageMetaDefaults(
      buildPageTitle(undefined, locale),
      DEFAULT_DESCRIPTION[locale],
    )
  }, [locale])
}
