using DessertApp.Models.Entities;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Queries
{
    public record GetOrderDetailsQuery(int OrderId) : IRequest<PurchaseOrder?>;
}
