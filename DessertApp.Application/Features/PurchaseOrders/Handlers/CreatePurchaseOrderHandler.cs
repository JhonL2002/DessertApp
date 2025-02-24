using DessertApp.Application.Features.PurchaseOrders.Commands;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers
{
    public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryAnalysisRepository _inventoryAnalysisRepository;

        public CreatePurchaseOrderHandler(IUnitOfWork unitOfWork, IInventoryAnalysisRepository inventoryAnalysisRepository)
        {
            _unitOfWork = unitOfWork;
            _inventoryAnalysisRepository = inventoryAnalysisRepository;
        }

        public async Task<bool> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
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
                    RemainingOrders = (int)(12 / analysis.OrderFrequency)
                };

                totalCost += detail.Subtotal;
                purchaseOrder.OrderDetails.Add(detail);
            }

            purchaseOrder.TotalCost = totalCost;

            await _unitOfWork.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
