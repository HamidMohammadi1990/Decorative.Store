# راهنمای انتشار (Publish)

چک‌لیست کوتاه برای deploy فروشگاه **Diba Gallery** — فرانت React + API دات‌نت.

---

## ۱. فایل env فرانت (`ReactUI/.env.production`)

این فایل را قبل از build بسازید (یا در CI/CD ست کنید):

```env
# آدرس عمومی سایت — بدون / در آخر
VITE_SITE_URL=https://dibagallery.com

# آدرس API برای SSR و prerender (Node به بک‌اند وصل می‌شود)
SSR_API_TARGET=https://api.dibagallery.com

# معمولاً خالی بماند — مرورگر از همان دامنه /api/* استفاده می‌کند
VITE_API_BASE_URL=
```

| متغیر | اجباری | توضیح |
|--------|--------|--------|
| `VITE_SITE_URL` | **بله** (production) | canonical، Open Graph، sitemap، اشتراک لینک |
| `SSR_API_TARGET` | **بله** (SSR/prerender) | fetch سرور هنگام render — باید API در دسترس Node باشد |
| `VITE_API_BASE_URL` | خیر | فقط اگر API روی دامنه جدا است و proxy ندارید |
| `VITE_AUTH_USE_MOCK` | خیر | **هرگز `true` نگذارید** در production |
| `VITE_MAP_TILE_URL` | خیر | فقط برای نقشه پولی (Neshan، Map.ir، …) |
| `VITE_WEB_VITALS_ENDPOINT` | خیر | URL برای ارسال Core Web Vitals |

> در development می‌توانید `VITE_SITE_URL` را خالی بگذارید — از `window.location.origin` استفاده می‌شود.

---

## ۲. تنظیمات API (`BackEndProject`)

فایل production (مثلاً `appsettings.Production.json` یا secrets سرور):

```json
{
  "ConnectionStrings": {
    "EditionDbContext": "…"
  },
  "CorsSettings": {
    "AllowedOrigins": [
      "https://dibagallery.com",
      "https://www.dibagallery.com"
    ]
  },
  "AllowedHosts": "dibagallery.com;www.dibagallery.com;api.dibagallery.com",
  "SiteSettings": {
    "JwtSettings": {
      "SecretKey": "…",
      "EncryptKey": "…"
    }
  },
  "SecuritySettings": {
    "GeneralKey": "…"
  },
  "RedisConfiguration": { "…": "…" }
}
```

| مورد | چرا |
|------|-----|
| **ConnectionStrings** | دیتابیس SQL Server |
| **CorsSettings** | اگر فرانت مستقیم به API وصل شود (بدون proxy) |
| **JwtSettings / SecuritySettings** | کلیدهای امن — از dev کپی نکنید |
| **Redis** | cache و session |
| **ForwardedHeaders** | پشت reverse proxy (IIS/Nginx) |

API را publish کنید و مطمئن شوید `/Uploads` و `/api/v1/*` در دسترس هستند.

---

## ۳. Build و اجرا

### حالت پیشنهادی — SSR + SEO کامل

```bash
cd ReactUI
npm ci
npm run build:seo    # client + SSR + prerender صفحات ثابت
npm run start:ssr    # سرور Node روی پورت 5173 (یا PORT)
```

| اسکریپت | کار |
|---------|-----|
| `npm run build` | فقط SPA (بدون SSR) |
| `npm run build:ssr` | client + bundle سرور |
| `npm run build:seo` | **build:ssr + prerender** — برای ایندکس بهتر |
| `npm run start:ssr` | production با proxy به `/api` و `/Uploads` |

متغیرهای runtime سرور Node:

```env
NODE_ENV=production
PORT=5173
HOST=0.0.0.0
SSR_API_TARGET=https://api.dibagallery.com
```

### پشت Nginx / IIS

- ترافیک عمومی → Node (`start:ssr`) یا CDN برای `dist/client`
- `/api/*` و `/Uploads/*` → proxy به API دات‌نت
- `/sitemap-dynamic.xml` → proxy به `/api/v1/seo/sitemap.xml`
- HTTPS اجباری

---

## ۴. سئو — بعد از deploy

1. **`VITE_SITE_URL`** باید همان دامنه نهایی باشد (https، بدون slash آخر).
2. **Google Search Console** — sitemap را ثبت کنید:  
   `https://dibagallery.com/sitemap.xml`
3. **پیش‌نمایش اشتراک لینک** (OG/Twitter):
   - [Facebook Debugger](https://developers.facebook.com/tools/debug/)
   - [LinkedIn Inspector](https://www.linkedin.com/post-inspector/)
4. **`robots.txt`** — در `ReactUI/public/robots.txt` (مسیرهای account/cart/checkout noindex).
5. صفحات prerender شده: `/`, `/about`, `/contact`, `/blog`, `/room-layout`, و صفحات CMS ثابت (لیست در `ReactUI/scripts/prerender-routes.mjs`).

---

## ۵. چک‌لیست سریع قبل از go-live

- [ ] `VITE_SITE_URL` = دامنه واقعی
- [ ] `SSR_API_TARGET` = API زنده و reachable از سرور Node
- [ ] دیتابیس migrate شده
- [ ] JWT و Security keys production (تازه)
- [ ] CORS / proxy درست
- [ ] HTTPS فعال
- [ ] `npm run build:seo` بدون خطا
- [ ] View Source یک صفحه محصول → `<title>`, `og:image`, JSON-LD
- [ ] `/sitemap.xml` و `/robots.txt` باز می‌شوند
- [ ] تصاویر `/Uploads` لود می‌شوند

---

## ۶. نمونه `.env.production` (کپی-پیست)

```env
VITE_SITE_URL=https://dibagallery.com
SSR_API_TARGET=https://api.dibagallery.com
VITE_API_BASE_URL=
NODE_ENV=production
PORT=5173
HOST=0.0.0.0
```

---

## ۷. عیب‌یابی رایج

| مشکل | احتمال |
|------|--------|
| OG بدون تصویر / لینک اشتباه | `VITE_SITE_URL` ست نشده یا build قدیمی |
| صفحه خالی در SSR | `SSR_API_TARGET` اشتباه یا API down |
| CORS error | `CorsSettings` یا proxy `/api` |
| sitemap خالی | API سئو یا proxy `sitemap-dynamic.xml` |
| زبان اشتباه در SSR | cookie locale یا `?hl=fa` / `?hl=en` |

---

*آخرین به‌روزرسانی: ساختار ReactUI + Store.Api — سئو چندزبانه (fa/en).*
