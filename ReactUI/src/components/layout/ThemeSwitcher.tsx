import { useTranslation } from 'react-i18next'
import { MoonIcon, SunIcon, SystemThemeIcon } from '@/components/ui/ThemeIcons'
import { useTheme } from '@/hooks/useTheme'
import type { ThemeMode } from '@/models/shared/theme.model'

const THEME_OPTIONS: {
  value: ThemeMode
  labelKey: 'themeLight' | 'themeDark' | 'themeSystem'
  Icon: typeof SunIcon
}[] = [
  { value: 'light', labelKey: 'themeLight', Icon: SunIcon },
  { value: 'dark', labelKey: 'themeDark', Icon: MoonIcon },
  { value: 'system', labelKey: 'themeSystem', Icon: SystemThemeIcon },
]

export function ThemeSwitcher() {
  const { t } = useTranslation()
  const { theme, setTheme } = useTheme()

  return (
    <div
      role="group"
      aria-label={t('common.theme')}
      className="inline-flex items-center gap-0.5 rounded-full border border-warm-muted bg-warm-soft/60 p-0.5"
    >
      {THEME_OPTIONS.map(({ value, labelKey, Icon }) => {
        const isActive = theme === value

        return (
          <button
            key={value}
            type="button"
            aria-pressed={isActive}
            aria-label={t(`common.${labelKey}`)}
            title={t(`common.${labelKey}`)}
            onClick={() => setTheme(value)}
            className={`flex size-8 items-center justify-center rounded-full transition-all ${
              isActive
                ? 'bg-warm text-warm-text shadow-sm'
                : 'text-text-muted hover:text-warm'
            }`}
          >
            <Icon />
          </button>
        )
      })}
    </div>
  )
}
