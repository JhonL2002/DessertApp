using DessertApp.Application.Features.Inventories.Commands;
using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;

namespace DessertApp.Application.Features.Inventories.Handlers
{
    class ProcessFirstReplenishmentHandler : IRequestHandler<ProcessFirstReplenishmentCommand>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessFirstReplenishmentHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProcessFirstReplenishmentCommand request, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderDetailsQuery(request.OrderId), cancellationToken);

            var ingredientUpdated = new List<Ingredient>();

            if (order == null || !order.IsApproved)
                return;

            bool allCompleted = true;

            foreach (var detail  in order.OrderDetails)
            {
                var ingredient = detail.Ingredient;
                if (ingredient != null && detail.Quantity > 0)
                {
                    int amountToAdd = Math.Min(detail.Quantity, 100);
                    ingredient.Stock += amountToAdd;
                    ingredient.IsAvailable = amountToAdd > 0;

                    if (detail.Quantity > ingredient.Stock)
                    {
                        allCompleted = false;
                    }
                    else
                    {
                        ingredient.Stock = detail.Quantity;
                        ingredientUpdated.Add(ingredient);
                    }
                }
            }

            if (ingredientUpdated.Count != 0)
            {
                await _unitOfWork.Ingredients.UpdateRangeAsync(ingredientUpdated, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            if (!allCompleted)
            {
                await Task.Delay(1000, cancellationToken);
                await _mediator.Send(new ProcessFirstReplenishmentCommand(request.OrderId), cancellationToken);
            }
        }
    }
}
