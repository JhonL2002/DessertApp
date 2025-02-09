using DessertApp.Application.DessertServices;
using DessertApp.Models.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CalculationService _calculationService;

        public HomeController(ILogger<HomeController> logger, CalculationService calculationService)
        {
            _logger = logger;
            _calculationService = calculationService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var calculatedSum = await _calculationService.CalculateAnnualDessertDemand(cancellationToken);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(calculatedSum);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionDetails != null)
            {
                _logger.LogError($"Catched exception: {exceptionDetails.Error.Message}");
            }
            return View();
        }
    }
}
