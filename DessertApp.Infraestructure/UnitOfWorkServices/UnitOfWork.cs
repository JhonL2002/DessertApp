using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Infraestructure.UnitOfWorkServices
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDomainGenericRepository<Ingredient, int> _ingredientRepository;
        private readonly IDomainGenericRepository<IngredientUnit, int> _ingredientUnitRepository;
        private readonly IDomainGenericRepository<MeasurementUnit, int> _measurementUnitRepository;
        private readonly IDomainGenericRepository<DessertIngredient, int> _dessertIngredientRepository;
        private readonly IDomainGenericRepository<InventoryAnalysis, int> _inventoryAnalysisRepository;
        private readonly IDomainGenericRepository<PurchaseOrder, int> _purchaseOrderRepository;
        private readonly IDomainGenericRepository<UnitConversion, int> _unitConversionRepository;
        private readonly IDomainGenericRepository<PendingReplenishment, int> _pendingReplenishmentRepository;
        private readonly IDomainGenericRepository<Dessert, int> _dessertRepository;
        private readonly IDomainGenericRepository<DessertDemand, int> _dessertDemandRepository;

        public UnitOfWork(
            AppDbContext appDbContext,
            IDomainGenericRepository<Ingredient, int> ingredientRepository,
            IDomainGenericRepository<IngredientUnit, int> ingredientUnitRepository,
            IDomainGenericRepository<MeasurementUnit, int> measurementUnitRepository,
            IDomainGenericRepository<DessertIngredient, int> dessertIngredientRepository,
            IDomainGenericRepository<InventoryAnalysis, int> inventoryAnalysisRepository,
            IDomainGenericRepository<PurchaseOrder, int> purchaseOrderRepository,
            IDomainGenericRepository<UnitConversion, int> unitConversionRepository,
            IDomainGenericRepository<PendingReplenishment, int> pendingReplenishmentRepository,
            IDomainGenericRepository<Dessert, int> dessertRepository,
            IDomainGenericRepository<DessertDemand, int> dessertDemandRepository)
        {
            _appDbContext = appDbContext;
            _ingredientRepository = ingredientRepository;
            _ingredientUnitRepository = ingredientUnitRepository;
            _measurementUnitRepository = measurementUnitRepository;
            _dessertIngredientRepository = dessertIngredientRepository;
            _inventoryAnalysisRepository = inventoryAnalysisRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitConversionRepository = unitConversionRepository;
            _pendingReplenishmentRepository = pendingReplenishmentRepository;
            _dessertRepository = dessertRepository;
            _dessertDemandRepository = dessertDemandRepository;
        }

        public IDomainGenericRepository<Ingredient, int> Ingredients => _ingredientRepository;
        public IDomainGenericRepository<IngredientUnit, int> IngredientUnits => _ingredientUnitRepository;
        public IDomainGenericRepository<MeasurementUnit, int> MeasurementUnits => _measurementUnitRepository;
        public IDomainGenericRepository<DessertIngredient, int> DessertIngredients => _dessertIngredientRepository;
        public IDomainGenericRepository<InventoryAnalysis, int> InventoryAnalysis => _inventoryAnalysisRepository;
        public IDomainGenericRepository<PurchaseOrder, int> PurchaseOrders => _purchaseOrderRepository;
        public IDomainGenericRepository<UnitConversion, int> UnitConversions => _unitConversionRepository;
        public IDomainGenericRepository<PendingReplenishment, int> PendingReplenishments => _pendingReplenishmentRepository;
        public IDomainGenericRepository<Dessert, int> Desserts => _dessertRepository;
        public IDomainGenericRepository<DessertDemand, int> DessertDemands => _dessertDemandRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appDbContext.Dispose();
            }
        }
    }
}
