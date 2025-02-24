
namespace DessertApp.Models.Entities
{
    public class PendingReplenishment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
