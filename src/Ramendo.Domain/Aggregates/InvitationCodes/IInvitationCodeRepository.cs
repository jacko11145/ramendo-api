namespace Ramendo.Domain.Aggregates.InvitationCodes;

public interface IInvitationCodeRepository
{
    Task<InvitationCode?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<InvitationCode>> GetAllAsync(CancellationToken ct = default);
    Task<int> CountActiveAsync(CancellationToken ct = default);
    Task AddAsync(InvitationCode code, CancellationToken ct = default);
    Task UpdateAsync(InvitationCode code, CancellationToken ct = default);
    Task DeleteAsync(string code, CancellationToken ct = default);
}
