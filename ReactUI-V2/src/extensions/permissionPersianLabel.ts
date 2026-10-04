/** Persian labels for admin permission titles (English titles from API). */

const ENTITY_FA: Record<string, string> = {
  product: 'محصولات',
  'product group': 'گروه محصولات',
  'product comment': 'نظرات محصول',
  'product comment group': 'گروه نظرات محصول',
  'product question': 'پرسش‌های محصول',
  'product question group': 'گروه پرسش محصول',
  'product description': 'توضیحات محصول',
  'product file': 'فایل‌های محصول',
  'product image': 'تصاویر محصول',
  'product price': 'قیمت محصول',
  'product price delivery option': 'گزینه ارسال قیمت محصول',
  'product property': 'ویژگی‌های محصول',
  'product feature type': 'نوع ویژگی محصول',
  'product order item attachment type': 'نوع پیوست سفارش محصول',
  category: 'دسته‌بندی',
  'sub category': 'زیردسته',
  user: 'کاربر',
  users: 'کاربران',
  'users group': 'گروه کاربران',
  role: 'نقش',
  'role group': 'گروه نقش‌ها',
  'role permission': 'دسترسی نقش',
  'role permission group': 'گروه دسترسی نقش',
  'user role': 'نقش کاربر',
  'user role group': 'گروه نقش کاربر',
  permission: 'دسترسی',
  'permission group': 'گروه دسترسی‌ها',
  'managed permission': 'دسترسی سفارشی',
  order: 'سفارش',
  'order group': 'گروه سفارشات',
  property: 'ویژگی',
  'property group': 'گروه ویژگی‌ها',
  'property category': 'دسته ویژگی',
  'property item': 'آیتم ویژگی',
  tag: 'برچسب',
  'tag group': 'گروه برچسب‌ها',
  'blog post': 'مقاله بلاگ',
  'blog post group': 'گروه مقالات',
  'blog post category': 'دسته بلاگ',
  'blog post tag': 'برچسب مقاله',
  'blog post comment': 'نظر بلاگ',
  'blog post file': 'فایل مقاله',
  'blog post like': 'پسند مقاله',
  page: 'صفحه CMS',
  'cms group': 'گروه مدیریت محتوا',
  section: 'بخش',
  'section type': 'نوع بخش',
  'section item': 'آیتم بخش',
  'page section': 'بخش صفحه',
  'content policy': 'سیاست محتوا',
  'content policy group': 'گروه سیاست محتوا',
  'content policy rule': 'قانون سیاست محتوا',
  'content policy record access': 'دسترسی رکورد سیاست محتوا',
  'content policy metadata': 'فراداده سیاست محتوا',
  wallet: 'کیف پول',
  'wallet transaction': 'تراکنش کیف پول',
  discount: 'تخفیف',
  'delivery type': 'نوع ارسال',
  'delivery option': 'گزینه ارسال',
  'delivery type group': 'گروه نوع ارسال',
  'user address': 'آدرس کاربر',
  'user address group': 'گروه آدرس کاربر',
  province: 'استان',
  city: 'شهر',
  location: 'موقعیت مکانی',
  'location group': 'گروه موقعیت مکانی',
  bank: 'بانک',
  company: 'شرکت',
  'company comment': 'نظر شرکت',
  'company pos device': 'دستگاه POS شرکت',
  'company story': 'استوری برند',
  'company story comment': 'نظر استوری برند',
  'company story like': 'پسند استوری برند',
  'user story comment': 'نظر استوری کاربر',
  'financial year': 'سال مالی',
  'chart of account': 'سرفصل حساب',
  'financial group': 'گروه مالی',
  'company group': 'گروه شرکت',
  'marketing promo': 'پیشنهاد بازاریابی',
  'marketing promo group': 'گروه پیشنهاد بازاریابی',
  'newsletter subscriber': 'عضو خبرنامه',
  'assistant faq': 'سوالات دستیار هوش مصنوعی',
  'assistant faq group': 'گروه سوالات دستیار',
  'profile completion': 'تکمیل پروفایل',
  'profile completion group': 'گروه تکمیل پروفایل',
  'profile completion user state': 'وضعیت تکمیل پروفایل کاربر',
  'profile question': 'سؤال پروفایل',
  'room type': 'نوع فضا',
  'room type group': 'گروه انواع فضا',
  language: 'زبان',
  'language group': 'گروه زبان‌ها',
  'web site setting': 'تنظیمات وب‌سایت',
  'comment topic': 'موضوع نظر',
  'comment topic group': 'گروه موضوع نظر',
  'post type': 'نوع پست',
  'post type group': 'گروه نوع پست',
  status: 'وضعیت',
  'status group': 'گروه وضعیت',
}

