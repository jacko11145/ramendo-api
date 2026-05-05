# ramendo-api — CLAUDE.md

## 專案簡介

拉麵道 (Ramendo) 後端 API，ASP.NET Core 10 + DDD + CQRS。
供 `ramendo-web`（Vue 3 前台）和 `ramendo-admin`（Vue 3 後台）呼叫。

---

## 本地執行

```bash
cd src/Ramendo.Api
dotnet run
# Swagger UI: http://localhost:5000/swagger
# Health: http://localhost:5000/health
```

### 前置需求

- .NET 10 SDK
- PostgreSQL 15（或直接用 Supabase 連線字串）
- 環境變數（見下方）

---

## 環境變數

```
ConnectionStrings__DefaultConnection=Host=...;Database=ramendo;Username=...;Password=...
Jwt__Secret=<32字元以上隨機字串>
Jwt__Issuer=ramendo-api
Jwt__Audience=ramendo-client
Jwt__AccessTokenExpirationMinutes=15
Jwt__RefreshTokenExpirationDays=7
Google__ClientId=...
Google__ClientSecret=...
Cloudinary__CloudName=...
Cloudinary__ApiKey=...
Cloudinary__ApiSecret=...
```

`appsettings.Development.json` 放本地開發值（不 commit）。

---

## EF Core Migrations

```bash
cd src/Ramendo.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../Ramendo.Api
dotnet ef database update --startup-project ../Ramendo.Api
```

---

## Solution 結構

```
ramendo-api.slnx
src/
  Ramendo.Domain/          ← 實體、值物件、Repository 介面、Domain Events
  Ramendo.Application/     ← CQRS Commands/Queries/Handlers、DTOs、例外
  Ramendo.Infrastructure/  ← EF Core、Repository 實作、JWT、RankingService
  Ramendo.Api/             ← Controllers、Middleware、Program.cs
tests/
  Ramendo.Domain.Tests/
  Ramendo.Application.Tests/
  Ramendo.Api.IntegrationTests/
```

### 依賴方向

```
Api → Application → Domain
Infrastructure → Domain (+ Application interfaces)
Api → Infrastructure (DI only)
```

---

## DDD 命名規則

| 分類 | 命名範例 |
|------|---------|
| Aggregate Root | `User`, `RamenShop`, `Review` |
| Value Object | `ExperiencePoints`, `VIPMembership`, `BusinessHours` |
| Domain Event | `UserRegisteredEvent`, `ReviewCreatedEvent` |
| Repository Interface | `IUserRepository`, `IRamenShopRepository` |
| Repository Impl | `UserRepository`, `RamenShopRepository` |

---

## CQRS 命名慣例

| 分類 | 命名範例 |
|------|---------|
| Command | `RegisterUserCommand`, `CreateShopCommand` |
| Query | `GetShopsQuery`, `GetShopByGuidQuery` |
| Handler | `RegisterUserCommandHandler`, `GetShopsQueryHandler` |
| DTO | `ShopListDto`, `ShopDetailDto`, `CreateUpdateShopDto` |

---

## API 回應格式

所有 endpoint 統一回傳 `ApiResponse<T>`:

```json
{ "success": true, "data": {...}, "message": null, "errors": null }
{ "success": false, "data": null, "message": "Not found", "errors": ["..."] }
```

---

## 實作進度 Checklist

### Phase 1 — 後端基礎

