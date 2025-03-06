
namespace DessertApp.Services.Application.ProductionServices
{
    public interface IProductionService
    {
        Task<int> ProduceDessertsAsync(int dessertId, string month, CancellationToken cancellationToken);
    }
}
