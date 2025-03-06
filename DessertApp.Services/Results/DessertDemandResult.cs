using DessertApp.Models.Entities;

namespace DessertApp.Services.Results
{
    public class DessertDemandResult
    {
        public List<DessertAnalysis> DessertAnalysisList { get; set; }
        public List<DessertDemand> DessertDemandList { get; set; }
    }
}