#### Domain Layer ✅
- [x] `Common/Entity.cs`
- [x] `Common/AggregateRoot.cs`
- [x] `Common/ValueObject.cs`
- [x] `Common/IDomainEvent.cs`
- [x] `Aggregates/Users/User.cs`
- [x] `Aggregates/Users/UserRole.cs`
- [x] `Aggregates/Users/ExperiencePoints.cs`
- [x] `Aggregates/Users/VIPMembership.cs`
- [x] `Aggregates/Users/Favorite.cs`
- [x] `Aggregates/Users/UserShop.cs`
- [x] `Aggregates/Users/IUserRepository.cs`
- [x] `Aggregates/Users/Events/UserRegisteredEvent.cs`
- [x] `Aggregates/Users/Events/ExperienceAwardedEvent.cs`
- [x] `Aggregates/Shops/RamenShop.cs`
- [x] `Aggregates/Shops/MenuItem.cs`
- [x] `Aggregates/Shops/OptionType.cs`, `OptionValue.cs`, `ItemOption.cs`, `MenuItemOptionValue.cs`
- [x] `Aggregates/Shops/BusinessHours.cs`
- [x] `Aggregates/Shops/NewsItem.cs`
- [x] `Aggregates/Shops/IRamenShopRepository.cs`
- [x] `Aggregates/Shops/Events/ShopCreatedEvent.cs`, `ShopVerifiedEvent.cs`, `MenuItemAddedEvent.cs`
- [x] `Aggregates/Reviews/Review.cs`, `MenuItemRating.cs`, `ReviewOption.cs`
- [x] `Aggregates/Reviews/IReviewRepository.cs`
- [x] `Aggregates/Reviews/Events/ReviewCreatedEvent.cs`, `ReviewDeletedEvent.cs`
- [x] `Aggregates/Submissions/ShopSubmission.cs`, `SubmissionStatus.cs`
- [x] `Aggregates/Submissions/IShopSubmissionRepository.cs`
- [x] `Aggregates/InvitationCodes/InvitationCode.cs`
- [x] `Aggregates/InvitationCodes/IInvitationCodeRepository.cs`
- [x] `Services/IRankingService.cs`
- [x] `Services/IPermissionService.cs`

#### Application Layer — Commands/Queries/DTOs ✅
- [x] `Common/ApiResponse.cs`
- [x] `Common/PagedResult.cs`
- [x] `Common/Exceptions.cs`
- [x] `Auth/Commands/` (Register, Login, GoogleAuth, RefreshToken)
- [x] `Auth/DTOs/AuthDtos.cs`
- [x] `Shops/Commands/` (Create, Update, Delete, AddMenuItem, ReorderMenuItems)
- [x] `Shops/Queries/` (GetShops, GetShopByGuid)
- [x] `Shops/DTOs/ShopDtos.cs`
- [x] `Reviews/Commands/` (Create, Delete, RateMenuItem)
- [x] `Reviews/Queries/GetReviewsByShopQuery.cs`
- [x] `Reviews/DTOs/ReviewDtos.cs`
- [x] `Rankings/Commands/UpdateRankingSettingsCommand.cs`
- [x] `Rankings/Queries/GetRankingsQuery.cs`
- [x] `Rankings/DTOs/RankingDtos.cs`
- [x] `Users/Commands/` (Delete, UpdateRole, UpdateStatus, SetVip, AdjustExperience)
- [x] `Users/Queries/GetUsersQuery.cs`
- [x] `Users/DTOs/UserDtos.cs`
- [x] `Submissions/Commands/` (Create, Approve, Reject)
- [x] `Submissions/Queries/GetSubmissionsQuery.cs`
- [x] `Submissions/DTOs/SubmissionDtos.cs`
- [x] `InvitationCodes/Commands/` (Create, Delete, Toggle)
- [x] `InvitationCodes/Queries/GetInvitationCodesQuery.cs`
- [x] `InvitationCodes/DTOs/InvitationCodeDtos.cs`
- [x] `Favorites/Commands/ToggleFavoriteCommand.cs`
- [x] `Favorites/Queries/GetUserFavoritesQuery.cs`
- [x] `Favorites/DTOs/FavoriteDtos.cs`
- [x] `Settings/Commands/UpdatePermissionSettingsCommand.cs`
- [x] `Settings/Queries/` (GetPermissionSettings, GetDashboardStats)
- [x] `Settings/DTOs/SettingsDtos.cs`

