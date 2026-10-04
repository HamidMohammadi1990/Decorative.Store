import { useTranslation } from 'react-i18next'
import { useHydrated } from '@/hooks/useHydrated'
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

export type ThemeSwitcherAppearance = 'default' | 'storefront' | 'storefront-overlay'

interface ThemeSwitcherProps {
  className?: string
  /** `storefront-overlay` = on hero imagery (light icons). */
  appearance?: ThemeSwitcherAppearance
}

export function ThemeSwitcher({ className, appearance = 'default' }: ThemeSwitcherProps) {
  const { t } = useTranslation()
  const { theme, setTheme } = useTheme()
  const hydrated = useHydrated()

  const shellClass =
    appearance === 'storefront-overlay'
      ? 'border-white/30 bg-black/15'
      : appearance === 'storefront'
        ? 'border-border bg-surface-muted'
        : 'border-warm-muted bg-warm-soft/60'

  const activeClass =
    appearance === 'storefront-overlay'
      ? 'bg-white/25 text-white shadow-sm'
      : appearance === 'storefront'
        ? 'bg-warm text-warm-text shadow-sm'
        : 'bg-warm text-warm-text shadow-sm'

  const idleClass =
    appearance === 'storefront-overlay'
      ? 'text-white/75 hover:text-white'
      : 'text-text-muted hover:text-warm'

  return (
    <div
      role="group"
      aria-label={t('common.theme')}
      className={`inline-flex items-center gap-0.5 rounded-full border p-0.5 ${shellClass} ${className ?? ''}`}
    >
      {THEME_OPTIONS.map(({ value, labelKey, Icon }) => {
        const isActive = hydrated && theme === value

        return (
          <button
            key={value}
            type="button"
            aria-pressed={isActive}
            aria-label={t(`common.${labelKey}`)}
            title={t(`common.${labelKey}`)}
            onClick={() => setTheme(value)}
            className={`flex size-7 items-center justify-center rounded-full transition-all sm:size-8 ${
              isActive ? activeClass : idleClass
            }`}
          >
            <Icon />
          </button>
        )
      })}
    </div>
  )
}
