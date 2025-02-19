using DessertApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Application.PurchaseOrderServices
{
    public interface IPurchaseOrderService
    {
        Task CreatePurchaseOrder(CancellationToken cancellationToken);
    }
}
