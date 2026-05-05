using MediatR;

namespace Ramendo.Application.Shops.Commands;

public sealed record DeleteShopCommand(Guid Guid) : IRequest;
