using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<PurchaseOrder>> GetAllOrdersAsync(CancellationToken cancellationToken);
        Task<PurchaseOrder?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken);
    }
}
