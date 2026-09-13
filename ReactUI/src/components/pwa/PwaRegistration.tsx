import { useEffect, useState, type ComponentType } from 'react'
import { createPortal } from 'react-dom'

/** Client-only PWA registration (dynamic import avoids SSR virtual module issues). */
export function PwaRegistration() {
  const [Prompt, setPrompt] = useState<ComponentType | null>(null)

  useEffect(() => {
    if (!import.meta.env.PROD) return
    void import('./PwaUpdatePrompt').then((mod) => {
      setPrompt(() => mod.PwaUpdatePrompt)
    })
  }, [])

  if (!Prompt) return null

  const portalTarget = document.getElementById('pwa-root')
  if (!portalTarget) return null

  return createPortal(<Prompt />, portalTarget)
}
