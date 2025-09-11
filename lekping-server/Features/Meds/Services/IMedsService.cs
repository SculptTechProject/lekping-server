using lekping.server.Features.Meds.Dtos;

namespace lekping.server.Features.Meds.Services
{
    public interface IMedsService
    {
        Task<IReadOnlyList<MedDto>> GetAllAsync(Guid userId, int skip, int take, CancellationToken ct);
        Task<MedDto?> FindAsync(Guid userId, Guid id, CancellationToken ct);
        Task<MedDto> CreateAsync(Guid userId, CreateMedRequest req, CancellationToken ct);
        Task<MedDto?> UpdateAsync(Guid userId, UpdateMedRequest req, CancellationToken ct);
        Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken ct);
    }
}
