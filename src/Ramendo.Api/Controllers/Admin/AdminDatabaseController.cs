using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramendo.Application.Common;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/database")]
[Authorize(Roles = "Admin")]
public sealed class AdminDatabaseController(RamendoDbContext db) : ControllerBase
{
    private sealed record TableSizeRow(string TableName, int SizeKb);

    private string Tbl<T>() where T : class =>
        db.Model.FindEntityType(typeof(T))!.GetTableName()!;

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<List<TableStatsDto>>>> GetStats(CancellationToken ct)
    {
        var counts = new (string Name, int Count)[]
        {
            (Tbl<User>(),            await db.Users.CountAsync(ct)),
            (Tbl<RamenShop>(),       await db.RamenShops.CountAsync(ct)),
            (Tbl<MenuItem>(),        await db.MenuItems.CountAsync(ct)),
            (Tbl<OptionType>(),      await db.OptionTypes.CountAsync(ct)),
            (Tbl<OptionValue>(),     await db.OptionValues.CountAsync(ct)),
            (Tbl<ItemOption>(),      await db.ItemOptions.CountAsync(ct)),
            (Tbl<Review>(),          await db.Reviews.CountAsync(ct)),
            (Tbl<MenuItemRating>(),  await db.MenuItemRatings.CountAsync(ct)),
            (Tbl<ReviewOption>(),    await db.ReviewOptions.CountAsync(ct)),
            (Tbl<Favorite>(),        await db.Favorites.CountAsync(ct)),
            (Tbl<ShopSubmission>(),  await db.ShopSubmissions.CountAsync(ct)),
            (Tbl<InvitationCode>(),  await db.InvitationCodes.CountAsync(ct)),
            (Tbl<UserShop>(),        await db.UserShops.CountAsync(ct)),
            (Tbl<RefreshToken>(),    await db.RefreshTokens.CountAsync(ct)),
            (Tbl<SystemSetting>(),   await db.SystemSettings.CountAsync(ct)),
        };

        var sizes = await db.Database
            .SqlQueryRaw<TableSizeRow>("""
                SELECT relname AS table_name,
                       CAST(pg_total_relation_size(relid) / 1024 AS INTEGER) AS size_kb
                FROM pg_catalog.pg_statio_user_tables
                WHERE schemaname = 'public'
                """)
            .ToListAsync(ct);

        var sizeMap = sizes
            .GroupBy(s => s.TableName)
            .ToDictionary(g => g.Key, g => g.Sum(s => s.SizeKb));

        var result = counts
            .Select(c => new TableStatsDto(c.Name, c.Count, sizeMap.GetValueOrDefault(c.Name, 0)))
            .ToList();

        return Ok(ApiResponse<List<TableStatsDto>>.Ok(result));
    }
}
