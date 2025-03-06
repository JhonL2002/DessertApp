using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<PurchaseOrder?> GetOrderWithDetailsAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = _unitOfWork.PurchaseOrders.GetByIdAsync(
                orderId,
                cancellationToken,
                query => query
                    .Include(od => od.OrderDetails)
                        .ThenInclude(od => od.Ingredient)
                            .ThenInclude(i => i.IngredientUnit)
                    .Include(od => od.OrderDetails)
                        .ThenInclude(u => u.Unit));
            return order;
        }
    }
}
