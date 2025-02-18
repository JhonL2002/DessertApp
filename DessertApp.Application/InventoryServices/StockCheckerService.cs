using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Application.InventoryServices
{
    public class StockCheckerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockCheckerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HasZeroStockIngredientsAsync(CancellationToken cancellationToken)
        {
            var ingredientsWithZeroStock = await _unitOfWork.Ingredients.GetAllAsync(cancellationToken);

            var findZeroStock = ingredientsWithZeroStock.Any(i => i.Stock == 0);

            return findZeroStock;
        }

        public async Task<bool> HasInventoryAnalysisAsync(CancellationToken cancellationToken)
        {
            var inventoryAnalysis = await _unitOfWork.InventoryAnalysis.GetAllAsync(cancellationToken);
            return inventoryAnalysis.Any(i => i.Id != 0);
        }

        public async Task<bool> HasOrdersAsync(CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.PurchaseOrders.GetAllAsync(cancellationToken);
            return orders.Any();
        }
    }
}
