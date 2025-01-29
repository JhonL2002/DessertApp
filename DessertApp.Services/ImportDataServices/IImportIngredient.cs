
namespace DessertApp.Services.ImportDataServices
{
    public interface IImportIngredient<T> where T : class
    {
        Task<List<T>> ImportFromExternalSourceAsync(Stream externalSource);
    }
}
