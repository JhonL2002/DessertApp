using DessertApp.Application.InventoryServices;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Infraestructure.UserManagerServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserManagerService<IdentityResult,AppUser,IdentityOptions> _userManagerService;
        private readonly StockCheckerService _stockCheckerService;

        public HomeController(ILogger<HomeController> logger, StockCheckerService stockCheckerService, IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService)
        {
            _logger = logger;
            _stockCheckerService = stockCheckerService;
            _userManagerService = userManagerService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        { 
            var user = await _userManagerService.GetUserAsync(User);
            bool isAdmin = user != null && await _userManagerService.IsInRoleAsync(user, "Admin");

            bool hasStock = isAdmin && await _stockCheckerService.HasZeroStockIngredientsAsync(cancellationToken);

            bool hasInventoryAnalysis = isAdmin && await _stockCheckerService.HasInventoryAnalysisAsync(cancellationToken);

            bool hasOrders = isAdmin && await _stockCheckerService.HasOrdersAsync(cancellationToken);

            if (hasStock && !hasInventoryAnalysis)
            {
                ViewBag.ShowAlert = true;
                ViewBag.ShowOrderAlert = false;
            }else if (hasInventoryAnalysis && !hasOrders)
            {
                ViewBag.ShowAlert = false;
                ViewBag.ShowOrderAlert = true;
            }
            else
            {
                ViewBag.ShowAlert = false;
                ViewBag.ShowOrderAlert = false;
            }
            
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
