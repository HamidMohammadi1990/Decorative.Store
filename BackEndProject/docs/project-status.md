# وضعیت پروژه Edition

> **هدف این سند:** مرجع واحد برای برنامه‌نویس انسانی یا هوش مصنوعی تا بدون جست‌وجوی گسترده در کد، بداند **چه چیزی پیاده شده**، **چه چیزی نیمه‌کاره است**، **چه چیزی اصلاً وجود ندارد**، و **کجا باید ادامه کار داده شود**.
>
> **آخرین بازبینی:** ۱۵ ژوئیه ۲۰۲۶ (به‌روزرسانی: زیرساخت چندزبانگی — جدول `Language`، Registry/Redis، API ادمین زبان)

---

## فهرست

1. [خلاصه اجرایی](#۱-خلاصه-اجرایی)
2. [معماری و ساختار Solution](#۲-معماری-و-ساختار-solution)
3. [تکنولوژی‌ها](#۳-تکنولوژی‌ها)
4. [لایه Domain](#۴-لایه-domain)
5. [لایه Application (فیچرها)](#۵-لایه-application-فیچرها)
6. [Infrastructure و Persistence](#۶-infrastructure-و-persistence)
7. [API و کنترلرها](#۷-api-و-کنترلرها)
8. [احراز هویت، نشست و دسترسی](#۸-احراز-هویت-نشست-و-دسترسی)
9. [جریان سفارش، پرداخت و حسابداری](#۹-جریان-سفارش-پرداخت-و-حسابداری)
10. [Content Policy](#۱۰-content-policy)
11. [تست‌ها](#۱۱-تست‌ها)
12. [پیکربندی و استقرار](#۱۲-پیکربندی-و-استقرار)
13. [ماتریس فیچرها (پیاده / نیمه / نشده)](#۱۳-ماتریس-فیچرها)
14. [ضعف‌ها و کسری‌های فنی](#۱۴-ضعف‌ها-و-کسری‌های-فنی)
15. [تصمیمات معماری مهم](#۱۵-تصمیمات-معماری-مهم)
16. [کارهای باقی‌مانده (اولویت‌بندی)](#۱۶-کارهای-باقی‌مانده)
17. [راهنمای سریع برای توسعه](#۱۷-راهنمای-سریع-برای-توسعه)
18. [مستندسازی API (Swagger / ReDoc)](#۱۸-مستندسازی-api-swagger--redoc)
19. [چندزبانگی و زبان (Localization / Language)](#۱۹-چندزبانگی-و-زبان-localization--language)

---

## ۱. خلاصه اجرایی

**Edition** یک بک‌اند **.NET 10** با معماری **Clean Architecture** برای پلتفرم **چاپ و تجارت الکترونیک** است. دامنه کاری شامل:

- کاتالوگ محصولات چاپی با ویژگی‌ها، قیمت‌گذاری، فایل و پیوست
- CMS (صفحات، سکشن‌ها، بلاگ)
- سبد خرید و پرداخت چندشرکتی
- کیف پول و تراکنش بانکی
- RBAC ادمین با permission داینامیک
- موتور Content Policy برای فیلتر داده در سطح query

### وضعیت کلی

| حوزه | وضعیت | توضیح کوتاه |
|------|--------|-------------|
| CRUD کاتالوگ/CMS | ✅ قوی | ۵۰+ فیچر MediatR با Validator |
| احراز هویت کاربر | ✅ قوی | JWT، refresh token، نشست، OTP |
| RBAC ادمین | ✅ قوی | Permission از روی attribute کنترلر |
| **زیرساخت زبان (i18n DB)** | ✅ | جدول `Language`، Registry/Redis، middleware، API ادمین CRUD |
| **ترجمه محتوای DB** | ❌ | فقط زیرساخت — entityهای `*Translation` هنوز پیاده نشده |
| سبد خرید / خرید | ✅ قوی | Purchase، property، attachment |
| پرداخت | 🟡 نیمه | منطق wallet/bank/VAT کامل؛ درگاه واقعی stub |
| Verify پرداخت | 🟡 نیمه | Handler کامل؛ verify بانک همیشه موفق (stub) |
| پس از پرداخت | ❌ ضعیف | وضعیت سفارش فقط تا `InProgress` |
| SMS/Email | 🟡 نیمه | توکن در Redis؛ ارسال واقعی `true` برمی‌گرداند |
| حسابداری پیشرفته | 🟡 نیمه | سند مالی پرداخت + شارژ کیف پول؛ expense/POS/cheque فقط schema |
| **کیف پول (Wallet API)** | ✅ | لیست، تراکنش، شارژ ادمین، شارژ بانکی + verify |
| موجودی انبار | ❌ | وجود ندارد |
| تست API | ❌ | پروژه integration test برای API نیست |
| مستندات | 🟡 | این سند + `docs/content-policy-guide.md` + Swagger UI + پورتال `/docs` با دسته‌بندی کنترلر و Try it out داخلی |

**نمادها:** ✅ کامل — 🟡 نیمه‌کاره — ❌ پیاده نشده

---

## ۲. معماری و ساختار Solution

**فایل Solution:** `Edition.sln`

```
Edition.Api
  ├── Edition.WebFramework      (Base controllers, Swagger, ApiResult)
  ├── Edition.Infrastructure    (Redis, SMS, Email, Image, Identity, Serilog)
  └── Edition.Infrastructure.Persistence  (EF Core, Repositories, Migrations, Seed)

Edition.Infrastructure ──► Edition.Application
Edition.Infrastructure.Persistence ──► Edition.Application
Edition.Application ──► Edition.Domain + Edition.Common
```

### پروژه‌های سورس (۷)

| پروژه | مسیر | نقش |
|-------|------|-----|
| Edition.Domain | `src/Core/Edition.Domain/` | Entity، Enum، Repository Interface، Content Policy Domain |
| Edition.Application | `src/Core/Edition.Application/` | MediatR Features، Validators، Mappers، Services |
| Edition.Common | `src/Cross-Cutting/Edition.Common/` | Localization، Exception، Security helpers، **ApiControllerCategory** (enum + resource) |
| Edition.Infrastructure | `src/Infrastructure/Edition.Infrastructure/` | سرویس‌های بیرونی |
| Edition.Infrastructure.Persistence | `src/Infrastructure/Edition.Infrastructure.Persistence/` | EF Core، Repository، Migration |
| Edition.WebFramework | `src/Presentation/Edition.WebFramework/` | زیرساخت API |
| Edition.Api | `src/Presentation/Edition.Api/` | Host، Controllers، Middleware، DI |

### پروژه‌های تست (۶)

| پروژه | مسیر |
|-------|------|
| Edition.Arch.Tests | `tests/Edition.ArchTests/` |
| Edition.Common.Tests | `tests/Edition.Common.Tests/` |
| Edition.Domain.Tests | `tests/Edition.Domain.Tests/` |
| Edition.Application.Tests | `tests/Edition.Application.Tests/` |
| Edition.Infrastructure.Tests | `tests/Edition.Infrastructure.Tests/` |
| Edition.Infrastructure.Persistence.Tests | `tests/Edition.Infrastructure.Persistence.Tests/` |

**الگوی اصلی:** CQRS با MediatR + FluentValidation + Repository + Unit of Work

---

## ۳. تکنولوژی‌ها

| حوزه | کتابخانه / ابزار |
|------|------------------|
| Runtime | .NET 10 (`net10.0`) |
| ORM | EF Core 10 + SQL Server |
| CQRS | MediatR 12 |
| Validation | FluentValidation 12 |
| DI | Autofac + Castle DynamicProxy (Cache Interceptor روی Repository) |
| Auth | JWT Bearer |
| Cache | StackExchange.Redis + RedLock.net |
| Logging | Serilog (sink به SQL Server) |
| API Docs | Swashbuckle + Asp.Versioning |
| Test | xUnit v3، FluentAssertions، NSubstitute، NetArchTest، Testcontainers.MsSql، Respawn |
| Image | ImageSharp |
| Password | BCrypt |

---

## ۴. لایه Domain

### آمار کلی

- **~۸۰ Entity** در `src/Core/Edition.Domain/Entities/`
- **۴۳ Enum** در `src/Core/Edition.Domain/Enums/`
- **۶۳ Repository Interface** در `src/Core/Edition.Domain/Repositories/`

### گروه‌بندی Entityها

#### کاتالوگ و CMS
`Category`, `SubCategory`, `Product`, `ProductDescription`, `ProductFile`, `ProductFeatureType`, `ProductProperty`, `ProductPropertyPrice`, `ProductPropertyRule`, `ProductPrice`, `ProductPriceDeliveryOption`, `ProductOrderItemAttachmentType`, `Property`, `PropertyCategory`, `PropertyItem`, `PropertyItemPrice`, `PropertyItemDependency`, `Tag`, `Page`, `PageSection`, `Section`, `SectionItem`, `SectionType`, `PostType`, `WebSiteSetting`

#### بلاگ و نظرات
`BlogPost`, `BlogPostCategory`, `BlogPostTag`, `BlogPostComment`, `BlogPostLike`, `CommentTopic`, `ProductComment`, `CompanyComment`

#### استوری کمپانی
`CompanyStory`, `CompanyStoryItem`, `CompanyStoryComment`, `CompanyStoryLike`

#### تجارت و سفارش
`Order`, `OrderItem`, `OrderItemProperty`, `OrderItemAttachment`, `OrderItemAttachmentType`, `OrderItemAttachmentTypeRestriction`, `OrderNote`, `OrderVat`, `OrderCommission`, `Discount`, `DeliveryType`, `DeliveryOption`, `Company`, `CompanyProduct`, `CompanyPosDevice`

#### کاربر و دسترسی
`User`, `UserRole`, `UserAddress`, `UserSession`, `RefreshToken`, `Role`, `Permission`, `RolePermission`

#### مالی و بانک
`Bank`, `BankAccount`, `BankTransaction`, `Wallet`, `WalletTransaction`, `Currency`, `ChartOfAccount`, `FinancialYear`, `FinancialDocument`, `FinancialDocumentDetail`, `Expense`, `ExpenseType`, `ChequeTransaction`, `PosTransaction`

#### Content Policy
`ContentPolicy`, `ContentPolicyRule`, `ContentPolicyRecordAccess`

#### چندزبانگی (زیرساخت)
`Language` — مرجع زبان‌های فعال؛ **بدون** جدول `*Translation` تا فاز ۲+

### منطق دامنه مهم

| موضوع | فایل(ها) | توضیح |
|-------|----------|-------|
| چرخه سفارش | `Entities/Order.cs` | سبد، تخفیف، VAT، commission، tracking code، `CompletePayment()` |
| Content Policy | `ContentPolicies/` | Resolver، FilterExpressions، Merge mode |
| Query Filter | `QueryFilters/` | فیلتر داینامیک روی query |

### Entityهایی که در DB هستند ولی Feature مستقل ندارند

این‌ها عمدتاً از طریق aggregate سفارش یا آینده حسابداری استفاده می‌شوند:

`Expense`, `ExpenseType`, `ChequeTransaction`, `PosTransaction`, `Currency`, `CompanyProduct`, `OrderNote`, `OrderCommission`, `OrderVat`, `WalletTransaction`, `FinancialDocumentDetail`, `PropertyItemDependency`

---

## ۵. لایه Application (فیچرها)

**مسیر:** `src/Core/Edition.Application/Features/` — **۵۷ پوشه فیچر**

### الگوی هر فیچر

```
Features/{Name}/
  Commands/{Action}/   → Request, Response, Handler, Validator
  Queries/{Action}/    → Request, Response, Handler, Validator
```

### Pipeline Behaviors

| Behavior | مسیر | نقش |
|----------|------|-----|
| ValidationBehavior | `Common/Behaviors/ValidationBehavior.cs` | FluentValidation قبل از Handler |
| ContentPolicyQueryBehavior | `Common/Behaviors/ContentPolicyQueryBehavior.cs` | فیلتر query |
| ContentPolicyResourceBehavior | `Common/Behaviors/ContentPolicyResourceBehavior.cs` | فیلتر resource |

### Content Policy — فلو query (GetAll / Search)

```
ContentPolicyQueryBehavior → request.ContentPolicyFilter (property روی request)
Handler → mapper.Map(request).WithContentPolicy(...) → RequestDto.ContentFilter
Repository → ApplyContentPolicyFilter(request.ContentFilter)
```

- Requestهای لیست/جستجو از `ContentPolicyRequest<TEntity>` ارث می‌برند
- propertyهای داخلی (`ContentPolicyFilter`, `IsContentPolicyResolved`, `ContentPolicyDenyAll`) با `[JsonIgnore]` دوگانه (Newtonsoft + System.Text.Json) از API و Swagger مخفی می‌شوند
- جزئیات کامل: `docs/content-policy-guide.md` بخش ۰

### جدول فیچرها

**راهنما:** ✅ = پیاده | ❌ = ندارد | **جزئی** = عمداً محدود

| فیچر | Commands | Get/GetAll | Search | وضعیت |
|------|----------|------------|--------|--------|
| Banks | CRUD | ✅ | ✅ | ✅ |
| BlogPostCategories | CRUD | ✅ | ✅ | ✅ |
| BlogPostComments | CRUD + Approve | ✅ | ✅ | ✅ |
| BlogPostLikes | Create | ✅ | ❌ | ✅ |
| BlogPostTags | CRUD | ✅ | ✅ | ✅ |
| BlogPosts | CRUD + Publish | ✅ | ✅ | ✅ |
| Categories | CRUD | ✅ | ✅ + tree | ✅ |
| ChartOfAccounts | CRUD | ✅ | ❌ | ✅ |
| Cities | CRUD | ✅ | ✅ | ✅ |
| CommentTopics | CRUD | ✅ | ❌ | ✅ |
| Companies | CRUD | ✅ | ✅ | ✅ |
| CompanyComments | CRUD + ChangeStatus | ✅ | ✅ | ✅ |
| CompanyStories | CRUD + Activate/Deactivate | ✅ | ✅ | ✅ |
| CompanyStoryComments | CRUD + Approve | ✅ | ✅ | ✅ |
| CompanyStoryLikes | Create | ✅ | ❌ | ✅ |
| CompanyPosDevices | CRUD | ✅ | ❌ | ✅ |
| ContentPolicies | CRUD | ✅ | ❌ | ✅ |
| ContentPolicyMetadata | — | ۸ query | ❌ | ✅ |
| ContentPolicyRecordAccesses | Create/Set/Delete | ✅ | ❌ | ✅ |
| ContentPolicyRules | CRUD | ✅ | ❌ | ✅ |
| DeliveryOptions | CRUD | ✅ | ✅ | ✅ |
| DeliveryTypes | CRUD | ✅ | ✅ | ✅ |
| Discounts | CRUD | ✅ | ❌ | ✅ |
| FinancialYears | CRUD | ✅ | ❌ | ✅ |
| **Languages** | CRUD + SetDefault | ✅ | ❌ | ✅ (ادمین — بدون public search) |
| **Orders** | Purchase, Payment, Verify, RemoveItem, RemoveDiscount | ✅ | ❌ | 🟡 |
| Pages | CRUD | ✅ + slug | ✅ | ✅ |
| PageSections | CRUD | ✅ | ✅ | ✅ |
| Permissions | CRUD | ✅ | ❌ | ✅ |
| PostTypes | CRUD | ✅ | ✅ | ✅ |
| ProductComments | CRUD + ChangeStatus | ✅ | ✅ | ✅ |
| ProductDescriptions | CRUD | ✅ | ✅ | ✅ |
| ProductFeatureTypes | CRUD | ✅ | ✅ | ✅ |
| ProductFiles | CreateRange, Delete, Status | ✅ | ✅ | ✅ |
| ProductOrderItemAttachmentTypes | CRUD | ✅ | ✅ | ✅ |
| ProductPriceDeliveryOptions | CRUD | ✅ | ❌ | ✅ |
| ProductPrices | CRUD | ✅ | ❌ | ✅ |
| ProductProperties | CRUD | ✅ | ✅ | ✅ |
| ProductPropertyPrices | CRUD | ✅ | ❌ | ✅ |
| ProductPropertyRules | CRUD | ✅ | ✅ | ✅ |
| **Products** | CRUD | ✅ | ❌ | 🟡 (بدون Search) |
| Properties | CRUD | ✅ | ❌ | ✅ |
| PropertyCategories | CRUD | ✅ | ❌ | ✅ |
| PropertyItemPrices | CRUD | ✅ | ❌ | ✅ |
| PropertyItems | CRUD | ✅ | ❌ | ✅ |
| Provinces | CRUD | ✅ | ✅ | ✅ |
| RefreshTokens | Generate | ❌ | ❌ | ✅ |
| RolePermissions | Create/Delete | ✅ | ❌ | ✅ |
| Roles | CRUD | ✅ | ❌ | ✅ |
| Sections | CRUD | ✅ | ✅ | ✅ |
| SectionItems | CRUD | ✅ | ✅ | ✅ |
| SectionTypes | CRUD | ✅ | ✅ | ✅ |
| SubCategories | CRUD | ✅ | ✅ | ✅ |
| Tags | CRUD | ✅ | ❌ | ✅ |
| UserAddresses | CRUD | ✅ | ❌ | ✅ |
| UserRoles | Create/Delete | ✅ | ❌ | ✅ |
| **Wallets** | Create, Update, Status, AdminCharge, Charge, VerifyCharge | GetMy, Get, GetAll, Transactions | ❌ | ✅ |
| **Users** | SignIn, Register, OTP, Password, Email, Phone | ✅ | ❌ | ✅ |
| UserSessions | Revoke | GetActive | ❌ | ✅ |
| WebSiteSettings | Update | Get + GetPublic | ❌ | ✅ |

### سرویس‌های Application مهم

| سرویس | مسیر | نقش |
|-------|------|-----|
| AccountingService | `Services/AccountingService.cs` | JWT، refresh token، permission، block token |
| UserSessionService | `Services/UserSessionService.cs` | مدیریت نشست |
| BankPaymentVerificationService | `Services/Orders/BankPaymentVerificationService.cs` | **STUB** — verify بانک |
| LocalFileService | `Common/Utilities/Services/LocalFileService.cs` | آپلود فایل محلی |
| ContentPolicies/* | `Services/ContentPolicies/` | ۱۵+ فایل موتور policy |
| LanguageRegistry | `Services/Localization/LanguageRegistry.cs` | کش Redis زبان‌ها (`edition:languages:*`) |
| LanguageBootstrapService | `Infrastructure.Persistence/Localization/` | seed اولیه از `LocalizationSettings` + warmup Redis |

---

## ۶. Infrastructure و Persistence

### Repository

- **۶۳/۶۳** interface در Domain پیاده‌سازی EF دارند
- مسیر: `src/Infrastructure/Edition.Infrastructure.Persistence/Repositories/`
- ثبت DI: Autofac `RepositoryModule` + `CacheInterceptor`

### DbContext

`EditionDbContext.cs` — ۷۵+ DbSet (شامل `Language`, `CompanyStory`, `CompanyStoryItem`, `CompanyStoryComment`, `CompanyStoryLike`)

### Migrationها (تا ۱۵ ژوئیه ۲۰۲۶)

| Migration | موضوع |
|-----------|--------|
| `20260208115637_Init` | schema اولیه |
| `20260208120715_*` | طول title دسته‌بندی |
| `20260208122008_*` | طول title محصول |
| `20260615120000_*` | فیلدهای تخفیف سفارش |
| `20260615140000_*` | tracking code تصادفی |
| `20260615160000_*` | user session |
| `20260615170000_*` | password hash |
| `20260615180000` تا `20260615290000` | Content Policy engine |
| `20260715090000_add-language-table` | جدول `Language` (Code، IsDefault unique filter) |
| `AddCompanyStory` | **در انتظار اجرا** — جداول استوری کمپانی (EF config آماده است) |

### سرویس‌های بیرونی

| سرویس | وضعیت | فایل |
|-------|--------|------|
| Redis Cache | ✅ | `CacheProviders/Redis/` |
| SMS | 🟡 توکن OK، ارسال fake | `SmsProviders/SmsService.cs` |
| Email | 🟡 توکن OK، ارسال fake | `EmailProviders/EmailServie.cs` (typo در نام کلاس) |
| Image | ✅ | `Services/ImageService.cs` |
| Identity Context | ✅ | `Identity/HttpCurrentUserContext.cs` |
| Current Language Context | ✅ | `Infrastructure/Localization/CurrentLanguageContext.cs` |
| Serilog → SQL | ✅ | `LogProviders/SerilogConfig.cs` |
| File Storage | ✅ محلی | `wwwroot/Uploads/Products/` |

### Seed

`SeedData/SeedService.cs` — داده اولیه + permission + محصولات JSON

**زبان:** seed جدول `Language` در **startup** توسط `LanguageBootstrapService` انجام می‌شود (اگر جدول خالی باشد، از `LocalizationSettings` در appsettings). این seed جدا از `SeedService` است.

**نکته:** `SeedChartOfAccountsAsync()` کامنت شده — حساب‌های ۱۱، ۱۴۶، ۱۴۷ در پرداخت hardcode هستند.

---

## ۷. API و کنترلرها

### Base Controller

| کلاس | Route | Auth |
|------|-------|------|
| `BaseApiController` | `api/v{version}/{controller}` | پیش‌فرض: بدون احراز |
| `BaseApiAdminController` | `api/v{version}/admin/{controller}` | `[Authorize]` + Permission |

فایل: `src/Presentation/Edition.WebFramework/Api/BaseApiController.cs`

### دسته‌بندی کنترلر در مستندات API

هر کنترلر (ادمین و پابلیک) با اتریبیوت **`[ApiControllerCategory(...)]`** در گروه مستندات قرار می‌گیرد.

| مورد | مسیر / توضیح |
|------|----------------|
| Attribute | `Edition.Api/Attributes/ApiControllerCategoryAttribute.cs` |
| Enum | `Edition.Common/Enums/ApiControllerCategory.cs` |
| Resource (en/fa) | `Edition.Common/Resources/ControllerCategoryResources.{resx,fa.resx}` |
| Extension | `Edition.Common/Extensions/ApiControllerCategoryExtensions.cs` — `GetDisplayName()` / `GetEnglishDisplayName()` |

**دسته‌های enum:** Authentication، General، Users، Company، Catalog، Blog، CMS، Orders، Wallet، Financial، Product، Location، Property، Content Policy، Access Control، Comments، Delivery، Post Type، Tags، **Localization**

**وضعیت:** ✅ روی **همه ۹۳ کنترلر** (Admin + Public) اعمال شده است.

**قانون:** کنترلر جدید **باید** `[ApiControllerCategory(...)]` داشته باشد؛ در غیر این صورت تولید Swagger خطا می‌دهد.

### قرارداد Endpoint (هدف معماری — در حال تکمیل)

| Endpoint | لایه | احراز | کاربرد |
|----------|------|-------|--------|
| `POST .../search` | **Public** | ❌ | جست‌وجوی سایت |
| `POST/GET .../get-all` | **Admin** | ✅ + Permission | لیست پنل |
| `POST/GET .../get` | **Admin** | ✅ + Permission | جزئیات پنل |
| `POST .../create` و CUD | **Admin** (یا Public با `[Authorize]`) | بسته به منبع | تغییر داده |

### کنترلرهای Public با `search` (بدون احراز)

`bank`, `blog-post`, `blog-post-category`, `blog-post-comment`, `blog-post-tag`, `category` (+ `tree`), `city`, `company` (+ CUD با Authorize), `company-comment`, `company-story`, `company-story-comment`, `delivery-option`, `delivery-type`, `page` (+ `get-by-slug`), `page-section`, `post-type`, `product-comment`, `product-description`, `product-feature-type`, `product-file`, `product-order-item-attachment-type`, `product-property`, `product-property-rule`, `province`, `section`, `section-item`, `section-type`, `sub-category`

### استثناهای Public

| کنترلر | Endpoint | دلیل |
|--------|----------|------|
| WebSiteSetting | `POST get` | `GetPublicWebSiteSettingRequest` — تنظیمات عمومی سایت |
| Category | `GET tree` | درخت دسته با محصول |
| Page | `POST get-by-slug` | صفحه CMS با slug |
| BlogPostLike | `POST create` | لایک بدون search |
| CompanyStoryLike | `POST create` | لایک استوری بدون search |
| Account | auth endpoints | ورود/ثبت‌نام |
| Order | checkout/purchase/payment | سبد و پرداخت |
| UserAddress | CRUD با Authorize | آدرس کاربر |

### کنترلرهای Public **بدون** `search` (فقط ادمین read)

این موجودیت‌ها Handler جست‌وجو ندارند؛ خواندن فقط از ادمین:

`product`, `tag`, `discount`, `comment-topic`, `property`, `property-category`, `property-item`, `property-item-price`, `product-price`, `product-property-price`, `product-price-delivery-option`

> **وضعیت refactor:** برخی از این کنترلرهای public هنوز `get-all`/`get` دارند و باید حذف شوند (فقط endpoint ادمین بماند). اسکریپت کمکی: `tools/refactor-controller-endpoints.py`

### OrderController (Public)

`src/Presentation/Edition.Api/Controllers/v1/OrderController.cs`

| Method | Route | وضعیت |
|--------|-------|--------|
| POST | `checkout` | ✅ |
| POST | `purchase` | ✅ |
| POST | `validate-discount` | ✅ |
| POST | `payment` | ✅ |
| POST | `payment/verify` | ✅ (stub verify) |
| DELETE | `discount` | ✅ |
| DELETE | `item` | ✅ |

**ندارد:** لیست سفارش کاربر، جزئیات سفارش کاربر (فقط در Admin)

### Admin OrderController

`get-all`, `get`, `by-status`, `detail`, `status-summary`

### CompanyStory (استوری کمپانی)

**مسیر فیچر:** `Features/CompanyStories/`, `Features/CompanyStoryComments/`, `Features/CompanyStoryLikes/`

#### API عمومی

| Method | Route | Handler / توضیح |
|--------|-------|-----------------|
| POST | `/api/v1/company-story/search` | `SearchCompanyStoryHandler` — استوری‌های فعال و منقضی‌نشده |
| POST | `/api/v1/company-story-comment/search` | `SearchCompanyStoryCommentHandler` — نظرات تأییدشده |
| POST | `/api/v1/company-story-comment/create` | `[Authorize]` — ثبت نظر |
| PUT | `/api/v1/company-story-comment/update` | `[Authorize]` — ویرایش نظر |
| DELETE | `/api/v1/company-story-comment/delete` | `[Authorize]` — حذف نظر |
| POST | `/api/v1/company-story-like/create` | لایک (کاربر یا IP) |

#### API ادمین

| Method | Route | Permission |
|--------|-------|------------|
| POST | `/api/v1/admin/company-story/get-all` | ListCompanyStory |
| POST | `/api/v1/admin/company-story/get` | GetCompanyStoryById |
| POST | `/api/v1/admin/company-story/create` | CreateCompanyStory |
| PUT | `/api/v1/admin/company-story/update` | UpdateCompanyStory |
| POST | `/api/v1/admin/company-story/activate` | ActivateCompanyStory |
| POST | `/api/v1/admin/company-story/deactivate` | DeactivateCompanyStory |
| POST | `/api/v1/admin/company-story-comment/get-all` | ListCompanyStoryComment |
| POST | `/api/v1/admin/company-story-comment/get` | GetCompanyStoryCommentById |
| POST | `/api/v1/admin/company-story-comment/approve` | ApproveCompanyStoryComment |
| POST | `/api/v1/admin/company-story-like/get-all` | ListCompanyStoryLike |
| POST | `/api/v1/admin/company-story-like/get` | GetCompanyStoryLikeById |

**EF Config:** `CompanyStoryConfig`, `CompanyStoryItemConfig`, `CompanyStoryCommentConfig`, `CompanyStoryLikeConfig`

**Migration:** `dotnet ef migrations add AddCompanyStory` سپس `database update`

### Language (زیرساخت چندزبانگی — ادمین)

**مسیر فیچر:** `Features/Languages/`  
**مستند برنامه‌ریزی:** `docs/multilingual-tables-plan.md`

#### وضعیت فازها

| فاز | محتوا | وضعیت |
|-----|--------|--------|
| **۱** | جدول `Language`، `ILanguageRegistry` (Redis)، `ICurrentLanguageContext`، middleware، bootstrap از `LocalizationSettings` | ✅ |
| **۱.۵** | API ادمین CRUD + `set-default` + invalidate Redis | ✅ |
| **۲+** | جداول `*Translation` برای entityهای محتوا (Product، Category، …) | ❌ هنوز شروع نشده |

#### API ادمین

Base: `/api/v1/admin/language` — دسته Swagger: **Localization**

| Method | Route | Permission | توضیح |
|--------|-------|------------|--------|
| POST | `get-all` | ListLanguage | لیست صفحه‌بندی‌شده با فیلتر Code/Name/IsActive/IsDefault |
| POST | `get` | GetLanguageById | جزئیات یک زبان |
| POST | `create` | CreateLanguage | ایجاد — `isDefault` همیشه `false` |
| PUT | `update` | UpdateLanguage | ویرایش — غیرفعال کردن زبان پیش‌فرض ممنوع |
| PUT | `set-default` | SetDefaultLanguage | تنظیم پیش‌فرض + پاک کردن default سایر رکوردها |
| DELETE | `delete` | DeleteLanguage | حذف — زبان پیش‌فرض یا آخرین زبان ممنوع |

**Body نمونه (create):**
```json
{ "code": "en-US", "name": "English", "isActive": true, "displayOrder": 2, "isRtl": false }
```

#### جریان runtime (هر request)

```
Accept-Language (یا fallback)
    → UseRequestLocalization (SupportedCultures از Registry/DB)
    → CurrentLanguageMiddleware
    → ICurrentLanguageContext (Scoped: LanguageId, LanguageCode)
    → Repositoryهای آینده (Search با زبان جاری — فاز ۲+)
```

#### کش Redis

| Key | محتوا |
|-----|--------|
| `edition:languages:data` | JSON لیست زبان‌ها |
| `edition:languages:version` | نسخه — با CUD ادمین `INCR` + invalidate |

پیکربندی: `LanguageCacheConfiguration` در appsettings (پیش‌فرض: `CacheInstance: AppSettings`).

#### دو لایه Localization

| لایه | منبع | مثال |
|------|------|------|
| پیام‌های API / validation | `.resx` + `RequestLocalizationOptions` | `MessageKeys.InvalidId` |
| محتوای DB (آینده) | `*Translation` + `LanguageId` | عنوان محصول per زبان |

**نکته:** `LocalizationSettings` در appsettings **فقط bootstrap/seed** است — runtime از جدول `Language` + Redis می‌خواند؛ تغییر appsettings بعد از deploy روی زبان‌های در حال اجرا اثر ندارد.

#### فایل‌های کلیدی

| نقش | مسیر |
|-----|------|
| Entity | `Edition.Domain/Entities/Language.cs` |
| Repository | `Infrastructure.Persistence/Repositories/LanguageRepository.cs` |
| Registry | `Application/Services/Localization/LanguageRegistry.cs` |
| Bootstrap | `Infrastructure.Persistence/Localization/LanguageBootstrapService.cs` |
| Middleware | `Edition.Api/Middlewares/CurrentLanguageMiddleware.cs` |
| Controller | `Edition.Api/Controllers/v1/Admin/LanguageController.cs` |
| Normalizer | `Edition.Common/Localization/LanguageCultureNormalizer.cs` |

**Permissionهای جدید (۴۸۰–۴۸۷):** `ManageLanguageGroup`, `ManageLanguage`, `ListLanguage`, `GetLanguageById`, `CreateLanguage`, `UpdateLanguage`, `DeleteLanguage`, `SetDefaultLanguage` — پس از deploy باید seed شوند.

### Middleware و Filter

| نام | مسیر | نقش |
|-----|------|-----|
| PermissionAuthorizeAttribute | `Filters/PermissionAuthorizeAttribute.cs` | فقط admin |
| BlockTokenControlMiddleware | `Middlewares/` | JWT block list |
| CurrentLanguageMiddleware | `Middlewares/` | resolve `LanguageId` از Accept-Language / Registry |
| CustomExceptionHandler | `Middlewares/` | خطای یکپارچه + correlation id |
| LocalizationResultFilter | `Filters/` | پیام خطای localized |

---

## ۸. احراز هویت، نشست و دسترسی

### کاربر (Public — AccountController)

- Sign-in (username/password و phone OTP)
- Register
- Refresh token
- Forget password (email/phone token)
- Change password / email / phone
- مدیریت نشست (`active-sessions`, revoke)
- `is-authenticated`, `user-info`, `user-check`

### ادمین

- JWT اجباری روی `BaseApiAdminController`
- `PermissionAuthorizeAttribute` → `IAccountingService.HasPermissionAsync`
- Permissionها از attribute کنترلر (`ControllerInfo`, `ActionInfo`) و در seed از `PermissionModule.GetPermissions()` ساخته می‌شوند

### امنیت ID

- Entity IDها در API با `SecuritySettings:EntityKeys` encrypt می‌شوند
- **مهم:** کلید `BankTransaction` باید در config باشد (برای `BankTransactionId` در پاسخ پرداخت)
- **جدید:** کلیدهای `CompanyStory`, `CompanyStoryComment`, `CompanyStoryLike` برای فیچر استوری کمپانی

---

## ۹. جریان سفارش، پرداخت و حسابداری

### نمودار وضعیت فعلی

```
[خالی] ──purchase──► Pending ──payment──► Pending (منتظر بانک)
                         │                      │
                         │                      └──verify OK──► InProgress
                         │
                         └── wallet-only payment ──► InProgress

InProgress ──► Completed / Rejected  ❌ (handler ندارد)
```

### Handlerها

| مرحله | Handler | مسیر |
|-------|---------|------|
| Checkout | CheckoutOrderHandler | `Features/Orders/Commands/Checkout/` |
| افزودن به سبد | PurchaseOrderHandler | `Features/Orders/Commands/Purchase/` |
| پیش‌نمایش تخفیف | ValidateDiscountOrderHandler | `Features/Orders/Commands/ValidateDiscount/` |
| حذف تخفیف | RemoveOrderDiscountHandler | `Features/Orders/Commands/RemoveDiscount/` |
| حذف آیتم | RemoveOrderItemHandler | `Features/Orders/Commands/RemoveItem/` |
| پرداخت | PaymentOrderHandler | `Features/Orders/Commands/Payment/` |
| Verify | VerifyPaymentOrderHandler | `Features/Orders/Commands/VerifyPayment/` |

### پرداخت — پیاده شده

- **چندشرکتی:** `OrderPaymentCompanyAllocator` — تقسیم تخفیف، VAT، مبلغ نهایی per company
- **گزینه پرداخت:** `BankOnly`, `WalletOnly`, `WalletAndBank`
- **VAT:** ۱۰٪ (`OrderPaymentConstants.VatRate`)
- **سند مالی:** یک `FinancialDocument` per company با سطرهای بدهکار/بستانکار
- **کیف پول:** کسر از wallet + `WalletTransaction`
- **بانک:** یک `BankTransaction` برای کل مبلغ بانک (روی اولین financial document)
- **تخفیف:** consume در payment؛ release در شکست verify
- **خروجی:** `PaymentUrl` از `BankAccount.PaymentUrl`

### پرداخت — Stub / ناقص

```csharp
// OrderPaymentConstants.cs
public const bool BankVerificationStubSucceeded = true;

// BankPaymentVerificationService.cs
// TODO: call bankAccount.VerifyPaymentUrl gateway
```

| موضوع | وضعیت |
|-------|--------|
| Verify واقعی درگاه | ❌ stub |
| DocumentNumber سند مالی | همیشه `""` |
| Chart of Account seed | غیرفعال — IDهای ۱۱، ۱۴۶، ۱۴۷ hardcode |
| OrderCommission | entity هست، در payment استفاده نمی‌شود |
| TotalCommissionPrice | ستون هست، محاسبه نمی‌شود |
| VAT در cart/checkout response | ❌ |
| Cancel سفارش / پرداخت | ❌ |
| Refund | enum هست، flow نیست |
| Wallet API (شارژ، لیست تراکنش) | ✅ پیاده شد — بخش [۹.۱](#۹۱-مدیریت-کیف-پول-wallet) |
| موجودی انبار | ❌ |

### کلاس‌های مشترک پرداخت

`OrderPaymentConstants`, `OrderPaymentCompanyAllocator`, `OrderCompanyPaymentSlice`, `OrderPaymentAmounts`, `OrderPaymentFinancialDocumentBuilder`, `OrderPaymentSettlement`

### ۹.۱ مدیریت کیف پول (Wallet)

**مسیر فیچر:** `Features/Wallets/`

#### API کاربر (نیاز به `[Authorize]`)

| Method | Route | Handler |
|--------|-------|---------|
| POST | `/api/v1/wallet/my-wallets` | `GetMyWalletsHandler` — ایجاد خودکار کیف پول پیش‌فرض |
| POST | `/api/v1/wallet/get` | `GetMyWalletHandler` |
| POST | `/api/v1/wallet/transactions` | `GetMyWalletTransactionsHandler` |
| POST | `/api/v1/wallet/charge` | `ChargeWalletHandler` — شارژ از درگاه بانک |
| POST | `/api/v1/wallet/charge/verify` | `VerifyWalletChargeHandler` |

#### API ادمین

| Method | Route | Permission |
|--------|-------|------------|
| POST | `/api/v1/admin/wallet/get-all` | ListWallet |
| POST | `/api/v1/admin/wallet/get` | GetWalletById |
| POST | `/api/v1/admin/wallet/transactions` | ListWalletTransaction |
| POST | `/api/v1/admin/wallet/create` | CreateWallet |
| PUT | `/api/v1/admin/wallet/update` | UpdateWallet |
| PUT | `/api/v1/admin/wallet/status` | UpdateWalletStatus |
| POST | `/api/v1/admin/wallet/charge` | AdminChargeWallet — شارژ دستی |

#### جریان شارژ

```
کاربر/ادمین ──charge──► سند مالی (WalletTransaction type)
                         ├── Admin: تراکنش Completed + افزایش موجودی فوری
                         └── User+Bank: تراکنش Pending + BankTransaction Pending
                                    └── verify OK ──► Completed + افزایش موجودی
```

**کلاس‌های مشترک:** `WalletConstants`, `WalletFinancialDocumentBuilder`, `WalletChargeSettlement`

**Repositoryها:** `IWalletRepository` (کامل), `IWalletTransactionRepository`, گسترش `IBankTransactionRepository` با `GetWalletChargeAsync`

**تست‌ها:** `WalletTests` (domain), `WalletChargeSettlementTests` (application)

**پیکربندی:** افزودن کلید `WalletTransaction` به `SecuritySettings:EntityKeys`

---

## ۱۰. Content Policy

موتور پیشرفته برای محدودسازی دسترسی به داده بر اساس نقش/کاربر.

- **راهنمای تفصیلی:** `docs/content-policy-guide.md`
- Domain: `Edition.Domain/ContentPolicies/`
- Application: `Services/ContentPolicies/` + MediatR Behaviors
- Admin API: `ContentPolicyController`, `ContentPolicyRuleController`, `ContentPolicyRecordAccessController`, `ContentPolicyMetadataController`

**وضعیت:** ✅ پیاده‌سازی عمیق — 🟡 پوشش تست محدود

### ۱۰.۱ ContentPolicyMetadata — Permission اختصاصی

کنترلر `ContentPolicyMetadataController` دیگر از permissionهای `ContentPolicy` عمومی استفاده **نمی‌کند**؛ گروه و actionهای جدا در `PermissionType` تعریف شده‌اند (IDهای 457–465).

**کنترلر:** `src/Presentation/Edition.Api/Controllers/v1/Admin/ContentPolicyMetadataController.cs`

| Permission | ID | Endpoint |
|------------|-----|----------|
| `ManageContentPolicyMetadataGroup` | 457 | گروه منوی ادمین (ControllerInfo) |
| `ManageContentPolicyMetadata` | 458 | صفحه/کنترلر (ControllerInfo) |
| `ContentPolicy_GetEntityTypes` | 459 | `POST .../entity-types` |
| `ContentPolicy_GetEntitySchema` | 460 | `POST .../entity-schema` |
| `ContentPolicy_GetRuleOptions` | 461 | `POST .../rule-options` |
| `ContentPolicy_GetPropertyOperators` | 462 | `POST .../property-operators` |
| `ContentPolicy_ValidateRules` | 463 | `POST .../validate-rules` |
| `ContentPolicy_Preview` | 464 | `POST .../preview` |
| `ContentPolicy_CompareMerge` | 465 | `POST .../compare-merge` |

**Resource (نمایش در UI):** کلیدهای `PermissionType_*` در `EnumResources.resx` و `EnumResources.fa.resx`

**Seed:** مانند سایر admin controllerها، `PermissionModule.GetPermissions()` این permissionها را از attributeها کشف می‌کند — پس از deploy باید seed/sync permission اجرا شود.

---

## ۱۱. تست‌ها

راهنما: `tests/README.md`

| پروژه | پوشش |
|-------|------|
| Arch.Tests | قوانین وابستگی لایه‌ها (NetArchTest) |
| Common.Tests | PasswordHasher، extensions، utilities، **LanguageCultureNormalizer** |
| Domain.Tests | Order discount/VAT، ContentPolicy، pagination، **Wallet balance**، **Language entity** |
| Application.Tests | Order payment، **Language validators/handlers/mapper**، purchase validation، content policy |
| Infrastructure.Tests | Auth context، **CurrentLanguageContext** |
| Persistence.Tests | DbContext، UnitOfWork، BankRepository + **LanguageRepository** integration (Testcontainers) |

### پوشش خوب

- معماری لایه‌ها
- ریاضیات تقسیم پرداخت چندشرکتی
- settlement موفق/ناموفق (سفارش + **شارژ کیف پول**)
- domain تخفیف و VAT
- **زیرساخت زبان:** validator/handler/mapper + repository integration

### پوشش ضعیف / ندارد

- تست Handler کامل `PaymentOrderHandler` / `VerifyPaymentOrderHandler` / `PurchaseOrderHandler`
- تست API (Edition.Api.Tests وجود ندارد)
- تست اکثر CRUD handlers
- SMS/Email/Bank gateway
- بیشتر repositoryها (فقط Bank + **Language** integration)
- **ترجمه entityهای محتوا** (فاز ۲+ i18n)

---

## ۱۲. پیکربندی و استقرار

**فایل:** `src/Presentation/Edition.Api/appsettings.json`

| Section | کاربرد |
|---------|--------|
| ConnectionStrings:EditionDbContext | SQL Server |
| LocalizationSettings | **فقط seed/bootstrap** زبان — runtime از DB/Redis |
| LanguageCacheConfiguration | کلیدها و TTL کش زبان در Redis |
| CorsSettings | origins (خالی = deny) |
| SecuritySettings | GeneralKey، EntityKeys (encrypt ID) |
| SiteSettings:JwtSettings | JWT |
| RedisConfiguration | Redis |
| Serilog | لاگ به SQL |
| CurrencyConfiguration | Toman، round up |
| PhoneNumberTokenProviderConfiguration | OTP |
| EmailTokenProviderConfiguration | Email token |
| ContentPolicyCacheConfiguration | TTL کش policy |

**نکته:** مقادیر حساس در repo خالی هستند — از User Secrets یا env در محیط واقعی پر شوند.

### پیش‌نیاز اجرا

1. SQL Server + connection string
2. Redis
3. Migration: `dotnet ef database update`
4. Docker (برای persistence integration tests)

---

## ۱۳. ماتریس فیچرها

### ✅ پیاده و قابل استفاده

- CRUD کامل اکثر منابع CMS/کاتالوگ
- بلاگ + نظرات + لایک
- شرکت‌ها و نظرات شرکت
- **استوری کمپانی** (استوری با آیتم رسانه، نظر با تأیید ادمین، لایک با IP/کاربر)
- احراز هویت کامل کاربر + نشست
- RBAC ادمین
- Content Policy
- سبد خرید چندشرکتی
- پرداخت wallet/bank/VAT/تخفیف
- Verify flow (با stub)
- آپلود فایل محصول
- Seed داده توسعه
- مستندسازی API با دسته‌بندی کنترلر (`ApiControllerCategory`) در Swagger و ReDoc
- پورتال `/docs` با Try it out داخلی (بدون وابستگی به Swagger UI)
- **زیرساخت زبان:** جدول `Language`، Registry Redis، middleware، API ادمین زبان

### 🟡 نیمه‌کاره

- درگاه بانک (verify واقعی)
- SMS/Email (ارسال واقعی)
- پس از پرداخت (workflow سفارش)
- حسابداری (فقط سند پرداخت؛ expense/POS/cheque ندارد)
- جداسازی کامل public/admin endpoint
- Search برای Product, Tag, Discount, Property*, ProductPrice*
- VAT در پاسخ cart
- Commission
- **چندزبانگی محتوا:** جداول `*Translation`، Search per زبان، migration داده تک‌زبانه موجود

### ❌ پیاده نشده

- موجودی انبار
- Refund / Cancel order
- API کیف پول کاربر — ✅ (لیست، تراکنش، شارژ بانکی)
- Order notes
- Workflow وضعیت OrderItem (design approval, production, ...)
- Public API تاریخچه سفارش کاربر
- Edition.Api.Tests
- Message bus / event-driven
- Object storage (S3/MinIO) — فقط local file

---

## ۱۴. ضعف‌ها و کسری‌های فنی

### معماری

| موضوع | توضیح |
|-------|--------|
| Hardcode حساب‌ها | `CustomerChartOfAccountId=11` و ... بدون validate در startup |
| یک BankTransaction برای چند شرکت | مبلغ کل روی اولین financial document — تصمیم آگاهانه ولی گزارش per-company bank ندارد |
| `IAccountingService` نام‌گذاری گمراه‌کننده | در واقع auth است نه حسابداری سفارش |
| Controller refactor ناقص | برخی public هنوز get-all/get دارند |
| Entity بدون Feature | Expense, POS, Cheque در schema گیر کرده‌اند |

### کیفیت کد

| موضوع | فایل |
|-------|------|
| Typo | `EmailServie.cs` |
| Typo | `IPropertyItemrepository.cs` |
| Stub مستند | `BankVerificationStubSucceeded` |
| Seed NotImplemented | `SeedService.cs` default branch |
| README خالی | `README.md` فقط `# Edition` |

### امنیت / عملیات

| موضوع | توضیح |
|-------|--------|
| CORS خالی | production باید تنظیم شود |
| Connection string خالی در repo | OK برای dev؛ خطر اگر commit شود با مقدار واقعی |
| Wallet بدون guard منفی | ✅ `DecreaseBalance` اکنون موجودی ناکافی را رد می‌کند |
| Block token | پیاده — وابسته به Redis/SQL |

### تست

- ~۳۹۰ validator بدون تست template-driven
- بدون contract test برای API
- integration test فقط BankRepository

---

## ۱۵. تصمیمات معماری مهم

1. **MediatR per feature** — هر use case فایل‌های جدا؛ تست و کشف آسان
2. **Repository + UoW** — بدون generic repository سنگین؛ interface per aggregate
3. **Cache Interceptor** — روی repository با Castle DynamicProxy
4. **Permission از reflection** — attribute روی admin controller → seed خودکار
5. **Content Policy در pipeline** — نه فقط در controller
6. **Encrypted entity IDs** — در API boundary
7. **یک سبد Pending per user** — `GetPendingOrderByUserIdAsync`
8. **تخفیف consume در payment** نه validate — validate فقط preview
9. **دو لایه Localization** — پیام API از `.resx`؛ محتوای DB از `Language` + `*Translation` (فاز ۲+)
10. **`LocalizationSettings` = seed** — حقیقت runtime در جدول `Language` و Redis Registry

---

## ۱۶. کارهای باقی‌مانده

### اولویت بالا (Production blocker)

1. پیاده‌سازی واقعی `BankPaymentVerificationService` با `VerifyPaymentUrl`
2. تنظیم `SecuritySettings:EntityKeys` شامل `BankTransaction`
3. فعال‌سازی یا validate chart of accounts (seed یا startup check)
4. تکمیل refactor endpoint: حذف `get-all`/`get` از public controllers باقی‌مانده
5. SMS/Email provider واقعی

### اولویت متوسط

6. Public API لیست/جزئیات سفارش کاربر
7. Workflow پس از پرداخت (`Completed`, وضعیت OrderItem)
8. VAT در پاسخ checkout/cart
9. `SearchProductRequest` و endpoint public برای محصول
10. Handler test برای payment pipeline
11. Cancel order / timeout bank payment
12. **فاز ۲ i18n:** پایلوت `DeliveryType` / `PostType` با `*Translation`
13. **فاز ۸.۵:** migration داده تک‌زبانه موجود به جداول ترجمه

### اولویت پایین

12. ~~Wallet management API~~ ✅
13. OrderCommission محاسبه و ثبت
14. Expense / POS / Cheque features
15. Inventory
16. Edition.Api.Tests
17. README و onboarding توسعه‌دهنده

---

## ۱۷. راهنمای سریع برای توسعه

### اگر می‌خواهی فیچر جدید اضافه کنی

1. Entity در `Edition.Domain/Entities/`
2. `I*Repository` در `Edition.Domain/Repositories/`
3. EF config + migration در Persistence
4. `Repository` implementation
5. Feature folder در Application (Command/Query + Validator + Handler)
6. Mapper در `Mappings/`
7. Controller Public (search) + Admin (get-all/get/CUD)
8. Permission attribute روی admin
9. **`[ApiControllerCategory(ApiControllerCategory.X)]`** روی کنترلر — برای منوی Swagger/ReDoc
10. Message keys در `Edition.Common/Localization/` در صورت نیاز
11. Test در Application.Tests یا Domain.Tests

### اگر روی کیف پول کار می‌کنی — فایل‌های کلیدی

```
Features/Wallets/Commands/Charge/ChargeWalletHandler.cs
Features/Wallets/Commands/VerifyCharge/VerifyWalletChargeHandler.cs
Features/Wallets/Commands/AdminCharge/AdminChargeWalletHandler.cs
Features/Wallets/Common/WalletFinancialDocumentBuilder.cs
Features/Wallets/Common/WalletChargeSettlement.cs
Controllers/v1/WalletController.cs
Controllers/v1/Admin/WalletController.cs
```

### اگر روی پرداخت کار می‌کنی — فایل‌های کلیدی

```
Features/Orders/Commands/Purchase/PurchaseOrderHandler.cs
Features/Orders/Commands/Payment/PaymentOrderHandler.cs
Features/Orders/Commands/VerifyPayment/VerifyPaymentOrderHandler.cs
Features/Orders/Common/OrderPayment*.cs
Services/Orders/BankPaymentVerificationService.cs
Domain/Entities/Order.cs
Controllers/v1/OrderController.cs
```

### اگر روی API سایت کار می‌کنی

- Public: `POST /api/v1/{resource}/search` — بدون token
- Admin: `POST /api/v1/admin/{resource}/get-all` — با JWT + permission
- مستندات تعاملی: `/swagger` — Swagger UI
- مستندات خواندنی: `/docs` — پورتال ReDoc (جزئیات: [بخش ۱۸](#۱۸-مستندسازی-api-swagger--redoc))

### اگر روی چندزبانگی (i18n) کار می‌کنی — فایل‌های کلیدی

| موضوع | مسیر |
|-------|------|
| برنامه کامل | `docs/multilingual-tables-plan.md` |
| Registry / Redis | `Application/Services/Localization/LanguageRegistry.cs` |
| Context هر request | `ICurrentLanguageContext` + `CurrentLanguageMiddleware` |
| API ادمین | `Controllers/v1/Admin/LanguageController.cs` |
| Repository آینده | از `ICurrentLanguageContext.LanguageId` — **نه** پارامتر `languageId` در signature |

### دستورات مفید

```powershell
dotnet build Edition.sln
dotnet test Edition.sln
dotnet test Edition.sln --filter "Category!=Integration"   # بدون Docker
dotnet ef database update --project src/Infrastructure/Edition.Infrastructure.Persistence
```

---

## ۱۸. مستندسازی API (Swagger / ReDoc)

### آدرس‌ها

| URL | نقش |
|-----|-----|
| `/` | لندینگ پورتال (`wwwroot/index.html`) |
| `/docs` | پورتال مستندات — هدر Edition، انتخاب زبان، لینک **Try it out** → `/swagger` |
| `/docs/read` | ReDoc (spec از `/swagger/v1/swagger.json`) — منوی دسته‌بندی‌شده + Try it out داخلی |
| `/swagger` | Swagger UI — **Try it out** تعاملی — لینک **Docs** → `/docs` |
| `/swagger/v1/swagger.json` | OpenAPI spec خام |

**ثبت در DI:** `Program.cs` → `AddSwagger()` + `UseSwaggerAndUI()`  
**پیکربندی اصلی:** `src/Presentation/Edition.WebFramework/Swagger/SwaggerConfigurationExtensions.cs`

### دسته‌بندی کنترلرها (Swagger + Docs)

| موضوع | رفتار |
|-------|--------|
| **گروه منو (`x-tagGroups`)** | یک گروه به ازای هر `ApiControllerCategory` — مثلاً Company، Product، CMS |
| **نام گروه** | همیشه **انگلیسی** (`GetEnglishDisplayName`) — مستقل از `Accept-Language` |
| **Tag کنترلر** | یک tag به ازای هر کنترلر (مثلاً `Company`) — **ادمین و پابلیک جدا نیستند** |
| **Audience** | روی هر operation با `[Public]` / `[Admin]` / `[Auth]` در summary مشخص می‌شود |
| **Swagger UI** | گروه‌های تاشو با `wwwroot/swagger-ui/edition-tag-groups.js` |
| **ReDoc** | منوی سلسله‌مراتبی از `x-tagGroups` (گروه → کنترلر → operation) |

**نمونه ساختار منو:**

```
Company
  └── Company
        ├── POST /api/v1/company/search     [Public]
        └── POST /api/v1/admin/company/...  [Admin]
```

### Swagger UI (`/swagger`)

| تنظیم | مقدار / رفتار |
|-------|----------------|
| Doc expansion | `DocExpansion.None` — **تب‌های کنترلر به‌صورت پیش‌فرض بسته** |
| OperationId | کوتاه: `{controller}_{action}` — ادمین: `admin_{controller}_{action}` |
| OAuth2 | Password flow → `POST /api/v1/account/sign-in` |
| Models | `DefaultModelsExpandDepth(-1)` — schemaها مخفی |
| Try it out | پیش‌فرض فعال |
| Authorization | `EnablePersistAuthorization()` — توکن بعد از refresh حفظ می‌شود |
| Locale (درخواست API) | `Accept-Language` از `localStorage` (`edition-api-language`) — fa-IR / en-US |
| Tag groups | `edition-tag-groups.js` — خواندن `x-tagGroups` از spec |

**استایل:** `wwwroot/swagger-ui/custom.css` + `wwwroot/docs/edition-header.css`

**هدر Swagger:** برند Edition API + انتخاب زبان + لینک Docs (از `edition-header.js`)

### ReDoc / پورتال (`/docs`)

| بخش | توضیح |
|-----|--------|
| `docs/index.html` | پورتال با هدر Edition، انتخاب زبان، لینک Swagger |
| `docs/read` | ReDoc embed — spec همان swagger.json |
| `redoc-api-tester.js` | **Try it out داخلی** داخل ReDoc (بدون redirect به Swagger): Execute، curl، Responses، Authorization (Bearer JWT) |
| `redoc-custom.css` | استایل هماهنگ با ReDoc + پنل تست API |
| `docs-portal.js` | مقداردهی locale در هدر پورتال |
| `edition-locale.js` | ذخیره زبان در `localStorage` — **بدون رفرش خودکار صفحه** هنگام تغییر زبان |

**جهت متن:** LTR ثابت در `/docs` و `/swagger` (بدون toggle RTL/LTR)

**برچسب audience روی هر operation (در summary):**

- `[Public]` — endpoint عمومی
- `[Admin]` — endpoint ادمین (`BaseApiAdminController`)
- `[Auth]` — احراز هویت (`Account`)

تشخیص ادمین از `BaseApiAdminController` است (نه فقط namespace).

### فایل‌های Swagger (WebFramework)

| فایل | نقش |
|------|-----|
| `SwaggerConfigurationExtensions.cs` | ثبت Swashbuckle، Swagger UI، ReDoc |
| `SwaggerTagDescriptor.cs` | خواندن `ApiControllerCategoryAttribute`؛ tag = نام human-readable کنترلر |
| `SwaggerTagGroupsDocumentFilter.cs` | `x-tagGroups` بر اساس enum category — نام گروه انگلیسی |
| `SwaggerControllerTagsOperationFilter.cs` | tag هر operation + توضیح `{Category} · {Controller}` |
| `SwaggerAudienceOperationFilter.cs` | برچسب `[Public]` / `[Admin]` / `[Auth]` |
| `SwaggerControllerAudience.cs` | تشخیص public/admin/auth |
| `ApplySummariesOperationFilter.cs` | summary پیش‌فرض + ایمن برای POST+body |
| `UnauthorizedResponsesOperationFilter.cs` | 401/403 + OAuth2 |
| `RemoveVersionParameters` / `SetVersionInPaths` | versioning در path |
| `SwaggerJsonOnlyOperationFilter.cs` | محدودیت content-type JSON |

**Assets استاتیک:**

| مسیر | فایل‌ها |
|------|---------|
| `wwwroot/docs/` | `index.html`, `edition-header.css`, `edition-header.js`, `edition-locale.js`, `docs-portal.js`, `redoc-custom.css`, `redoc-api-tester.js`, `docs-custom.css` |
| `wwwroot/swagger-ui/` | `custom.css`, `edition-tag-groups.js` |

### نکات فنی (رفع خطای Swagger 500)

Swashbuckle برای schema از **نام کوتاه** نوع استفاده می‌کند؛ DTOهای هم‌نام در namespaceهای مختلف باعث `Failed to generate Operation` می‌شوند. اصلاح‌های انجام‌شده:

| نوع قبلی (تداخل) | نام جدید |
|------------------|----------|
| `GetContentPolicyRuleResponse` (nested در Get policy) | `GetContentPolicyRuleSummaryResponse` |
| `CreateContentPolicyRuleRequest` (item در Create policy) | `CreateContentPolicyRuleItemRequest` |
| `UpdateContentPolicyRuleRequest` (item در Update policy) | `UpdateContentPolicyRuleItemRequest` |

**قانون برای توسعه:** DTOهای nested در command/query policy را با پسوند `Summary` / `Item` نام‌گذاری کن تا با DTOهای CRUD مستقل `ContentPolicyRule` تداخل نگیرند.

### افزودن کنترلر جدید به مستندات

1. `[ApiVersion("1")]` روی کنترلر
2. `[ControllerName("kebab-name")]` برای URL
3. **`[ApiControllerCategory(ApiControllerCategory.X)]`** — دسته منو (الزامی)
4. ادمین: `[ControllerInfo]` + `[ActionInfo]` — permission و audience خودکار
5. پس از build، `/swagger/v1/swagger.json` باید بدون 500 لود شود و کنترلر در گروه صحیح `x-tagGroups` دیده شود

---

## ۱۹. چندزبانگی و زبان (Localization / Language)

> **مرجع تفصیلی:** [`docs/multilingual-tables-plan.md`](multilingual-tables-plan.md)

### هدف

پشتیبانی از چند زبان برای **محتوای دیتابیس** (عنوان، slug، توضیحات، …) با نگه‌داشتن یک جدول مرجع `Language` و الگوی `EntityId + LanguageId` در جداول ترجمه. فازهای ۱ و ۱.۵ **فقط زیرساخت** را پیاده کرده‌اند؛ entityهای کاتالوگ/CMS هنوز تک‌زبانه‌اند.

### آنچه پیاده شده (فاز ۱ + ۱.۵)

| جزء | وضعیت | توضیح |
|-----|--------|-------|
| جدول `Language` | ✅ | `Code`, `Name`, `IsActive`, `IsDefault`, `DisplayOrder`, `IsRtl` |
| Migration | ✅ | `20260715090000_add-language-table` |
| `ILanguageRegistry` | ✅ | Singleton — resolve `Code ↔ Id` از Redis |
| `LanguageBootstrapService` | ✅ | startup: seed از `LocalizationSettings` اگر DB خالی |
| `ICurrentLanguageContext` | ✅ | Scoped — `LanguageId` هر request |
| `CurrentLanguageMiddleware` | ✅ | بعد از `UseRequestLocalization` |
| `RequestLocalizationOptions` | ✅ | از Registry/DB — نه runtime از appsettings |
| API ادمین | ✅ | CRUD + `set-default` |
| Invalidate Redis | ✅ | بعد از هر CUD ادمین |
| تست | ✅ | Domain، Application (validator/handler/mapper)، Persistence integration |

### آنچه هنوز پیاده نشده

| موضوع | فاز پیشنهادی |
|-------|----------------|
| `ProductTranslation`, `CategoryTranslation`, … | ۲–۸ |
| `Search` public با زبان جاری | ۲+ |
| `GetAll` ادمین با همه ترجمه‌ها | ۲+ |
| Migration داده تک‌زبانه موجود | ۸.۵ |
| Content Policy + i18n | ۹ |

### قراردادهای مهم برای توسعه بعدی

1. **Repository** پارامتر `languageId` در signature نمی‌گیرد — از `ICurrentLanguageContext` می‌خواند.
2. **`GetAll` (ادمین)** همه زبان‌ها؛ **`Search` (public)** فقط زبان جاری + fallback به پیش‌فرض.
3. **Create/Update ادمین** یک `languageId` در body — نه آرایه `translations[]`.
4. **Slug** per language برای entityهای SEO (مثلاً Product).
5. **`LocalizationSettings`** در appsettings حذف نمی‌شود — نقش: seed/env جدید.

### دستورات

```powershell
# اعمال migration زبان
dotnet ef database update --project src/Infrastructure/Edition.Infrastructure.Persistence

# تست‌های مرتبط
dotnet test Edition.sln --filter "FullyQualifiedName~Language"
dotnet test Edition.sln --filter "FullyQualifiedName~Language&Category!=Integration"
```

---

## پیوست: نقشه مسیر فایل‌های مهم

```
src/
├── Core/
│   ├── Edition.Domain/          Entities, Enums, Repositories, ContentPolicies
│   └── Edition.Application/     Features/, Services/, Mappings/, Common/Behaviors
├── Cross-Cutting/Edition.Common/ Localization, Exceptions, **ApiControllerCategory enum + resources**
├── Infrastructure/
│   ├── Edition.Infrastructure/           Redis, SMS, Email, Image
│   └── Edition.Infrastructure.Persistence/ DbContext, Repositories, Migrations, Seed
└── Presentation/
    ├── Edition.WebFramework/    BaseApiController, Swagger/ReDoc filters
    └── Edition.Api/               Program.cs, Controllers, **Attributes/ApiControllerCategoryAttribute**, wwwroot/docs, wwwroot/swagger-ui

tests/                           ۶ پروژه تست
docs/
  ├── project-status.md          ← این سند
  ├── content-policy-guide.md    راهنمای Content Policy
  └── multilingual-tables-plan.md  برنامه چندزبانگی DB (فازها، الگوها، تصمیمات)
tools/
  └── refactor-controller-endpoints.py
```

**Swagger/ReDoc:** `Edition.WebFramework/Swagger/` + `Edition.Api/wwwroot/docs/` + `Edition.Api/wwwroot/swagger-ui/` + `Edition.Api/Attributes/ApiControllerCategoryAttribute.cs`

---

*این سند باید پس از هر تغییر معماری مهم (پرداخت، auth، controller layout، migration بزرگ) به‌روز شود.*
