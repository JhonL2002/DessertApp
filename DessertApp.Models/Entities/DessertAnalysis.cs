namespace DessertApp.Models.Entities
{
    public class DessertAnalysis
    {
        public string DessertName { get; set; } = string.Empty;

        public List<int[]> MonthlyDemands { get; set; }
    }
}
