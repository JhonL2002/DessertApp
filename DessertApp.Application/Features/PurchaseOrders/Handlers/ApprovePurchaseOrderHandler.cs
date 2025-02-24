using DessertApp.Application.Features.PurchaseOrders.Commands;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;

namespace DessertApp.Application.Features.PurchaseOrders.Handlers
{
    public class ApprovePurchaseOrderHandler : IRequestHandler<ApprovePurchaseOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovePurchaseOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null || order.IsApproved)
                return false;

            order.IsApproved = true;
            await _unitOfWork.PurchaseOrders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
