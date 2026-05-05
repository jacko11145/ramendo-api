using MediatR;
using Ramendo.Application.Shops.DTOs;

namespace Ramendo.Application.Shops.Commands;

public sealed record UpdateShopCommand(Guid Guid, CreateUpdateShopDto Dto) : IRequest;
