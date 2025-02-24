using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class PendingReplenishmentRepository : IPendingReplenishmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PendingReplenishmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPendingReplenishmentAsync(PendingReplenishment pendingReplenishment, CancellationToken cancellationToken)
        {
            await _unitOfWork.PendingReplenishments.AddAsync(pendingReplenishment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<PendingReplenishment?> GetNextPendingReplenishmentAsync(CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.PendingReplenishments
                .GetAllAsync(cancellationToken);

            var nextOrder = orders.OrderBy(p => p.CreatedAt)
                .FirstOrDefault();

            return nextOrder;
        }

        public async Task RemovePendingReplenishmentAsync(PendingReplenishment pendingReplenishment, CancellationToken cancellationToken)
        {
            await _unitOfWork.PendingReplenishments.DeleteAsync(pendingReplenishment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
