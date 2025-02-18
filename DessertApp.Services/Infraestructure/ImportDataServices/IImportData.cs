namespace DessertApp.Services.Infraestructure.ImportDataServices
{
    public interface IImportData<T> where T : class
    {
        Task<List<T>> ImportFromExternalSourceAsync(Stream externalSource);
    }
}
