using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DessertApp.Models.Entities
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int UnitId { get; set; }
        public MeasurementUnit Unit { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(1, double.MaxValue)]
        public decimal UnitCost { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Subtotal { get; set; }

        public DateTime? NextOrderDate { get; set; }
        public int RemainingOrders { get; set; }
    }
}
