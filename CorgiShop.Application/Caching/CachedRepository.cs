using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Model.Abstractions;
using Microsoft.Extensions.Logging;

namespace CorgiShop.Application.Caching;

public class CachedRepository<T> : IRepository<T>
    where T : class, IRepositoryEntity
{
    private readonly ICachingService _cachingService;
    private readonly IRepository<T> _repository;
    private readonly ILogger _logger;

    private const string GlobalCacheKeyBase = "CorgiShop";
    private string RepoCacheKeyBase => typeof(T).Name;

    public CachedRepository(
        ICachingService cachingService,
        IRepository<T> repository,
        ILogger logger)
    {
        _cachingService = cachingService;
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> Count() => await _cachingService.GetOrRefresh(GetCountCacheKey(), _repository.Count);

    public async Task<T> GetById(int id) => await _cachingService.GetOrRefresh(GetEntityCacheKey(id), () => _repository.GetById(id));

    public async Task<IEnumerable<T>> List() => await _cachingService.GetOrRefresh(GetListCacheKey(), _repository.List);

    public async Task<IEnumerable<T>> ListPaginated(int limit, int offset) => 
        await _cachingService.GetOrRefresh(GetPageCacheKey(limit, offset), () => _repository.ListPaginated(limit, offset));

    public async Task<T> Create(T newEntity)
    {
        var createdEntity = await _repository.Create(newEntity);
        await ClearAllCache();
        return createdEntity;
    }

    public async Task<T> Update(T entity)
    {
        var updatedEntity = await _repository.Update(entity);
        await ClearIdAndAllCache(entity.Id);
        return updatedEntity;
    }

    public async Task Delete(int id)
    {
        await _repository.Delete(id);
        await ClearIdAndAllCache(id);
    }

    public async Task SoftDelete(int id)
    {
        await _repository.SoftDelete(id);
        await ClearIdAndAllCache(id);
    }

    private async Task ClearIdAndAllCache(int id)
    {
        _logger.LogDebug($"Clearing entity cache for: {GetEntityCacheKey(id)}");
        await _cachingService.ClearCache(GetEntityCacheKey(id));
        await ClearAllCache();
    }

    private async Task ClearAllCache()
    {
        _logger.LogDebug($"Clearing all cache for: {GetCacheKeyBase()}");
        await _cachingService.ClearCache(GetCountCacheKey());
        await _cachingService.ClearCache(GetListCacheKey());
        await _cachingService.ClearCache(GetPageCacheKeyBase());
    }

    private string GetCacheKeyBase() => $"{GlobalCacheKeyBase}:{RepoCacheKeyBase}";
    private string GetCountCacheKey() => $"{GetCacheKeyBase()}:count";
    private string GetEntityCacheKey(int id) => $"{GetCacheKeyBase()}:{id}";
    private string GetListCacheKey() => $"{GetCacheKeyBase()}:list";
    private string GetPageCacheKeyBase() => $"{GetCacheKeyBase()}:Page";
    private string GetPageCacheKey(int limit, int offset) => $"{GetPageCacheKeyBase()}:{limit}-{offset}";

}
