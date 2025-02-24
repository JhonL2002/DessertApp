using DessertApp.Application.Features.Inventories.Commands;
using DessertApp.Application.Features.PurchaseOrders.Commands;
using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Productions
{
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateInitialOrder(CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new CreatePurchaseOrderCommand(), cancellationToken);

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
            var orders = await _mediator.Send(new GetAllOrdersQuery(), cancellationToken);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int orderId, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderDetailsQuery(orderId), cancellationToken);
            return order != null ? View(order) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ApproveOrder(int orderId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ApprovePurchaseOrderCommand(orderId), cancellationToken);

            if (result)
            {
                var pendingReplenishment = new PendingReplenishment { OrderId = orderId };
                var repository = HttpContext.RequestServices.GetService<IPendingReplenishmentRepository>();
                await repository!.AddPendingReplenishmentAsync(pendingReplenishment, cancellationToken);

                return RedirectToAction("Index");
            }

            return View(result);
        }
    }
}
