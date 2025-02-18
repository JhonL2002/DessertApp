using DessertApp.Application.DessertServices;
using DessertApp.Models.Entities;
using DessertApp.Services.Application.DessertServices;
using DessertApp.Services.Infraestructure.ImportDataServices;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Productions
{
    public class ProductionController : Controller
    {
        private readonly IDessertDemandService _dessertDemandService;
        private readonly DessertCalculationService _dessertCalculationService;
        private readonly IImportData<DessertAnalysis> _importData;

        public ProductionController(IDessertDemandService dessertDemandService, IImportData<DessertAnalysis> importData, DessertCalculationService dessertCalculationService)
        {
            _dessertDemandService = dessertDemandService;
            _importData = importData;
            _dessertCalculationService = dessertCalculationService;
        }

        public IActionResult Import()
        {
            return View();
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
                var dessertAnalysis = await _importData.ImportFromExternalSourceAsync(stream);

                //Convert imported data
                var demands = await _dessertDemandService.GetDessertDemands(dessertAnalysis, cancellationToken);

                //Send data to calculation service
                await _dessertCalculationService.ProcessInventoryAnalysisAsync(demands, cancellationToken);

                //Redirect to success view
                TempData["SuccessMessage"] = $"{demands.Count} Demand added successfully!";
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error has occurred while process file: {ex.Message}");
                return View();
            }
        }
    }
}
