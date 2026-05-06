# 拉麵道 API — ramendo-api

ASP.NET Core 10 後端 API，採用 DDD + CQRS 架構，供前台 `ramendo-web` 與後台 `ramendo-admin` 呼叫。

---

## 技術棧

| 類別 | 技術 |
|------|------|
| 框架 | ASP.NET Core 10 |
| 架構 | DDD + CQRS (MediatR) |
| ORM | Entity Framework Core 10 + Npgsql |
| 資料庫 | PostgreSQL 15 (Supabase) |
| 認證 | JWT (Access + Refresh Token) + Google OAuth |
| 圖片 | Cloudinary |
| 測試 | xUnit |
| 部署 | Docker → Render.com |

---

## 專案結構

```
src/
  Ramendo.Domain/          ← 實體、值物件、Repository 介面、Domain Events
  Ramendo.Application/     ← CQRS Commands/Queries/Handlers、DTOs、例外
  Ramendo.Infrastructure/  ← EF Core、Repository 實作、JWT、外部服務
  Ramendo.Api/             ← Controllers、Middleware、Program.cs
tests/
  Ramendo.Domain.Tests/
  Ramendo.Application.Tests/
  Ramendo.Api.IntegrationTests/
```

### 依賴方向

```
Api → Application → Domain
Infrastructure → Domain
Api → Infrastructure (DI 注入)
```

---

## 本地執行

```bash
cd src/Ramendo.Api
dotnet run
# Swagger UI: http://localhost:5000/swagger
# Health:     http://localhost:5000/health
```

### 前置需求

- .NET 10 SDK
- PostgreSQL 15（或 Supabase 連線字串）

---

## 環境變數

複製 `appsettings.Development.json.example`（若無則手動建立）：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=ramendo;Username=...;Password=..."
  },
  "Jwt": {
    "Secret": "<32字元以上隨機字串>",
    "Issuer": "ramendo-api",
    "Audience": "ramendo-client",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Google": { "ClientId": "...", "ClientSecret": "..." },
  "Cloudinary": { "CloudName": "...", "ApiKey": "...", "ApiSecret": "..." }
}
```

---

## EF Core Migrations

```bash
cd src/Ramendo.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../Ramendo.Api
dotnet ef database update --startup-project ../Ramendo.Api
```

---

## API 端點

### 公開端點

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/auth/register` | 註冊（邀請碼選填） |
| POST | `/api/auth/login` | 登入 |
| POST | `/api/auth/google` | Google OAuth 登入 |
| POST | `/api/auth/refresh` | 刷新 Token |
| GET  | `/api/shops` | 店家列表（篩選/排序） |
| GET  | `/api/shops/{guid}` | 店家詳情 |
| GET  | `/api/shops/{guid}/reviews` | 店家評論 |
| GET  | `/api/rankings` | 排行榜 |

### 登入後

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET  | `/api/user/favorites` | 我的收藏 |
| POST | `/api/user/favorites/{shopGuid}` | 切換收藏 |
| GET  | `/api/user/submissions` | 我的提案 |
| POST | `/api/submissions` | 提案店家 |
| POST | `/api/reviews` | 發表評論 |
| DELETE | `/api/reviews/{id}` | 刪除評論 |

### 管理員 (`/api/admin/*`)

- 用戶管理：列表、角色、狀態、VIP、經驗值調整
- 店家管理：CRUD、菜單管理
- 評論管理：列表、刪除
- 提案審核：列表、核准、駁回
- 邀請碼管理：CRUD、啟用/停用
- 排行榜設定：權重、顯示條件
- 系統設定：各功能所需最低等級
- 資料庫統計

---

## API 回應格式

```json
{ "success": true, "data": { ... }, "message": null, "errors": null }
{ "success": false, "data": null, "message": "錯誤訊息", "errors": ["..."] }
```

---

## 用戶等級系統

- 等級公式：`level = floor(experiencePoints / 100) + 1`
- 發表評論、提案審核通過等行為可獲得經驗值
- 各功能有最低等級限制（可在系統設定調整）

---

## 部署

| 項目 | 說明 |
|------|------|
| 平台 | Render.com（Free Tier） |
| 執行方式 | Docker Container |
| 資料庫 | Supabase PostgreSQL |
| 圖片儲存 | Cloudinary |
| CI/CD | GitHub Actions → Render deploy hook |

> Render free tier 閒置 15 分鐘後冷啟動 ~30 秒。
> 建議用 UptimeRobot 每 14 分鐘 ping `GET /health` 維持熱啟動。
