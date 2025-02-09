using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using System.Text;

namespace DessertApp.Application.DessertServices
{
    public class CalculationService
    {
        private readonly IDessertRepository _dessertService;

        public CalculationService(IDessertRepository dessertService)
        {
            _dessertService = dessertService;
        }

        public async Task<decimal> CalculateAnnualDessertDemand(CancellationToken cancellationToken)
        {
            List<PurchaseOrderDetail> values = new();

            var desserts = await _dessertService.GetDessertsWithIngredientsAsync(cancellationToken) ?? [];

            foreach (var dessert in desserts)
            {
                foreach (var des in dessert.DessertIngredients)
                {
                    var orderDetail = new PurchaseOrderDetail
                    {
                        IngredientId = des.IngredientId,
                        Ingredient = new Ingredient
                        {
                            Id = des.IngredientId,
                            Name = des.Ingredient.Name
                        },
                        Unit = new MeasurementUnit
                        {
                            Id = des.UnitId,
                            Name = des.Unit.Name
                        },
                        Quantity = (int)(des.QuantityRequired * (dessert.AnnualDemand ?? 0))
                    };

                    values.Add(orderDetail);
                }
            }

            StringBuilder information = new();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("****There are the annual ingredients demand to order****");
            Console.ResetColor();

            foreach (var item in values)
            {
                information.AppendFormat("Ingredient: {0}, Required Quantity: {1}{2}", item.Ingredient.Name, item.Quantity, item.Unit.Name);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(information.ToString());

                information.Clear();
            }

            Console.ResetColor();

            return desserts.Sum(d => d.DessertIngredients?.Sum(di => di.QuantityRequired) ?? 0);
        }
    }
}
