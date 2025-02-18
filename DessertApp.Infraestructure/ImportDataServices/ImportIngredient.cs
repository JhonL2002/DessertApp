using ClosedXML.Excel;
using DessertApp.Services.DTOs;
using DessertApp.Services.Infraestructure.ImportDataServices;

namespace DessertApp.Infraestructure.ImportDataServices
{
    public class ImportIngredient : IImportData<IngredientUnitImportDto>
    {
        public async Task<List<IngredientUnitImportDto>> ImportFromExternalSourceAsync(Stream externalSource)
        {
            var dtos = new List<IngredientUnitImportDto>();

            using (var workbook = new XLWorkbook(externalSource))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    var dto = new IngredientUnitImportDto
                    {
                        IngredientName = row.Cell(1).GetValue<string>(),
                        Stock = row.Cell(2).GetValue<int?>(),
                        UnitName = row.Cell(3).GetValue<string>(),
                        ItemsPerUnit = row.Cell(4).GetValue<int>(),
                        CostPerUnit = row.Cell(5).GetValue<decimal?>(),
                        OrderingCost = row.Cell(6).GetValue<decimal?>(),
                        MonthlyHoldingCostRate = row.Cell(7).GetValue<decimal?>(),
                        AnnualDemand = row.Cell(8).GetValue<int?>()
                    };

                    dtos.Add(dto);
                }
            }
            return await Task.FromResult(dtos);
        }
    }
}
