namespace DessertApp.Services.Infraestructure.ImportDataServices
{
    public interface IImportData<T> where T : class
    {
        Task<T> ImportFromExternalSourceAsync(Stream externalSource, CancellationToken cancellationToken);
    }
}
