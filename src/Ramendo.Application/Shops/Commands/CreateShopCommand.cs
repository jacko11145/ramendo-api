using MediatR;
using Ramendo.Application.Shops.DTOs;

namespace Ramendo.Application.Shops.Commands;

public sealed record CreateShopCommand(CreateUpdateShopDto Dto) : IRequest<string>;
