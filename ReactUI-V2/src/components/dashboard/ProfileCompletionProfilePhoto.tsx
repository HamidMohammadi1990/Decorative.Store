import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { UserAvatar } from '@/components/ui/UserAvatar'
import { InlineLoading } from '@/components/ui/Spinner'
import { resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { authService } from '@/services/authService'
import { useUserStore } from '@/stores/userStore'
import { showToast } from '@/stores/toastStore'

const MAX_FILE_MB = 5

export function ProfileCompletionProfilePhoto() {
  const { t } = useTranslation()
  const { locale } = useCurrentLanguageId()
  const accessToken = useUserStore((s) => s.accessToken)
  const user = useUserStore((s) => s.user)
  const updateUserProfileImage = useUserStore((s) => s.updateUserProfileImage)

  const [preview, setPreview] = useState(user?.profileImageUrl ?? '')
  const [uploading, setUploading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    setPreview(user?.profileImageUrl ?? '')
  }, [user?.profileImageUrl])

  const displayName = [user?.firstName, user?.lastName].filter(Boolean).join(' ') || user?.email || ''

  const handlePick = async (file: File | null) => {
    if (!file) return
    if (file.size > MAX_FILE_MB * 1024 * 1024) {
      setError(t('dashboard.users.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.users.authRequired'))
      return
    }

    setUploading(true)
    setError(null)
    try {
      const uploaded = await authService.uploadProfileImage(accessToken, locale, file)
      setPreview(uploaded.imageUrl)
      updateUserProfileImage(uploaded.imageUrl)
      showToast(t('dashboard.profileCompletion.profilePhotoSaved'), 'success')
    } catch (err) {
      const message = resolveAdminMutationError(err, t('dashboard.users.uploadFailed'))
      setError(message)
      showToast(message, 'error')
    } finally {
      setUploading(false)
    }
  }

  return (
    <section className="mb-6 rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6">
      <h2 className="text-sm font-semibold uppercase tracking-[0.12em] text-text-muted">
        {t('dashboard.profileCompletion.profilePhotoTitle')}
      </h2>
      <p className="mt-1 text-sm text-text-muted">{t('dashboard.profileCompletion.profilePhotoHint')}</p>

      <div className="mt-4 flex flex-col gap-4 sm:flex-row sm:items-center">
        <UserAvatar name={displayName} imageUrl={preview || user?.profileImageUrl} size="xl" />
        <div className="min-w-0 flex-1 space-y-2">
          <input
            type="file"
            accept="image/*"
            disabled={uploading}
            onChange={(e) => void handlePick(e.target.files?.[0] ?? null)}
            className="block w-full text-sm text-text-muted file:me-3 file:rounded-sm file:border-0 file:bg-warm-soft file:px-3 file:py-2 file:text-sm file:font-medium file:text-warm"
          />
          {uploading && <InlineLoading label={t('dashboard.users.uploading')} />}
          {error && <p className="text-sm text-sale">{error}</p>}
        </div>
      </div>
    </section>
  )
}
