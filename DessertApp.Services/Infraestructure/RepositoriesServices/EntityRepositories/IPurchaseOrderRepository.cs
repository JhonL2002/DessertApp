using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetOrderWithDetailsAsync(int orderId, CancellationToken cancellationToken);
    }
}
