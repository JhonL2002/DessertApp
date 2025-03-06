using DessertApp.Application.DessertServices;
using DessertApp.Services.Application.DessertServices;
using DessertApp.Services.Application.ProductionServices;
using DessertApp.Services.Infraestructure.ImportDataServices;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using DessertApp.Services.Results;
using DessertApp.ViewModels.EntitiesVM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DessertApp.Controllers.Productions
{
    public class ProductionController : Controller
    {
        private readonly IDessertDemandService _dessertDemandService;
        private readonly DessertCalculationService _dessertCalculationService;
        private readonly IImportData<DessertDemandResult> _importData;
        private readonly IProductionService _productionService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductionController(IDessertDemandService dessertDemandService, IImportData<DessertDemandResult> importData, DessertCalculationService dessertCalculationService, IProductionService productionService, IUnitOfWork unitOfWork)
        {
            _dessertDemandService = dessertDemandService;
            _importData = importData;
            _dessertCalculationService = dessertCalculationService;
            _productionService = productionService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Import()
        {
            return View();
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var desserts = await _unitOfWork.Desserts.GetAllAsync(cancellationToken);
            var model = new DessertProductionVM
            {
                DessertId = desserts.FirstOrDefault()?.Id ?? 0
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid file";
            }

            try
            {
                //Read file and get data
                using var stream = file!.OpenReadStream();
                var result = await _importData.ImportFromExternalSourceAsync(stream, cancellationToken);

                //Convert imported data
                var demandsDTO = await _dessertDemandService.GetDessertDemands(result.DessertAnalysisList, cancellationToken);

                //Send data to calculation service
                await _dessertCalculationService.ProcessInventoryAnalysisAsync(demandsDTO, cancellationToken);

                //Add demands to database
                await _unitOfWork.DessertDemands.AddRangeAsync(result.DessertDemandList, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                //Redirect to success view
                TempData["SuccessMessage"] = $"{demandsDTO.Count} Demand added successfully!";

                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error has occurred while process file: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Produce(DessertProductionVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var monthToUse = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                ? DateTime.UtcNow.ToString("MMMM")
                : model.SelectedMonth;

            var producedQuantity = await _productionService.ProduceDessertsAsync(model.DessertId, monthToUse, cancellationToken);
            model.ProducedAmount = producedQuantity;

            ViewData["Message"] = producedQuantity > 0 ? "Desserts produced successfully" : "Couldn't produce desserts. Verify ingredients";

            return RedirectToAction("Index", model);
        }
    }
}
