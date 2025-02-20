using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly DomainPersistentRepository<PurchaseOrder, int> _purchaseOrderRepository;

        public PurchaseOrderRepository(DomainPersistentRepository<PurchaseOrder, int> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public Task<bool> ApprovePurchaseOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            bool isApproved = false;
            return Task.FromResult(isApproved);
        }

        public Task<IEnumerable<PurchaseOrder>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            var orders = _purchaseOrderRepository.GetAllAsync(cancellationToken);
            return orders;
        }

        public Task<PurchaseOrder?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = _purchaseOrderRepository.GetByIdAsync(
                orderId,
                cancellationToken,
                query => query
                    .Include(od => od.OrderDetails)
                        .ThenInclude(i => i.Ingredient)
                    .Include(od => od.OrderDetails)
                        .ThenInclude(u => u.Unit));
            return order;
        }
    }
}
