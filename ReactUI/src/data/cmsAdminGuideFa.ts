/** Fallback hints when AdminDescription is empty in DB (Persian). */
export const CMS_PAGE_TYPE_HINTS_FA: Record<number, string> = {
  1: 'صفحه عمومی CMS — برای محتوای سفارشی با slug دلخواه.',
  2: 'صفحه خانه/فروشگاه — معمولاً slug=shop؛ شامل هیرو، پرومو، فوتر و …',
  3: 'صفحه دسته — قالب لیست محصولات یک دسته.',
  4: 'صفحه محصول — قالب جزئیات محصول.',
  5: 'صفحه تسویه — مراحل checkout.',
}

export const CMS_ENTITY_ADMIN_HINT_FA = {
  page:
    'توضیح دهید این صفحه در سایت کجا دیده می‌شود (مثلاً: صفحه اصلی فروشگاه، /shop).',
  sectionType:
    'توضیح دهید این نوع بخش در کدام ناحیه UI استفاده می‌شود (مثلاً HeroCarousel = اسلایدر بالای صفحه).',
  section:
    'توضیح دهید این بخش کدام بلوک محتواست و روی کدام صفحه(ها) با Page Sections وصل شده.',
  sectionItem:
    'توضیح دهید این آیتم داخل کدام بخش است و در UI چه چیزی نمایش می‌دهد (لینک، تصویر، متن).',
  pageSection:
    'توضیح دهید چرا این بخش به این صفحه وصل شده و ترتیب نمایش (Priority) چیست.',
} as const