#### Application Layer — Handlers ✅
- [x] `Auth/Handlers/RegisterUserCommandHandler.cs`
- [x] `Auth/Handlers/LoginCommandHandler.cs`
- [x] `Auth/Handlers/GoogleAuthCommandHandler.cs`
- [x] `Auth/Handlers/RefreshTokenCommandHandler.cs`
- [x] `Shops/Handlers/GetShopsQueryHandler.cs`
- [x] `Shops/Handlers/GetShopByGuidQueryHandler.cs`
- [x] `Shops/Handlers/CreateShopCommandHandler.cs`
- [x] `Shops/Handlers/UpdateShopCommandHandler.cs`
- [x] `Shops/Handlers/DeleteShopCommandHandler.cs`
- [x] `Shops/Handlers/AddMenuItemCommandHandler.cs`
- [x] `Shops/Handlers/ReorderMenuItemsCommandHandler.cs`
- [x] `Reviews/Handlers/GetReviewsByShopQueryHandler.cs`
- [x] `Reviews/Handlers/CreateReviewCommandHandler.cs`
- [x] `Reviews/Handlers/DeleteReviewCommandHandler.cs`
- [x] `Reviews/Handlers/RateMenuItemCommandHandler.cs`
- [x] `Rankings/Handlers/GetRankingsQueryHandler.cs`
- [x] `Rankings/Handlers/UpdateRankingSettingsCommandHandler.cs`
- [x] `Users/Handlers/GetUsersQueryHandler.cs`
- [x] `Users/Handlers/DeleteUserCommandHandler.cs`
- [x] `Users/Handlers/UpdateUserRoleCommandHandler.cs`
- [x] `Users/Handlers/UpdateUserStatusCommandHandler.cs`
- [x] `Users/Handlers/SetVipMembershipCommandHandler.cs`
- [x] `Users/Handlers/AdjustExperienceCommandHandler.cs`
- [x] `Submissions/Handlers/GetSubmissionsQueryHandler.cs`
- [x] `Submissions/Handlers/CreateSubmissionCommandHandler.cs`
- [x] `Submissions/Handlers/ApproveSubmissionCommandHandler.cs`
- [x] `Submissions/Handlers/RejectSubmissionCommandHandler.cs`
- [x] `InvitationCodes/Handlers/GetInvitationCodesQueryHandler.cs`
- [x] `InvitationCodes/Handlers/CreateInvitationCodeCommandHandler.cs`
- [x] `InvitationCodes/Handlers/DeleteInvitationCodeCommandHandler.cs`
- [x] `InvitationCodes/Handlers/ToggleInvitationCodeCommandHandler.cs`
- [x] `Favorites/Handlers/GetUserFavoritesQueryHandler.cs`
- [x] `Favorites/Handlers/ToggleFavoriteCommandHandler.cs`
- [x] `Settings/Handlers/GetPermissionSettingsQueryHandler.cs`
- [x] `Settings/Handlers/UpdatePermissionSettingsCommandHandler.cs`
- [x] `Settings/Handlers/GetDashboardStatsQueryHandler.cs`
- [x] `Common/IJwtTokenService.cs` (interface)
- [x] `Common/IPasswordHasher.cs` (interface)
- [x] `Common/IGoogleTokenValidator.cs` (interface)
- [x] `Common/ISystemSettingsRepository.cs` (interface)

#### Infrastructure Layer ✅
- [x] `Persistence/RamendoDbContext.cs`
- [x] `Persistence/SystemSetting.cs`
- [x] `Persistence/RefreshToken.cs`
- [x] `Persistence/Configurations/` (全部 13 個)
- [x] `Persistence/Repositories/UserRepository.cs`
- [x] `Persistence/Repositories/RamenShopRepository.cs`
- [x] `Persistence/Repositories/ReviewRepository.cs`
- [x] `Persistence/Repositories/ShopSubmissionRepository.cs`
- [x] `Persistence/Repositories/InvitationCodeRepository.cs`
- [x] `Persistence/Repositories/FavoriteRepository.cs`
- [x] `Persistence/Repositories/SystemSettingsRepository.cs`
- [x] `Services/JwtTokenService.cs` (implements IJwtTokenService)
- [x] `Services/PasswordHasher.cs` (implements IPasswordHasher)
- [x] `Services/GoogleTokenValidatorService.cs` (implements IGoogleTokenValidator)
- [x] `Services/RankingService.cs`
- [x] `Services/PermissionService.cs`
- [x] `Services/BusinessHoursService.cs`
- [x] `DependencyInjection.cs`

