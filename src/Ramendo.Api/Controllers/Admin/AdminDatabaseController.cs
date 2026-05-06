using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramendo.Application.Common;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/database")]
[Authorize(Roles = "Admin")]
public sealed class AdminDatabaseController(RamendoDbContext db) : ControllerBase
{
    private sealed record TableSizeRow(string TableName, int SizeKb);

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<List<TableStatsDto>>>> GetStats(CancellationToken ct)
    {
        var counts = new (string Name, int Count)[]
        {
            ("users",               await db.Users.CountAsync(ct)),
            ("ramen_shops",         await db.RamenShops.CountAsync(ct)),
            ("menu_items",          await db.MenuItems.CountAsync(ct)),
            ("option_types",        await db.OptionTypes.CountAsync(ct)),
            ("option_values",       await db.OptionValues.CountAsync(ct)),
            ("item_options",        await db.ItemOptions.CountAsync(ct)),
            ("reviews",             await db.Reviews.CountAsync(ct)),
            ("menu_item_ratings",   await db.MenuItemRatings.CountAsync(ct)),
            ("review_options",      await db.ReviewOptions.CountAsync(ct)),
            ("favorites",           await db.Favorites.CountAsync(ct)),
            ("shop_submissions",    await db.ShopSubmissions.CountAsync(ct)),
            ("invitation_codes",    await db.InvitationCodes.CountAsync(ct)),
            ("user_shops",          await db.UserShops.CountAsync(ct)),
            ("refresh_tokens",      await db.RefreshTokens.CountAsync(ct)),
            ("system_settings",     await db.SystemSettings.CountAsync(ct)),
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
