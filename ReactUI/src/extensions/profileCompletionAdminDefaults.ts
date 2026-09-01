import type { AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'

function emptyLocalized() {
  return { en: '', fa: '' }
}

export function createEmptyAdminQuestion(
  order: number,
): AdminProfileCompletionConfig['questions'][number] {
  return {
    id: `question-${Date.now().toString(36)}`,
    type: 'text',
    required: true,
    order,
    labels: emptyLocalized(),
    hints: emptyLocalized(),
    placeholders: emptyLocalized(),
    options: [],
  }
}
