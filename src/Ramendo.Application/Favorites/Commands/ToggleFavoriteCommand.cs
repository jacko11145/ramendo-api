using MediatR;

namespace Ramendo.Application.Favorites.Commands;

public sealed record ToggleFavoriteCommand(Guid UserId, Guid ShopGuid) : IRequest<bool>;
