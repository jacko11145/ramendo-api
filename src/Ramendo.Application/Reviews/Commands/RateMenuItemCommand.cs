using MediatR;
using Ramendo.Application.Reviews.DTOs;

namespace Ramendo.Application.Reviews.Commands;

public sealed record RateMenuItemCommand(Guid UserId, Guid MenuItemId, CreateMenuItemRatingDto Dto)
    : IRequest<MenuItemRatingDto>;