const WORD_FA: Record<string, string> = {
  manage: 'مدیریت',
  list: 'مشاهده لیست',
  create: 'ایجاد',
  update: 'ویرایش',
  delete: 'حذف',
  get: 'مشاهده',
  assign: 'اختصاص',
  approve: 'تأیید',
  publish: 'انتشار',
  change: 'تغییر',
  answer: 'پاسخ به',
  check: 'بررسی',
  activate: 'فعال‌سازی',
  deactivate: 'غیرفعال‌سازی',
  set: 'تنظیم',
  admin: 'مدیر',
  charge: 'شارژ',
  group: 'گروه',
  default: 'پیش‌فرض',
  range: 'دسته',
  status: 'وضعیت',
  summary: 'خلاصه',
  detail: 'جزئیات',
  preview: 'پیش‌نمایش',
  validate: 'اعتبارسنجی',
  compare: 'مقایسه',
  merge: 'ادغام',
  metadata: 'فراداده',
  schema: 'ساختار',
  options: 'گزینه‌ها',
  operators: 'عملگرها',
  entity: 'موجودیت',
  types: 'انواع',
  type: 'نوع',
  rules: 'قوانین',
  rule: 'قانون',
  record: 'رکورد',
  access: 'دسترسی',
  policy: 'سیاست',
  content: 'محتوا',
  disclaimer: 'سلب مسئولیت',
  password: 'رمز عبور',
  permission: 'دسترسی',
  permissions: 'دسترسی‌ها',
}

const EXACT_TITLE_FA: Record<string, string> = {
  product: 'فروشگاه',
  'check permission': 'بررسی دسترسی',
}

type Pattern = {
  match: RegExp
  format: (entity: string) => string
}

const TITLE_PATTERNS: Pattern[] = [
  { match: /^manage (.+) group$/i, format: (e) => `مدیریت گروه ${translateEntity(e)}` },
  { match: /^manage (.+)$/i, format: (e) => `مدیریت ${translateEntity(e)}` },
  { match: /^list (.+)$/i, format: (e) => `مشاهده لیست ${translateEntity(e)}` },
  { match: /^create (.+)$/i, format: (e) => `ایجاد ${translateEntity(e)}` },
  { match: /^update (.+)$/i, format: (e) => `ویرایش ${translateEntity(e)}` },
  { match: /^delete (.+)$/i, format: (e) => `حذف ${translateEntity(e)}` },
  { match: /^get (.+) by id$/i, format: (e) => `مشاهده جزئیات ${translateEntity(e)}` },
  { match: /^get (.+)$/i, format: (e) => `دریافت ${translateEntity(e)}` },
  { match: /^assign (.+)$/i, format: (e) => `اختصاص ${translateEntity(e)}` },
  { match: /^approve (.+)$/i, format: (e) => `تأیید ${translateEntity(e)}` },
  { match: /^publish (.+)$/i, format: (e) => `انتشار ${translateEntity(e)}` },
  { match: /^change (.+)$/i, format: (e) => `تغییر ${translateEntity(e)}` },
  { match: /^answer (.+)$/i, format: (e) => `پاسخ به ${translateEntity(e)}` },
  { match: /^activate (.+)$/i, format: (e) => `فعال‌سازی ${translateEntity(e)}` },
  { match: /^deactivate (.+)$/i, format: (e) => `غیرفعال‌سازی ${translateEntity(e)}` },
  { match: /^admin charge (.+)$/i, format: (e) => `شارژ ${translateEntity(e)} توسط مدیر` },
  { match: /^set default (.+)$/i, format: (e) => `تنظیم ${translateEntity(e)} پیش‌فرض` },
  { match: /^set (.+)$/i, format: (e) => `تنظیم ${translateEntity(e)}` },
  { match: /^content policy (.+)$/i, format: (e) => `سیاست محتوا — ${translateEntity(e)}` },
]

function translateEntity(raw: string): string {
  const key = raw.trim().toLowerCase()
  if (ENTITY_FA[key]) return ENTITY_FA[key]

  return raw
    .trim()
    .split(/\s+/)
    .map((word) => WORD_FA[word.toLowerCase()] ?? word)
    .filter(Boolean)
    .join(' ')
}

/** User-friendly Persian label for a permission title from the API. */
export function getPermissionPersianLabel(title: string): string {
  const trimmed = title.trim()
  if (!trimmed) return ''

  const exact = EXACT_TITLE_FA[trimmed.toLowerCase()]
  if (exact) return exact

  const lower = trimmed.toLowerCase()
  for (const pattern of TITLE_PATTERNS) {
    const matched = lower.match(pattern.match)
    if (matched?.[1]) {
      return pattern.format(matched[1])
    }
  }

  const tokenized = translateEntity(trimmed)
  return tokenized !== trimmed ? tokenized : trimmed
}

const LEVEL_TYPE_FA: Record<string, string> = {
  tab: 'بخش اصلی',
  page: 'صفحه پنل',
  action: 'عملیات',
  product: 'ریشه فروشگاه',
}

/** Persian label for permission level type (Tab / Page / Action). */
export function getPermissionLevelPersianLabel(levelTypeTitle: string): string {
  const key = levelTypeTitle.trim().toLowerCase()
  return LEVEL_TYPE_FA[key] ?? levelTypeTitle
}

/** Normalize for bilingual permission search. */
export function normalizePermissionSearchText(text: string): string {
  return text
    .trim()
    .toLowerCase()
    .replace(/[\u064A\u0649]/g, '\u06CC')
    .replace(/\u0643/g, '\u06A9')
    .replace(/\u200c/g, ' ')
    .replace(/\s+/g, ' ')
}
