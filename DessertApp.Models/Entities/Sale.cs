using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public string CreatedByUserId { get; set; } = string.Empty;

        //A Sale has many SaleDetail
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        
    }
}
