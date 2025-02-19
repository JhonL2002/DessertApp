﻿using DessertApp.Application.DessertServices;
using DessertApp.Application.InventoryServices;
using DessertApp.Application.PurchaseOrderServices;
using DessertApp.Application.Strategies;
using DessertApp.Services.Application.DessertServices;
using DessertApp.Services.Application.PurchaseOrderServices;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Application.ApplicationServicesInjections
{
    public static class ApplicationServiceInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDessertDemandService, DessertDemandService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

            services.AddScoped<DessertCalculationService>();
            services.AddScoped<StockCheckerService>();

            //Strategy for send email services
            services.AddScoped(typeof(EmailServiceStrategy<>));
            return services;
        }
    }
}
