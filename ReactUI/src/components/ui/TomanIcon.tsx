import { useTranslation } from 'react-i18next'

interface TomanIconProps {
  size?: number
  className?: string
}

/** Currency unit label beside formatted amounts — تومان / Toman by locale. */
export function TomanIcon({ size = 14, className }: TomanIconProps) {
  const { t, i18n } = useTranslation()
  const isFa = i18n.language === 'fa'

  return (
    <span
      aria-hidden
      className={`inline-block shrink-0 font-semibold leading-none ${className ?? ''}`}
      style={{
        fontFamily: isFa ? '"Vazirmatn", Tahoma, sans-serif' : 'inherit',
        fontSize: Math.round(size * 0.72),
      }}
    >
      {t('common.toman')}
    </span>
  )
}
