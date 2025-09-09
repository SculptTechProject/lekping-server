using LekPing.Server.Features.Items.Dtos;

namespace LekPing.Server.Features.Items;

public interface IItemsService
{
    Task<IReadOnlyList<ItemDto>> GetAllAsync(int skip, int take, CancellationToken ct);
    Task<ItemDto?> FindAsync(Guid id, CancellationToken ct);
    Task<ItemDto> CreateAsync(CreateItemRequest req, CancellationToken ct);
    Task<ItemDto?> UpdateAsync(UpdateItemRequest req, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
