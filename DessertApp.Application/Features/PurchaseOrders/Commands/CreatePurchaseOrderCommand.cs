using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Commands
{
    public record CreatePurchaseOrderCommand() : IRequest<bool>;
}
