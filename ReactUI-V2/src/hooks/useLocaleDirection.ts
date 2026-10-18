import { getDirection } from '@/extensions/getDirection'
import { useSettingsStore } from '@/stores/settingsStore'

export function useLocaleDirection() {
  const locale = useSettingsStore((s) => s.locale)
  return getDirection(locale)
}
