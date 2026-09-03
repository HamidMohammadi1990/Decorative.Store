import type { SiteManagementGuideContent } from '@/models/dashboard/siteManagementGuide.model'

export const SITE_MANAGEMENT_GUIDE_FA: SiteManagementGuideContent = {
  sections: [
    {
      id: 'intro',
      title: '۱. مقدمه — این راهنما برای چیست؟',
      summary: 'نقشه راه تحویل و مدیریت روزمره سایت',
      blocks: [
        {
          type: 'paragraph',
          text: 'این صفحه مرجع کامل مدیریت فروشگاه دکوراتیو است. با خواندن آن می‌توانید بدانید هر بخش از سایت (صفحه اصلی، هدر، فوتر، بلاگ، محصولات، صفحات قانونی و …) از کدام قسمت داشبورد مدیریت می‌شود.',
        },
        {
          type: 'callout',
          variant: 'tip',
          title: 'نکته مهم',
          text: 'محتوای متنی سایت دو زبانه است (فارسی و انگلیسی). در پنل‌های CMS هنگام ویرایش، زبان محتوا را از بالای فرم انتخاب کنید و برای هر زبان جداگانه ذخیره کنید.',
        },
        {
          type: 'list',
          items: [
            'آدرس فروشگاه: صفحه اصلی سایت',
            'آدرس داشبورد: /account/dashboard (بعد از ورود)',
            'برای مدیریت محتوا به نقش مدیر (Admin) نیاز دارید',
            'تغییرات CMS معمولاً بلافاصله در سایت دیده می‌شوند؛ گاهی یک بار رفرش کافی است',
          ],
        },
      ],
    },
    {
      id: 'dashboard-overview',
      title: '۲. آشنایی با داشبورد',
      summary: 'منوی کناری و گروه‌های اصلی',
      blocks: [
        {
          type: 'paragraph',
          text: 'پس از ورود، از منوی سمت راست (یا منوی موبایل) به بخش‌های مختلف دسترسی دارید. گروه‌ها به این شکل تقسیم شده‌اند:',
        },
        {
          type: 'table',
          headers: ['گروه منو', 'کاربرد'],
          rows: [
            ['حساب کاربری', 'کیف پول، سبد، سفارشات، آدرس‌ها، علاقه‌مندی‌ها و … (بیشتر برای مشتری)'],
            ['کاربران و دسترسی', 'مدیریت کاربران و نقش‌ها (دسترسی ادمین)'],
            ['فروشگاه و محصولات', 'دسته‌ها، محصولات، تصاویر، کد تخفیف'],
            ['ویژگی‌ها', 'فیلترها و مشخصات فنی محصول (رنگ، جنس، ابعاد و …)'],
            ['مجله', 'دسته، تگ، مقالات و نظرات بلاگ'],
            ['مدیریت محتوا (CMS)', 'صفحات، بخش‌ها، فوتر، پرومو، خبرنامه، راهنما'],
          ],
        },
        {
          type: 'callout',
          variant: 'info',
          text: 'این راهنما در گروه «مدیریت محتوا» قرار دارد. هر وقت گم شدید، همین صفحه را باز کنید.',
        },
      ],
    },
    {
      id: 'cms-basics',
      title: '۳. اصول CMS — قبل از هر ویرایشی بخوانید',
      summary: 'سلسله‌مراتب صفحه، بخش و آیتم',
      blocks: [
        {
          type: 'paragraph',
          text: 'سیستم مدیریت محتوا (CMS) سایت بر پایهٔ چهار لایه ساخته شده. ترتیب کار معمولاً از پایین به بالا یا برعکس است، اما برای ویرایش روزمره بیشتر با «صفحات» و «آیتم‌های بخش» سر و کار دارید.',
        },
        { type: 'diagram', id: 'cms-hierarchy' },
        {
          type: 'ordered',
          items: [
            'انواع بخش (Section Types): نام نوع بلوک — مثل SiteHeader، ContentHero. معمولاً از قبل تعریف شده و نیاز به ساخت دستی ندارید.',
            'بخش‌ها (Sections): یک نمونه از نوع بخش با عنوان و توضیح. مثلاً بخش «هدر فارسی» از نوع SiteHeader.',
            'آیتم‌های بخش (Section Items): محتوای واقعی داخل هر بخش — لینک منو، متن فوتر، پارagraph و …',
            'صفحات CMS (Pages): هر صفحه storefront یک slug دارد — shop، about، privacy و …',
            'بخش‌های صفحه (Page Sections): مشخص می‌کند کدام Section روی کدام Page و با چه اولویتی نمایش داده شود.',
          ],
        },
        {
          type: 'callout',
          variant: 'warning',
          title: 'اشتباه رایج',
          text: 'اگر بخشی را ویرایش کردید ولی در سایت نمی‌بینید، احتمالاً آن Section به Page مربوطه وصل نشده (Page Sections) یا isActive خاموش است.',
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'صفحات CMS', path: '/account/dashboard/cms-pages' },
            { label: 'انواع بخش', path: '/account/dashboard/cms-section-types' },
            { label: 'بخش‌ها', path: '/account/dashboard/cms-sections' },
            { label: 'آیتم‌های بخش', path: '/account/dashboard/cms-section-items' },
            { label: 'بخش‌های صفحه', path: '/account/dashboard/cms-page-sections' },
          ],
        },
      ],
    },
    {
      id: 'site-map',
      title: '۴. نقشه صفحات سایت',
      summary: 'هر URL کجا می‌رود؟',
      blocks: [
        { type: 'diagram', id: 'site-map' },
        {
          type: 'table',
          headers: ['مسیر سایت', 'توضیح', 'مدیریت از'],
          rows: [
            ['/', 'صفحه اصلی فروشگاه', 'CMS → slug: shop + Marketing Promos + Catalog'],
            ['/product/{slug}', 'صفحه جزئیات محصول', 'محصولات + تصاویر + توضیحات'],
            ['/{category-slug}', 'لیست محصولات دسته', 'دسته‌ها و زیردسته‌ها'],
            ['/search', 'جستجوی محصول', 'خودکار از کatalog'],
            ['/blog', 'لیست مقالات', 'مقالات + CMS slug: blog'],
            ['/blog/{slug}', 'مقاله تکی', 'مقالات'],
            ['/about', 'درباره ما', 'CMS → slug: about'],
            ['/contact', 'تماس با ما', 'CMS → slug: contact'],
            ['/privacy, /terms, /legal, …', 'صفحات قانونی و اطلاعاتی', 'CMS → slug همان نام'],
            ['/design-services', 'خدمات طراحی', 'CMS → slug: design-services'],
            ['/p/{slug}', 'صفحه عمومی CMS', 'CMS → slug دلخواه'],
            ['/cart, /checkout', 'سبد و پرداخت', 'سفارشات (بخشی WIP)'],
            ['/account/dashboard', 'پنل مدیریت', 'ورود ادمین'],
          ],
        },
      ],
    },
    {
      id: 'chrome',
      title: '۵. هدر، فوتر و نوار بالایی (Chrome)',
      summary: 'بخش‌های مشترک همه صفحات',
      blocks: [
        {
          type: 'paragraph',
          text: 'هدر، فوتر، نوار تخفیف بالای سایت و نوار utility در تقریباً همه صفحات یکسان هستند. منبع اصلی آن‌ها صفحه CMS با slug برابر shop است؛ برای صفحات محتوایی (privacy و …) در صورت نبود فوتر اختصاصی، chrome مشترک merge می‌شود.',
        },
        {
          type: 'table',
          headers: ['بخش سایت', 'نوع Section', 'کجا ویرایش شود'],
          rows: [
            ['نوار پرومو بالای سایت', 'PromoAnnouncement', 'آیتم‌های بخش مربوطه + Marketing Promos (لینک disclaimer)'],
            ['نوار تلفن / لینک‌های utility', 'UtilityBar', 'Section Items'],
            ['لوگو، جستجو، برچسب‌ها', 'SiteHeader', 'Section Items (primaryNav از دسته‌ها خودکار اضافه می‌شود)'],
            ['ستون‌های فوتر', 'FooterColumn', 'Section Items هر ستون'],
            ['خبرنامه و لینک‌های پایین', 'SiteFooter', 'Section Items'],
          ],
        },
        {
          type: 'ordered',
          items: [
            'در «صفحات CMS» صفحه با slug=shop را پیدا کنید (یا about/contact برای chrome اختصاصی).',
            'در «بخش‌های صفحه» ببینید کدام Sectionها به این Page وصل شده‌اند.',
            'در «آیتم‌های بخش» محتوای هر آیتم (عنوان، URL، توضیح) را برای زبان fa/en ویرایش کنید.',
          ],
        },
        {
          type: 'callout',
          variant: 'tip',
          text: 'منوی دسته‌بندی محصولات در هدر به‌صورت خودکار از «دسته‌ها» ساخته می‌شود؛ لازم نیست دستی در CMS تک تک دسته را اضافه کنید.',
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'صفحات CMS', path: '/account/dashboard/cms-pages' },
            { label: 'آیتم‌های بخش', path: '/account/dashboard/cms-section-items' },
            { label: 'حراج‌ها و پرومو', path: '/account/dashboard/marketing-promos' },
          ],
        },
      ],
    },
    {
      id: 'homepage',
      title: '۶. صفحه اصلی',
      summary: 'اسلایدر، ویترین، کارت‌های حراج',
      blocks: [
        { type: 'diagram', id: 'homepage-layout' },
        {
          type: 'table',
          headers: ['بلوک صفحه اصلی', 'منبع محتوا'],
          rows: [
            ['اسلایدر Hero', 'CMS — بخش HeroCarousel (آیتم‌های بخش روی page shop)'],
            ['کارت‌های SALE / NEW', 'Marketing Promos + CMS PromoTileStrip'],
            ['نوار دسته‌ها', 'CategoryNav از CMS + دسته‌های فعال فروشگاه'],
            ['ویترین محصولات', 'FeaturedShopGrid — مجموعه‌های ویژه از API محصولات'],
            ['نوار خدمات طراحی', 'DesignServicesStrip در CMS'],
          ],
        },
        {
          type: 'ordered',
          items: [
            'متن و لینک نوار پرومو: Marketing Promos یا Section PromoAnnouncement.',
            'اسلایدر: در «آیتم‌های بخش» برای Section نوع HeroCarousel — عنوان، تصویر، آدرس (لینک CTA)، توضیح (eyebrow:…|زیرعنوان|cta:متن دکمه).',
            'دسته‌ها: ابتدا در «دسته‌ها» بسازید؛ سپس در صورت نیاز ترتیب در CMS.',
            'محصولات ویترین: محصولات را فعال و در دسته مناسب قرار دهید؛ API featured را پر می‌کند.',
          ],
        },
        {
          type: 'storeLinks',
          links: [{ label: 'مشاهده صفحه اصلی', path: '/' }],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'حراج‌ها و محصولات جدید', path: '/account/dashboard/marketing-promos' },
            { label: 'دسته‌ها', path: '/account/dashboard/categories' },
            { label: 'محصولات', path: '/account/dashboard/products' },
            { label: 'بخش‌های صفحه (shop)', path: '/account/dashboard/cms-page-sections' },
            { label: 'آیتم‌های بخش (اسلایدر)', path: '/account/dashboard/cms-section-items' },
          ],
        },
      ],
    },
    {
      id: 'about-contact',
      title: '۷. درباره ما و تماس با ما',
      summary: 'صفحات اختصاصی با section type مخصوص',
      blocks: [
        {
          type: 'paragraph',
          text: 'این دو صفحه ساختار اختصاصی دارند و از slug های about و contact در CMS تغذیه می‌شوند.',
        },
        {
          type: 'table',
          headers: ['صفحه', 'مسیر', 'Section Types'],
          rows: [
            ['درباره ما', '/about', 'AboutHero, AboutStoryBlock, AboutStatsStrip, AboutValuesGrid, AboutTimeline, AboutCtaStrip'],
            ['تماس', '/contact', 'ContactHero, ContactMethodsGrid, ContactLocationsGrid, ContactFormIntro'],
          ],
        },
        {
          type: 'ordered',
          items: [
            'صفحه about یا contact را در CMS Pages پیدا کنید.',
            'با Page Sections ببینید کدام Sectionها وصل شده‌اند.',
            'هر Section Type فیلدهای خاص خود را در Section Items دارد (مثلاً آدرس، تلفن، نقشه).',
            'فرم تماس: ContactFormIntro متن بالای فرم را کنترل می‌کند؛ ارسال فرم به backend وصل است.',
          ],
        },
        {
          type: 'storeLinks',
          links: [
            { label: 'درباره ما', path: '/about' },
            { label: 'تماس', path: '/contact' },
          ],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'صفحات CMS', path: '/account/dashboard/cms-pages' },
            { label: 'آیتم‌های بخش', path: '/account/dashboard/cms-section-items' },
          ],
        },
      ],
    },
    {
      id: 'legal-pages',
      title: '۸. صفحات قانونی و محتوایی',
      summary: 'Privacy، Terms، Returns و …',
      blocks: [
        {
          type: 'paragraph',
          text: 'صفحات زیر از موتور محتوای عمومی (Content Pages) استفاده می‌کنند. هر کدام slug جدا در CMS دارند.',
        },
        {
          type: 'table',
          headers: ['مسیر سایت', 'Slug در CMS'],
          rows: [
            ['/privacy', 'privacy'],
            ['/terms', 'terms'],
            ['/legal', 'legal'],
            ['/returns', 'returns'],
            ['/delivery', 'delivery'],
            ['/promo-terms', 'promo-terms'],
            ['/sustainability', 'sustainability'],
            ['/careers', 'careers'],
            ['/stores', 'stores'],
            ['/design-services', 'design-services'],
          ],
        },
        {
          type: 'paragraph',
          text: 'Section Types این صفحات: ContentHero (عنوان)، ContentBodyBlock (پارagraphها)، ContentStepsGrid (مراحل)، ContentCtaStrip (دکمه پایین صفحه).',
        },
        {
          type: 'callout',
          variant: 'info',
          text: 'صفحه جدید: یک Page با slug دلخواه بسازید، Sectionها را وصل کنید، سپس از /p/{slug} یا با تعریف مسیر در کد قابل دسترس می‌شود.',
        },
        {
          type: 'storeLinks',
          links: [
            { label: 'حریم خصوصی', path: '/privacy' },
            { label: 'شرایط استفاده', path: '/terms' },
            { label: 'بازگشت کالا', path: '/returns' },
          ],
        },
      ],
    },
    {
      id: 'catalog',
      title: '۹. فروشگاه — دسته‌ها و محصولات',
      summary: 'قلب e-commerce',
      blocks: [
        {
          type: 'ordered',
          items: [
            'دسته (Categories): ساختار اصلی منو و URL.',
            'زیردسته (SubCategories): سطح دوم.',
            'محصول (Products): عنوان، قیمت، slug، موجودی، فعال/غیرفعال.',
            'تصاویر محصول: آپلود، تعیین تصویر اصلی.',
            'توضیحات محصول: متن بلند HTML/Markdown.',
            'ویژگی‌ها: Property Categories → Properties → Property Items → اتصال به محصول.',
            'کد تخفیف: Discount Codes برای کمپین.',
          ],
        },
        {
          type: 'callout',
          variant: 'tip',
          text: 'slug محصول در URL استفاده می‌شود (/product/my-sofa). بعد از تغییر slug، لینک‌های قدیمی کار نمی‌کنند.',
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'دسته‌ها', path: '/account/dashboard/categories' },
            { label: 'زیردسته‌ها', path: '/account/dashboard/sub-categories' },
            { label: 'محصولات', path: '/account/dashboard/products' },
            { label: 'تصاویر محصول', path: '/account/dashboard/product-images' },
            { label: 'صدور کد تخفیف', path: '/account/dashboard/discount-codes' },
            { label: 'ویژگی‌ها', path: '/account/dashboard/properties' },
          ],
        },
      ],
    },
    {
      id: 'blog',
      title: '۱۰. مجله (بلاگ)',
      summary: 'مقالات، دسته، تگ، نظرات',
      blocks: [
        {
          type: 'ordered',
          items: [
            'دسته بلاگ: ساختار /blog/category/{slug}.',
            'تگ‌ها: برچسب مقالات.',
            'مقالات: عنوان، slug، خلاصه، بدنه، تصویر شاخص، انتشار.',
            'تگ مقاله: اتصال many-to-many.',
            'نظرات: تایید / رد در پنل نظرات بلاگ.',
            'Chrome بلاگ: CMS page slug=blog (هدر مخصوص بلاگ).',
          ],
        },
        {
          type: 'storeLinks',
          links: [{ label: 'مجله', path: '/blog' }],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'مقالات', path: '/account/dashboard/blog-posts' },
            { label: 'دسته بلاگ', path: '/account/dashboard/blog-categories' },
            { label: 'نظرات بلاگ', path: '/account/dashboard/blog-post-comments' },
          ],
        },
      ],
    },
    {
      id: 'marketing-tools',
      title: '۱۱. پرومو، خبرنامه و استوری',
      summary: 'ابزارهای بازاریابی',
      blocks: [
        {
          type: 'table',
          headers: ['ابزار', 'کاربرد', 'پنل'],
          rows: [
            ['Marketing Promos', 'کارت SALE/NEW و نوار disclaimer', 'حراج‌ها و محصولات جدید'],
            ['Newsletter', 'ایمیل‌های ثبت‌شده از فوتر', 'اعضای خبرنامه'],
            ['Stories', 'استوری اینستاگرامی بالای صفحه', 'استوری‌ها'],
            ['Assistant FAQ', 'سوالات پیشنهادی چت AI', 'سوالات هوش مصنوعی'],
          ],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'حراج‌ها', path: '/account/dashboard/marketing-promos' },
            { label: 'خبرنامه', path: '/account/dashboard/newsletter-subscribers' },
            { label: 'استوری‌ها', path: '/account/dashboard/stories' },
            { label: 'سوالات AI', path: '/account/dashboard/assistant-faq' },
          ],
        },
      ],
    },
    {
      id: 'users-profile',
      title: '۱۲. کاربران، نقش‌ها و تکمیل پروفایل',
      summary: 'دسترسی ادمین و کمپین پروفایل',
      blocks: [
        {
          type: 'list',
          items: [
            'کاربران: لیست حساب‌ها، فعال/غیرفعال.',
            'نقش‌ها: تعریف Permission برای پنل (مثلاً CMS، محصولات).',
            'سوالات پروفایل: طراحی کمپین تکمیل پروفایل (سوالات + گزینه‌ها).',
            'پاسخ‌های کاربران: مشاهده پاسخ‌های ثبت‌شده.',
          ],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'کاربران', path: '/account/dashboard/users' },
            { label: 'نقش‌ها', path: '/account/dashboard/roles' },
            { label: 'سوالات پروفایل', path: '/account/dashboard/profile-questions' },
            { label: 'پاسخ‌های کاربران', path: '/account/dashboard/profile-completion-answers' },
          ],
        },
      ],
    },
    {
      id: 'workflows',
      title: '۱۳. سناریوهای پرکاربرد — گام‌به‌گام',
      summary: 'کارهای روزمره',
      blocks: [
        {
          type: 'callout',
          variant: 'success',
          title: 'تغییر لینک فوتر',
          text: 'CMS → آیتم‌های بخش → Section مربوط به FooterColumn یا SiteFooter → فیلد URL را ویرایش → ذخیره.',
        },
        {
          type: 'callout',
          variant: 'success',
          title: 'اضافه کردن محصول جدید',
          text: 'دسته‌ها → محصولات → افزودن → slug و قیمت → تصاویر محصول → (اختیاری) توضیحات و ویژگی.',
        },
        {
          type: 'callout',
          variant: 'success',
          title: 'انتشار مقاله بلاگ',
          text: 'دسته بلاگ → مقالات → جدید → بدنه + تصویر → فعال → ذخیره → /blog/{slug}.',
        },
        {
          type: 'callout',
          variant: 'success',
          title: 'ویرایش متن «حریم خصوصی»',
          text: 'CMS Pages → slug=privacy → Page Sections → ContentBodyBlock items → ویرایش paragraph در Section Items.',
        },
      ],
    },
    {
      id: 'incomplete',
      title: '۱۴. بخش‌های در حال تکمیل',
      summary: 'هنوز نهایی نشده — بعداً به این راهنما اضافه می‌شود',
      incomplete: true,
      blocks: [
        {
          type: 'paragraph',
          text: 'برخی قابلیت‌ها در رابط کاربری وجود دارند ولی هنوز به‌طور کامل به backend وصل نشده‌اند یا داده mock دارند. قبل از اتکای عملیاتی، با تیم فنی هماهنگ کنید.',
        },
        {
          type: 'table',
          headers: ['بخش', 'وضعیت فعلی'],
          rows: [
            ['کیف پول (Wallet)', 'داده نمایشی / mock'],
            ['مدیریت سفارشات ادمین', 'در حال توسعه'],
            ['برخی API های checkout / delivery', 'ناقص'],
            ['Room Layout', 'صفحه آزمایشی'],
            ['مقایسه محصولات', 'عملکرد پایه'],
          ],
        },
        {
          type: 'callout',
          variant: 'warning',
          text: 'این فهرست به‌روزرسانی می‌شود. نسخه‌های بعدی این صفحه راهنما تکمیل خواهند شد.',
        },
      ],
    },
  ],
}