#### API Layer — Controllers ✅
- [x] `Program.cs`
- [x] `Middleware/ExceptionHandlingMiddleware.cs`
- [x] `Controllers/AuthController.cs`
- [x] `Controllers/ShopsController.cs`
- [x] `Controllers/RankingsController.cs`
- [x] `Controllers/ReviewsController.cs`
- [x] `Controllers/FavoritesController.cs`
- [x] `Controllers/SubmissionsController.cs`
- [x] `Controllers/UserController.cs`
- [x] `Controllers/Admin/AdminShopsController.cs`
- [x] `Controllers/Admin/AdminUsersController.cs`
- [x] `Controllers/Admin/AdminInvitationCodesController.cs`
- [x] `Controllers/Admin/AdminSettingsController.cs`
- [x] `Controllers/Admin/AdminReviewsController.cs`
- [x] `Controllers/Admin/AdminSubmissionsController.cs`
- [x] `Controllers/Admin/AdminRankingsController.cs`
- [x] `Controllers/Admin/AdminDatabaseController.cs`

#### 設定與部署
- [x] `appsettings.json`
- [x] `appsettings.Development.json`
- [x] `Dockerfile`
- [x] `.github/workflows/ci.yml`
- [x] `.github/workflows/deploy.yml`
- [x] EF Core migration (initial) — `20260504_InitialCreate`

---

### Phase 2 — 前台 `ramendo-web` (Vue 3) ✅
- [x] Vite + Vue 3 + TypeScript + Tailwind 初始化
- [x] `api/client.ts` (Axios + JWT 自動刷新)
- [x] `stores/auth.store.ts` (Pinia)
- [x] `router/index.ts` + auth guard
- [x] `DefaultLayout.vue` (Navbar + Footer)
- [x] `HomeView.vue`
- [x] `ShopListView.vue`
- [x] `ShopDetailView.vue`
- [x] `RankingsView.vue`
- [x] `auth/LoginView.vue`, `RegisterView.vue`
- [x] `user/DashboardView.vue`
- [x] `user/SubmitShopView.vue`
- [x] `CLAUDE.md`
- [x] `docs/pages/` (8 個 .md)

### Phase 3 — 後台 `ramendo-admin` (Vue 3) ✅
- [x] Vite + Vue 3 + TypeScript + Tailwind 初始化
- [x] `AdminLayout.vue` (Sidebar + Header)
- [x] `AdminDashboardView.vue`
- [x] `shops/` (List / Create / Edit)
- [x] `users/AdminUserListView.vue`
- [x] `reviews/AdminReviewListView.vue`
- [x] `submissions/AdminSubmissionListView.vue`
- [x] `invitation-codes/AdminInvitationCodesView.vue`
- [x] `settings/AdminSettingsView.vue`
- [x] `rankings/AdminRankingsView.vue`
- [x] `database/AdminDatabaseView.vue`
- [x] `CLAUDE.md`
- [x] `docs/pages/` (8 個 .md)

---

## CI/CD

### ramendo-api

**ci.yml** — PR: test → Fortify SAST → build  
**deploy.yml** — main push: publish → Render deploy hook

### ramendo-web / ramendo-admin

**ci.yml** — PR: type-check → lint → Fortify SAST  
**deploy.yml** — main push: `npm run build` → Cloudflare Pages

---

## 部署架構

```
Browser
  ├─ Cloudflare Pages → ramendo-web (Vue SPA)
  ├─ Cloudflare Pages → ramendo-admin (Vue SPA)
  └─ Render.com Free Tier → ramendo-api (Docker)
                              └─ Supabase (PostgreSQL 15)
                              └─ Cloudinary (圖片)
```

> Render free tier 閒置 15 分鐘後冷啟動 ~30s。
> 用 UptimeRobot 每 14 分鐘 ping `GET /health` 避免冷啟動。
