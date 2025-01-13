using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }

        //Relationship with Sale
        [ForeignKey(nameof(Sale))]
        public int SaleId { get; set; }

        //Relationship with Dessert
        [ForeignKey(nameof(Dessert))]
        public int DessertId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        //One SaleDetail can have one Sale
        public Sale Sale { get; set; }

        //One SaleDetail can have one DessertId
        public Dessert Dessert { get; set; }

    }
}
