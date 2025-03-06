using ClosedXML.Excel;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.ImportDataServices;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using DessertApp.Services.Results;

namespace DessertApp.Infraestructure.ImportDataServices
{
    public class ImportAllDessertDemands : IImportData<DessertDemandResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImportAllDessertDemands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DessertDemandResult> ImportFromExternalSourceAsync(Stream externalSource, CancellationToken cancellationToken)
        {
            var dtos = new List<DessertAnalysis>();
            var demands = new List<DessertDemand>();

            using (var workbook = new XLWorkbook(externalSource))
            {
                var worksheet = workbook.Worksheet(1);
                //Skip header row
                var rows = worksheet.RowsUsed().Skip(1);
                var months = worksheet.Row(1).Cells().Skip(1).Select(cell => cell.GetValue<string>()).ToList();

                foreach (var row in rows)
                {
                    string dessertName = row.Cell(1).GetValue<string>();
                    var dessert = await _unitOfWork.Desserts.GetByFieldAsync("Name", dessertName, cancellationToken);
                    
                    if (dessert == null)
                    {
                        continue;
                    }

                    var dto = new DessertAnalysis
                    {
                        MonthlyDemands = Enumerable.Range(1, 12)
                            .Select(i => new int[] { i, row.Cell(i+1).GetValue<int>() })
                            .ToList(),
                        DessertName = dessertName
                    };
                    dtos.Add(dto);

                    for (int i = 0; i < months.Count; i++)
                    {
                        var demand = new DessertDemand
                        {
                            DessertId = dessert.Id,
                            Month = months[i],
                            Demand = row.Cell(i + 2).GetValue<int>()
                        };
                        demands.Add(demand);
                    }
                }
            }
            return new DessertDemandResult
            {
                DessertAnalysisList = dtos,
                DessertDemandList = demands
            };
        }
    }
}
