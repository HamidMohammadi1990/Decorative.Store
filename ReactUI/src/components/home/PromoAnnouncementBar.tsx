import { useTranslation } from 'react-i18next'
import type { PromoAnnouncement } from '@/models/home/promoAnnouncement.model'
import { Container } from '@/components/ui/Container'
import { TextLink } from '@/components/ui/TextLink'
import { usePromoDismissStore } from '@/stores/promoDismissStore'

interface PromoAnnouncementBarProps {
  data: PromoAnnouncement
}

export function PromoAnnouncementBar({ data }: PromoAnnouncementBarProps) {
  const { t } = useTranslation()
  const isDismissed = usePromoDismissStore((s) => s.isDismissed(data.id))
  const dismiss = usePromoDismissStore((s) => s.dismiss)

  if (isDismissed) return null

  return (
    <div className="bg-surface-inverse text-center text-sm text-text-inverse">
      <Container className="relative flex items-center justify-center gap-2 py-2.5 pe-10">
        <span className="font-medium">{data.message}</span>
        <TextLink
          link={data.link}
          className="font-semibold text-text-inverse transition-opacity hover:opacity-80"
        />
        <button
          type="button"
          onClick={() => dismiss(data.id)}
          aria-label={t('common.closePromo')}
          className="absolute end-2 top-1/2 flex size-7 -translate-y-1/2 items-center justify-center rounded-sm text-text-inverse/80 transition-colors hover:bg-white/10 hover:text-text-inverse"
        >
          <CloseIcon />
        </button>
      </Container>
    </div>
  )
}

function CloseIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 14 14" aria-hidden>
      <path
        d="M3 3l8 8M11 3 3 11"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </svg>
  )
}
