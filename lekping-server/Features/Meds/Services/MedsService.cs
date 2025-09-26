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

        public async Task<IReadOnlyList<MedDto>> GetAllAsync(Guid userId, int skip, int take, CancellationToken ct)
        {
            return await _db.Meds.AsNoTracking()
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.BrandName)
                .Skip(skip).Take(take)
                .Select(m => m.ToDto())
                .ToListAsync(ct);
        }

        public async Task<MedDto?> FindAsync(Guid userId, Guid id, CancellationToken ct)
        {
            return await _db.Meds.AsNoTracking()
                .Where(m => m.UserId == userId && m.Id == id)
                .Select(m => m.ToDto())
                .SingleOrDefaultAsync(ct);
        }

        public async Task<MedDto> CreateAsync(Guid userId, CreateMedRequest req, CancellationToken ct)
        {
            var e = new Domain.Entities.Med(
                userId,
                req.BrandName, req.GenericName,
                req.StrengthValue, req.StrengthUnit,
                req.Form, req.PackageSize, req.Ean
            );

            _db.Meds.Add(e);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new InvalidOperationException("This drug already exists on your list.", ex);
            }

            return e.ToDto();
        }

        public async Task<MedDto?> UpdateAsync(Guid userId, UpdateMedRequest req, CancellationToken ct)
        {
            var e = await _db.Meds
                .Where(m => m.UserId == userId && m.Id == req.Id)
                .SingleOrDefaultAsync(ct);

            if (e is null) return null;

            e.Update(req.BrandName, req.GenericName, req.StrengthValue,
                     req.StrengthUnit, req.Form, req.PackageSize, req.Ean);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new InvalidOperationException("Conflict: Duplicate drug in your list.", ex);
            }

            return e.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken ct)
        {
            var e = await _db.Meds
                .Where(m => m.UserId == userId && m.Id == id)
                .SingleOrDefaultAsync(ct);

            if (e is null) return false;

            _db.Meds.Remove(e);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
