# Deploy — Sarv Home (Docker)

استقرار production روی VPS لینوکس با Docker Compose: **SQL Server**، **Redis**، **API (`BackEndProject`)**، **فرانت SSR (`ReactUI-V2`)** و **Nginx**.

**روش پیشنهادی:** سورس را روی سرور ببرید (Git یا rsync) و **build داخل Docker روی لینوکس** — نه build روی ویندوز و کپی `dist`/`bin`. یک `docker compose up --build` هر دو سرویس را می‌سازد.

### جلوگیری از ایندکس (قبل از لانچ)

در `.env`: `BLOCK_SEARCH_INDEXING=true` (پیش‌فرض در `.env.example`). بعد از آماده شدن سایت:

```env
BLOCK_SEARCH_INDEXING=false
```

سپس `docker compose --env-file .env up -d --build web` (build دوباره برای embed شدن فلگ در کلاینت).

## معماری

```text
Internet → nginx:80 → web:3000 (SSR)
                  └→ api:8080 (/api, /Uploads, /swagger, /docs)
api → sqlserver:1433
api → redis:6379
```

Volumeها (داده پایدار):

| Volume | محتوا |
|--------|--------|
| `decorative-store-mssql-data` | فایل‌های دیتابیس SQL Server |
| `decorative-store-redis-data` | داده Redis (AOF) |
| `decorative-store-api-uploads` | تصاویر و فایل‌های آپلود (`wwwroot/Uploads`) |

پورت SQL و Redis روی host **publish نمی‌شوند** (فقط شبکه داخلی Docker).

## پیش‌نیاز سرور

- Ubuntu 22.04+ (یا هر توزیع با Docker Engine 24+)
- حداقل **4 GB RAM** (SQL Server Express + API + Node)
- دامنه یا IP عمومی

```bash
sudo apt update && sudo apt install -y ca-certificates curl
curl -fsSL https://get.docker.com | sudo sh
sudo usermod -aG docker $USER
# re-login
```

## راه‌اندازی (فردا روی VPS)

1. کلون پروژه روی سرور.
2. از پوشه `deploy`:

```bash
cp .env.example .env
nano .env   # PUBLIC_SITE_URL، پسوردها، JWT را عوض کنید
```

3. اولین بار (migration + seed permission):

```env
DATABASE_APPLY_MIGRATIONS=true
DATABASE_SEED_PERMISSIONS=true
```

4. Build و اجرا:

```bash
cd deploy
docker compose --env-file .env up -d --build
docker compose ps
docker compose logs -f api
```

5. بعد از موفقیت اولین بالا آمدن، در `.env` بگذارید:

```env
DATABASE_SEED_PERMISSIONS=false
```

و API را یک‌بار restart کنید:

```bash
docker compose up -d api
```

6. سایت:
   - با دامنه: `https://sarv-home.com` (وقتی `HTTP_PORT=80` و DNS/Cloudflare آماده است)
   - با IP و پورت: `http://SERVER_IP:HTTP_PORT` — health: همان آدرس + `/health`

> **فایروال:** `sudo ufw allow 80/tcp` و `443/tcp` (برای دامنه). برای تست با پورت سفارشی: `ufw allow HTTP_PORT/tcp`

## دامنه (بدون :3032 در آدرس)

1. در `.env` روی سرور (فایل gitignore — خودتان ویرایش می‌کنید):

```env
HTTP_PORT=80
PUBLIC_SITE_URL=https://sarv-home.com
ALLOWED_HOSTS=sarv-home.com,www.sarv-home.com,sarv-home.ir,www.sarv-home.ir
```

2. مطمئن شوید هیچ سرویس دیگری پورت **80** روی host را نگرفته: `sudo ss -tlnp | grep ':80 '`

3. بعد از تغییر `PUBLIC_SITE_URL` حتماً **web** را rebuild کنید:

```bash
docker compose --env-file .env up -d --build web api
```

4. **Cloudflare:** رکورد A برای `@` و `www` → IP سرور؛ ابر **Proxied**؛ SSL/TLS → **Flexible** (سرور فقط HTTP روی 80 داخل Docker).

## HTTPS (گواهی روی خود سرور — اختیاری)

اگر Cloudflare Proxied است، معمولاً کافی است. برای Certbot روی host یا **Full (strict)** بعداً volume گواهی به nginx اضافه کنید.

## بک‌آپ

```bash
# دیتابیس (sa password from .env)
docker compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C \
  -Q "BACKUP DATABASE [DecorativeStoreDB] TO DISK = N'/var/opt/mssql/backup/db.bak' WITH INIT"

docker cp decorative-store-sqlserver-1:/var/opt/mssql/backup/db.bak ./backup-$(date +%F).bak

# uploads
docker run --rm -v decorative-store-api-uploads:/data -v "$PWD":/backup alpine \
  tar czf /backup/uploads-$(date +%F).tar.gz -C /data .
```

## عیب‌یابی

| مشکل | اقدام |
|------|--------|
| API بالا نمی‌آید | `docker compose logs api` — اتصال SQL/Redis |
| Migration | `Database__ApplyMigrationsOnStartup=true` در env |
| CORS | `PUBLIC_SITE_URL` دقیقاً همان origin مرورگر |
| فایل آپلود | volume `api_uploads` — اولین بار از image seed می‌شود؛ بعداً فقط backup/restore |
| تصویر لود نمی‌شود | `curl -I http://127.0.0.1:HTTP_PORT/Uploads/Products/…` — 404 یعنی فایل در volume نیست |

## دستورات مفید

```bash
docker compose pull
docker compose up -d --build
docker compose down          # بدون حذف volume
docker compose down -v       # خطرناک — حذف DB و uploads
```
