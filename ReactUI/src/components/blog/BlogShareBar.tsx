import { useTranslation } from 'react-i18next'
import { ShareBar } from '@/components/ui/ShareBar'

interface BlogShareBarProps {
  title: string
  url: string
}

export function BlogShareBar({ title, url }: BlogShareBarProps) {
  const { t } = useTranslation()

  return (
    <ShareBar
      heading={t('blog.shareTitle')}
      title={title}
      url={url}
      shareActionLabel={t('blog.share')}
    />
  )
}