export const SITE_MANAGEMENT_GUIDE_EN: SiteManagementGuideContent = {
  sections: [
    {
      id: 'intro',
      title: '1. Introduction — what is this guide?',
      summary: 'Handover map for day-to-day site management',
      blocks: [
        {
          type: 'paragraph',
          text: 'This page is the full reference for running the Decorative Store. It explains where each storefront area (home, header, footer, blog, products, legal pages, etc.) is managed in the dashboard.',
        },
        {
          type: 'callout',
          variant: 'tip',
          title: 'Important',
          text: 'Storefront copy is bilingual (Persian and English). In CMS panels, pick the content language at the top of the form and save each locale separately.',
        },
        {
          type: 'list',
          items: [
            'Shop URL: site homepage',
            'Dashboard URL: /account/dashboard (after sign-in)',
            'Admin role required for content management',
            'CMS changes usually appear immediately; refresh if needed',
          ],
        },
      ],
    },
    {
      id: 'dashboard-overview',
      title: '2. Dashboard overview',
      summary: 'Sidebar groups',
      blocks: [
        {
          type: 'paragraph',
          text: 'After sign-in, use the sidebar (or mobile menu) to reach each area:',
        },
        {
          type: 'table',
          headers: ['Menu group', 'Purpose'],
          rows: [
            ['My account', 'Wallet, cart, orders, addresses, wishlist (customer-facing)'],
            ['Users & access', 'Users and roles (admin permissions)'],
            ['Store & products', 'Categories, products, images, discount codes'],
            ['Properties', 'Filters and product specs (color, material, size…)'],
            ['Journal', 'Blog categories, tags, posts, comments'],
            ['Content (CMS)', 'Pages, sections, footer, promos, newsletter, this guide'],
          ],
        },
      ],
    },
    {
      id: 'cms-basics',
      title: '3. CMS fundamentals',
      summary: 'Page → section → item hierarchy',
      blocks: [
        {
          type: 'paragraph',
          text: 'The CMS is built in four layers. Daily editing mostly involves Pages and Section Items.',
        },
        { type: 'diagram', id: 'cms-hierarchy' },
        {
          type: 'ordered',
          items: [
            'Section Types: block names like SiteHeader, ContentHero (usually pre-seeded).',
            'Sections: instances of a type with title/description.',
            'Section Items: actual content — menu links, footer text, paragraphs.',
            'CMS Pages: storefront pages with slugs — shop, about, privacy…',
            'Page Sections: links sections to pages with display priority.',
          ],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'CMS pages', path: '/account/dashboard/cms-pages' },
            { label: 'Section types', path: '/account/dashboard/cms-section-types' },
            { label: 'Sections', path: '/account/dashboard/cms-sections' },
            { label: 'Section items', path: '/account/dashboard/cms-section-items' },
            { label: 'Page sections', path: '/account/dashboard/cms-page-sections' },
          ],
        },
      ],
    },
    {
      id: 'site-map',
      title: '4. Site map',
      blocks: [{ type: 'diagram', id: 'site-map' }],
    },
    {
      id: 'chrome',
      title: '5. Header, footer & top bars',
      blocks: [
        {
          type: 'paragraph',
          text: 'Shared chrome comes mainly from CMS page slug=shop. Content pages merge shared chrome when they have no own footer.',
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'CMS pages', path: '/account/dashboard/cms-pages' },
            { label: 'Section items', path: '/account/dashboard/cms-section-items' },
            { label: 'Marketing promos', path: '/account/dashboard/marketing-promos' },
          ],
        },
      ],
    },
    {
      id: 'homepage',
      title: '6. Homepage',
      blocks: [
        { type: 'diagram', id: 'homepage-layout' },
        {
          type: 'table',
          headers: ['Home block', 'Content source'],
          rows: [
            ['Hero slider', 'CMS HeroCarousel section items on shop page'],
            ['SALE / NEW tiles', 'Marketing Promos + CMS PromoTileStrip'],
            ['Category strip', 'CMS CategoryNav + active catalog categories'],
            ['Featured shop grid', 'FeaturedShopGrid — featured product collections API'],
            ['Design services strip', 'CMS DesignServicesStrip'],
          ],
        },
        {
          type: 'ordered',
          items: [
            'Hero slider: Section Items for a HeroCarousel section — title, image, URL (CTA link), description (eyebrow:…|subtitle|cta:button label).',
            'Promo bar: Marketing Promos or PromoAnnouncement section.',
            'Categories: create in Categories first; adjust order in CMS if needed.',
          ],
        },
        {
          type: 'storeLinks',
          links: [{ label: 'View homepage', path: '/' }],
        },
        {
          type: 'adminLinks',
          links: [
            { label: 'Marketing promos', path: '/account/dashboard/marketing-promos' },
            { label: 'Categories', path: '/account/dashboard/categories' },
            { label: 'Page sections (shop)', path: '/account/dashboard/cms-page-sections' },
            { label: 'Section items (hero slider)', path: '/account/dashboard/cms-section-items' },
          ],
        },
      ],
    },
    {
      id: 'about-contact',
      title: '7. About & Contact',
      blocks: [
        {
          type: 'storeLinks',
          links: [
            { label: 'About', path: '/about' },
            { label: 'Contact', path: '/contact' },
          ],
        },
      ],
    },
    {
      id: 'legal-pages',
      title: '8. Legal & content pages',
      blocks: [
        {
          type: 'table',
          headers: ['Path', 'CMS slug'],
          rows: [
            ['/privacy', 'privacy'],
            ['/terms', 'terms'],
            ['/returns', 'returns'],
            ['/delivery', 'delivery'],
            ['/design-services', 'design-services'],
          ],
        },
      ],
    },
    {
      id: 'catalog',
      title: '9. Catalog — categories & products',
      blocks: [
        {
          type: 'adminLinks',
          links: [
            { label: 'Categories', path: '/account/dashboard/categories' },
            { label: 'Products', path: '/account/dashboard/products' },
          ],
        },
      ],
    },
    {
      id: 'blog',
      title: '10. Journal (blog)',
      blocks: [
        {
          type: 'adminLinks',
          links: [{ label: 'Blog posts', path: '/account/dashboard/blog-posts' }],
        },
      ],
    },
    {
      id: 'marketing-tools',
      title: '11. Promos, newsletter & stories',
      blocks: [
        {
          type: 'adminLinks',
          links: [
            { label: 'Marketing promos', path: '/account/dashboard/marketing-promos' },
            { label: 'Newsletter', path: '/account/dashboard/newsletter-subscribers' },
          ],
        },
      ],
    },
    {
      id: 'users-profile',
      title: '12. Users, roles & profile completion',
      blocks: [
        {
          type: 'adminLinks',
          links: [
            { label: 'Users', path: '/account/dashboard/users' },
            { label: 'Roles', path: '/account/dashboard/roles' },
          ],
        },
      ],
    },
    {
      id: 'workflows',
      title: '13. Common workflows',
      blocks: [
        {
          type: 'callout',
          variant: 'success',
          title: 'Change a footer link',
          text: 'CMS → Section items → Footer section → edit URL → save.',
        },
      ],
    },
    {
      id: 'incomplete',
      title: '14. Work in progress',
      incomplete: true,
      blocks: [
        {
          type: 'callout',
          variant: 'warning',
          text: 'Wallet uses mock data; admin orders and some checkout APIs are still being completed.',
        },
      ],
    },
  ],
}

export function getSiteManagementGuide(locale: 'fa' | 'en'): SiteManagementGuideContent {
  return locale === 'fa' ? SITE_MANAGEMENT_GUIDE_FA : SITE_MANAGEMENT_GUIDE_EN
}
