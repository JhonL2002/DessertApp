using DessertApp.Application.Features.Inventories.Commands;
using DessertApp.Application.Features.PurchaseOrders.Queries;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Application.BackgroundWorkers
{
    public class ReplenishmentWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ReplenishmentWorker> _logger;

        public ReplenishmentWorker(IServiceScopeFactory serviceScopeFactory, ILogger<ReplenishmentWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ReplenishmentWorker started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var repository = scope.ServiceProvider.GetRequiredService<IPendingReplenishmentRepository>();

                    var pendingReplenishment = await repository.GetNextPendingReplenishmentAsync(stoppingToken);

                    if (pendingReplenishment != null)
                    {
                        await mediator.Send(new ProcessFirstReplenishmentCommand(pendingReplenishment.OrderId), stoppingToken);
                        await repository.RemovePendingReplenishmentAsync(pendingReplenishment, stoppingToken);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
