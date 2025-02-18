using DessertApp.Services.DTOs;

namespace DessertApp.Services.Application.Reports
{
    public interface IDessertReportService
    {
        string GenerateMonthlyReport(List<DessertDemandsImportlDTO> orderDetails, CancellationToken cancellationToken);
    }
}
