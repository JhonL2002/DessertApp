using MediatR;

namespace DessertApp.Application.Features.Inventories.Commands
{
    public class ProcessFirstReplenishmentCommand : IRequest
    {
        public int OrderId { get; }

        public ProcessFirstReplenishmentCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
