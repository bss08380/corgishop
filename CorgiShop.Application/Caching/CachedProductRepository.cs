using CorgiShop.Domain.Abstractions;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CorgiShop.Application.Caching;

public  class CachedProductRepository : CachedRepository<Product>, IProductRepository
{
	public CachedProductRepository(
		ICachingService cachingService,
        IProductRepository repo,
		ILogger<CachedProductRepository> logger)
		: base(cachingService, repo, logger)
	{
	}
}
