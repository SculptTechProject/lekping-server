using System.Collections.Concurrent;
using LekPing.Server.Features.Items.Dtos;

namespace LekPing.Server.Features.Items;

public sealed class ItemsServiceInMemory : IItemsService
{
    private readonly ConcurrentDictionary<Guid, ItemDto> _store = new();

    public Task<IReadOnlyList<ItemDto>> GetAllAsync(int skip, int take, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<ItemDto>>(
            _store.Values.Skip(skip).Take(take).ToList()
        ); // return a paginated list

    public Task<ItemDto?> FindAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_store.TryGetValue(id, out var v) ? v : null);

    public Task<ItemDto> CreateAsync(CreateItemRequest req, CancellationToken ct)
    {
        var created = new ItemDto(Guid.NewGuid(), req.Name.Trim(), DateTime.UtcNow);
        _store[created.Id] = created;
        return Task.FromResult(created); // return the created item with its new ID
    }

    public Task<ItemDto?> UpdateAsync(UpdateItemRequest req, CancellationToken ct)
    {
        if (!_store.TryGetValue(req.Id, out var existing))
            return Task.FromResult<ItemDto?>(null); // not found

        var updated = new ItemDto(req.Id, req.Name.Trim(), existing.CreatedAt);
        _store[req.Id] = updated;
        return Task.FromResult<ItemDto?>(updated); // return the updated item
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_store.TryRemove(id, out _)); // return true if removed, false if not found
}
