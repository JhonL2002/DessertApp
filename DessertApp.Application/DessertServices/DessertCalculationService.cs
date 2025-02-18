using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Application.DessertServices
{
    public class DessertCalculationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DessertCalculationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessInventoryAnalysisAsync(List<DessertDemandsImportlDTO> orderDetails, CancellationToken cancellationToken)
        {
            var calculations = new List<IngredientCalculationDTO>();
            var groupedByIngredient = orderDetails.GroupBy(od => od.IngredientId);

            foreach (var ingredientGroup in groupedByIngredient)
            {
                var firstDetail = ingredientGroup.First();
                var annualDemand = ingredientGroup.Sum(od => od.Quantity);
                var orderingCost = firstDetail.OrderingCost ?? 0;
                var costPerUnit = firstDetail.UnitCost ?? 0;
                var holdingCostRate = firstDetail.MonthlyHoldingCostRate ?? 0;
                var eoq = CalculateEOQ(annualDemand, orderingCost, holdingCostRate, costPerUnit);
                var orderFrequency = OrderFrequency(annualDemand, eoq);
                var optimalOrderingPeriod = OptimalOrderingPeriod(orderFrequency);
                var costOfMaintaining = ((holdingCostRate / 100) * costPerUnit) * 12;

                calculations.Add(new IngredientCalculationDTO
                {
                    IngredientId = firstDetail.IngredientId,
                    IngredientName = firstDetail.Ingredient.Name,
                    EOQ = eoq,
                    OrderFrequency = orderFrequency,
                    OptimalOrderingPeriod = optimalOrderingPeriod,
                    OrderingCost = orderingCost,
                    AnnualDemand = annualDemand,
                    CostOfMaintainingUnitsInInventory = costOfMaintaining
                });
            }

            //Save changes into database
            await SaveInventoryAnalysisToDatabase(calculations, cancellationToken);
        }

        private int CalculateEOQ(decimal annualDemand, decimal orderingCost, decimal holdingCostRate, decimal costPerUnit)
        {
            if (orderingCost == 0 || holdingCostRate == 0)
                return 0;

            var annualHoldingCost = ((holdingCostRate * costPerUnit) / 100) * 12;

            return (int)Math.Sqrt(((2 * (double)annualDemand * (double)orderingCost)) / (double)annualHoldingCost);
        }

        private decimal OrderFrequency(decimal annualDemand, decimal eoq)
        {
            return annualDemand / eoq;
        }

        private decimal OptimalOrderingPeriod(decimal orderFrequency)
        {
            return 12 / orderFrequency;
        }

        private async Task SaveInventoryAnalysisToDatabase(List<IngredientCalculationDTO> calculations, CancellationToken cancellationToken)
        {
            var existingAnalysis = await _unitOfWork.InventoryAnalysis.GetAllAsync(cancellationToken);

            var analysisEntities = calculations.Select(c => new InventoryAnalysis
            {
                IngredientId = c.IngredientId,
                IngredientName = c.IngredientName,
                EOQ = c.EOQ,
                OrderFrequency = c.OrderFrequency,
                OptimalOrderingPeriod = c.OptimalOrderingPeriod,
                OrderingCost = c.OrderingCost,
                AnnualDemand = c.AnnualDemand,
                CostOfMaintainingUnitsInInventory = c.CostOfMaintainingUnitsInInventory
            }).ToList();

            await _unitOfWork.InventoryAnalysis.AddRangeAsync(analysisEntities, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
