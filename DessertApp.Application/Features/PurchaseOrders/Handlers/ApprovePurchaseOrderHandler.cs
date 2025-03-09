using DessertApp.Application.Features.PurchaseOrders.Commands;
using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers
{
    public class ApprovePurchaseOrderHandler : IRequestHandler<ApprovePurchaseOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public ApprovePurchaseOrderHandler(IUnitOfWork unitOfWork, IMediator mediator, IPurchaseOrderRepository purchaseOrderRepository)
        {
            _unitOfWork = unitOfWork;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<bool> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _purchaseOrderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);

            if (order == null || order.IsApproved)
                return false;

            foreach (var detail in order.OrderDetails)
            {
                detail.RemainingOrders = detail.RemainingOrders - 1;
            }

            order.IsApproved = true;
            await _unitOfWork.PurchaseOrders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
