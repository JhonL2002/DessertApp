using DessertApp.Models.Entities;

namespace DessertApp.Services.Application.PurchaseOrderServices
{
    public interface IPurchaseOrderService
    {
        Task CreatePurchaseOrderAsync(CancellationToken cancellationToken);
        Task<IEnumerable<PurchaseOrder>> GetAllOrdersAsync(CancellationToken cancellationToken);
        Task<bool> ApprovePurchaseOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<PurchaseOrder?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken);
    }
}
