using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<PurchaseOrder>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetAllOrdersHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IEnumerable<PurchaseOrder>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderRepository.GetAllOrdersAsync(cancellationToken);
        }
    }
}
