import { useCallback } from 'react'
import { useTranslation } from 'react-i18next'
import { askConfirm, type ConfirmOptions } from '@/stores/confirmStore'

export function useConfirm() {
  const { t } = useTranslation()

  return useCallback(
    (options: ConfirmOptions) =>
      askConfirm({
        title: options.title ?? t('common.confirmDeleteTitle'),
        message: options.message,
        confirmLabel: options.confirmLabel ?? t('common.confirmDelete'),
        cancelLabel: options.cancelLabel ?? t('common.cancel'),
        tone: options.tone ?? 'danger',
      }),
    [t],
  )
}
