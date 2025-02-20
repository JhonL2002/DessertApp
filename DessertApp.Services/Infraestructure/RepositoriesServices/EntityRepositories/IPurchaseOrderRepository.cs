using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IPurchaseOrderRepository
    {
        Task<bool> ApprovePurchaseOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<PurchaseOrder>> GetAllOrdersAsync(CancellationToken cancellationToken);
        Task<PurchaseOrder?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken);
    }
}
