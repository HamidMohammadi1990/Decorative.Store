import { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { Container } from '@/components/ui/Container'
import { RoomLayoutStudio } from '@/components/room/RoomLayoutStudio'
import {
  buildBreadcrumbJsonLd,
  buildWebApplicationJsonLd,
} from '@/components/seo/jsonLdBuilders'
import { absoluteUrl } from '@/config/site'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useShopPageMeta, resolveOgImageSrc } from '@/hooks/useShopPageMeta'

const ROOM_LAYOUT_OG_IMAGE = '/images/home/living-room.svg'

export function RoomLayoutPage() {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()

  const pageUrl = absoluteUrl('/room-layout')
  const ogImage = resolveOgImageSrc(ROOM_LAYOUT_OG_IMAGE)

  const jsonLd = useMemo(
    () => [
      buildWebApplicationJsonLd({
        name: t('roomLayout.title'),
        description: t('roomLayout.subtitle'),
        url: pageUrl,
        image: ogImage,
        locale,
      }),
      buildBreadcrumbJsonLd([
        { name: t('product.breadcrumbHome'), path: '/' },
        { name: t('roomLayout.title') },
      ]),
    ],
    [locale, ogImage, pageUrl, t],
  )

  useShopPageMeta({
    title: t('roomLayout.title'),
    description: t('roomLayout.subtitle'),
    image: ogImage,
    imageAlt: `${t('roomLayout.eyebrow')} — ${t('roomLayout.title')}`,
    keywords: t('seo.roomLayoutKeywords'),
    path: '/room-layout',
    jsonLd,
  })
  return (
    <div className="bg-gradient-to-b from-surface-muted/50 via-surface to-surface py-8 md:py-12">
      <Container>
        <header className="mb-6 max-w-2xl md:mb-8">
          <p className="text-[10px] font-semibold uppercase tracking-[0.16em] text-warm">
            {t('roomLayout.eyebrow')}
          </p>
          <h1 className="mt-2 text-2xl font-semibold tracking-tight text-text md:text-3xl">
            {t('roomLayout.title')}
          </h1>
          <p className="mt-3 text-sm leading-relaxed text-text-muted md:text-base">
            {t('roomLayout.subtitle')}
          </p>
        </header>

        <RoomLayoutStudio />
      </Container>
    </div>
  )
}
