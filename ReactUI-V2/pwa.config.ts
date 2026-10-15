/** Shared PWA manifest values (used by Vite PWA plugin). */
export const PWA = {
  name: 'Sarv Home',
  shortName: 'Sarv Home',
  description:
    'Shop contemporary furniture, rugs, and home décor. Design-led collections and interior inspiration.',
  themeColor: '#9a7448',
  backgroundColor: '#f7f6f4',
  themeColorDark: '#c4a06a',
  backgroundColorDark: '#1a120c',
  startUrl: '/?source=pwa',
  scope: '/',
  categories: ['shopping', 'lifestyle', 'home'] as const,
} as const
