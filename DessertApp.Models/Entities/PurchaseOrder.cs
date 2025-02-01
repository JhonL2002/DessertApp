using System.ComponentModel.DataAnnotations;

namespace DessertApp.Models.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Range(1, double.MaxValue)]
        public decimal TotalCost { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<PurchaseOrderDetail> OrderDetails { get; set; } = [];
    }
}
