using DessertApp.Models.Entities;
using DessertApp.Services.Application.PurchaseOrderServices;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Application.PurchaseOrderServices
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryAnalysisRepository _inventoryAnalysisRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderService(IUnitOfWork unitOfWork, IInventoryAnalysisRepository inventoryAnalysisRepository, IPurchaseOrderRepository purchaseOrderRepository)
        {
            _unitOfWork = unitOfWork;
            _inventoryAnalysisRepository = inventoryAnalysisRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<bool> ApprovePurchaseOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return false;
            }

            if (order.IsApproved)
            {
                return false;
            }

            //Approve the order
            order.IsApproved = true;
            await _unitOfWork.PurchaseOrders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task CreatePurchaseOrderAsync(CancellationToken cancellationToken)
        {
            var analyses = await _inventoryAnalysisRepository.GetInventoryAnalysesWithDetailsAsync(cancellationToken);
            if (!analyses.Any()) throw new Exception("No inventory analysis found");

            var purchaseOrder = new PurchaseOrder
            {
                OrderDate = DateTime.Now,
                ExpectedDeliveryDate = DateTime.Now.AddDays(7),
                IsApproved = false,
                TotalCost = 0,
                OrderDetails = new List<PurchaseOrderDetail>()
            };

            decimal totalCost = 0;

            foreach (var analysis in analyses)
            {
                var eoq = analysis.EOQ;
                var unitCost = analysis.Ingredient.IngredientUnit!.CostPerUnit ?? 0;
                var subtotal = eoq * unitCost;
                var orderFrequency = Math.Round(analysis.OrderFrequency);

                //Calculate NextOrderDate, NextOrderDate cannot be set on Weekend
                var nextOrderDate = DateTime.Now.AddMonths((int)analysis.OptimalOrderingPeriod);

                //Change to Monday if NextOrderDate is Saturday or Sunday
                if (nextOrderDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    nextOrderDate = nextOrderDate.AddDays(2);
                }
                else if (nextOrderDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    nextOrderDate = nextOrderDate.AddDays(1);
                }


                var detail = new PurchaseOrderDetail
                {
                    PurchaseOrderId = purchaseOrder.Id,
                    IngredientId = analysis.IngredientId,
                    UnitId = analysis.Ingredient.IngredientUnit!.UnitId,
                    Quantity = eoq,
                    UnitCost = unitCost,
                    Subtotal = subtotal,
                    NextOrderDate = DateTime.Now.AddMonths((int)analysis.OptimalOrderingPeriod),
                    RemainingOrders = (int)(12/analysis.OrderFrequency)
                };

                totalCost += detail.Subtotal;
                purchaseOrder.OrderDetails.Add(detail);
            }

            purchaseOrder.TotalCost = totalCost;

            await _unitOfWork.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            var orders = await _purchaseOrderRepository.GetAllOrdersAsync(cancellationToken);
            return orders;
        }

        public async Task<PurchaseOrder?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _purchaseOrderRepository.GetOrderDetailsAsync(orderId, cancellationToken);
            return order;
        }
    }
}
