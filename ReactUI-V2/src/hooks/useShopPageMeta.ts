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
import { stripLocaleHintFromPath } from '@/seo/canonicalPath'
import { buildHreflangAlternates } from '@/seo/hreflang'
import { OG_IMAGE_HEIGHT, OG_IMAGE_WIDTH } from '@/seo/ogConstants'

interface UseShopPageMetaOptions {
  title?: string
  description?: string
  image?: string
  imageAlt?: string
  imageWidth?: number
  imageHeight?: number
  type?: PageMetaType
  noindex?: boolean
  jsonLd?: object | object[] | null
  keywords?: string
  author?: string
  publishedTime?: string
  modifiedTime?: string
  productPrice?: number
  productCurrency?: string
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
    imageAlt,
    imageWidth = OG_IMAGE_WIDTH,
    imageHeight = OG_IMAGE_HEIGHT,
    type = 'website',
    noindex = false,
    jsonLd = null,
    keywords,
    author,
    publishedTime,
    modifiedTime,
    productPrice,
    productCurrency,
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
    const rawPath = path ?? `${location.pathname}${location.search}`
    const canonicalPath = stripLocaleHintFromPath(rawPath)
    const canonical = absoluteUrl(canonicalPath)
    const resolvedImage = resolveOgImageSrc(image)

    return {
      title: resolvedTitle,
      description: resolvedDescription,
      canonical,
      image: resolvedImage,
      imageAlt: imageAlt ?? title,
      imageWidth,
      imageHeight,
      type,
      locale,
      noindex,
      jsonLd,
      keywords,
      author,
      publishedTime,
      modifiedTime,
      productPrice,
      productCurrency,
      alternates: noindex ? undefined : buildHreflangAlternates(rawPath),
    }
  }, [
    active,
    author,
    description,
    image,
    imageAlt,
    imageHeight,
    imageWidth,
    jsonLd,
    keywords,
    locale,
    location.pathname,
    location.search,
    modifiedTime,
    noindex,
    path,
    productCurrency,
    productPrice,
    publishedTime,
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
