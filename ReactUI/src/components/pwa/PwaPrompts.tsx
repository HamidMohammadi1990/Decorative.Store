import { PwaInstallPrompt } from '@/components/pwa/PwaInstallPrompt'
import { PwaUpdatePrompt } from '@/components/pwa/PwaUpdatePrompt'

/** Production-only PWA UI (install + service worker update). */
export function PwaPrompts() {
  return (
    <>
      <PwaInstallPrompt />
      <PwaUpdatePrompt />
    </>
  )
}
