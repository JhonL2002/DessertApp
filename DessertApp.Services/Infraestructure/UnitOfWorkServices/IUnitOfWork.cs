using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;

namespace DessertApp.Services.Infraestructure.UnitOfWorkServices
{
    public interface IUnitOfWork : IDisposable
    {
        //Add repositories here using IDomainGenericRepository
        IDomainGenericRepository<Ingredient, int> Ingredients { get; }
        IDomainGenericRepository<IngredientUnit, int> IngredientUnits { get; }
        IDomainGenericRepository<MeasurementUnit, int> MeasurementUnits { get; }
        IDomainGenericRepository<DessertIngredient, int> DessertIngredients { get; }
        IDomainGenericRepository<InventoryAnalysis, int> InventoryAnalysis { get; }
        IDomainGenericRepository<PurchaseOrder, int> PurchaseOrders { get; }

        //Commit changes to database
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        //Clear database resources
        protected virtual void Dispose(bool disposing) { }
    }
}
