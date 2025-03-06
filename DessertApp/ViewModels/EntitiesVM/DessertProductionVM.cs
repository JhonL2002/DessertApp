namespace DessertApp.ViewModels.EntitiesVM
{
    public class DessertProductionVM
    {
        public int DessertId { get; set; }
        public string SelectedMonth { get; set; } = DateTime.UtcNow.ToString("MMM");
        public List<string> AvailableMonths { get; set; } = new()
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        public int? ProducedAmount { get; set; }
    }
}
