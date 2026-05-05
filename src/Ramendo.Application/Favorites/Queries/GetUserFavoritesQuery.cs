using MediatR;
using Ramendo.Application.Favorites.DTOs;

namespace Ramendo.Application.Favorites.Queries;

public sealed record GetUserFavoritesQuery(Guid UserId) : IRequest<IReadOnlyList<FavoriteShopDto>>;
