using DessertApp.Models.Entities;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Queries
{
    public record GetAllOrdersQuery() : IRequest<IEnumerable<PurchaseOrder>>;
}
