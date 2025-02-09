using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices;
using DessertApp.Services.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore.Storage;

namespace DessertApp.Infraestructure.UnitOfWorkServices
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDomainGenericRepository<Ingredient, int> _ingredientRepository;
        private readonly IDomainGenericRepository<IngredientUnit, int> _ingredientUnitRepository;
        private readonly IDomainGenericRepository<MeasurementUnit, int> _measurementUnitRepository;
        private readonly IDomainGenericRepository<DessertIngredient, int> _dessertIngredientRepository;

        public UnitOfWork(
            AppDbContext appDbContext,
            IDomainGenericRepository<Ingredient, int> ingredientRepository,
            IDomainGenericRepository<IngredientUnit, int> ingredientUnitRepository,
            IDomainGenericRepository<MeasurementUnit, int> measurementUnitRepository,
            IDomainGenericRepository<DessertIngredient, int> dessertIngredientRepository)
        {
            _appDbContext = appDbContext;
            _ingredientRepository = ingredientRepository;
            _ingredientUnitRepository = ingredientUnitRepository;
            _measurementUnitRepository = measurementUnitRepository;
            _dessertIngredientRepository = dessertIngredientRepository;
        }

        public IDomainGenericRepository<Ingredient, int> Ingredients => _ingredientRepository;
        public IDomainGenericRepository<IngredientUnit, int> IngredientUnits => _ingredientUnitRepository;
        public IDomainGenericRepository<MeasurementUnit, int> MeasurementUnits => _measurementUnitRepository;
        public IDomainGenericRepository<DessertIngredient, int> DessertIngredients => _dessertIngredientRepository;

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
