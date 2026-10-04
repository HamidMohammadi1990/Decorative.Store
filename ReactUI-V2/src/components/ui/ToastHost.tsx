import { Portal } from '@/components/ui/Portal'
import { useToastStore } from '@/stores/toastStore'

export function ToastHost() {
  const toasts = useToastStore((state) => state.toasts)

  if (toasts.length === 0) return null

  return (
    <Portal>
      <div
        aria-live="polite"
        className="pointer-events-none fixed inset-x-0 bottom-20 z-[200] flex flex-col items-center gap-2 px-4 sm:bottom-24"
      >
        {toasts.map((toast) => (
          <div
            key={toast.id}
            role="status"
            className={`pointer-events-auto max-w-sm rounded-xl border px-4 py-3 text-center text-sm font-medium shadow-lg backdrop-blur-sm ${
              toast.tone === 'error'
                ? 'border-sale/35 bg-surface/95 text-sale'
                : 'border-warm/35 bg-surface/95 text-text'
            }`}
          >
            {toast.message}
          </div>
        ))}
      </div>
    </Portal>
  )
}
