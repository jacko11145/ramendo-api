using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramendo.Application.Common;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/database")]
[Authorize(Roles = "Admin")]
public sealed class AdminDatabaseController(RamendoDbContext db) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<Dictionary<string, int>>>> GetStats(CancellationToken ct)
    {
        var stats = new Dictionary<string, int>
        {
            ["users"] = await db.Users.CountAsync(ct),
            ["ramen_shops"] = await db.RamenShops.CountAsync(ct),
            ["menu_items"] = await db.MenuItems.CountAsync(ct),
            ["option_types"] = await db.OptionTypes.CountAsync(ct),
            ["option_values"] = await db.OptionValues.CountAsync(ct),
            ["item_options"] = await db.ItemOptions.CountAsync(ct),
            ["reviews"] = await db.Reviews.CountAsync(ct),
            ["menu_item_ratings"] = await db.MenuItemRatings.CountAsync(ct),
            ["review_options"] = await db.ReviewOptions.CountAsync(ct),
            ["favorites"] = await db.Favorites.CountAsync(ct),
            ["shop_submissions"] = await db.ShopSubmissions.CountAsync(ct),
            ["invitation_codes"] = await db.InvitationCodes.CountAsync(ct),
            ["user_shops"] = await db.UserShops.CountAsync(ct),
            ["system_settings"] = await db.SystemSettings.CountAsync(ct),
            ["refresh_tokens"] = await db.RefreshTokens.CountAsync(ct),
        };

        return Ok(ApiResponse<Dictionary<string, int>>.Ok(stats));
    }
}
