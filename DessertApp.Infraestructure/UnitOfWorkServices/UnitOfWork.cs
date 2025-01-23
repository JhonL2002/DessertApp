using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices;
using DessertApp.Services.UnitOfWorkServices;

namespace DessertApp.Infraestructure.UnitOfWorkServices
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDomainGenericRepository<Ingredient, int> _ingredientRepository;
        private readonly IDomainGenericRepository<IngredientUnit, int> _ingredientUnitRepository;
        private readonly IDomainGenericRepository<MeasurementUnit, int> _measurementUnitRepository;

        public UnitOfWork(
            AppDbContext appDbContext,
            IDomainGenericRepository<Ingredient, int> ingredientRepository,
            IDomainGenericRepository<IngredientUnit, int> ingredientUnitRepository,
            IDomainGenericRepository<MeasurementUnit, int> measurementUnitRepository)
        {
            _appDbContext = appDbContext;
            _ingredientRepository = ingredientRepository;
            _ingredientUnitRepository = ingredientUnitRepository;
            _measurementUnitRepository = measurementUnitRepository;
        }

        public IDomainGenericRepository<Ingredient, int> Ingredients => _ingredientRepository;
        public IDomainGenericRepository<IngredientUnit, int> IngredientUnits => _ingredientUnitRepository;
        public IDomainGenericRepository<MeasurementUnit, int> MeasurementUnits => _measurementUnitRepository;

        public async Task<Ingredient> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken)
        {
            return await Ingredients.GetByIdWithDetailsAsync(
                id, cancellationToken,
                ingredient => ingredient.IngredientUnit!);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            //Create a new ingredient
            await _ingredientRepository.CreateAsync(ingredient, cancellationToken);

            //Get the default measurement if is not specified
            if (ingredientUnit.UnitId <= 0)
            {
                var defaultUnit = await _appDbContext.Set<MeasurementUnit>()
                     .FindAsync([1], cancellationToken) ?? throw new Exception("Unable to find a default measurement unit");

                //Assign a default measurement unit
                ingredientUnit.UnitId = defaultUnit.Id;
                ingredientUnit.Unit = defaultUnit;
            }

            //Create a new releated IngredientUnit
            await _ingredientUnitRepository.CreateAsync(ingredientUnit, cancellationToken);

            //Save all changes
            await _appDbContext.SaveChangesAsync(cancellationToken);
            
            return ingredient;
        }

        public async Task<Ingredient> DeleteAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            //Delete associated ingredient unit
            await _ingredientUnitRepository.DeleteAsync(ingredientUnit, cancellationToken);

            //Delete ingredient
            await _ingredientRepository.DeleteAsync(ingredient, cancellationToken);

            //Save changes
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return ingredient;
        }

        public async Task<Ingredient> UpdateAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            //Update ingredient
            await _ingredientRepository.UpdateAsync(ingredient, cancellationToken);

            //Update releated IngredientUnit
            await _ingredientUnitRepository.UpdateAsync(ingredientUnit, cancellationToken);

            //Save changes
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return ingredient;
        }

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
