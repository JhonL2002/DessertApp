using ClosedXML.Excel;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.ImportDataServices;

namespace DessertApp.Infraestructure.ImportDataServices
{
    public class ImportMonthlyDessertDemand : IImportData<DessertAnalysis>
    {
        public async Task<List<DessertAnalysis>> ImportFromExternalSourceAsync(Stream externalSource)
        {
            var dtos = new List<DessertAnalysis>();

            using (var workbook = new XLWorkbook(externalSource))
            {
                var worksheet = workbook.Worksheet(1);

                //Skip header row
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    var dto = new DessertAnalysis
                    {
                        MonthlyDemands = Enumerable.Range(1, 12)
                            .Select(i => new int[] { i, row.Cell(i+1).GetValue<int>() })
                            .ToList(),
                        DessertName = row.Cell(1).GetValue<string>()
                    };
                    dtos.Add(dto);
                }
            }
            return await Task.FromResult(dtos);
        }
    }
}
