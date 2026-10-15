# Deploy — Diba Gallery (Docker)

استقرار production روی VPS لینوکس با Docker Compose: **SQL Server**، **Redis**، **API (`BackEndProject`)**، **فرانت SSR (`ReactUI-V2`)** و **Nginx**.

**روش پیشنهادی:** سورس را روی سرور ببرید (Git یا rsync) و **build داخل Docker روی لینوکس** — نه build روی ویندوز و کپی `dist`/`bin`. یک `docker compose up --build` هر دو سرویس را می‌سازد.

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

6. سایت: `http://SERVER_IP:HTTP_PORT` (مثلاً `http://185.213.167.240:3031`) — health: همان آدرس + `/health`

> **فایروال:** `sudo ufw allow 3031/tcp` (یا پورت `HTTP_PORT` شما)

## HTTPS (بعد از DNS)

دامنه را به IP سرور اشاره دهید. پیشنهاد: **Certbot** روی host یا **Caddy/Traefik** جلوی compose.  
تا قبل از TLS، `PUBLIC_SITE_URL` را با `http://` تنظیم کنید.

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
| فایل آپلود | volume `api_uploads` — دست نزنید به مسیر داخل image |

## دستورات مفید

```bash
docker compose pull
docker compose up -d --build
docker compose down          # بدون حذف volume
docker compose down -v       # خطرناک — حذف DB و uploads
```
