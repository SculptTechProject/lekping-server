using lekping.server.Domain.Entities;

namespace lekping.server.Features.Meds.Persistence
{
    public interface IMedsRepository
    {
        Task<List<Med>> GetAllMedsAsync(int skip, int take, CancellationToken ct = default);
        Task<Med?> FindAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Med med, CancellationToken ct = default);
        Task UpdateAsync(Med med, CancellationToken ct = default);
        Task RemoveAsync(Med med, CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
