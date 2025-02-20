using DessertApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
