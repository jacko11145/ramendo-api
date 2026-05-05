using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Favorites.Commands;
using Ramendo.Application.Favorites.DTOs;
using Ramendo.Application.Favorites.Queries;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/user/favorites")]
[Authorize]
public sealed class FavoritesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FavoriteShopDto>>>> GetFavorites(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetUserFavoritesQuery(userId), ct);
        return Ok(ApiResponse<IReadOnlyList<FavoriteShopDto>>.Ok(result));
    }

    [HttpPost("{shopGuid:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> Toggle(Guid shopGuid, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isFavorited = await mediator.Send(new ToggleFavoriteCommand(userId, shopGuid), ct);
        var msg = isFavorited ? "Added to favorites." : "Removed from favorites.";
        return Ok(ApiResponse<bool>.Ok(isFavorited, msg));
    }
}
