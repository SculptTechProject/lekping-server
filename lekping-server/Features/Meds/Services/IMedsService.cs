using lekping.server.Features.Meds.Dtos;

namespace lekping.server.Features.Meds.Services
{
    public interface IMedsService
    {
        Task<IReadOnlyList<MedDto>> GetAllAsync(int skip, int take, CancellationToken ct);
        Task<MedDto?> FindAsync(Guid id, CancellationToken ct);
        Task<MedDto> CreateAsync(CreateMedRequest req, CancellationToken ct);
        Task<MedDto?> UpdateAsync(UpdateMedRequest req, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
