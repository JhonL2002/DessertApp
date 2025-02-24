using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers
{
    public class GetOrderDetailsHandler : IRequestHandler<GetOrderDetailsQuery, PurchaseOrder?>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetOrderDetailsHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrder?> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderRepository.GetOrderDetailsAsync(request.OrderId, cancellationToken);
        }
    }
}
