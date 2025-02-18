using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.CacheServices;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DessertApp.Infraestructure.CacheServices
{
    public class DessertCategoryCacheService : IDessertCategoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IDessertCategoryRepository _dessertCategoryService;
        private readonly ILogger<DessertCategoryCacheService> _logger;

        public DessertCategoryCacheService(IMemoryCache cache, IDessertCategoryRepository dessertCategoryService, ILogger<DessertCategoryCacheService> logger)
        {
            _cache = cache;
            _dessertCategoryService = dessertCategoryService;
            _logger = logger;
        }

        public async Task<IEnumerable<DessertCategory>> GetCategoriesWithDessertsAsync(CancellationToken cancellationToken)
        {
            //Try to get dessert categories from cache
            if (!_cache.TryGetValue("categoriesWithDesserts", out IEnumerable<DessertCategory>? categories))
            {
                //Log when cache is not found
                _logger.LogInformation("Cache miss- fetching from database");
                //If not exist, get categories from database
                categories = await _dessertCategoryService.GetAllWithDessertsAsync(cancellationToken);

                //Put some cache configurations here (expiration)
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                //Save fetched categories into cache
                _cache.Set("categoriesWithDesserts", categories, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation("Cache hit - using cached data");
            }

            //Return dessert categories (from database or cache)
            return categories!;
        }

        public void RemoveCategoriesCache()
        {
            _cache.Remove("categoriesWithDesserts");
        }
    }
}
