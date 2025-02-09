using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices;
using System.Runtime.CompilerServices;

namespace DessertApp.Services.UnitOfWorkServices
{
    public interface IUnitOfWork : IDisposable
    {
        //Add repositories here using IDomainGenericRepository
        IDomainGenericRepository<Ingredient, int> Ingredients { get; }
        IDomainGenericRepository<IngredientUnit, int> IngredientUnits {  get; }
        IDomainGenericRepository<MeasurementUnit, int> MeasurementUnits { get; }
        IDomainGenericRepository<DessertIngredient, int> DessertIngredients { get; }

        //Commit changes to database
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        //Clear database resources
        protected virtual void Dispose(bool disposing) { }
    }
}
