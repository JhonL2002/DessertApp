using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Commands
{
    public record ApprovePurchaseOrderCommand(int OrderId) : IRequest<bool>;
}
