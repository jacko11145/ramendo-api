using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Domain.Services;

public interface IRankingService
{
    IReadOnlyList<(RamenShop Shop, float Score)> Rank(IEnumerable<RamenShop> shops, string type);
}
