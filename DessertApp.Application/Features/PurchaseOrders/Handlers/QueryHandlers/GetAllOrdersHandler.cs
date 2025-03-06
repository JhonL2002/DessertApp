using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers.QueryHandlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<PurchaseOrder>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOrdersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PurchaseOrder>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PurchaseOrders.GetAllAsync(cancellationToken);
        }
    }
}
