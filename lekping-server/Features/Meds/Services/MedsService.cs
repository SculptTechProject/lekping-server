using lekping.server.Features.Meds.Mapping;
using lekping.server.Features.Meds.Dtos;
using Microsoft.EntityFrameworkCore;
using lekping.server.Infrastructure.Persistence;

namespace lekping.server.Features.Meds.Services
{
    public sealed class MedsService : IMedsService
    {
        private readonly AppDbContext _db;
        public MedsService(AppDbContext db) => _db = db;

        public async Task<MedDto> CreateAsync(CreateMedRequest req, CancellationToken ct)
        {
            var e = new Domain.Entities.Med(
                req.BrandName, req.GenericName,
                req.StrengthValue, req.StrengthUnit,
                req.Form, req.PackageSize, req.Ean
            );
            _db.Meds.Add(e);
            await _db.SaveChangesAsync(ct);
            return e.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _db.Meds.FindAsync(new object?[] { id }, ct);
            if (e is null) return false;
            _db.Meds.Remove(e);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<MedDto>> GetAllAsync(int skip, int take, CancellationToken ct)
            => await _db.Meds.AsNoTracking()
                .OrderBy(x => x.BrandName)
                .Skip(skip).Take(take)
                .Select(x => x.ToDto())
                .ToListAsync(ct);

        public async Task<MedDto?> FindAsync(Guid id, CancellationToken ct)
            => await _db.Meds.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => x.ToDto())
                .SingleOrDefaultAsync(ct);

        public async Task<MedDto?> UpdateAsync(UpdateMedRequest req, CancellationToken ct)
        {
            var e = await _db.Meds.FindAsync(new object?[] { req.Id }, ct);
            if (e is null) return null;

            e.Update(req.BrandName, req.GenericName, req.StrengthValue,
                     req.StrengthUnit, req.Form, req.PackageSize, req.Ean);

            await _db.SaveChangesAsync(ct);
            return e.ToDto();
        }
    }
}
