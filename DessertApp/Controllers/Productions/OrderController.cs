using DessertApp.Services.Application.PurchaseOrderServices;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Productions
{
    public class OrderController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public OrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateInitialOrder(CancellationToken cancellationToken)
        {
            try
            {
                await _purchaseOrderService.CreatePurchaseOrderAsync(cancellationToken);

                TempData["SuccessMessage"] = "The first purchase order has been generated successfully";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while generating the first purchase order {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var orderDetails = await _purchaseOrderService.GetAllOrdersAsync(cancellationToken);
            return View(orderDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int orderId, CancellationToken cancellationToken)
        {
            var orderDetails = await _purchaseOrderService.GetOrderDetailsAsync(orderId, cancellationToken);
            return View(orderDetails);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveOrder(int orderId, CancellationToken cancellationToken)
        {
            var isApproved = await _purchaseOrderService.ApprovePurchaseOrderAsync(orderId, cancellationToken);

            if (isApproved)
            {
                return RedirectToAction("Index");
            }

            return View(isApproved);
        }
    }
}
